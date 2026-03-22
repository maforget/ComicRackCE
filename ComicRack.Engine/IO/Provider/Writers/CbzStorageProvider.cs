using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Writers
{
    [FileFormat("eComic (ZIP)", KnownFileFormats.CBZ, ".cbz")]
    public class CbzStorageProvider : PackedStorageProvider
    {
        private const bool Zip64 = false;

        private Stream file;

        private ZipOutputStream zos;

        protected override void OnCreateFile(string target, StorageSetting setting)
        {
            file = File.Create(target, 100000);
            zos = new ZipOutputStream(file);
            zos.UseZip64 = UseZip64.Off;
            zos.SetLevel(setting.ComicCompression switch
            {
                ExportCompression.Medium => 5,
                ExportCompression.Strong => 9,
                _ => 0
            });
        }

        protected override void OnCloseFile()
        {
            zos.Close();
            file.Close();
        }

        protected override void AddEntry(string name, byte[] data)
        {
            var now = DateTime.Now;
            var entry = new ZipEntry(name)
            {
                Size = data.Length,
                DateTime = now,
            };
            if (!EngineConfiguration.Default.UseLegacyZipConfiguration)
            {
                entry.CompressionMethod = zos.GetLevel() == 0 ? CompressionMethod.Stored : CompressionMethod.Deflated; // Set to Stored if no compression, otherwise the compression method is always Deflated even when the level is 0.
                entry.ExtraData = MakeNtfsExtraField(now); // Sets the NTFS extra field to to set the timestamp, which "might" speeds up updating the archive in 7-Zip 
            }

            zos.PutNextEntry(entry);
            zos.Write(data, 0, data.Length);
            zos.CloseEntry();
        }

        private static byte[] MakeNtfsExtraField(DateTime dt)
        {
            // Convert to Windows FILETIME (100-nanosecond intervals since 1601-01-01 UTC)
            long fileTime = dt.ToUniversalTime().ToFileTime();
            byte[] timeBytes = BitConverter.GetBytes(fileTime);

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            bw.Write((ushort)0x000A); // NTFS tag
            bw.Write((ushort)32); // data size
            bw.Write((uint)0); // reserved
            bw.Write((ushort)0x0001); // Tag for attribute #1 (timestamps)
            bw.Write((ushort)24); // Size of attribute #1 (3 x 8 bytes)
            bw.Write(timeBytes); // mtime - File last modification time
            bw.Write((ulong)0); // atime - File last access time (it's empty with 7-Zip)
            bw.Write((ulong)0); // ctime - File creation time (it's empty with 7-Zip)
            return ms.ToArray();
        }
    }
}

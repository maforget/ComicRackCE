using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo;
using ICSharpCode.SharpZipLib.Tar;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive
{
    public class TarSharpZipEngine : FileBasedAccessor
    {
        private const int BufferSize = 131072;

        public TarSharpZipEngine()
            : base(5)
        {
        }

        public override bool IsFormat(string source)
        {
            try
            {
                using (FileStream inputStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
                {
                    using (TarInputStream tarInputStream = new TarInputStream(inputStream, Encoding.UTF8))
                    {
                        return tarInputStream.GetNextEntry() != null;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public override IEnumerable<ProviderImageInfo> GetEntryList(string source)
        {
            using (FileStream fs = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
            {
                using (TarInputStream tis = new TarInputStream(fs, Encoding.UTF8))
                {
                    while (true)
                    {
                        TarEntry nextEntry = tis.GetNextEntry();
                        if (nextEntry == null)
                        {
                            break;
                        }

                        if (!nextEntry.IsDirectory)
                        {
                            yield return new ProviderImageInfo(0, nextEntry.Name, nextEntry.Size);
                        }
                    }
                }
            }
        }

        public override byte[] ReadByteImage(string source, ProviderImageInfo info)
        {
            try
            {
                using (FileStream inputStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
                {
                    using (TarInputStream tarInputStream = new TarInputStream(inputStream, Encoding.UTF8))
                    {
                        TarEntry nextEntry;
                        do
                        {
                            nextEntry = tarInputStream.GetNextEntry();
                        }
                        while (!(nextEntry.Name == info.Name));
                        byte[] array = new byte[info.Size];
                        if (tarInputStream.Read(array, 0, array.Length) != array.Length)
                        {
                            throw new IOException();
                        }
                        return array;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public override ComicInfo ReadInfo(string source)
        {
            try
            {
                byte[] buffer = new byte[BufferSize];

                using (FileStream inputStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
                using (TarInputStream tarInputStream = new TarInputStream(inputStream, Encoding.UTF8))
                {
                    return XmlInfoProviders.Readers.DeserializeAll(s =>
                    {
                        TarEntry nextEntry;
                        do
                        {
                            nextEntry = tarInputStream.GetNextEntry();
                        } while (String.Compare(Path.GetFileName(nextEntry.Name), s, StringComparison.OrdinalIgnoreCase) != 0);

						MemoryStream inStream = new MemoryStream();
                        int bytesRead;
                        while ((bytesRead = tarInputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            inStream.Write(buffer, 0, bytesRead);
                        }

                        inStream.Position = 0;

                        return inStream;
                    });
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using cYo.Common.ComponentModel;
using cYo.Common.Compression.SevenZip;
using cYo.Common.IO;
using cYo.Common.Win32;
using cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive
{
    public class SevenZipEngine : FileBasedAccessor
    {
        private const int SevenZipCheckSize = 131072;

        public static readonly string PackExe = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\7z.exe");

        public static readonly string PackDll32 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\7z.dll");

        public static readonly string PackDll64 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\7z64.dll");

        private static readonly Regex rxList = new Regex("Path = (?<filename>.*)\\r\\n(Folder.*\\r\\n)*?Size = (?<size>\\d+)", RegexOptions.Compiled);

        private bool libraryMode;

        private static SevenZipFactory sevenZipFactory;

        private static SevenZipFactory SevenZipFactory
        {
            get
            {
                if (sevenZipFactory == null)
                {
                    sevenZipFactory = new SevenZipFactory(Environment.Is64BitProcess ? PackDll64 : PackDll32);
                }
                return sevenZipFactory;
            }
        }

        public SevenZipEngine(int format, bool libraryMode)
            : base(format)
        {

            this.libraryMode = libraryMode;
        }

        public override bool IsFormat(string source)
        {
            if (base.HasSignature)
            {
                return base.IsFormat(source);
            }
            try
            {
                IInArchive archive;
                using (OpenArchive(source, out archive))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public override IEnumerable<ProviderImageInfo> GetEntryList(string source)
        {
            if (libraryMode)
            {
                IInArchive archive;
                using (OpenArchive(source, out archive))
                {
                    int count = archive.GetNumberOfItems();
                    for (int i = 0; i < count; i++)
                    {
                        PropVariant value = default(PropVariant);
                        PropVariant value2 = default(PropVariant);
                        archive.GetProperty(i, ItemPropId.kpidPath, ref value);
                        archive.GetProperty(i, ItemPropId.kpidSize, ref value2);
                        yield return new ProviderImageInfo(i, value.GetObject().ToString(), value2.longValue);
                    }
                }
                yield break;
            }
            ExecuteProcess.Result result = ExecuteProcess.Execute(PackExe, "l -slt \"" + FileMethods.GetShortName(source) + "\"", ExecuteProcess.Options.StoreOutput);
            MatchCollection source2 = rxList.Matches(result.ConsoleText);
            foreach (ProviderImageInfo item in from m in source2.OfType<Match>()
                                               select new ProviderImageInfo(0, m.Groups["filename"].Value, long.Parse(m.Groups["size"].Value)))
            {
                yield return item;
            }
        }

        public override byte[] ReadByteImage(string source, ProviderImageInfo info)
        {
            return GetFileData(source, info);
        }

        private IDisposable OpenArchive(string source, out IInArchive archive)
        {
            IInArchive a = (archive = SevenZipFactory.CreateInArchive(MapFileFormat(base.Format)));
            InStreamWrapper archiveStream = new InStreamWrapper(File.OpenRead(source));
            long maxCheckStartPosition = SevenZipCheckSize;
            if (archive.Open(archiveStream, ref maxCheckStartPosition, new StubOpenCallback()) != 0)
            {
                archiveStream.Dispose();
                throw new FileLoadException();
            }
            return new Disposer(delegate
            {
                archiveStream.Dispose();
                a.Close();
                Marshal.ReleaseComObject(a);
            });
        }

        private static byte[] GetFileData(IInArchive archive, int fileNumber)
        {
            MemoryStream memoryStream;
            try
            {
                PropVariant value = default(PropVariant);
                archive.GetProperty(fileNumber, ItemPropId.kpidSize, ref value);
                memoryStream = new MemoryStream((int)value.longValue);
            }
            catch (Exception)
            {
                memoryStream = new MemoryStream();
            }
            archive.Extract(new int[1]
            {
                fileNumber
            }, 1, 0, new ExtractToStreamCallback(fileNumber, memoryStream));
            return memoryStream.ToArray();
        }

        private byte[] GetFileData(string source, string file)
        {
            try
            {
                if (libraryMode)
                {
                    IInArchive archive;
                    using (OpenArchive(source, out archive))
                    {
                        int numberOfItems = archive.GetNumberOfItems();
                        for (int i = 0; i < numberOfItems; i++)
                        {
                            PropVariant value = default(PropVariant);
                            archive.GetProperty(i, ItemPropId.kpidPath, ref value);
                            if (file.Equals(value.GetObject().ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                return GetFileData(archive, i);
                            }
                        }
                    }
                }
                else
                {
                    ExecuteProcess.Result result = ExecuteProcess.Execute(PackExe, "e -so \"" + FileMethods.GetShortName(source) + "\" \"" + file + "\"", ExecuteProcess.Options.StoreOutput);
                    if (result.ExitCode == 0)
                    {
                        return result.Output;
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        private byte[] GetFileData(string source, ProviderImageInfo ii)
        {
            if (!libraryMode)
            {
                return GetFileData(source, ii.Name);
            }
            try
            {
                IInArchive archive;
                using (OpenArchive(source, out archive))
                {
                    return GetFileData(archive, ii.Index);
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
				return XmlInfoProviders.Readers.DeserializeAll(s => new MemoryStream(GetFileData(source, s)));
			}
			catch
            {
                return null;
            }
        }

        public override bool WriteInfo(string source, ComicInfo comicInfo)
        {
            return UpdateComicInfo(source, base.Format, comicInfo);
        }

        private static KnownSevenZipFormat MapFileFormat(int format)
        {
            switch (format)
            {
                case KnownFileFormats.CBZ:
                    return KnownSevenZipFormat.Zip;
                case KnownFileFormats.CB7:
                    return KnownSevenZipFormat.SevenZip;
                case KnownFileFormats.CBT:
                    return KnownSevenZipFormat.Tar;
                case KnownFileFormats.CBR:
                    return KnownSevenZipFormat.Rar;
                case KnownFileFormats.RAR5:
                    return KnownSevenZipFormat.Rar5;
                default:
                    throw new NotSupportedException("Type if not supported");
            }
        }

        public static bool UpdateComicInfo(string file, int format, ComicInfo comicInfo)
        {
            bool flag;
            string arg;
            switch (format)
            {
                case KnownFileFormats.CBZ:
                    flag = false;
                    arg = "zip";
                    break;
                case KnownFileFormats.CB7:
                    flag = true;
                    arg = "7z";
                    break;
                case KnownFileFormats.CBT:
                    flag = false;
                    arg = "tar";
                    break;
                default:
                    return false;
            }
            try
            {
                if (flag)
                {
                    string parameters = $"u -t{arg} -siComicInfo.xml \"{file}\"";
                    if (ExecuteProcess.Execute(PackExe, parameters, comicInfo.ToArray(), null, ExecuteProcess.Options.None).ExitCode == 0)
                    {
                        return true;
                    }
                }
                else
                {
                    string text = Path.Combine(EngineConfiguration.Default.TempPath, Guid.NewGuid().ToString());
                    string text2 = Path.Combine(text, "ComicInfo.xml");
                    try
                    {
                        Directory.CreateDirectory(text);
                        using (Stream outStream = File.Create(text2))
                        {
                            comicInfo.Serialize(outStream);
                        }
                        string parameters2 = $"u -t{arg} \"{file}\" \"{text2}\"";
                        if (ExecuteProcess.Execute(PackExe, parameters2, null, ExecuteProcess.Options.None).ExitCode == 0)
                        {
                            return true;
                        }
                    }
                    finally
                    {
                        try
                        {
                            FileUtility.SafeDelete(text2);
                            Directory.Delete(text);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}

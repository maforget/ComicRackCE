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
using cYo.Common.Xml;
using cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive
{
    public class SevenZipEngine : FileBasedAccessor
    {
        private const int SevenZipCheckSize = 131072;

        public static readonly string ConsoleExe32 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\7z.exe"); // uses the 7z.dll which is 32bit, so only runs in 32bit mode, but supports more formats than the standalone 64bit version.
        public static readonly string StandaloneExe64 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\7za64.exe"); // Required to use the standalone exe on 64bit systems because 7z.dll is 32bit so wouldn't work in 64bit processes.
        public static readonly string PackExe = Environment.Is64BitProcess ? StandaloneExe64 : ConsoleExe32; // Provides proper executable based on architecture.

        public static readonly string PackDll32 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\7z.dll");
        public static readonly string PackDll64 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\7z64.dll");

        private static readonly Regex rxList = new Regex("Path = (?<filename>.*)\\r\\n(Folder.*\\r\\n)*?Size = (?<size>\\d+)", RegexOptions.Compiled);
        private static readonly Regex rxError = new Regex("Error:.+(^.+$)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Compiled);

        private bool libraryMode;

        private bool standalone = true;
        public string SevenZipExe => standalone ? PackExe : ConsoleExe32; // Gets the architecture appropriate executable in standalone mode. Otherwise returns the 32bit console exe.

        private static SevenZipFactory sevenZipFactory;
        private static SevenZipFactory SevenZipFactory => sevenZipFactory ??= new SevenZipFactory(Environment.Is64BitProcess ? PackDll64 : PackDll32);

        /// <summary>
        ///  Library mode is preferred for reading since it does not require spawning a separate process and does not have the overhead of starting a process.
        /// </summary>
        /// <param name="format">The format id from <see cref="KnownFileFormats"/></param>
        /// <param name="libraryMode">Will use the dll of 7-Zip instead of the executable</param>
        /// <param name="standalone">Only applicable when <paramref name="libraryMode"/> is <c>false</c>.<br/><br/>Standalone mode is preferred for updating since it will run in 64bit mode on 64bit systems, while the console version only runs in 32bit mode. But that standalone mode has limited support for formats, so we need to use the console version for those formats.</param>
        public SevenZipEngine(int format, bool libraryMode, bool standalone = true)
            : base(format)
        {

            this.libraryMode = libraryMode;
            this.standalone = standalone;
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
            ExecuteProcess.Result result = ExecuteProcess.Execute(SevenZipExe, "l -slt \"" + FileMethods.GetShortName(source) + "\"", ExecuteProcess.Options.StoreOutput);
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
                    ExecuteProcess.Result result = ExecuteProcess.Execute(SevenZipExe, "e -so \"" + FileMethods.GetShortName(source) + "\" \"" + file + "\"", ExecuteProcess.Options.StoreOutput);
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

        private T Read<T>(string source) where T : class
        {
            try
            {
                return XmlInfoProviders.Readers.DeserializeAll<T>(s => new MemoryStream(GetFileData(source, s))) as T;
            }
            catch
            {
                return null;
            }
        }

        public override ComicInfo ReadInfo(string source) => Read<ComicInfo>(source);

        public override ComicBook ReadBook(string source) => Read<ComicBook>(source);

        public override bool WriteInfo(string source, ComicInfo comicInfo)
        {
            return UpdateComicInfo(source, base.Format, standalone, comicInfo);
        }

        public override bool WriteBook(string source, ComicBook comicInfo)
        {
            return UpdateComicInfo(source, base.Format, standalone, comicInfo);
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

        // InlineUpdate: pass the byte[] directly to 7-Zip to update the archive without needing to create a temporary file. Only works with some formats, so for the others we need to create a temporary file.
        record UpdateSettings(bool InlineUpdate, string arg);

        public static bool UpdateComicInfo(string file, int format, bool standalone, ComicInfo comicInfo)
        {
            UpdateSettings setting = format switch
            {
                KnownFileFormats.CBZ => new UpdateSettings(InlineUpdate: false, arg: "zip"),
                KnownFileFormats.CB7 => new UpdateSettings(InlineUpdate: true, arg: "7z"),
                KnownFileFormats.CBT => new UpdateSettings(InlineUpdate: false, arg: "tar"),
                _ => throw new NotSupportedException("Format not supported for updating ComicInfo.xml")
            };

            return Update(file, standalone, comicInfo, setting);
        }

        private static bool Update(string file, bool standalone, ComicInfo comicInfo, UpdateSettings updateSetting)
        {
            try
            {
                ComicBook comicBook = comicInfo as ComicBook;
                bool isComicBook = comicBook != null;
                string filename = isComicBook ? "ComicBook.xml" : "ComicInfo.xml";

                if (updateSetting.InlineUpdate)
                {
                    byte[] data = isComicBook ? comicBook?.ToArrayFull(onlyPortable: true) : comicInfo.ToArray();
                    string parameters = $"u -t{updateSetting.arg} -si{filename} \"{file}\"";
                    return ExecuteUpdateProcess(parameters, standalone, data);
                }
                else
                {
                    string tempDir = Path.Combine(EngineConfiguration.Default.TempPath, Guid.NewGuid().ToString());
                    string tempPath = Path.Combine(tempDir, filename);
                    try
                    {
                        Directory.CreateDirectory(tempDir);
                        XmlUtility.Store(tempPath, comicInfo);
                        using (Stream outStream = File.Create(tempPath))
                        {
                            if (isComicBook)
                                comicBook?.SerializeFull(outStream, onlyPortable: true);
                            else
                                comicInfo.Serialize(outStream);
                        }
                        string parameters2 = $"u -t{updateSetting.arg} \"{file}\" \"{tempPath}\"";
                        return ExecuteUpdateProcess(parameters2, standalone);
                    }
                    finally
                    {
                        try
                        {
                            FileUtility.SafeDelete(tempPath);
                            Directory.Delete(tempDir);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (WriteErrorException) // We only want the WriteErrorException to be propagated, so that it shows the error message to the user.
            {
                throw;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private static bool ExecuteUpdateProcess(string parameters, bool standalone, byte[] inputData = null)
        {
            string exe = standalone ? PackExe : ConsoleExe32; // standalone mode is preferred for updating since it will run in 64bit mode on 64bit systems, while the console version will run in 32bit mode (or otherwise unsupported formats). Standalone mode has limited support for formats, so we need to use the console version for those.
            ExecuteProcess.Result result = ExecuteProcess.Execute(exe, parameters, inputData, null, ExecuteProcess.Options.StoreOutput);
            if (result.ExitCode == 0)
            {
                return true;
            }
            else // ExitCode 1 is a non fatal Error, so it might still have updated the ComicInfo.xml, but we want to check the error message to be sure.
            {
                //TODO: 7-Zip leaves .tmp files that should be cleaned up in the directory.
                string t = result.ConsoleText;
                string errorMessage = rxError.IsMatch(t) ? rxError.Match(t).Groups[1]?.Value.Trim() : string.Empty;

                if (!string.IsNullOrEmpty(errorMessage))
                    throw new WriteErrorException(errorMessage);

                return false;
            }
        }
    }
}

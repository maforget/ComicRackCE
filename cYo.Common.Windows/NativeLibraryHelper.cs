using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Windows
{
    public class NativeLibraryHelper
    {
        private static List<string> libraryNames = new List<string>();

        public bool LibraryLoadStatus { get; set; } = false;

        public NativeLibraryHelper(string lpPathName = "")
        {
            LibraryLoadStatus = RegisterDirectory(lpPathName);
        }

        /// <summary>
        /// Register a directory for the dll search
        /// </summary>
        /// <param name="lpPathName">the path of the directory, if empty it will set the "Resources" folder</param>
        /// <returns>Returns if the functions was sucessful, if the call was cached it will return true</returns>
        public static bool RegisterDirectory(string lpPathName = "")
        {
			string applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string dllPath = string.IsNullOrEmpty(lpPathName) ? "Resources" : lpPathName;
            string path = Path.Combine(applicationPath, dllPath);

			//Only set the directory once.
			if (!libraryNames.Contains(path))
            {
                libraryNames.Add(path);
                return SetDllDirectory(path);
            }
            else
            {
                return true;
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);
    }


}

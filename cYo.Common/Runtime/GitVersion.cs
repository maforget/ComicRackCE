using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cYo.Common.Runtime
{
    public static class GitVersion
    {
        private static string currentCommit = null;
        private static bool? isDirty = null;

        public static string CurrentCommit => currentCommit ??= GetString("CurrentCommit", Assembly.GetEntryAssembly()).Trim();
        public static bool IsDirty => isDirty ??= !string.IsNullOrEmpty(GetString("isDirty", Assembly.GetEntryAssembly())?.Trim());

		public static string GetCurrentVersionInfo()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            string isDirtyText = !IsDirty ? "" : "-dirty";
			return string.IsNullOrEmpty(CurrentCommit) ? "" : $" [{CurrentCommit[..7]}{isDirtyText}]";
        }

		public static Stream GetStream(string resourceName, Assembly assembly)
        {
            string fullResourceName = assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains(resourceName));
            return assembly.GetManifestResourceStream(fullResourceName); ;
        }

        public static string GetString(string resourceName, Assembly containingAssembly)
        {
            string result = String.Empty;
            Stream sourceStream = GetStream(resourceName, containingAssembly);

            if (sourceStream != null)
            {
                using (StreamReader streamReader = new StreamReader(sourceStream))
                {
                    result = streamReader.ReadToEnd();
                }
            }

            return result;
        }
    }
}

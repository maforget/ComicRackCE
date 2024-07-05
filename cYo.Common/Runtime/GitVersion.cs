using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Runtime
{
    public static class GitVersion
    {
        public static string GetCurrentVersionInfo()
        {
            var isDirty = string.IsNullOrEmpty(Properties.Resources.isDirty?.Trim()) ? "" : "-dirty";
            var currentCommit = string.IsNullOrEmpty(Properties.Resources.CurrentCommit) ? "" : $" [{Properties.Resources.CurrentCommit?[..7]}{isDirty}]";

            return currentCommit;
        }
    }
}

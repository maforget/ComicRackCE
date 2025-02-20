using System;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Win32.FileOperations
{
	internal class NetFramework : FileOperation
	{
		public NetFramework() : base(ShellFileDeleteOptions.None)
		{
		}

		public override void DeleteFile(string file)
		{
			VerifyFile(file);
			File.Delete(file);
		}
	}
}

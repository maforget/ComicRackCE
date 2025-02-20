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
	internal class Shell : FileOperation
	{
		public Shell(ShellFileDeleteOptions options) : base(options)
		{
		}

		public override void DeleteFile(string file)
		{
			VerifyFile(file);

			//ref: https://learn.microsoft.com/en-us/windows/win32/api/shldisp/ne-shldisp-shellspecialfolderconstants
			//ref: https://learn.microsoft.com/en-us/windows/win32/shell/folder-movehere
			const int ssfBITBUCKET = 0xa;
			dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));
			var recycleBin = shell.Namespace(ssfBITBUCKET);
			recycleBin.MoveHere(file, GetDeleteFileFlags());
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public static class AutomaticProgressDialogExtension
	{
		public static bool ForEachProgress<T>(this IEnumerable<T> items, Action<T> action, IWin32Window parent = null, string caption = null, string description = null, bool enableCancel = true, int timeToWait = 1000)
		{
			return AutomaticProgressDialog.Process(parent, caption, description, timeToWait, delegate
			{
				T[] array = items.ToArray();
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					if (AutomaticProgressDialog.ShouldAbort)
					{
						break;
					}
					AutomaticProgressDialog.Value = i;
					action(array[i]);
				}
			}, enableCancel ? AutomaticProgressDialogOptions.EnableCancel : AutomaticProgressDialogOptions.None);
		}
	}
}

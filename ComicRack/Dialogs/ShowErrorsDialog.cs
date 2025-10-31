using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.Sync;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class ShowErrorsDialog : FormEx
	{
		public class ErrorItem
		{
			public string Item
			{
				get;
				set;
			}

			public string Message
			{
				get;
				set;
			}
		}

		public ShowErrorsDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
			this.RestorePosition();
		}

		private void AddError(ErrorItem ei)
		{
			ListViewItem listViewItem = lvErrors.Items.Add(ei.Item);
			listViewItem.SubItems.Add(ei.Message);
		}

		public static void ShowErrors<T>(IWin32Window parent, SmartList<T> errors, Func<T, ErrorItem> converter)
		{
			if (errors.Count == 0)
			{
				return;
			}
			ShowErrorsDialog dlg = new ShowErrorsDialog();
			try
			{
				foreach (T error in errors)
				{
					dlg.AddError(converter(error));
				}
				EventHandler<SmartListChangedEventArgs<T>> value = delegate(object s, SmartListChangedEventArgs<T> e)
				{
					if (e.Action == SmartListAction.Insert)
					{
						dlg.BeginInvoke(delegate
						{
							dlg.AddError(converter(e.Item));
						});
					}
				};
				errors.Changed += value;
				dlg.ShowDialog(parent);
				errors.Changed -= value;
				errors.Clear();
			}
			finally
			{
				if (dlg != null)
				{
					((IDisposable)dlg).Dispose();
				}
			}
		}

		public static ErrorItem ComicExporterConverter(ComicExporter ce)
		{
			return new ErrorItem
			{
				Item = ce.ComicBooks[0].FileNameWithExtension,
				Message = ce.LastError
			};
		}

		public static ErrorItem DeviceSyncErrorConverter(DeviceSyncError ce)
		{
			return new ErrorItem
			{
				Item = ce.Name,
				Message = ce.Message
			};
		}
	}
}

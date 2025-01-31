using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Viewer
{
	public static class Win7
	{
		private static TaskbarManager windowsTaskbar;

		private static Dictionary<string, TabbedThumbnail> thumbnails = new Dictionary<string, TabbedThumbnail>(StringComparer.OrdinalIgnoreCase);

		public static bool TabbedThumbnailsEnabled => false;

		public static bool Initialize()
		{
			if (!TaskbarManager.IsPlatformSupported)
			{
				return false;
			}
			try
			{
				windowsTaskbar = TaskbarManager.Instance;
				windowsTaskbar.SetProgressState(TaskbarProgressBarState.NoProgress);
				JumpList jumpList = JumpList.CreateJumpList();
				jumpList.KnownCategoryToDisplay = JumpListKnownCategoryType.Recent;
				jumpList.Refresh();
				JumpList jumpList2 = JumpList.CreateJumpList();
				jumpList2.KnownCategoryToDisplay = JumpListKnownCategoryType.Frequent;
				jumpList2.Refresh();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static void UpdateRecent(string file)
		{
			if (TaskbarManager.IsPlatformSupported && File.Exists(file))
			{
				JumpList.AddToRecent(file);
			}
		}

		public static void BuildCategory(string title, IEnumerable<string> files)
		{
		}

		public static void SetOverlayIcon(Bitmap bitmap, string text)
		{
			if (windowsTaskbar != null)
			{
				if (bitmap == null)
				{
					windowsTaskbar.SetOverlayIcon(null, null);
				}
				else
				{
					windowsTaskbar.SetOverlayIcon(bitmap.BitmapToIcon(), text);
				}
			}
		}

		public static void AddTabbedThumbnail(IWin32Window parent, string url, Action activated, Action closed, Func<Bitmap> requestThumbnail)
		{
			if (TabbedThumbnailsEnabled && !thumbnails.ContainsKey(url))
			{
				TabbedThumbnail tt = new TabbedThumbnail(parent.Handle, new Control());
				tt.TabbedThumbnailActivated += delegate
				{
					activated();
				};
				tt.TabbedThumbnailBitmapRequested += delegate
				{
					tt.SetImage(requestThumbnail());
				};
				tt.TabbedThumbnailClosed += delegate
				{
					closed();
				};
				thumbnails.Add(url, tt);
				TaskbarManager.Instance.TabbedThumbnail.AddThumbnailPreview(tt);
			}
		}

		public static void RemoveThumbnail(string url)
		{
			if (TabbedThumbnailsEnabled && thumbnails.TryGetValue(url, out var value))
			{
				TaskbarManager.Instance.TabbedThumbnail.RemoveThumbnailPreview(value);
				value.Dispose();
				thumbnails.Remove(url);
			}
		}

		public static void InvalidateThumbnail(string url)
		{
			if (TabbedThumbnailsEnabled && thumbnails.TryGetValue(url, out var value))
			{
				value.InvalidatePreview();
			}
		}

		public static void SetActiveThumbnail(string url)
		{
			if (TabbedThumbnailsEnabled && thumbnails.TryGetValue(url, out var value))
			{
				TaskbarManager.Instance.TabbedThumbnail.SetActiveTab(value);
			}
		}

		[DllImport("user32.dll")]
		private static extern int SendMessage(HandleRef hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		public static bool ShowShield(Button button)
		{
			if (button == null)
			{
				return false;
			}
			uint msg = 5644u;
			button.FlatStyle = FlatStyle.System;
			return SendMessage(new HandleRef(button, button.Handle), msg, new IntPtr(0), new IntPtr(1)) == 0;
		}
	}
}

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class QuickRatingDialog : Form
	{
		public QuickRatingDialog()
		{
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
		}

		private static void SetThumbnailImage(IBitmapDisplayControl iv, ComicBook cb, int page)
		{
			try
			{
				ThumbnailKey key = cb.GetThumbnailKey(page);
				iv.Tag = key;
				using (IItemLock<ThumbnailImage> itemLock = Program.ImagePool.GetThumbnail(key, onlyMemory: true))
				{
					if (itemLock != null)
					{
						iv.SetBitmap(itemLock.Item.Bitmap.CreateCopy());
						return;
					}
					Program.ImagePool.SlowThumbnailQueue.AddItem(key, iv, delegate
					{
						try
						{
							using (IItemLock<ThumbnailImage> itemLock2 = Program.ImagePool.GetThumbnail(key, cb))
							{
								if (object.Equals(key, iv.Tag))
								{
									iv.SetBitmap(itemLock2.Item.Bitmap.CreateCopy());
								}
							}
						}
						catch
						{
						}
					});
				}
			}
			catch
			{
			}
		}


		public static bool Show(IWin32Window parent, ComicBook book)
		{
			if (book == null)
			{
				return false;
			}
			using (QuickRatingDialog quickRatingDialog = new QuickRatingDialog())
			{
				quickRatingDialog.Text = quickRatingDialog.Text + " - " + book.CaptionWithoutTitle;
				quickRatingDialog.txReview.Text = book.Review;
				quickRatingDialog.txRating.Rating = book.Rating;
				quickRatingDialog.chkShow.Checked = Program.Settings.AutoShowQuickReview;
				SetThumbnailImage(quickRatingDialog.coverThumbnail, book, book.FrontCoverPageIndex);
				bool flag = quickRatingDialog.ShowDialog(parent) == DialogResult.OK;
				if (flag)
				{
					book.Review = quickRatingDialog.txReview.Text;
					book.Rating = quickRatingDialog.txRating.Rating;
					Program.Settings.AutoShowQuickReview = quickRatingDialog.chkShow.Checked;
				}
				return flag;
			}
		}

	}
}

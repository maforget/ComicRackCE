using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine.Controls
{
	public class ComicPageControl : UserControlEx
	{
		private bool pendingUpdate;

		private ComicBook[] pendingBooks;

		[DefaultValue(null)]
		public virtual Image Icon
		{
			get;
			set;
		}

		public void MarkAsDirty()
		{
			pendingUpdate = true;
		}

		public void ShowInfo(IEnumerable<ComicBook> books)
		{
			pendingBooks = null;
			if (base.Visible)
			{
				OnShowInfo(books);
				pendingUpdate = false;
				pendingBooks = null;
			}
			else
			{
				pendingUpdate = true;
				pendingBooks = books.ToArray();
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				if (pendingUpdate)
				{
					OnShowInfo(pendingBooks ?? new ComicBook[0]);
				}
				pendingUpdate = false;
				pendingBooks = null;
			}
		}

		protected virtual void OnShowInfo(IEnumerable<ComicBook> books)
		{
		}
	}
}

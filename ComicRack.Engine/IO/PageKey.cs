using System;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO
{
	[Serializable]
	public class PageKey : ImageKey
	{
		private BitmapAdjustment adjustment = BitmapAdjustment.Empty;

		public BitmapAdjustment Adjustment => adjustment;

		public PageKey(object source, string location, long size, DateTime modified, int index, ImageRotation rotation, BitmapAdjustment adjustment)
			: base(source, location, size, modified, index, rotation)
		{
			this.adjustment = adjustment;
		}

		public PageKey(ImageKey key)
			: base(key)
		{
		}

		protected override int CreateHashCode()
		{
			return base.CreateHashCode() ^ adjustment.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			PageKey pageKey = obj as PageKey;
			if (base.Equals(obj) && pageKey != null)
			{
				return pageKey.adjustment == adjustment;
			}
			return false;
		}
	}
}

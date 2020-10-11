using System.Drawing;

namespace cYo.Projects.ComicRack.Engine
{
	public struct ImagePartInfo
	{
		private readonly int part;

		private readonly Point offset;

		public static readonly ImagePartInfo Empty;

		public int Part => part;

		public Point Offset => offset;

		public ImagePartInfo(int part, int x, int y)
		{
			this.part = part;
			offset = new Point(x, y);
		}

		public ImagePartInfo(int part, Point offset)
		{
			this.part = part;
			this.offset = offset;
		}

		public ImagePartInfo(int part)
			: this(part, Point.Empty)
		{
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			ImagePartInfo imagePartInfo = (ImagePartInfo)obj;
			if (part == imagePartInfo.part)
			{
				return offset == imagePartInfo.offset;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return part.GetHashCode();
		}

		public override string ToString()
		{
			return $"{part}, {Offset}";
		}

		public static bool operator ==(ImagePartInfo a, ImagePartInfo b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(ImagePartInfo a, ImagePartInfo b)
		{
			return !(a == b);
		}
	}
}

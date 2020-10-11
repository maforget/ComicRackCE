namespace cYo.Common.Presentation.Ceco
{
	public struct FontSize
	{
		public int Size;

		public bool Relative;

		public static readonly FontSize Empty = new FontSize(0, relative: true);

		public FontSize(int size, bool relative)
		{
			Size = size;
			Relative = relative;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is FontSize))
			{
				return false;
			}
			FontSize fontSize = (FontSize)obj;
			if (fontSize.Relative == Relative)
			{
				return fontSize.Size == Size;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Size.GetHashCode() ^ Relative.GetHashCode();
		}

		public static bool operator ==(FontSize a, FontSize b)
		{
			return object.Equals(a, b);
		}

		public static bool operator !=(FontSize a, FontSize b)
		{
			return !(a == b);
		}
	}
}

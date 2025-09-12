using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Drawing
{
	public static class FC
	{
		private struct FontKey
		{
			private readonly string fontFamily;

			private readonly FontStyle fontStyle;

			private readonly float fontSize;

			private readonly int hashCode;

			public string FontFamily => fontFamily;

			public FontStyle FontStyle => fontStyle;

			public float FontSize => fontSize;

			public FontKey(string fontFamily, float fontSize, FontStyle fontStyle)
			{
				this.fontFamily = fontFamily;
				this.fontStyle = fontStyle;
				this.fontSize = fontSize;
				hashCode = fontFamily.GetHashCode() ^ fontStyle.GetHashCode() ^ fontSize.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				FontKey fontKey = (FontKey)obj;
				if (FontFamily == fontKey.FontFamily && FontStyle == fontKey.FontStyle)
				{
					return FontSize == fontKey.FontSize;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return hashCode;
			}
		}

		private struct FontItem
		{
			private readonly LinkedListNode<FontKey> node;

			private readonly Font font;

			public LinkedListNode<FontKey> Node => node;

			public Font Font => font;

			public FontItem(Font font, LinkedListNode<FontKey> node)
			{
				this.font = font;
				this.node = node;
			}
		}

		public static bool SystemFallBack = true;

		private const int MaxSize = 100;

		private static readonly Dictionary<FontKey, FontItem> fontCache = new Dictionary<FontKey, FontItem>();

		private static readonly LinkedList<FontKey> fontKeyList = new LinkedList<FontKey>();

		public static Font Get(string fontFamily, float fontSize, FontStyle fontStyle)
		{
			FontKey fontKey = new FontKey(fontFamily, fontSize, fontStyle);
			if (fontCache.TryGetValue(fontKey, out var value))
			{
				if (value.Node != fontKeyList.First)
				{
					fontKeyList.Remove(value.Node);
					fontKeyList.AddFirst(value.Node);
				}
			}
			else
			{
				Font font;
				try
				{
					font = new Font(fontFamily, fontSize, fontStyle);
				}
				catch
				{
					if (!SystemFallBack)
					{
						throw;
					}
					font = SystemFonts.DefaultFont;
				}
				value = new FontItem(font, fontKeyList.AddFirst(fontKey));
				fontCache.Add(fontKey, value);
			}
			while (fontKeyList.Count > MaxSize)
			{
				FontKey value2 = fontKeyList.Last.Value;
				fontKeyList.RemoveLast();
				Font font2 = fontCache[value2].Font;
				if (!font2.IsSystemFont)
				{
					font2.Dispose();
				}
				fontCache.Remove(value2);
			}
			return value.Font;
		}

		public static Font Get(string fontFamily, float fontSize)
		{
			return Get(fontFamily, fontSize, FontStyle.Regular);
		}

		public static Font Get(Font font, float fontSize, FontStyle fontStyle)
		{
			return Get(font.FontFamily.Name, fontSize, fontStyle);
		}

		public static Font Get(Font font, FontStyle fontStyle)
		{
			return Get(font, font.Size, fontStyle);
		}

		public static Font Get(Font font, float fontSize)
		{
			return Get(font, fontSize, font.Style);
		}

		public static Font GetRelative(Font font, float fontSize, FontStyle fontStyle)
		{
			return Get(font, fontSize * font.Size, fontStyle);
		}

		public static Font GetRelative(Font font, float fontSize)
		{
			return Get(font, fontSize * font.Size);
		}
	}
}

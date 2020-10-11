using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Presentation.Ceco
{
	public class BodyBlock : FlowBlock, IResources
	{
		private class FontKey
		{
			public readonly string FontFamily;

			public readonly float FontSize;

			public readonly FontStyle FontStyle;

			public FontKey(string fontFamily, float fontSize, FontStyle fontStyle)
			{
				if (string.IsNullOrEmpty(fontFamily))
				{
					fontFamily = "Arial";
				}
				if (fontSize == 0f)
				{
					fontSize = 8f;
				}
				FontFamily = fontFamily;
				FontSize = fontSize;
				FontStyle = fontStyle;
			}

			public override bool Equals(object obj)
			{
				FontKey fontKey = obj as FontKey;
				if (fontKey == null)
				{
					return false;
				}
				if (fontKey.FontFamily == FontFamily && fontKey.FontSize == FontSize)
				{
					return fontKey.FontStyle == FontStyle;
				}
				return false;
			}

			public override int GetHashCode()
			{
				int num = FontFamily.GetHashCode() ^ FontSize.GetHashCode();
				FontStyle fontStyle = FontStyle;
				return num ^ fontStyle.GetHashCode();
			}
		}

		private readonly Dictionary<FontKey, Font> fontCache = new Dictionary<FontKey, Font>();

		private Font GetCachedFont(FontKey fontKey)
		{
			if (fontCache.TryGetValue(fontKey, out var value))
			{
				return value;
			}
			return fontCache[fontKey] = new Font(fontKey.FontFamily, fontKey.FontSize, fontKey.FontStyle);
		}

		public override Font GetFont(string fontFamily, float fontSize, FontStyle fontStyle)
		{
			return GetCachedFont(new FontKey(fontFamily, fontSize, fontStyle));
		}

		public override Image GetImage(string source)
		{
			return Image.FromFile(source);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (Font value in fontCache.Values)
				{
					value.Dispose();
				}
			}
			base.Dispose(disposing);
		}
	}
}

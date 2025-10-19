using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;
using cYo.Common.Windows.Forms.Theme.Resources;

namespace cYo.Common.Windows.Forms
{
	public static class StyledRenderer
	{
		public enum AlphaStyle
		{
			None,
			Hot,
			Selected,
			SelectedHot,
			Focused
		}

		public class StyleDefinition
		{
			public int AlphaHot
			{
				get;
				private set;
			}

			public int AlphaSelected
			{
				get;
				private set;
			}

			public int AlphaSelectedHot
			{
				get;
				private set;
			}

			public int Rounding
			{
				get;
				private set;
			}

			public int FrameWidth
			{
				get;
				private set;
			}

			public float BackAlpha
			{
				get;
				private set;
			}

			public int BackGradient
			{
				get;
				private set;
			}

			public StyleDefinition(int hot, int selected, int selectedHot, int rounding, int frameWidth, float backAlpha, int backGradient)
			{
				AlphaHot = hot;
				AlphaSelected = selected;
				AlphaSelectedHot = selectedHot;
				Rounding = rounding;
				FrameWidth = frameWidth;
				BackAlpha = backAlpha;
				BackGradient = backGradient;
			}

			public StyleDefinition(StyleDefinition sd)
			{
				AlphaHot = sd.AlphaHot;
				AlphaSelected = sd.AlphaSelected;
				AlphaSelectedHot = sd.AlphaSelectedHot;
				Rounding = sd.Rounding;
				FrameWidth = sd.FrameWidth;
				BackAlpha = sd.BackAlpha;
				BackGradient = sd.BackGradient;
			}

			public StyleDefinition Frame(int rounding, int width)
			{
				StyleDefinition styleDefinition = new StyleDefinition(this);
				if (rounding >= 0)
				{
					styleDefinition.Rounding = rounding;
				}
				if (width >= 0)
				{
					styleDefinition.FrameWidth = width;
				}
				return styleDefinition;
			}

			public StyleDefinition NoGradient()
			{
				return new StyleDefinition(this)
				{
					BackGradient = 0
				};
			}
		}

		public static readonly Color VistaColor = Color.FromArgb(153, 222, 253);

		public static readonly StyleDefinition Vista = new StyleDefinition(92, 164, 255, 2, 1, 0.5f, 64);

		public static readonly StyleDefinition Windows8 = new StyleDefinition(92, 164, 255, 0, 1, 0.25f, 0);

		private static readonly Lazy<int> version = new Lazy<int>(() => Environment.OSVersion.Version.Major * 100 + Environment.OSVersion.Version.Minor);

		public static StyleDefinition Default
		{
			get
			{
				if (version.Value < 602)
				{
					return Vista;
				}
				return Windows8;
			}
		}

		public static void DrawRectangle(this Graphics gr, Rectangle rc, Color baseColor, int rounding, int frameWidth, int frameAlpha, int backAlphaStart, int backAlphaEnd)
		{
			using (gr.AntiAlias())
			{
				Color color = Color.FromArgb(frameAlpha, baseColor);
				if (backAlphaStart >= 0 && backAlphaEnd >= 0)
				{
					Color color2 = Color.FromArgb(backAlphaStart, baseColor);
					Color color3 = Color.FromArgb(backAlphaEnd, baseColor);
					Rectangle rectangle = rc;
					rectangle.Inflate(-frameWidth, -frameWidth);
					using (GraphicsPath path = rectangle.ConvertToPath(rounding, rounding))
					{
						using (Brush brush = new LinearGradientBrush(rectangle, color2, color3, -90f)
						{
							WrapMode = WrapMode.TileFlipXY
						})
						{
							gr.FillPath(brush, path);
						}
					}
				}
				if (frameWidth <= 0)
				{
					return;
				}
				int num = frameWidth / 2;
				rc.Inflate(-num, -num);
				using (GraphicsPath path2 = rc.ConvertToPath(rounding, rounding))
				{
					using (Pen pen = new Pen(color, frameWidth))
					{
						gr.DrawPath(pen, path2);
					}
				}
			}
		}

		public static AlphaStyle GetAlphaStyle(bool selected, bool hot, bool focused)
		{
			if ((hot || focused) && selected)
			{
				return AlphaStyle.SelectedHot;
			}
			if (hot)
			{
				return AlphaStyle.Hot;
			}
			if (selected)
			{
				return AlphaStyle.Selected;
			}
			if (focused)
			{
				return AlphaStyle.Focused;
			}
			return AlphaStyle.None;
		}

		public static Color GetSelectionColor(bool focused)
		{
			if (!focused)
			{
				return ThemeColors.StyledRenderer.Selection;
			}
			return ThemeColors.StyledRenderer.SelectionFocused;
		}

		public static void DrawStyledRectangle(this Graphics gr, Rectangle rc, int baseAlpha, Color baseColor, StyleDefinition style = null)
		{
			if (style == null)
			{
				style = Default;
			}
			int frameAlpha = Math.Abs(baseAlpha);
			int num = (int)((float)baseAlpha * style.BackAlpha);
			int backAlphaEnd = (num - style.BackGradient).Clamp(0, 255);
			gr.DrawRectangle(rc, baseColor, style.Rounding, style.FrameWidth, frameAlpha, num, backAlphaEnd);
		}

		public static void DrawStyledRectangle(this Graphics gr, Rectangle rc, AlphaStyle state, Color baseColor, StyleDefinition style = null)
		{
			int baseAlpha = 0;
			if (style == null)
			{
				style = Default;
			}
			switch (state)
			{
			case AlphaStyle.Hot:
				baseAlpha = style.AlphaHot;
				break;
			case AlphaStyle.Selected:
				baseAlpha = style.AlphaSelected;
				break;
			case AlphaStyle.SelectedHot:
				baseAlpha = style.AlphaSelectedHot;
				break;
			case AlphaStyle.Focused:
				baseAlpha = -style.AlphaSelected;
				break;
			}
			gr.DrawStyledRectangle(rc, baseAlpha, baseColor, style);
		}
	}
}

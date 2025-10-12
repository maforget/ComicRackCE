using System.Collections.Generic;
using System.Drawing;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	public abstract class ViewItemRenderer : ThumbRenderer
	{
		private Color backColor = Color.Transparent;

		private Color foreColor = SystemColorsEx.WindowText;

		private readonly List<TextLine> textLines = new List<TextLine>();

		public Color BackColor
		{
			get
			{
				return backColor;
			}
			set
			{
				backColor = value;
			}
		}

		public Color ForeColor
		{
			get
			{
				return foreColor;
			}
			set
			{
				foreColor = value;
			}
		}

		public Size Border
		{
			get;
			set;
		}

		public List<TextLine> TextLines => textLines;

		public void DisposeTextLines()
		{
			TextLines.ForEach(delegate(TextLine tl)
			{
				tl.Dispose();
			});
			TextLines.Clear();
		}
	}
}

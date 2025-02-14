using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using cYo.Common;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Viewer.Views;

namespace cYo.Projects.ComicRack.Viewer.Config
{
	[Serializable]
	public class DisplayWorkspace : IComparable<DisplayWorkspace>, INamed, IDescription
	{
		private string backgroundColor = "WhiteSmoke";

		private float paperTextureStrength = 1f;

		private ImageLayout paperTextureLayout = ImageLayout.Tile;

		private ImageLayout backgroundImageLayout = ImageLayout.Tile;

		[DefaultValue("")]
		public string Name
		{
			get;
			set;
		}

		[DefaultValue(WorkspaceType.Default)]
		public WorkspaceType Type
		{
			get;
			set;
		}

		public string Description
		{
			get
			{
				string separator = ", ";
				string text = string.Empty;
				if (Type.IsSet(WorkspaceType.WindowLayout))
				{
					text = text.AppendWithSeparator(separator, TR.Default["Windows"]);
				}
				if (Type.IsSet(WorkspaceType.ViewsSetup))
				{
					text = text.AppendWithSeparator(separator, TR.Default["Lists"]);
				}
				if (Type.IsSet(WorkspaceType.ComicPageLayout))
				{
					text = text.AppendWithSeparator(separator, TR.Default["Layout"]);
				}
				if (Type.IsSet(WorkspaceType.ComicPageDisplay))
				{
					text = text.AppendWithSeparator(separator, TR.Default["Display"]);
				}
				return text;
			}
		}

		[Browsable(false)]
		[DefaultValue(typeof(Size), "400, 250")]
		public Size PanelSize
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(DockStyle.Fill)]
		public DockStyle PanelDock
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(true)]
		public bool PanelVisible
		{
			get;
			set;
		}

		[Browsable(false)]
		public Rectangle FormBounds
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(FormWindowState.Normal)]
		public FormWindowState FormState
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(false)]
		public bool FullScreen
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(true)]
		public bool MinimalGui
		{
			get;
			set;
		}

		[Browsable(false)]
		public ItemViewConfig ComicBookDialogPagesConfig
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(false)]
		public bool ReaderUndocked
		{
			get;
			set;
		}

		[Browsable(false)]
		public Rectangle UndockedReaderBounds
		{
			get;
			set;
		}

        [Browsable(false)]
        public Rectangle ScriptOutputBounds
        {
            get;
            set;
        }

		[Browsable(false)]
		public Size PreferencesOutputSize
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(FormWindowState.Normal)]
		public FormWindowState UndockedReaderState
		{
			get;
			set;
		}

		[Browsable(false)]
		public ComicExplorerViewSettings DatabaseView
		{
			get;
			set;
		}

		[Browsable(false)]
		public ComicExplorerViewSettings FileView
		{
			get;
			set;
		}

		[Browsable(false)]
		public ItemViewConfig PagesViewConfig
		{
			get;
			set;
		}

		public BookPageLayout LandscapeLayout
		{
			get;
			set;
		}

		public BookPageLayout PortraitLayout
		{
			get;
			set;
		}

		public BookPageLayout Layout
		{
			get
			{
				if (!Screen.PrimaryScreen.IsLandscape())
				{
					return PortraitLayout;
				}
				return LandscapeLayout;
			}
		}

		[Browsable(false)]
		[DefaultValue(false)]
		public bool RightToLeftReading
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(PageTransitionEffect.Fade)]
		public PageTransitionEffect PageTransitionEffect
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool DrawRealisticPages
		{
			get;
			set;
		}

		[DefaultValue(ImageBackgroundMode.Color)]
		public ImageBackgroundMode PageImageBackgroundMode
		{
			get;
			set;
		}

		[DefaultValue("WhiteSmoke")]
		public string BackgroundColor
		{
			get
			{
				return backgroundColor;
			}
			set
			{
				backgroundColor = ColorExtensions.IsNamedColor(value);
			}
		}

		[XmlIgnore]
		public Color BackColor
		{
			get
			{
				return Color.FromName(BackgroundColor);
			}
			set
			{
				BackgroundColor = value.Name;
			}
		}

		[DefaultValue(null)]
		public string BackgroundTexture
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool PageMargin
		{
			get;
			set;
		}

		[DefaultValue(0.05f)]
		public float PageMarginPercentWidth
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string PaperTexture
		{
			get;
			set;
		}

		[DefaultValue(1f)]
		public float PaperTextureStrength
		{
			get
			{
				return paperTextureStrength;
			}
			set
			{
				paperTextureStrength = value.Clamp(0f, 1f);
			}
		}

		[DefaultValue(ImageLayout.Tile)]
		public ImageLayout PaperTextureLayout
		{
			get
			{
				return paperTextureLayout;
			}
			set
			{
				paperTextureLayout = value;
			}
		}

		[DefaultValue(ImageLayout.Tile)]
		public ImageLayout BackgroundImageLayout
		{
			get
			{
				return backgroundImageLayout;
			}
			set
			{
				backgroundImageLayout = value;
			}
		}

		[XmlIgnore]
		public bool IsWindowLayout
		{
			get
			{
				return IsType(WorkspaceType.WindowLayout);
			}
			set
			{
				SetType(WorkspaceType.WindowLayout, value);
			}
		}

		[XmlIgnore]
		public bool IsViewsSetup
		{
			get
			{
				return IsType(WorkspaceType.ViewsSetup);
			}
			set
			{
				SetType(WorkspaceType.ViewsSetup, value);
			}
		}

		[XmlIgnore]
		public bool IsComicPageLayout
		{
			get
			{
				return IsType(WorkspaceType.ComicPageLayout);
			}
			set
			{
				SetType(WorkspaceType.ComicPageLayout, value);
			}
		}

		[XmlIgnore]
		public bool IsComicPageDisplay
		{
			get
			{
				return IsType(WorkspaceType.ComicPageDisplay);
			}
			set
			{
				SetType(WorkspaceType.ComicPageDisplay, value);
			}
		}

		public DisplayWorkspace(string name)
		{
			Name = name;
			LandscapeLayout = new BookPageLayout();
			PortraitLayout = new BookPageLayout();
			PageImageBackgroundMode = ImageBackgroundMode.Color;
			DrawRealisticPages = true;
			PageTransitionEffect = PageTransitionEffect.Fade;
			PagesViewConfig = new ItemViewConfig();
			FileView = new ComicExplorerViewSettings();
			DatabaseView = new ComicExplorerViewSettings();
			UndockedReaderState = FormWindowState.Normal;
			FormState = FormWindowState.Normal;
			PanelVisible = true;
			PanelDock = DockStyle.Fill;
			PanelSize = new Size(400, 250).ScaleDpi();
			Type = WorkspaceType.Default;
			PageMarginPercentWidth = 0.05f;
			MinimalGui = true;
		}

		public DisplayWorkspace()
			: this(string.Empty)
		{
		}

		public bool IsType(WorkspaceType type)
		{
			return Type.IsSet(type);
		}

		public void SetType(WorkspaceType type, bool set)
		{
			Type = Type.SetMask(type, set);
		}

		public int CompareTo(DisplayWorkspace other)
		{
			return string.Compare(Name, other.Name);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}

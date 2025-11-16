using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Viewer.Config;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class ComicDisplaySettingsDialog : FormEx
	{
		private class TextureFileItem : ComboBoxSkinner.ComboBoxItem<string>
		{
			private Regex rxFormatCode = new Regex("\\s*\\[(?<code>[CSTZ])\\]\\z", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

			private bool failed;

			public string Name
			{
				get;
				set;
			}

			public bool IsCustom
			{
				get;
				set;
			}

			public string Default
			{
				get;
				set;
			}

			public ImageLayout Layout
			{
				get;
				set;
			}

			public Bitmap Sample
			{
				get;
				set;
			}

			public TextureFileItem(string file, bool custom = true)
				: base(file)
			{
				Default = TR.Default["None", "None"];
				base.IsOwnerDrawn = true;
				IsCustom = custom;
				if (!IsCustom)
				{
					ParseFileName(file);
				}
			}

			public TextureFileItem()
				: this(null, custom: false)
			{
			}

			public override string ToString()
			{
				if (string.IsNullOrEmpty(base.Item))
				{
					return Default;
				}
				if (!IsCustom)
				{
					return Name;
				}
				return Path.GetFileName(base.Item);
			}

			public override bool Equals(object obj)
			{
				if (obj is TextureFileItem)
				{
					return ((TextureFileItem)obj).Item == base.Item;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			private void ParseFileName(string path)
			{
				if (string.IsNullOrEmpty(path))
				{
					return;
				}
				string text = Path.GetFileNameWithoutExtension(path);
				Layout = ImageLayout.Tile;
				Match match = rxFormatCode.Match(text);
				if (match.Success)
				{
					switch (match.Groups["code"].Value.ToUpper())
					{
						case "C":
							Layout = ImageLayout.Center;
							break;
						case "S":
							Layout = ImageLayout.Stretch;
							break;
						case "Z":
							Layout = ImageLayout.Zoom;
							break;
					}
					text = rxFormatCode.Replace(text, string.Empty);
				}
				Name = TR.Load("Textures")[text, text.PascalToSpaced()];
			}

			public override Size Measure(Graphics gr, Font font)
			{
				Size result = base.Measure(gr, font);
				result.Height *= 2;
				return result;
			}

			public override void Draw(Graphics gr, Rectangle bounds, Color foreColor, Font font)
			{
				int height = Measure(gr, font).Height;
				if (Sample == null && !failed)
				{
					try
					{
						using (Bitmap image = Image.FromFile(base.Item) as Bitmap)
						{
							Sample = image.CreateCopy(new Size(height * 2, height).ToRectangle(), alwaysTrueCopy: true);
						}
					}
					catch
					{
						failed = true;
					}
				}
				try
				{
					if (Sample != null)
					{
						gr.DrawImage(Sample, bounds.X, bounds.Y);
					}
				}
				catch (Exception)
				{
				}
				using (SolidBrush brush = new SolidBrush(foreColor))
				{
					using (StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap)
					{
						Alignment = StringAlignment.Near,
						LineAlignment = StringAlignment.Center
					})
					{
						if (IsCustom)
						{
							stringFormat.Trimming = StringTrimming.EllipsisPath;
						}
						gr.DrawString(ToString(), font, brush, bounds.Pad(height * 2 + 4, 0), stringFormat);
					}
				}
			}
		}


		private DisplayWorkspace Workspace
		{
			get;
			set;
		}

		private Action<DisplayWorkspace> ApplyAction
		{
			get;
			set;
		}

		public override UIComponent UIComponent => UIComponent.Content;

		public ComicDisplaySettingsDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			this.RestorePosition();
			LocalizeUtility.Localize(this, null);
			new ComboBoxSkinner(cbPaperTexture);
			new ComboBoxSkinner(cbBackgroundTexture);
			labelBackgroundTexture.Location = labelBackgroundColor.Location;
			cbBackgroundTexture.Location = cpBackgroundColor.Location;
			btBrowseTexture.Top = cbBackgroundTexture.Top;
			cpBackgroundColor.FillKnownColors(includingSystem: false);
			cbBackgroundTexture.Items.Add(new TextureFileItem());
			string[] array = Program.LoadDefaultBackgroundTextures();
			foreach (string file in array)
			{
				cbBackgroundTexture.Items.Add(new TextureFileItem(file, custom: false));
			}
			cbPaperTexture.Items.Add(new TextureFileItem
			{
				Default = TR.Default["Default", "Default"]
			});
			string[] array2 = Program.LoadDefaultPaperTextures();
			foreach (string file2 in array2)
			{
				cbPaperTexture.Items.Add(new TextureFileItem(file2, custom: false));
			}
			LocalizeUtility.Localize(TR.Load(base.Name), cbPageTransition);
			LocalizeUtility.Localize(TR.Load(base.Name), cbBackgroundType);
			LocalizeUtility.Localize(TR.Load(base.Name), cbPaperLayout);
			LocalizeUtility.Localize(TR.Load(base.Name), cbTextureLayout);
		}

		protected override void OnClosed(EventArgs e)
		{
			foreach (TextureFileItem item in cbPaperTexture.Items)
			{
				item.Sample.SafeDispose();
			}
			foreach (TextureFileItem item2 in cbBackgroundTexture.Items)
			{
				item2.Sample.SafeDispose();
			}
			base.OnClosed(e);
		}

		private void btBroweTexture_Click(object sender, EventArgs e)
		{
			string texture = GetTexture();
			if (!string.IsNullOrEmpty(texture))
			{
				SelectTextureFile(cbBackgroundTexture, texture);
			}
		}

		private void btBrowsePaper_Click(object sender, EventArgs e)
		{
			string texture = GetTexture();
			if (!string.IsNullOrEmpty(texture))
			{
				SelectTextureFile(cbPaperTexture, texture);
			}
		}

		private void cbPaperTexture_SelectedIndexChanged(object sender, EventArgs e)
		{
			Label label = labelPaperStrength;
			ComboBox comboBox = cbPaperLayout;
			bool flag2 = (tbPaperStrength.Visible = cbPaperTexture.SelectedIndex != 0);
			bool visible = (comboBox.Visible = flag2);
			label.Visible = visible;
			TextureFileItem textureFileItem = (TextureFileItem)cbPaperTexture.SelectedItem;
			if (!textureFileItem.IsCustom)
			{
				cbPaperLayout.SelectedIndex = (int)textureFileItem.Layout;
			}
			cbPaperLayout.Visible = textureFileItem.IsCustom;
		}

		private void cbBackgroundTexture_SelectedIndexChanged(object sender, EventArgs e)
		{
			TextureFileItem textureFileItem = (TextureFileItem)cbBackgroundTexture.SelectedItem;
			if (!textureFileItem.IsCustom)
			{
				cbTextureLayout.SelectedIndex = (int)textureFileItem.Layout;
			}
			cbTextureLayout.Visible = textureFileItem.IsCustom;
		}

		private void cbBackgroundType_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = cbBackgroundType.SelectedIndex;
			Label label = labelBackgroundColor;
			bool visible = (cpBackgroundColor.Visible = selectedIndex == 1);
			label.Visible = visible;
			Label label2 = labelBackgroundTexture;
			ComboBox comboBox = cbBackgroundTexture;
			bool flag3 = (btBrowseTexture.Visible = selectedIndex == 2);
			visible = (comboBox.Visible = flag3);
			label2.Visible = visible;
			TextureFileItem textureFileItem = cbBackgroundTexture.SelectedItem as TextureFileItem;
			cbTextureLayout.Visible = selectedIndex == 2 && (textureFileItem?.IsCustom ?? true);
		}

		private void btApply_Click(object sender, EventArgs e)
		{
			if (ApplyAction != null)
			{
				Apply(Workspace);
				ApplyAction(Workspace);
			}
		}

		private void PercentTrackbarValueChanged(object sender, EventArgs e)
		{
			TrackBarLite trackBarLite = sender as TrackBarLite;
			toolTip.SetToolTip(trackBarLite, $"{trackBarLite.Value}%");
		}

		private void Apply(DisplayWorkspace ws)
		{
			ws.PageTransitionEffect = (PageTransitionEffect)cbPageTransition.SelectedIndex;
			ws.DrawRealisticPages = chkRealisticPages.Checked;
			ws.PageMargin = chkPageMargin.Checked;
			ws.PageMarginPercentWidth = (float)tbMargin.Value / 100f;
			ws.PageImageBackgroundMode = (ImageBackgroundMode)cbBackgroundType.SelectedIndex;
			ws.BackgroundColor = cpBackgroundColor.SelectedColorName;
			ws.BackgroundTexture = ((TextureFileItem)cbBackgroundTexture.SelectedItem).Item;
			ws.PaperTexture = ((TextureFileItem)cbPaperTexture.SelectedItem).Item;
			ws.PaperTextureStrength = (float)tbPaperStrength.Value / 100f;
			ws.PaperTextureLayout = (ImageLayout)cbPaperLayout.SelectedIndex;
			ws.BackgroundImageLayout = (ImageLayout)cbTextureLayout.SelectedIndex;
		}

		private void Update(DisplayWorkspace ws)
		{
			Workspace = ws;
			cbPageTransition.SelectedIndex = (int)ws.PageTransitionEffect;
			chkRealisticPages.Checked = ws.DrawRealisticPages;
			chkPageMargin.Checked = ws.PageMargin;
			tbMargin.Value = (int)(ws.PageMarginPercentWidth * 100f);
			cpBackgroundColor.SelectedColorName = ws.BackgroundColor;
			cbBackgroundType.SelectedIndex = (int)ws.PageImageBackgroundMode;
			SelectTextureFile(cbBackgroundTexture, ws.BackgroundTexture);
			SelectTextureFile(cbPaperTexture, ws.PaperTexture);
			tbPaperStrength.Value = (int)(ws.PaperTextureStrength * 100f);
			cbPaperLayout.SelectedIndex = (int)ws.PaperTextureLayout;
			cbTextureLayout.SelectedIndex = (int)ws.BackgroundImageLayout;
		}

		private void SelectTextureFile(ComboBox cb, string texture)
		{
			int num = cb.Items.OfType<TextureFileItem>().FindIndex((TextureFileItem i) => string.Equals(i.Item, texture, StringComparison.OrdinalIgnoreCase));
			if (num != -1)
			{
				cb.SelectedIndex = num;
				return;
			}
			TextureFileItem textureFileItem = cb.Items[cb.Items.Count - 1] as TextureFileItem;
			if (textureFileItem.IsCustom)
			{
				textureFileItem.Sample.SafeDispose();
				textureFileItem.Sample = null;
				cb.Items.Remove(textureFileItem);
			}
			textureFileItem = new TextureFileItem
			{
				Item = texture,
				IsCustom = true
			};
			cb.Items.Add(textureFileItem);
			cb.SelectedItem = textureFileItem;
		}

		private string GetTexture()
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = TR.Load("FileFilter")["PageImageSave", "JPEG Image|*.jpg|Windows Bitmap Image|*.bmp|PNG Image|*.png|GIF Image|*.gif|TIFF Image|*.tif"];
				openFileDialog.CheckFileExists = true;
				if (openFileDialog.ShowDialog(this) == DialogResult.OK)
				{
					return openFileDialog.FileName;
				}
			}
			return null;
		}

		public static bool Show(IWin32Window parent, bool enableHardware, DisplayWorkspace ws, Action<DisplayWorkspace> apply)
		{
			using (ComicDisplaySettingsDialog comicDisplaySettingsDialog = new ComicDisplaySettingsDialog())
			{
				comicDisplaySettingsDialog.Update(ws);
				comicDisplaySettingsDialog.ApplyAction = apply;
				comicDisplaySettingsDialog.grpEffects.Visible = enableHardware;
				if (comicDisplaySettingsDialog.ShowDialog(parent) != DialogResult.OK)
				{
					return false;
				}
				comicDisplaySettingsDialog.Apply(ws);
				apply?.Invoke(ws);
				return true;
			}
		}

	}
}

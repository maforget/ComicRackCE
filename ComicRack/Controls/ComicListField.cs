using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using cYo.Common.Localize;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class ComicListField
	{
		[DefaultValue(null)]
		public string DisplayProperty
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string EditProperty
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string Description
		{
			get;
			set;
		}

		public StringTrimming Trimming
		{
			get;
			set;
		}

		public Type ValueType
		{
			get;
			set;
		}

		public string DefaultText
		{
			get;
			set;
		}

		public ComicListField()
		{
		}

		public ComicListField(string displayProperty, string description, string editProperty = null, StringTrimming trimming = StringTrimming.EllipsisCharacter, Type valueType = null, string defaultText = null)
			: this()
		{
			DisplayProperty = displayProperty;
			Description = description;
			EditProperty = editProperty;
			Trimming = trimming;
			ValueType = valueType;
			DefaultText = defaultText;
		}

		public static void TranslateColumns(IEnumerable<IColumn> itemViewColumnCollection)
		{
			TR tR = TR.Load("Columns");
			foreach (ItemViewColumn item in itemViewColumnCollection)
			{
				ComicListField comicListField = (ComicListField)item.Tag;
				string displayProperty = comicListField.DisplayProperty;
				item.Text = tR[displayProperty, item.Text];
				comicListField.Description = tR[displayProperty + ".Description", comicListField.Description];
				for (int i = 0; i < item.FormatTexts.Length; i++)
				{
					item.FormatTexts[i] = tR[displayProperty + ".Format" + (i + 1), item.FormatTexts[i]];
				}
			}
		}
	}
}

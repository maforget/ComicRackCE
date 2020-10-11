using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using cYo.Common.Presentation.Ceco.Format;

namespace cYo.Common.Presentation.Ceco.Builders
{
	public static class XHtmlParser
	{
		public static FlowBlock Parse(string text)
		{
			return Parse(new StringReader("<content>" + text + "</content>"));
		}

		public static FlowBlock Parse(TextReader reader)
		{
			XmlReaderSettings settings = new XmlReaderSettings
			{
				IgnoreComments = true,
				IgnoreWhitespace = true
			};
			XmlReader reader2 = XmlReader.Create(reader, settings);
			return Parse(reader2);
		}

		public static FlowBlock Parse(XmlReader reader)
		{
			FlowBlock flowBlock = new FlowBlock();
			Parse(flowBlock, reader);
			return flowBlock;
		}

		private static void DefaultAttributes(Inline inline, IDictionary<string, string> attributes)
		{
			if (attributes == null)
			{
				return;
			}
			try
			{
				if (attributes.ContainsKey("align"))
				{
					inline.Align = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), attributes["align"], ignoreCase: true);
				}
			}
			catch
			{
			}
			try
			{
				if (attributes.ContainsKey("color"))
				{
					inline.ForeColor = ColorTranslator.FromHtml(attributes["color"]);
				}
			}
			catch
			{
			}
			try
			{
				if (attributes.ContainsKey("bgcolor"))
				{
					inline.BackColor = ColorTranslator.FromHtml(attributes["bgcolor"]);
				}
			}
			catch
			{
			}
		}

		private static void Parse(Span span, XmlReader reader)
		{
			while (reader.Read())
			{
				Span span2 = null;
				switch (reader.NodeType)
				{
				case XmlNodeType.Element:
				{
					Dictionary<string, string> dictionary = null;
					string text = reader.Name.ToLower();
					if (reader.HasAttributes)
					{
						while (reader.MoveToNextAttribute())
						{
							if (dictionary == null)
							{
								dictionary = new Dictionary<string, string>();
							}
							dictionary[reader.Name.ToLower()] = reader.Value;
						}
						reader.MoveToElement();
					}
					switch (text)
					{
					case "content":
						span2 = new TextFont(new FontSize(3, relative: false));
						break;
					case "a":
						span2 = new Anchor();
						if (dictionary != null && dictionary.ContainsKey("href"))
						{
							((Anchor)span2).HRef = dictionary["href"];
						}
						break;
					case "code":
					case "kbd":
					case "tt":
						span2 = new TextFont
						{
							FontFamily = "Courier New"
						};
						break;
					case "strong":
					case "b":
						span2 = new Bold();
						break;
					case "u":
						span2 = new Underline();
						break;
					case "samp":
					case "cite":
					case "em":
					case "i":
						span2 = new Italic();
						break;
					case "strike":
						span2 = new Strike();
						break;
					case "br":
						span2 = new LineBreak(0f);
						if (dictionary != null && dictionary.ContainsKey("clear"))
						{
							((LineBreak)span2).Clear = true;
						}
						break;
					case "p":
						span2 = new LineBreak(1f);
						break;
					case "pre":
					{
						TextFont textFont2 = new TextFont
						{
							FontFamily = "Courier New"
						};
						string text2 = reader.ReadString().Replace("\r\n", "\n");
						textFont2.Inlines.Add(new LineBreak());
						string[] array = text2.Split('\n');
						foreach (string text3 in array)
						{
							textFont2.Inlines.Add(new TextRun(text3));
							textFont2.Inlines.Add(new LineBreak(0f));
						}
						textFont2.Inlines.Add(new LineBreak(0f));
						span.Inlines.Add(textFont2);
						break;
					}
					case "big":
						span2 = new TextFont(new FontSize(1, relative: true));
						break;
					case "small":
						span2 = new TextFont(new FontSize(-1, relative: true));
						break;
					case "sub":
						span2 = new TextFont(0.7f, BaseAlignment.Bottom);
						break;
					case "sup":
						span2 = new TextFont(0.7f, BaseAlignment.Top);
						break;
					case "h1":
						span2 = new TextFont(6, FontStyle.Bold);
						break;
					case "h2":
						span2 = new TextFont(5, FontStyle.Bold);
						break;
					case "h3":
						span2 = new TextFont(4, FontStyle.Bold);
						break;
					case "h4":
						span2 = new TextFont(3, FontStyle.Bold);
						break;
					case "h5":
						span2 = new TextFont(2, FontStyle.Bold);
						break;
					case "h6":
						span2 = new TextFont(1, FontStyle.Bold);
						break;
					case "hr":
					{
						HorizontalRule horizontalRule = new HorizontalRule();
						span2 = horizontalRule;
						if (dictionary != null)
						{
							if (dictionary.ContainsKey("noshade"))
							{
								horizontalRule.Noshade = bool.Parse(dictionary["noshade"]);
							}
							if (dictionary.ContainsKey("size"))
							{
								horizontalRule.Thickness = int.Parse(dictionary["size"]);
							}
						}
						break;
					}
					case "center":
						span2 = new Alignment(HorizontalAlignment.Center);
						break;
					case "span":
						span2 = new Span();
						break;
					case "font":
						if (dictionary != null)
						{
							TextFont textFont = new TextFont();
							span2 = textFont;
							if (dictionary.ContainsKey("size"))
							{
								int num = int.Parse(dictionary["size"]);
								textFont.FontSize = new FontSize(num, dictionary["size"].StartsWith("+") || num < 0);
							}
							if (dictionary.ContainsKey("face"))
							{
								textFont.FontFamily = dictionary["face"];
							}
						}
						break;
					case "img":
					{
						if (dictionary == null)
						{
							break;
						}
						ImageItem imageItem = new ImageItem();
						span.Inlines.Add(imageItem);
						if (dictionary.ContainsKey("src"))
						{
							imageItem.Source = dictionary["src"];
						}
						if (dictionary.ContainsKey("width"))
						{
							imageItem.BlockWidth = new SizeValue(dictionary["width"]);
						}
						if (dictionary.ContainsKey("height"))
						{
							imageItem.BlockHeight = int.Parse(dictionary["height"]);
						}
						if (dictionary.ContainsKey("hspace"))
						{
							imageItem.Padding = new Size(int.Parse(dictionary["hspace"]), imageItem.Padding.Height);
						}
						if (dictionary.ContainsKey("vspace"))
						{
							imageItem.Padding = new Size(imageItem.Padding.Width, int.Parse(dictionary["vspace"]));
						}
						try
						{
							if (dictionary.ContainsKey("align"))
							{
								imageItem.VAlign = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), dictionary["align"], ignoreCase: true);
							}
						}
						catch
						{
						}
						DefaultAttributes(imageItem, dictionary);
						break;
					}
					case "table":
						span2 = new Table();
						if (dictionary != null)
						{
							Table table = (Table)span2;
							if (dictionary.ContainsKey("width"))
							{
								table.BlockWidth = new SizeValue(dictionary["width"]);
							}
							if (dictionary.ContainsKey("border"))
							{
								table.Border = int.Parse(dictionary["border"]);
							}
							if (dictionary.ContainsKey("cellpadding"))
							{
								table.CellPadding = int.Parse(dictionary["cellpadding"]);
							}
							if (dictionary.ContainsKey("cellspacing"))
							{
								table.CellSpacing = int.Parse(dictionary["cellspacing"]);
							}
							if (dictionary.ContainsKey("valign"))
							{
								table.VAlign = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), dictionary["valign"], ignoreCase: true);
							}
						}
						break;
					case "tr":
						span2 = new Table.Row();
						if (dictionary != null && dictionary.ContainsKey("valign"))
						{
							((Table.Row)span2).VAlign = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), dictionary["valign"], ignoreCase: true);
						}
						break;
					case "th":
					case "td":
						span2 = new Table.Cell();
						if (dictionary != null)
						{
							Table.Cell cell = (Table.Cell)span2;
							if (dictionary.ContainsKey("width"))
							{
								cell.BlockWidth = new SizeValue(dictionary["width"]);
							}
							if (dictionary.ContainsKey("height"))
							{
								cell.BlockHeight = int.Parse(dictionary["height"]);
							}
							if (dictionary.ContainsKey("valign"))
							{
								cell.VAlign = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), dictionary["valign"], ignoreCase: true);
							}
							if (dictionary.ContainsKey("colspan"))
							{
								cell.ColumSpan = int.Parse(dictionary["colspan"]);
							}
							if (dictionary.ContainsKey("rowspan"))
							{
								cell.RowSpan = int.Parse(dictionary["rowspan"]);
							}
						}
						break;
					}
					if (span2 != null)
					{
						DefaultAttributes(span2, dictionary);
						span.Inlines.Add(span2);
					}
					if (!reader.IsEmptyElement)
					{
						if (span2 == null)
						{
							Parse(span, reader);
						}
						else
						{
							Parse(span2, reader);
						}
					}
					break;
				}
				case XmlNodeType.EndElement:
					return;
				case XmlNodeType.Text:
					span.Inlines.Add(new TextRun(reader.Value.Replace("\n", " ")));
					break;
				}
			}
		}
	}
}

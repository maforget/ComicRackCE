using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using cYo.Common.Xml;

namespace cYo.Common.Drawing
{
	public class ColorSchemaCollection : List<ColorSchema>
	{
		public ColorSchema this[string name] => Find((ColorSchema item) => item.Name == name);

		public static ColorSchemaCollection Load(string file)
		{
			try
			{
				XmlSerializer serializer = XmlUtility.GetSerializer<ColorSchemaCollection>();
				using (StreamReader textReader = File.OpenText(file))
				{
					return (ColorSchemaCollection)serializer.Deserialize(textReader);
				}
			}
			catch (Exception)
			{
				return new ColorSchemaCollection();
			}
		}

		public bool Save(string file)
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ColorSchemaCollection));
				using (StreamWriter textWriter = File.CreateText(file))
				{
					xmlSerializer.Serialize(textWriter, this);
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}

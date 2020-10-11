using System;
using System.ComponentModel;
using System.Globalization;
using cYo.Common.Text;

namespace cYo.Common.Mathematics
{
	public class Vector4Converter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is Vector4)
			{
				string text = culture.TextInfo.ListSeparator + " ";
				Vector4 vector = (Vector4)value;
				return vector.X.ToString(culture) + text + vector.Y.ToString(culture) + text + vector.Z.ToString(culture) + text + vector.W.ToString(culture);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value.GetType() == typeof(string))
			{
				try
				{
					string s = (string)value;
					string[] array = s.Split(culture.TextInfo.ListSeparator, StringSplitOptions.None);
					return new Vector4(float.Parse(array[0], culture), float.Parse(array[1], culture), float.Parse(array[2], culture), float.Parse(array[3], culture));
				}
				catch
				{
					throw new ArgumentException("Invalid value: " + value);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}
	}
}

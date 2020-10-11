using System;
using System.ComponentModel;
using System.Globalization;

namespace cYo.Common.Mathematics
{
	public class Vector2Converter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is Vector2)
			{
				Vector2 vector = (Vector2)value;
				return vector.X.ToString(culture) + "; " + vector.Y.ToString(culture);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value.GetType() == typeof(string))
			{
				try
				{
					string text = (string)value;
					string[] array = text.Split(';');
					return new Vector2(float.Parse(array[0], culture), float.Parse(array[1], culture));
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

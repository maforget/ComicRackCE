using System;
using System.ComponentModel;
using System.Globalization;
using cYo.Common.Text;

namespace cYo.Common.Drawing
{
	public class BitmapAdjustmentConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				if (text == "Empty")
				{
					return BitmapAdjustment.Empty;
				}
				string[] array = text.Split(culture.TextInfo.ListSeparator, StringSplitOptions.RemoveEmptyEntries);
				return new BitmapAdjustment((array.Length != 0) ? float.Parse(array[0], culture) : 0f, (array.Length > 1) ? float.Parse(array[1], culture) : 0f, (array.Length > 2) ? float.Parse(array[2], culture) : 0f, (array.Length > 3) ? float.Parse(array[3], culture) : 0f);
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				BitmapAdjustment bitmapAdjustment = (BitmapAdjustment)value;
				return string.Format(culture, "{1}{0} {2}{0} {3}{0} {4}", culture.TextInfo.ListSeparator, bitmapAdjustment.Saturation, bitmapAdjustment.Brightness, bitmapAdjustment.Contrast, bitmapAdjustment.Gamma);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}

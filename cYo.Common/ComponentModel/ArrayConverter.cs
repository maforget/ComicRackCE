using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using cYo.Common.Text;

namespace cYo.Common.ComponentModel
{
	public class ArrayConverter<T> : TypeConverter
	{
		private readonly TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string) && tc.CanConvertFrom(context, sourceType))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string) && tc.CanConvertTo(context, destinationType))
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
				return (from x in text.Split(culture.TextInfo.ListSeparator, StringSplitOptions.RemoveEmptyEntries)
					select (T)tc.ConvertFrom(context, culture, x)).ToArray();
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			IEnumerable<T> enumerable = value as IEnumerable<T>;
			if (destinationType == typeof(string) && enumerable != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (T item in enumerable)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(culture.TextInfo.ListSeparator);
						stringBuilder.Append(" ");
					}
					stringBuilder.Append(tc.ConvertTo(context, culture, item, destinationType));
				}
				return stringBuilder.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}

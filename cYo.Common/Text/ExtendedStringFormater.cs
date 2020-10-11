using System;
using System.Collections.Generic;
using System.Text;

namespace cYo.Common.Text
{
	public static class ExtendedStringFormater
	{
		public static string Format(string format, Func<string, object> getValue)
		{
			bool success;
			return Format(format, getValue, out success);
		}

		public static string Format(string format, IDictionary<string, object> values)
		{
			return Format(format, (string s) => GetValue(values, s));
		}

		private static object GetValue(IDictionary<string, object> values, string key)
		{
			if (!values.TryGetValue(key, out var value))
			{
				return null;
			}
			return value;
		}

		private static string Format(string format, Func<string, object> getValue, out bool success)
		{
			StringBuilder stringBuilder = new StringBuilder();
			success = true;
			for (int i = 0; i < format.Length; i++)
			{
				char c = format[i];
				switch (c)
				{
				case '[':
				{
					bool success2;
					string value2 = Format(GetPart(format, ref i, '[', ']'), getValue, out success2);
					if (success2)
					{
						stringBuilder.Append(value2);
					}
					success |= success2;
					break;
				}
				case '{':
				{
					string value = FormatValue(GetPart(format, ref i, '{', '}'), getValue);
					success &= !string.IsNullOrEmpty(value);
					stringBuilder.Append(value);
					break;
				}
				case '\\':
					stringBuilder.Append(format[++i]);
					break;
				default:
					stringBuilder.Append(c);
					break;
				}
			}
			return stringBuilder.ToString();
		}

		private static string FormatValue(string format, Func<string, object> getValue)
		{
			string[] array = format.Split(':');
			object obj = getValue(array[0]);
			if (obj == null)
			{
				return string.Empty;
			}
			if (array.Length == 1)
			{
				return obj.ToString();
			}
			try
			{
				obj = Convert.ToDouble(obj);
			}
			catch
			{
			}
			try
			{
				return string.Format("{0:" + array[1] + "}", obj);
			}
			catch (Exception)
			{
				return obj.ToString();
			}
		}

		private static string GetPart(string text, ref int index, char openChar, char closeChar)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			bool flag = openChar == closeChar;
			while (index < text.Length)
			{
				char c = text[index];
				if (c == openChar && (!flag || num3 <= 0))
				{
					if (num3++ == 0)
					{
						num = index + 1;
					}
				}
				else if (c == closeChar && --num3 == 0)
				{
					num2 = index;
					break;
				}
				index++;
			}
			return text.Substring(num, num2 - num);
		}
	}
}

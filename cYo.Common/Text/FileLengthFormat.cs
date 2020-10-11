using System;

namespace cYo.Common.Text
{
	public class FileLengthFormat : IFormatProvider, ICustomFormatter
	{
		public object GetFormat(Type formatType)
		{
			return this;
		}

		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			long num;
			try
			{
				num = (long)arg;
			}
			catch (Exception innerException)
			{
				throw new ArgumentException($"The argument \"{arg}\" cannot be converted to an integer value.", innerException);
			}
			if (num < 1024)
			{
				return $"{num} Bytes";
			}
			if (num < 1048576)
			{
				return $"{(float)num / 1024f:.00} kB";
			}
			if (num < 1073741824)
			{
				return $"{(float)num / 1024f / 1024f:.00} MB";
			}
			return $"{(float)num / 1024f / 1024f / 1024f:.00} GB";
		}
	}
}

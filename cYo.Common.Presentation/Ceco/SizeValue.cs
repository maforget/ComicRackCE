using System;

namespace cYo.Common.Presentation.Ceco
{
	public struct SizeValue
	{
		public int Value;

		public bool IsPercent;

		public bool IsFixed => Value > 0;

		public bool IsAuto => Value == 0;

		public SizeValue(int value, bool isPercent)
		{
			Value = value;
			IsPercent = isPercent;
		}

		public SizeValue(string value)
		{
			string text = value.ToLower();
			IsPercent = false;
			if (text == "auto")
			{
				Value = 0;
			}
			else if (text.EndsWith("%"))
			{
				IsPercent = true;
				Value = int.Parse(text.Substring(0, text.Length - 1));
			}
			else
			{
				Value = int.Parse(text);
			}
		}

		public int GetSize(int size)
		{
			if (Value > 0)
			{
				return Math.Min(IsPercent ? (size * Value / 100) : Value, size);
			}
			return size;
		}

		public override bool Equals(object obj)
		{
			try
			{
				SizeValue sizeValue = (SizeValue)obj;
				return sizeValue.Value == Value && sizeValue.IsPercent == IsPercent;
			}
			catch
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode() ^ IsPercent.GetHashCode();
		}

		public static bool operator ==(SizeValue sv1, SizeValue sv2)
		{
			return object.Equals(sv1, sv2);
		}

		public static bool operator !=(SizeValue sv1, SizeValue sv2)
		{
			return !(sv1 == sv2);
		}
	}
}

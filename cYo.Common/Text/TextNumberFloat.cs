using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace cYo.Common.Text
{
	public class TextNumberFloat : IComparable<TextNumberFloat>
	{
		private float number;

		private string text;

		public float Number
		{
			get
			{
				return number;
			}
			protected set
			{
				IsNumber = true;
				number = value;
			}
		}

		public bool IsNumber
		{
			get;
			private set;
		}

		public string Text
		{
			get
			{
				return text;
			}
			private set
			{
				if (!(text == value))
				{
					text = value.Trim();
					IsNumber = false;
					OnParseText(text);
				}
			}
		}

		public TextNumberFloat()
		{
		}

		public TextNumberFloat(string text)
			: this()
		{
			Text = text;
		}

		public int CompareTo(TextNumberFloat other)
		{
			int num = 0;
			if (IsNumber && other.IsNumber)
			{
				num = Number.CompareTo(other.Number);
			}
			if (num == 0)
			{
				num = string.Compare(Text, other.Text, StringComparison.Ordinal);
			}
			return num;
		}

		protected virtual void OnParseText(string s)
		{
			if (s.TryParse(out float f, invariant: true))
			{
				Number = f;
			}
		}

		public static bool Parse(string text, int start, out float accu, out int n)
		{
			bool flag = false;
			int num = 1;
			int num2 = 10;
			int num3 = 1;
			accu = 0f;
			for (n = start; n < text.Length; n++)
			{
				char c = text[n];
				switch (c)
				{
				case ' ':
					if (flag)
					{
						accu *= num;
						return true;
					}
					break;
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					flag = true;
					accu = accu * (float)num2 + (float)(c - 48) / (float)num3;
					if (num3 != 1)
					{
						num3 *= 10;
					}
					break;
				case ',':
				case '.':
					flag = true;
					if (num3 == 1)
					{
						num2 = 1;
						num3 = 10;
					}
					break;
				case '-':
					if (flag)
					{
						accu *= num;
						return true;
					}
					num *= -1;
					break;
				case '+':
					if (flag)
					{
						accu *= num;
						return true;
					}
					num = 1;
					break;
				case '¼':
					flag = true;
					accu += 0.25f;
					break;
				case '½':
					flag = true;
					accu += 0.5f;
					break;
				case '¾':
					flag = true;
					accu += 0.75f;
					break;
				default:
					accu *= num;
					return flag;
				}
			}
			accu *= num;
			return flag;
		}

		public static bool ParseExpression(string text, int start, out float accu, out int n)
		{
			char c = '+';
			bool result = false;
			accu = 0f;
			for (n = start; n < text.Length; n++)
			{
				if (!Parse(text, n, out var accu2, out var n2))
				{
					return result;
				}
				n = n2;
				result = true;
				switch (c)
				{
				case '/':
					accu /= accu2;
					break;
				case '+':
					accu += accu2;
					break;
				case '-':
					accu -= accu2;
					break;
				case '*':
					accu *= accu2;
					break;
				}
				if (n < text.Length)
				{
					c = text[n];
					if (!"*/+-".Contains(c))
					{
						return true;
					}
				}
			}
			return result;
		}

		public static bool TryParseExpression(string text, out float accu)
		{
			int n;
			return ParseExpression(text, 0, out accu, out n);
		}

		public static float Parse(string text)
		{
			if (!TryParseExpression(text, out var accu))
			{
				throw new FormatException();
			}
			return accu;
		}

        public IEnumerable<float> GetRange()
        {
            Regex regex = new Regex(@"(\d+)[ -]+(\d+)", RegexOptions.IgnoreCase);
            Match match = regex.Match(this.Text ?? string.Empty);

            if (match.Success)
            {
                bool ba = match.Groups[1].Value.TryParse(out float a, true);
                bool bb = match.Groups[2].Value.TryParse(out float b, true);

                if ((ba && bb) && (b > a))
                {
					return RangeF.ToEnumerable(a, b, 1.0f);
                }
            }
            return Enumerable.Empty<float>();
        }
    }
}

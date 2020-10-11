using System.Text;

namespace cYo.Common.Text
{
	public static class Base32
	{
		private static readonly char[] Base32Chars = new char[32]
		{
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7'
		};

		public static string ToBase32String(byte[] inArray)
		{
			if (inArray == null)
			{
				return null;
			}
			int num = inArray.Length;
			int num2 = num / 5;
			int num3 = num - 5 * num2;
			StringBuilder stringBuilder = new StringBuilder();
			int num4 = 0;
			for (int i = 0; i < num2; i++)
			{
				byte b = inArray[num4++];
				byte b2 = inArray[num4++];
				byte b3 = inArray[num4++];
				byte b4 = inArray[num4++];
				byte b5 = inArray[num4++];
				stringBuilder.Append(Base32Chars[b >> 3]);
				stringBuilder.Append(Base32Chars[((b << 2) & 0x1F) | (b2 >> 6)]);
				stringBuilder.Append(Base32Chars[(b2 >> 1) & 0x1F]);
				stringBuilder.Append(Base32Chars[((b2 << 4) & 0x1F) | (b3 >> 4)]);
				stringBuilder.Append(Base32Chars[((b3 << 1) & 0x1F) | (b4 >> 7)]);
				stringBuilder.Append(Base32Chars[(b4 >> 2) & 0x1F]);
				stringBuilder.Append(Base32Chars[((b4 << 3) & 0x1F) | (b5 >> 5)]);
				stringBuilder.Append(Base32Chars[b5 & 0x1F]);
			}
			if (num3 > 0)
			{
				byte b6 = inArray[num4++];
				stringBuilder.Append(Base32Chars[b6 >> 3]);
				switch (num3)
				{
				case 1:
					stringBuilder.Append(Base32Chars[(b6 << 2) & 0x1F]);
					break;
				case 2:
				{
					byte b7 = inArray[num4];
					stringBuilder.Append(Base32Chars[((b6 << 2) & 0x1F) | (b7 >> 6)]);
					stringBuilder.Append(Base32Chars[(b7 >> 1) & 0x1F]);
					stringBuilder.Append(Base32Chars[(b7 << 4) & 0x1F]);
					break;
				}
				case 3:
				{
					byte b7 = inArray[num4++];
					byte b8 = inArray[num4];
					stringBuilder.Append(Base32Chars[((b6 << 2) & 0x1F) | (b7 >> 6)]);
					stringBuilder.Append(Base32Chars[(b7 >> 1) & 0x1F]);
					stringBuilder.Append(Base32Chars[((b7 << 4) & 0x1F) | (b8 >> 4)]);
					stringBuilder.Append(Base32Chars[(b8 << 1) & 0x1F]);
					break;
				}
				case 4:
				{
					byte b7 = inArray[num4++];
					byte b8 = inArray[num4++];
					byte b9 = inArray[num4];
					stringBuilder.Append(Base32Chars[((b6 << 2) & 0x1F) | (b7 >> 6)]);
					stringBuilder.Append(Base32Chars[(b7 >> 1) & 0x1F]);
					stringBuilder.Append(Base32Chars[((b7 << 4) & 0x1F) | (b8 >> 4)]);
					stringBuilder.Append(Base32Chars[((b8 << 1) & 0x1F) | (b9 >> 7)]);
					stringBuilder.Append(Base32Chars[(b9 >> 2) & 0x1F]);
					stringBuilder.Append(Base32Chars[(b9 << 3) & 0x1F]);
					break;
				}
				}
			}
			return stringBuilder.ToString();
		}
	}
}

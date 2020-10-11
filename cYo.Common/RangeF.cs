namespace cYo.Common
{
	public struct RangeF
	{
		public float Start;

		public float Length;

		public static readonly RangeF Empty;

		public float End => Start + Length;

		public bool IsEmpty
		{
			get
			{
				if (Start == 0f)
				{
					return Length == 0f;
				}
				return false;
			}
		}

		public RangeF(float start = 0f, float length = 0f)
		{
			Start = start;
			Length = length;
		}
	}
}

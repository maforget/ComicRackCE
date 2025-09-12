using System.Collections.Generic;

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

        public static IEnumerable<float> ToEnumerable(float from, float to, float step = 1.0f)
        {
            if (step <= 0.0f) 
				step = (step == 0.0f) ? 1.0f : -step;

            if (from <= to)
                for (float d = from; d <= to; d += step) yield return d;
            else
                for (float d = from; d >= to; d -= step) yield return d;
        }
    }
}

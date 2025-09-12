using System;
using System.Collections.Generic;

namespace cYo.Common
{
	public static class BitUtility
	{
		public static int SetMask(this int n, int mask, bool set = true)
		{
			if (!set)
			{
				return n & ~mask;
			}
			return n | mask;
		}

		public static int Clear(this int n, int mask)
		{
			return n.SetMask(mask, set: false);
		}

		public static int Flip(this int n, int mask)
		{
			return n.SetMask(mask, !n.IsSet(mask));
		}

		public static T SetMask<T>(this Enum n, T mask, bool set = true)
		{
			int num = Convert.ToInt32(n);
			int num2 = Convert.ToInt32(mask);
			return (T)(object)(set ? (num | num2) : (num & ~num2));
		}

		public static T Clear<T>(this Enum n, T mask)
		{
			return n.SetMask(mask, set: false);
		}

		public static T Flip<T>(this Enum n, T mask)
		{
			return n.SetMask(mask, !n.IsSet(mask));
		}

		public static bool IsSet(this int n, int mask, bool all)
		{
			if (all)
			{
				return (n & mask) == mask;
			}
			return (n & mask) != 0;
		}

		public static bool IsSet(this int n, int mask)
		{
			return n.IsSet(mask, all: true);
		}

		public static bool IsNotSet(this int n, int mask)
		{
			return !n.IsSet(mask);
		}

		public static bool IsSet<T>(this Enum n, T mask, bool all = true)
		{
			int num = Convert.ToInt32(n);
			int num2 = Convert.ToInt32(mask);
			if (all)
			{
				return (num & num2) == num2;
			}
			return (num & num2) != 0;
		}

		public static bool IsNotSet<T>(this Enum n, T mask, bool all = true)
		{
			return !n.IsSet(mask, all);
		}

		public static int GetBitCount(int n)
		{
			int num = 0;
			for (int i = 0; i < 32; i++)
			{
				if (n == 0)
				{
					break;
				}
				if (((uint)n & (true ? 1u : 0u)) != 0)
				{
					num++;
				}
				n >>= 1;
			}
			return num;
		}

		public static IEnumerable<int> GetBitValues(int n, bool set = true)
		{
			for (int i = 0; i < 32; i++)
			{
				int num = 1 << i;
				bool flag = (n & num) != 0;
				if (flag == set)
				{
					yield return num;
				}
			}
		}

		public static int EndianSwap(this int x)
		{
			return ((x >> 24) & 0xFF) | ((x << 8) & 0xFF0000) | ((x >> 8) & 0xFF00) | (x << 24);
		}

		public static long EndianSwap(this long x)
		{
			return ((x >> 56) & 0xFF) | ((x << 40) & 0xFF000000000000L) | ((x << 24) & 0xFF0000000000L) | ((x << 8) & 0xFF00000000L) | ((x >> 8) & 0xFF000000u) | ((x >> 24) & 0xFF0000) | ((x >> 40) & 0xFF00) | (x << 56);
		}

		public static int HiInt(this long x)
		{
			return (int)(x >> 32);
		}

		public static int LoInt(this long x)
		{
			return (int)(x & 0xFFFFFFFFu);
		}

		public static int CreateMask(params bool[] flags)
		{
			int num = 0;
			foreach (bool flag in flags)
			{
				num <<= 1;
				if (flag)
				{
					num |= 1;
				}
			}
			return num;
		}
	}
}

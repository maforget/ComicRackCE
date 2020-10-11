using System;
using cYo.Common.Localize;

namespace cYo.Common
{
	public static class DateTimeExtensions
	{
		private static Lazy<string[]> relativeFormat = new Lazy<string[]>(() => TR.Load("Common").GetStrings("RelativeDateTimeFormat", "minute|minutes|hour|hours|day|days|week|weeks|month|months|year|years|{0} ago|in {0}", '|'));

		public static bool IsDateOnly(this DateTime dt)
		{
			if (dt.Hour == 0 && dt.Minute == 0 && dt.Second == 0)
			{
				return dt.Millisecond == 0;
			}
			return false;
		}

		public static DateTime DateOnly(this DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, dt.Day);
		}

		public static int CompareTo(this DateTime dt, DateTime value, bool ignoreTime)
		{
			if (!ignoreTime)
			{
				return dt.CompareTo(value);
			}
			int num = Math.Sign(dt.Year - value.Year);
			if (num != 0)
			{
				return num;
			}
			num = Math.Sign(dt.Month - value.Month);
			if (num != 0)
			{
				return num;
			}
			return Math.Sign(dt.Day - value.Day);
		}

		public static DateTime SafeToLocalTime(this DateTime dt)
		{
			if (!(dt == DateTime.MinValue))
			{
				return dt.ToLocalTime();
			}
			return dt;
		}

		public static string ToRelativeDateString(this DateTime dt, DateTime toDate)
		{
			string[] value = relativeFormat.Value;
			string format;
			if (dt > toDate)
			{
				CloneUtility.Swap(ref dt, ref toDate);
				format = value[value.Length - 1];
			}
			else
			{
				format = value[value.Length - 2];
			}
			TimeSpan timeSpan = toDate - dt;
			int num = (int)timeSpan.TotalMinutes;
			int num2 = (int)timeSpan.TotalHours;
			int num3 = (int)timeSpan.TotalDays;
			int num4 = num3 / 7;
			int num5 = num4 / 4;
			int num6 = num4 / 52;
			string arg;
			if (num <= 1)
			{
				arg = "1 " + value[0];
			}
			else
			{
				switch (num2)
				{
				case 0:
					arg = num + " " + value[1];
					break;
				case 1:
					arg = "1 " + value[2];
					break;
				default:
					switch (num3)
					{
					case 0:
						arg = num2 + " " + value[3];
						break;
					case 1:
						arg = "1 " + value[4];
						break;
					default:
						switch (num4)
						{
						case 0:
							arg = num3 + " " + value[5];
							break;
						case 1:
							arg = "1 " + value[6];
							break;
						default:
							switch (num5)
							{
							case 0:
								arg = num4 + " " + value[7];
								break;
							case 1:
								arg = "1 " + value[8];
								break;
							default:
								switch (num6)
								{
								case 0:
									arg = num5 + " " + value[9];
									break;
								case 1:
									arg = "1 " + value[10];
									break;
								default:
									arg = num6 + " " + value[11];
									break;
								}
								break;
							}
							break;
						}
						break;
					}
					break;
				}
			}
			return string.Format(format, arg);
		}
	}
}

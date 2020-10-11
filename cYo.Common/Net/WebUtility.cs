using System;

namespace cYo.Common.Net
{
	public static class WebUtility
	{
		public static string UrlEncode(string text)
		{
			return Uri.EscapeDataString(text);
		}
	}
}

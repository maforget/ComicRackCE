using System;
using System.Collections.Generic;
using System.IO;

namespace cYo.Common.IO
{
	public class DriveChecker
	{
		private readonly Dictionary<string, bool> cache = new Dictionary<string, bool>();

		public bool IsConnected(string path)
		{
			try
			{
				string text = Path.GetPathRoot(path).ToLower();
				if (!cache.TryGetValue(text, out var value))
				{
					try
					{
						value = Directory.Exists(text);
					}
					catch
					{
						value = false;
					}
					cache[text] = value;
				}
				return value;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}

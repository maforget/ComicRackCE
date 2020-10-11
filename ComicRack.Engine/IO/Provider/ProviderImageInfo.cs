using System;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	[Serializable]
	public class ProviderImageInfo : IComparable<ProviderImageInfo>
	{
		public int Index
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public long Size
		{
			get;
			set;
		}

		public ProviderImageInfo()
		{
		}

		public ProviderImageInfo(int index)
		{
			Index = index;
		}

		public ProviderImageInfo(int index, string name, long size)
		{
			Index = index;
			Name = name;
			Size = size;
		}

		public int CompareTo(ProviderImageInfo other)
		{
			if (other == null)
			{
				return 1;
			}
			return string.Compare(Name, other.Name);
		}
	}
}

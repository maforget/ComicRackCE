namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class ItemSizeInfo
	{
		public int Minimum
		{
			get;
			set;
		}

		public int Maximum
		{
			get;
			set;
		}

		public int Value
		{
			get;
			set;
		}

		public ItemSizeInfo(int min, int max, int value)
		{
			Minimum = min;
			Maximum = max;
			Value = value;
		}
	}
}

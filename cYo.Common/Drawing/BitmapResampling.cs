namespace cYo.Common.Drawing
{
	public enum BitmapResampling
	{
		FastAndUgly = 0,
		FastBilinear = 1,
		FastBicubic = 2,
		BilinearHQ = 3,
		GdiPlus = 4,
		GdiPlusHQ = 5,
		Default = 3,
		BestQuality = 5
	}
}

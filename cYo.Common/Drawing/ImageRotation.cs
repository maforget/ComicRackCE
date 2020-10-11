using System.ComponentModel;

namespace cYo.Common.Drawing
{
	public enum ImageRotation : byte
	{
		None,
		[Description("90°")]
		Rotate90,
		[Description("180°")]
		Rotate180,
		[Description("270°")]
		Rotate270
	}
}

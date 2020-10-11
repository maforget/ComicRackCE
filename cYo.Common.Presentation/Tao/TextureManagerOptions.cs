using System;

namespace cYo.Common.Presentation.Tao
{
	[Flags]
	public enum TextureManagerOptions
	{
		None = 0x0,
		MipMapFilter = 0x1,
		AnisotropicFilter = 0x2,
		TextureCompression = 0x4,
		SquareTextures = 0x8,
		BigTexturesAs16Bit = 0x10,
		BigTexturesAs24Bit = 0x20,
		Default = 0x1
	}
}

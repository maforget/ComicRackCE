using System;
using System.ComponentModel;
using cYo.Common.Mathematics;
using cYo.Common.Runtime;

namespace cYo.Common.Presentation.Tao
{
	[Serializable]
	public class TextureManagerSettings : ICloneable
	{
		[CommandLineSwitch(ShortName = "hwmtm")]
		[DefaultValue(1024)]
		public int MaxTextureMemoryMB
		{
			get;
			set;
		}

		public bool IsMaxTextureMemoryMBDefault => MaxTextureMemoryMB == 1024;

		[CommandLineSwitch(ShortName = "hwmtc")]
		[DefaultValue(1024)]
		public int MaxTextureCount
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "hwmttsa")]
		[DefaultValue(16192)]
		public int MaxTextureTileSizeArbitrary
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "hwmttss")]
		[DefaultValue(512)]
		public int MaxTextureTileSizeSquare
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "hwmtts")]
		[DefaultValue(16)]
		public int MinTextureTileSize
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "hwo")]
		[DefaultValue(TextureManagerOptions.MipMapFilter)]
		public TextureManagerOptions TextureManagerOptions
		{
			get;
			set;
		}

		public bool IsTextureManagerOptionsDefault => TextureManagerOptions == TextureManagerOptions.MipMapFilter;

		public bool MipMapping
		{
			get
			{
				return (TextureManagerOptions & TextureManagerOptions.MipMapFilter) != 0;
			}
			set
			{
				TextureManagerOptions = TextureManagerOptions.SetMask(TextureManagerOptions.MipMapFilter, value);
			}
		}

		public int MaxTextureTileSize
		{
			get
			{
				if (!IsSquareTextures)
				{
					return MaxTextureTileSizeArbitrary;
				}
				return MaxTextureTileSizeSquare;
			}
		}

		public bool IsMipMapFilter => (TextureManagerOptions & TextureManagerOptions.MipMapFilter) != 0;

		public bool IsAnisotropicFilter => (TextureManagerOptions & TextureManagerOptions.AnisotropicFilter) != 0;

		public bool IsSquareTextures => (TextureManagerOptions & TextureManagerOptions.SquareTextures) != 0;

		public bool IsTextureCompression => (TextureManagerOptions & TextureManagerOptions.TextureCompression) != 0;

		public bool IsBigTexturesAs16Bit => (TextureManagerOptions & TextureManagerOptions.BigTexturesAs16Bit) != 0;

		public bool IsBigTexturesAs24Bit => (TextureManagerOptions & TextureManagerOptions.BigTexturesAs24Bit) != 0;

		public TextureManagerSettings()
		{
			MaxTextureMemoryMB = 1024;
			MaxTextureCount = 1024;
			MaxTextureTileSizeSquare = 512;
			MaxTextureTileSizeArbitrary = 16192;
			MinTextureTileSize = 16;
			TextureManagerOptions = TextureManagerOptions.MipMapFilter;
		}

		public void Validate()
		{
			MaxTextureCount = MaxTextureCount.Clamp(64, 1024);
			MaxTextureMemoryMB = MaxTextureMemoryMB.Clamp(16, 8192);
			MaxTextureTileSizeArbitrary = MaxTextureTileSizeArbitrary.Clamp(64, OpenGlInfo.MaxTextureSize);
			MaxTextureTileSizeSquare = MaxTextureTileSizeSquare.Clamp(64, OpenGlInfo.MaxTextureSize);
			if (!OpenGlInfo.SupportsNonPower2Textures)
			{
				TextureManagerOptions |= TextureManagerOptions.SquareTextures;
			}
			if (!OpenGlInfo.SupportsAnisotopricFilter)
			{
				TextureManagerOptions &= ~TextureManagerOptions.AnisotropicFilter;
			}
			if (!OpenGlInfo.SupportsTextureCompression)
			{
				TextureManagerOptions &= ~TextureManagerOptions.TextureCompression;
			}
		}

		public object Clone()
		{
			return CloneUtility.Clone(this);
		}
	}
}

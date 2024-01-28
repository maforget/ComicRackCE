//using Tao.OpenGl;
using OpenTK.Graphics.OpenGL;

namespace cYo.Common.Presentation.Tao
{
	public static class OpenGlInfo
	{
		private static int maxTextureSize;

		private static float version;

		private static bool? supportsAnisotropicFilter;

		private static bool? supportsNonPower2Textures;

		private static bool? supportsTextureCompression;

		public static int MaxTextureSize
		{
			get
			{
				if (maxTextureSize == 0)
				{
                    maxTextureSize = GL.GetInteger(GetPName.MaxTextureSize);
				}
				return maxTextureSize;
			}
		}

		public static float Version
		{
			get
			{
				if (version == 0f)
				{
					try
					{
						string text = GL.GetString(StringName.Version).Trim();
						version = float.Parse(text.Substring(0, 1)) + float.Parse(text.Substring(2, 1)) / 10f;
					}
					catch
					{
						version = 1f;
					}
				}
				return version;
			}
		}

		public static bool SupportsAnisotopricFilter
		{
			get
			{
				if (!supportsAnisotropicFilter.HasValue)
				{
                    supportsAnisotropicFilter = GL.GetString(StringName.Extensions).Contains("GL_EXT_texture_filter_anisotropic");
				}
				return supportsAnisotropicFilter.Value;
			}
		}

		public static bool SupportsNonPower2Textures
		{
			get
			{
				if (!supportsNonPower2Textures.HasValue)
				{
                    supportsNonPower2Textures = GL.GetString(StringName.Extensions).Contains("ARB_texture_non_power_of_two");
				}
				return supportsNonPower2Textures.Value;
			}
		}

		public static bool SupportsTextureCompression
		{
			get
			{
				if (!supportsTextureCompression.HasValue)
				{
                    supportsTextureCompression = GL.GetString(StringName.Extensions).Contains("GL_ARB_texture_compression");
				}
				return supportsTextureCompression.Value;
			}
		}
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using cYo.Common.Mathematics;

namespace cYo.Common.Drawing
{
	[Serializable]
	[TypeConverter(typeof(BitmapAdjustmentConverter))]
	public struct BitmapAdjustment
	{
		public static readonly BitmapAdjustment Empty = new BitmapAdjustment(0f);

		[DefaultValue(0f)]
		public float Saturation
		{
			get;
			set;
		}

		[DefaultValue(0f)]
		public float Contrast
		{
			get;
			set;
		}

		[DefaultValue(0f)]
		public float Brightness
		{
			get;
			set;
		}

		[DefaultValue(0f)]
		public float Gamma
		{
			get;
			set;
		}

		[XmlIgnore]
		public Color WhitePointColor
		{
			get;
			set;
		}

		[DefaultValue(0)]
		public int WhitePointArgb
		{
			get
			{
				int num = WhitePointColor.ToArgb();
				if (num != 0 && num != -1)
				{
					return num;
				}
				return 0;
			}
			set
			{
				WhitePointColor = Color.FromArgb(value);
			}
		}

		[DefaultValue(BitmapAdjustmentOptions.None)]
		public BitmapAdjustmentOptions Options
		{
			get;
			set;
		}

		[DefaultValue(0)]
		public int Sharpen
		{
			get;
			set;
		}

		public bool HasColorTransformations
		{
			get
			{
				if (EpsTest(Contrast, 0f) && EpsTest(Saturation, 0f) && EpsTest(Brightness, 0f))
				{
					return !WhitePointColor.IsBlackOrWhite();
				}
				return true;
			}
		}

		public bool HasAutoContrast => (Options & BitmapAdjustmentOptions.AutoContrast) != 0;

		public bool HasSharpening => Sharpen != 0;

		public bool HasGamma => !EpsTest(Gamma, 0f);

		public bool IsEmpty
		{
			get
			{
				if (!HasColorTransformations && Options == BitmapAdjustmentOptions.None)
				{
					return Sharpen == 0;
				}
				return false;
			}
		}

		public BitmapAdjustment(float saturation, float brightness, float contrast, float gamma, Color whitePoint, BitmapAdjustmentOptions options = BitmapAdjustmentOptions.None, int sharpen = 0)
		{
			this = default(BitmapAdjustment);
			Brightness = brightness;
			Contrast = contrast;
			Saturation = saturation;
			Gamma = gamma;
			WhitePointColor = whitePoint;
			Options = options;
			Sharpen = sharpen;
		}

		public BitmapAdjustment(float saturation, float brightness, float contrast, float gamma, BitmapAdjustmentOptions options, int sharpen)
			: this(saturation, brightness, contrast, gamma, Color.White, options, sharpen)
		{
		}

		public BitmapAdjustment(Color whitePoint)
			: this(0f, 0f, 0f, 0f, whitePoint)
		{
		}

		public BitmapAdjustment(float saturation, float brightness = 0f, float contrast = 0f, float gamma = 0f)
			: this(saturation, brightness, contrast, gamma, Color.White)
		{
		}

		public BitmapAdjustment ChangeSaturation(float saturation)
		{
			Saturation = saturation;
			return this;
		}

		public BitmapAdjustment ChangeBrightness(float brightness)
		{
			Brightness = brightness;
			return this;
		}

		public BitmapAdjustment ChangeContrast(float contrast)
		{
			Contrast = contrast;
			return this;
		}

		public BitmapAdjustment ChangeGamma(float gamma)
		{
			Gamma = gamma;
			return this;
		}

		public BitmapAdjustment ChangeWhitepoint(Color whitePoint)
		{
			WhitePointColor = whitePoint;
			return this;
		}

		public BitmapAdjustment ChangeOption(BitmapAdjustmentOptions options)
		{
			Options = options;
			return this;
		}

		public BitmapAdjustment ChangeSharpness(int sharpness)
		{
			Sharpen = sharpness;
			return this;
		}

		public override bool Equals(object compare)
		{
			if (!(compare is BitmapAdjustment))
			{
				return false;
			}
			BitmapAdjustment bitmapAdjustment = (BitmapAdjustment)compare;
			if (Saturation == bitmapAdjustment.Saturation && Brightness == bitmapAdjustment.Brightness && Contrast == bitmapAdjustment.Contrast && Gamma == bitmapAdjustment.Gamma && WhitePointArgb == bitmapAdjustment.WhitePointArgb && Options == bitmapAdjustment.Options)
			{
				return Sharpen == bitmapAdjustment.Sharpen;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Saturation.GetHashCode() ^ (Contrast * 10f).GetHashCode() ^ (Brightness * 100f).GetHashCode() ^ (Gamma * 10000f).GetHashCode() ^ WhitePointArgb.GetHashCode() ^ Options.GetHashCode() ^ (Sharpen.GetHashCode() << 4);
		}

		public override string ToString()
		{
			TypeConverter converter = TypeDescriptor.GetConverter(this);
			return converter.ConvertToString(this);
		}

		public static bool operator ==(BitmapAdjustment a, BitmapAdjustment b)
		{
			return object.Equals(a, b);
		}

		public static bool operator !=(BitmapAdjustment a, BitmapAdjustment b)
		{
			return !(a == b);
		}

		private static bool EpsTest(float f, float t)
		{
			return f.CompareTo(t, 0.01f);
		}

		public static BitmapAdjustment Add(BitmapAdjustment c1, BitmapAdjustment c2)
		{
			Color whitePoint = (c1.WhitePointColor.IsBlackOrWhite() ? c2.WhitePointColor : c1.WhitePointColor);
			return new BitmapAdjustment(c1.Saturation + c2.Saturation, c1.Brightness + c2.Brightness, c1.Contrast + c2.Contrast, c1.Gamma + c2.Gamma, whitePoint, c1.Options | c2.Options, Math.Max(c1.Sharpen, c2.Sharpen));
		}

		public static BitmapAdjustment Parse(string text)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(BitmapAdjustment));
			return (BitmapAdjustment)converter.ConvertFromString(text);
		}
	}
}

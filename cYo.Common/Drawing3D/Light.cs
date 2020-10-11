using System;
using System.Drawing;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;

namespace cYo.Common.Drawing3D
{
	public class Light
	{
		public LightType LightType
		{
			get;
			set;
		}

		public Vector3 Position
		{
			get;
			set;
		}

		public Vector3 Direction
		{
			get;
			set;
		}

		public bool DistanceFallOff
		{
			get;
			set;
		}

		public ColorF DiffuseColor
		{
			get;
			set;
		}

		public float DiffusePower
		{
			get;
			set;
		}

		public ColorF SpecularColor
		{
			get;
			set;
		}

		public float SpecularPower
		{
			get;
			set;
		}

		public float SpecularHardness
		{
			get;
			set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public Light()
		{
			DiffuseColor = Color.White;
			DiffusePower = 1f;
			SpecularPower = 0f;
			SpecularHardness = 0.3f;
			DistanceFallOff = false;
			Enabled = true;
		}

		public ColorF Calculate(Vector3 position, Vector3 viewDirection, Vector3 surfaceNormal)
		{
			switch (LightType)
			{
			case LightType.Point:
				return CalclatePointLight(this, position, viewDirection, surfaceNormal).Diffuse;
			default:
				return CalculateDirectionalLight(this, -Direction, 1f, viewDirection, surfaceNormal).Diffuse;
			}
		}

		public static LightingResult CalclatePointLight(Light light, Vector3 position, Vector3 viewDirection, Vector3 surfaceNormal)
		{
			Vector3 lightDirection = light.Position - position;
			float lightDistance = lightDirection.Length();
			return CalculateDirectionalLight(light, lightDirection, lightDistance, viewDirection, surfaceNormal);
		}

		public static LightingResult CalculateDirectionalLight(Light light, Vector3 lightDirection, float lightDistance, Vector3 viewDirection, Vector3 surfaceNormal)
		{
			LightingResult result = default(LightingResult);
			if (light.DiffusePower <= 0f)
			{
				return result;
			}
			lightDirection.Normalize();
			surfaceNormal.Normalize();
			lightDistance = ((!light.DistanceFallOff) ? 1f : (lightDistance * lightDistance));
			float num = Vector3.Dot(lightDirection, surfaceNormal).Clamp(0f, 1f);
			result.Diffuse = light.DiffuseColor * (num * light.DiffusePower / lightDistance);
			Vector3 a = 2f * Vector3.Dot(lightDirection, surfaceNormal) * surfaceNormal - lightDirection;
			a.Normalize();
			num = (float)Math.Pow(Vector3.Dot(a, viewDirection).Clamp(0f, 1f), light.SpecularHardness);
			result.Specular = light.SpecularColor * (num * light.SpecularPower / lightDistance);
			return result;
		}
	}
}

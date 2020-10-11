using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;

namespace cYo.Common.Drawing3D
{
	public class CyoGl
	{
		private Stack<Matrix4> modelMatrixStack = new Stack<Matrix4>();

		private List<Light> lights = new List<Light>();

		public RectangleF Viewport
		{
			get;
			set;
		}

		public IFrameBuffer FrameBuffer
		{
			get;
			set;
		}

		public Matrix4 Projection
		{
			get;
			set;
		}

		public Matrix4 ModelView
		{
			get;
			set;
		}

		public ITexture Texture
		{
			get;
			set;
		}

		public bool BacksideCulling
		{
			get;
			set;
		}

		public ColorF AmbientLight
		{
			get;
			set;
		}

		public List<Light> Lights => lights;

		public ShadingModel ShadingModel
		{
			get;
			set;
		}

		public bool Wireless
		{
			get;
			set;
		}

		public CyoGl()
		{
			Viewport = new Rectangle(-1, -1, 2, 2);
			Projection = Matrix4.Identity;
			ModelView = Matrix4.Identity;
			AmbientLight = new ColorF(0.1f);
			ShadingModel = ShadingModel.Flat;
		}

		public void PushMatrix()
		{
			modelMatrixStack.Push(ModelView);
		}

		public void PopMatrix()
		{
			ModelView = modelMatrixStack.Pop();
		}

		public Vector4 GetPlaneVector(Vector3 v1, Vector3 v2, Vector3 v3)
		{
			Vector4 vector = GetSurfaceNormal(v1, v2, v3, ModelView).Vector4;
			vector[3] = vector[0] * v1[0] + vector[1] * v1[1] + vector[2] * v1[2];
			return vector;
		}

		public Matrix4 GetShadowMatrix(Vector4 plane, Vector4 lightPosition)
		{
			Matrix4 result = default(Matrix4);
			float num = Vector4.Dot(plane, lightPosition);
			result[0, 0] = num - lightPosition[0] * plane[0];
			result[0, 1] = (0f - lightPosition[0]) * plane[1];
			result[0, 2] = (0f - lightPosition[0]) * plane[2];
			result[0, 3] = (0f - lightPosition[0]) * plane[3];
			result[1, 0] = (0f - lightPosition[1]) * plane[0];
			result[1, 1] = num - lightPosition[1] * plane[1];
			result[1, 2] = (0f - lightPosition[1]) * plane[2];
			result[1, 3] = (0f - lightPosition[1]) * plane[3];
			result[2, 0] = (0f - lightPosition[2]) * plane[0];
			result[2, 1] = (0f - lightPosition[2]) * plane[1];
			result[2, 2] = num - lightPosition[2] * plane[2];
			result[2, 3] = (0f - lightPosition[2]) * plane[3];
			result[3, 0] = (0f - lightPosition[3]) * plane[0];
			result[3, 1] = (0f - lightPosition[3]) * plane[1];
			result[3, 2] = (0f - lightPosition[3]) * plane[2];
			result[3, 3] = num - lightPosition[3] * plane[3];
			return result;
		}

		public void DrawTriangle(Vertex p1, Vertex p2, Vertex p3)
		{
			if (!BacksideCulling || !IsBackside(p1, p2, p3))
			{
				Vertex vertex = new Vertex(p1);
				Vertex vertex2 = new Vertex(p2);
				Vertex vertex3 = new Vertex(p3);
				if (ShadingModel == ShadingModel.Gourard)
				{
					Vector3 surfaceNormal = GetSurfaceNormal(vertex, vertex2, vertex3, ModelView);
					vertex.SetColor(Light(vertex, surfaceNormal));
					vertex2.SetColor(Light(vertex2, surfaceNormal));
					vertex3.SetColor(Light(vertex3, surfaceNormal));
				}
				vertex = Project(vertex);
				vertex2 = Project(vertex2);
				vertex3 = Project(vertex3);
				if (!Wireless)
				{
					Rasterizer.RasterizeTriangle(FrameBuffer, vertex, vertex2, vertex3, Texture);
					return;
				}
				Rasterizer.RasterizeLine(FrameBuffer, vertex, vertex2);
				Rasterizer.RasterizeLine(FrameBuffer, vertex2, vertex3);
				Rasterizer.RasterizeLine(FrameBuffer, vertex3, vertex);
			}
		}

		public void DrawQuad(Vertex p1, Vertex p2, Vertex p3, Vertex p4)
		{
			if (!BacksideCulling || !IsBackside(p1, p2, p4))
			{
				Vertex vertex = new Vertex(p1);
				Vertex vertex2 = new Vertex(p2);
				Vertex vertex3 = new Vertex(p3);
				Vertex vertex4 = new Vertex(p4);
				if (ShadingModel == ShadingModel.Gourard)
				{
					Vector3 surfaceNormal = GetSurfaceNormal(vertex, vertex2, vertex4, ModelView);
					vertex.SetColor(Light(vertex, surfaceNormal));
					vertex2.SetColor(Light(vertex2, surfaceNormal));
					vertex3.SetColor(Light(vertex3, surfaceNormal));
					vertex4.SetColor(Light(vertex4, surfaceNormal));
				}
				vertex = Project(vertex);
				vertex2 = Project(vertex2);
				vertex3 = Project(vertex3);
				vertex4 = Project(vertex4);
				if (!Wireless)
				{
					Rasterizer.RasterizeQuad(FrameBuffer, vertex, vertex2, vertex3, vertex4, Texture);
					return;
				}
				Rasterizer.RasterizeLine(FrameBuffer, vertex, vertex2);
				Rasterizer.RasterizeLine(FrameBuffer, vertex2, vertex3);
				Rasterizer.RasterizeLine(FrameBuffer, vertex3, vertex4);
				Rasterizer.RasterizeLine(FrameBuffer, vertex4, vertex);
			}
		}

		public void DrawQuad(Vertex[] v)
		{
			if (v.Length != 4)
			{
				throw new ArgumentException();
			}
			DrawQuad(v[0], v[1], v[2], v[3]);
		}

		private ColorF Light(Vertex p, Vector3 surfaceNormal)
		{
			ColorF ambientLight = AmbientLight;
			Vector3 viewDirection = new Vector3(0f, 0f, 1f);
			foreach (Light light in lights)
			{
				if (light.Enabled)
				{
					ambientLight += light.Calculate((Vector3)p * ModelView, viewDirection, surfaceNormal);
				}
			}
			ambientLight.Clamp();
			return ambientLight;
		}

		public Vertex Project(Vertex p, bool toFramebuffer)
		{
			Vector3 vector = (Vector3)p * ModelView * Projection;
			Vertex vertex = new Vertex(p);
			vertex.X = vector.X / vector.Z;
			vertex.Y = (0f - vector.Y) / vector.Z;
			vertex.Z = vector.Z;
			if (toFramebuffer)
			{
				Size size = FrameBuffer.Size;
				float num = (float)size.Width / Viewport.Width;
				float num2 = (float)size.Height / Viewport.Height;
				vertex.X = num * (vertex.X - Viewport.Left);
				vertex.Y = num2 * (vertex.Y - Viewport.Top);
			}
			return vertex;
		}

		public Vertex Project(Vertex v)
		{
			return Project(v, toFramebuffer: true);
		}

		public Vector3 GetSurfaceNormal(Vector3 p1, Vector3 p2, Vector3 p3, Matrix4 matrix)
		{
			Vector3 b = p1 * matrix;
			Vector3 b2 = p2 * matrix;
			Vector3 a = p3 * matrix;
			Vector3 a2 = a - b;
			Vector3 b3 = a - b2;
			return Vector3.Cross(a2, b3);
		}

		public bool IsBackside(Vertex p1, Vertex p2, Vertex p3)
		{
			Vector3 surfaceNormal = GetSurfaceNormal(p1, p2, p3, ModelView * Projection);
			Vector3 a = (Vector3)p1 * ModelView * Projection;
			return Vector3.Dot(a, surfaceNormal) >= 0f;
		}

		public RectangleF GetViewportExtent(IEnumerable<Vertex> vertices, bool toFrameBuffer)
		{
			float num = float.MaxValue;
			float num2 = float.MaxValue;
			float num3 = float.MinValue;
			float num4 = float.MinValue;
			foreach (Vertex vertex2 in vertices)
			{
				Vertex vertex = Project(vertex2, toFrameBuffer);
				num = Math.Min(num, vertex.X);
				num2 = Math.Min(num2, vertex.Y);
				num3 = Math.Max(num3, vertex.X);
				num4 = Math.Max(num4, vertex.Y);
			}
			return new RectangleF(num, num2, num3 - num, num4 - num2);
		}

		public RectangleF GetViewportExtent(IEnumerable<Vertex> vertices)
		{
			return GetViewportExtent(vertices, toFrameBuffer: true);
		}

		public static Bitmap RotateBitmap(Bitmap bitmap, Size size, float distance, float rx, float ry, float rz, bool trim, bool filter)
		{
			CyoGl cyoGl = new CyoGl();
			Size size2 = size;
			float num = (float)bitmap.Width / (float)bitmap.Height;
			float num2 = 1f;
			if (num > 1f)
			{
				num = 1f;
				num2 = (float)bitmap.Height / (float)bitmap.Width;
			}
			cyoGl.Projection = Matrix4.PerspectiveFOVInfinity(Numeric.DegToRad(45f), 1f, 0f - distance);
			cyoGl.ModelView = Matrix4.Translation((0f - num) / 2f, (0f - num2) / 2f, 0f);
			cyoGl.ModelView *= Matrix4.RotationX(Numeric.DegToRad(rx));
			cyoGl.ModelView *= Matrix4.RotationY(Numeric.DegToRad(ry));
			cyoGl.ModelView *= Matrix4.RotationZ(Numeric.DegToRad(rz));
			Vertex[] array = new Vertex[4]
			{
				new Vertex(0f, num2, 0f, 0f, 0f),
				new Vertex(num, num2, 0f, bitmap.Width - 1, 0f),
				new Vertex(num, 0f, 0f, bitmap.Width - 1, bitmap.Height - 1),
				new Vertex(0f, 0f, 0f, 0f, bitmap.Height - 1)
			};
			RectangleF viewportExtent = cyoGl.GetViewportExtent(array, toFrameBuffer: false);
			float num3 = Math.Max(viewportExtent.Width, viewportExtent.Height);
			viewportExtent.X += (viewportExtent.Width - num3) / 2f;
			viewportExtent.Y += (viewportExtent.Height - num3) / 2f;
			float num6 = (viewportExtent.Width = (viewportExtent.Height = num3));
			cyoGl.Viewport = viewportExtent;
			if (filter)
			{
				size = size.Scale(1.5f);
			}
			Bitmap bitmap2 = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
			try
			{
				using (BitmapFrameBuffer frameBuffer = new BitmapFrameBuffer(bitmap2))
				{
					using (BitmapFrameBuffer texture = new BitmapFrameBuffer(bitmap))
					{
						cyoGl.FrameBuffer = frameBuffer;
						cyoGl.Texture = texture;
						cyoGl.DrawQuad(array);
					}
				}
				Rectangle rectangle = Rectangle.Truncate(cyoGl.GetViewportExtent(array));
				if (filter)
				{
					rectangle = rectangle.Scale(2f / 3f);
					using (Bitmap bmp = bitmap2)
					{
						bitmap2 = bmp.Scale(size2, BitmapResampling.GdiPlusHQ);
					}
				}
				if (!trim)
				{
					Bitmap result = bitmap2;
					bitmap2 = null;
					return result;
				}
				if (rectangle.Width == 0 || rectangle.Height == 0)
				{
					return null;
				}
				return bitmap2.CreateCopy(rectangle);
			}
			finally
			{
				bitmap2?.Dispose();
			}
		}

		public static Bitmap RotateBitmap(Bitmap bitmap, Size size, float distance, Vector3 rot, bool trim, bool filter)
		{
			return RotateBitmap(bitmap, size, distance, rot.X, rot.Y, rot.Z, trim, filter);
		}

		public static Bitmap RotateBitmap(Bitmap bitmap, Size size, float distance, Vector3 rot, bool trim)
		{
			return RotateBitmap(bitmap, size, distance, rot, trim, filter: false);
		}
	}
}

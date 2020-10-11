using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Presentation;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine.IO
{
	public abstract class MemoryOptimizedImage : DisposableObject
	{
		private class RendererMemoryOptimizedImage : RendererImage
		{
			private readonly WeakReference<MemoryOptimizedImage> weakReference;

			public override bool IsValid
			{
				get
				{
					if (Moi != null)
					{
						return !Moi.IsDisposed;
					}
					return false;
				}
			}

			public override Bitmap Bitmap
			{
				get
				{
					if (Moi != null)
					{
						return Moi.Bitmap;
					}
					return null;
				}
			}

			public override Size Size
			{
				get
				{
					if (Moi != null)
					{
						return Moi.Size;
					}
					return Size.Empty;
				}
			}

			private MemoryOptimizedImage Moi => weakReference.GetData();

			public RendererMemoryOptimizedImage(MemoryOptimizedImage image)
			{
				weakReference = new WeakReference<MemoryOptimizedImage>(image);
			}

			public override bool Equals(object obj)
			{
				RendererMemoryOptimizedImage rendererMemoryOptimizedImage = obj as RendererMemoryOptimizedImage;
				if (rendererMemoryOptimizedImage != null)
				{
					return rendererMemoryOptimizedImage.Moi == Moi;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return 0;
			}

			public static implicit operator RendererMemoryOptimizedImage(MemoryOptimizedImage image)
			{
				return new RendererMemoryOptimizedImage(image);
			}
		}

		private volatile Bitmap bitmap;

		private byte[] data;

		private int timeToStay = 5;

		private volatile bool optimized = true;

		private Size size;

		private volatile int releaseTimeCounter;

		private static readonly Thread freeMemoryThread;

		private static readonly EventWaitHandle waitHandle;

		private static readonly List<MemoryOptimizedImage> openImages;

		public virtual Bitmap Bitmap
		{
			get
			{
				releaseTimeCounter = 0;
				Bitmap bitmap = this.bitmap;
				if (bitmap != null)
				{
					return bitmap;
				}
				return UpdateImageFromData(data);
			}
			set
			{
				Image image = bitmap;
				if (image == value)
				{
					return;
				}
				if (value == null)
				{
					UpdateDataFromImage();
				}
				bitmap = value;
				if (image != null)
				{
					using (ItemMonitor.Lock(image))
					{
						image.Dispose();
					}
				}
			}
		}

		public byte[] Data
		{
			get
			{
				UpdateDataFromImage();
				return data;
			}
		}

		public int TimeToStay
		{
			get
			{
				return timeToStay;
			}
			set
			{
				timeToStay = value;
			}
		}

		public bool Optimized
		{
			get
			{
				return optimized;
			}
			set
			{
				optimized = value;
			}
		}

		public virtual Size Size
		{
			get
			{
				Bitmap bitmap = Bitmap;
				if (bitmap != null)
				{
					int num = 0;
					while (size.IsEmpty && num++ < 100)
					{
						try
						{
							size = bitmap.Size;
						}
						catch (InvalidOperationException)
						{
							Thread.Sleep(100);
							continue;
						}
						catch (Exception)
						{
						}
						break;
					}
				}
				return size;
			}
		}

		public int Width => Size.Width;

		public int Height => Size.Height;

		public bool IsImage => bitmap != null;

		public bool IsValid
		{
			get
			{
				if (bitmap == null)
				{
					return data != null;
				}
				return true;
			}
		}

		protected MemoryOptimizedImage(byte[] data, Size size)
		{
			this.data = data;
			if (size.IsEmpty)
			{
				JpegFile jpegFile = new JpegFile(data);
				if (jpegFile.IsValid)
				{
					size = jpegFile.Size;
				}
			}
			this.size = size;
			using (ItemMonitor.Lock(openImages))
			{
				openImages.Add(this);
			}
		}

		protected MemoryOptimizedImage(byte[] data)
			: this(data, Size.Empty)
		{
		}

		protected MemoryOptimizedImage(Bitmap bitmap)
			: this(null, bitmap?.Size ?? Size.Empty)
		{
			this.bitmap = bitmap;
		}

		protected override void Dispose(bool disposing)
		{
			using (ItemMonitor.Lock(openImages))
			{
				openImages.Remove(this);
			}
			Image image = bitmap;
			using (ItemMonitor.Lock(image))
			{
				image?.Dispose();
			}
			base.Dispose(disposing);
		}

		public void Save(string file)
		{
			using (FileStream s = File.Create(file))
			{
				Save(s);
			}
		}

		public virtual byte[] ToBytes()
		{
			return Data;
		}

		public virtual void Save(Stream s)
		{
			s.Write(Data, 0, Data.Length);
		}

		public Bitmap Detach()
		{
			Bitmap result = Bitmap;
			bitmap = null;
			return result;
		}

		protected Bitmap UpdateImageFromData(byte[] imageData)
		{
			return Bitmap = OnCreateImage(imageData);
		}

		protected void UpdateDataFromImage()
		{
			using (ItemMonitor.Lock(bitmap))
			{
				if (bitmap != null && data == null)
				{
					try
					{
						data = bitmap.ImageToJpegBytes();
					}
					catch
					{
					}
				}
			}
		}

		protected virtual Bitmap OnCreateImage(byte[] data)
		{
			try
			{
				if (data != null)
				{
					return BitmapExtensions.BitmapFromBytes(data);
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		protected virtual void OnReleaseImage()
		{
			try
			{
				Bitmap = null;
			}
			catch (Exception)
			{
			}
			releaseTimeCounter = 0;
		}

		static MemoryOptimizedImage()
		{
			freeMemoryThread = ThreadUtility.CreateWorkerThread("Free Image Memory", FreeImageMemory, ThreadPriority.Lowest);
			waitHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);
			openImages = new List<MemoryOptimizedImage>();
			freeMemoryThread.Start();
		}

		private static void FreeImageMemory()
		{
			while (!waitHandle.WaitOne(1000, exitContext: false))
			{
				MemoryOptimizedImage[] array = openImages.Lock().ToArray();
				foreach (MemoryOptimizedImage memoryOptimizedImage in array)
				{
					if (!memoryOptimizedImage.IsDisposed && memoryOptimizedImage.Optimized && memoryOptimizedImage.TimeToStay != 0 && memoryOptimizedImage.bitmap != null && memoryOptimizedImage.releaseTimeCounter++ > memoryOptimizedImage.TimeToStay)
					{
						memoryOptimizedImage.OnReleaseImage();
					}
				}
			}
		}

		public static implicit operator RendererImage(MemoryOptimizedImage image)
		{
			return new RendererMemoryOptimizedImage(image);
		}
	}
}

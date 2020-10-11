using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using cYo.Common.Threading;

namespace cYo.Common.Windows.Forms
{
	public class TextBoxStream : Stream
	{
		private TextBoxBase textBox;

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public TextBoxStream(TextBoxBase textBox)
		{
			this.textBox = textBox;
			this.textBox.Disposed += textBox_Disposed;
		}

		private void textBox_Disposed(object sender, EventArgs e)
		{
			textBox = null;
		}

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (textBox != null && !textBox.InvokeIfRequired(delegate
			{
				Write(buffer, offset, count);
			}))
			{
				using (ItemMonitor.Lock(this))
				{
					textBox.AppendText(Encoding.Default.GetString(buffer, offset, count));
					textBox.SelectionStart = textBox.Text.Length;
					textBox.ScrollToCaret();
				}
			}
		}
	}
}

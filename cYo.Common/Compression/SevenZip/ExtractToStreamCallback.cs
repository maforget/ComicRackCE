using System.IO;

namespace cYo.Common.Compression.SevenZip
{
	public class ExtractToStreamCallback : IProgress, IArchiveExtractCallback
	{
		private readonly int fileNumber;

		private readonly Stream stream;

		private OutStreamWrapper fileStream;

		public ExtractToStreamCallback(int fileNumber, Stream ms)
		{
			this.fileNumber = fileNumber;
			stream = ms;
		}

		public void SetTotal(long total)
		{
		}

		public void SetCompleted(ref long completeValue)
		{
		}

		public int GetStream(int index, out ISequentialOutStream outStream, AskMode askExtractMode)
		{
			outStream = null;
			if (index == fileNumber && askExtractMode == AskMode.kExtract)
			{
				fileStream = new OutStreamWrapper(stream);
				outStream = fileStream;
			}
			return 0;
		}

		public void PrepareOperation(AskMode askExtractMode)
		{
		}

		public void SetOperationResult(OperationResult resultEOperationResult)
		{
			if (fileStream != null)
			{
				fileStream.Dispose();
			}
		}
	}
}

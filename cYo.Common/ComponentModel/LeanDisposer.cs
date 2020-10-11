using System;

namespace cYo.Common.ComponentModel
{
	internal struct LeanDisposer : IDisposable
	{
		private Action method;

		private bool eatErrors;

		public LeanDisposer(Action method, bool eatErrors = false)
		{
			this.method = method;
			this.eatErrors = eatErrors;
		}

		public void Dispose()
		{
			try
			{
				method();
			}
			catch (Exception)
			{
				if (!eatErrors)
				{
					throw;
				}
			}
		}
	}
}

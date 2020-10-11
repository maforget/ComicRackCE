using System;

namespace cYo.Common.ComponentModel
{
	public class Disposer : DisposableObject
	{
		private readonly Action method;

		private readonly bool eatErrors;

		public Disposer(Action method, bool eatErrors = false)
		{
			this.method = method;
			this.eatErrors = eatErrors;
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					method();
				}
			}
			catch
			{
				if (!eatErrors)
				{
					throw;
				}
			}
			base.Dispose(disposing);
		}
	}
}

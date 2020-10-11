using System.Linq;
using cYo.Common.Collections;

namespace cYo.Common.Presentation.Panels
{
	public class AnimatorCollection : SmartList<Animator>
	{
		public bool AllCompleted => this.All((Animator a) => a.IsCompleted);

		public void Start()
		{
			ForEach(delegate(Animator a)
			{
				a.Start();
			});
		}
	}
}

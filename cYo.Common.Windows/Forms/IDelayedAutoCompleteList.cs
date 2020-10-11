using System;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public interface IDelayedAutoCompleteList
	{
		void SetLazyAutoComplete(Func<AutoCompleteStringCollection> predicate);

		void ResetLazyAutoComplete();

		void BuildAutoComplete();
	}
}

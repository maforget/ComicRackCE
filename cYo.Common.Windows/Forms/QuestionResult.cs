using System;

namespace cYo.Common.Windows.Forms
{
	[Flags]
	public enum QuestionResult
	{
		Cancel = 0x1,
		Ok = 0x2,
		Option = 0x4,
		Option2 = 0x8,
		OkWithOption = 0x6
	}
}

namespace cYo.Common.Windows
{
	public static class CommandKeyExtension
	{
		public static bool IsMouseButton(this CommandKey key)
		{
			key &= (CommandKey)65535;
			if (key != CommandKey.MouseLeft && key != CommandKey.MouseDoubleLeft && key != CommandKey.MouseMiddle && key != CommandKey.MouseDoubleMiddle && key != CommandKey.MouseRight && key != CommandKey.MouseDoubleRight && key != CommandKey.MouseButton4 && key != CommandKey.MouseDoubleButton4 && key != CommandKey.MouseButton5 && key != CommandKey.MouseDoubleButton5 && key != CommandKey.MouseWheelUp && key != CommandKey.MouseWheelDown && key != CommandKey.MouseTiltRight && key != CommandKey.MouseTiltLeft && key != CommandKey.TouchTap && key != CommandKey.TouchDoubleTap && key != CommandKey.TouchPressAndTap)
			{
				return key == CommandKey.TouchTwoFingerTap;
			}
			return true;
		}
	}
}

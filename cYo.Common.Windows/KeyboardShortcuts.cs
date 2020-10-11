using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace cYo.Common.Windows
{
	[Serializable]
	public class KeyboardShortcuts : ICloneable
	{
		private readonly List<KeyboardCommand> commands = new List<KeyboardCommand>();

		public List<KeyboardCommand> Commands => commands;

		public KeyboardShortcuts()
		{
		}

		public KeyboardShortcuts(KeyboardShortcuts copy)
		{
			Commands.AddRange(copy.Commands.Select((KeyboardCommand kc) => new KeyboardCommand(kc)));
		}

		public bool HandleKey(CommandKey key)
		{
			foreach (KeyboardCommand command in Commands)
			{
				if (command.Handles(key))
				{
					command.Invoke(key);
					return true;
				}
			}
			return false;
		}

		public bool HandleKey(CommandKey key, CommandKey modifiers)
		{
			return HandleKey(key | modifiers);
		}

		public bool HandleKey(CommandKey key, Keys modifiers)
		{
			return HandleKey(key, (CommandKey)modifiers);
		}

		public bool HandleKey(MouseButtons button, bool doubleClick, bool isTouch)
		{
			if (isTouch && HandleKey(doubleClick ? CommandKey.TouchDoubleTap : CommandKey.TouchTap, Control.ModifierKeys))
			{
				return true;
			}
			if ((button & MouseButtons.Left) != 0)
			{
				return HandleKey(doubleClick ? CommandKey.MouseDoubleLeft : CommandKey.MouseLeft, Control.ModifierKeys);
			}
			if ((button & MouseButtons.Right) != 0)
			{
				return HandleKey(doubleClick ? CommandKey.MouseDoubleRight : CommandKey.MouseRight, Control.ModifierKeys);
			}
			if ((button & MouseButtons.Middle) != 0)
			{
				return HandleKey(doubleClick ? CommandKey.MouseDoubleMiddle : CommandKey.MouseMiddle, Control.ModifierKeys);
			}
			if ((button & MouseButtons.XButton1) != 0)
			{
				return HandleKey(doubleClick ? CommandKey.MouseDoubleButton4 : CommandKey.MouseButton4, Control.ModifierKeys);
			}
			if ((button & MouseButtons.XButton2) != 0)
			{
				return HandleKey(doubleClick ? CommandKey.MouseDoubleButton5 : CommandKey.MouseButton5, Control.ModifierKeys);
			}
			return false;
		}

		public bool HandleKey(Keys k)
		{
			Keys keys = k & Keys.KeyCode;
			if (Enum.IsDefined(typeof(CommandKey), (int)keys))
			{
				return HandleKey((CommandKey)k);
			}
			return false;
		}

		public KeyboardCommand FindCommandByKey(string key)
		{
			return commands.FirstOrDefault((KeyboardCommand kc) => kc.Id == key);
		}

		public void SetKeyMapping(IEnumerable<StringPair> list)
		{
			foreach (StringPair item in list)
			{
				KeyboardCommand keyboardCommand = FindCommandByKey(item.Key);
				if (keyboardCommand != null)
				{
					keyboardCommand.KeyList = item.Value;
				}
			}
		}

		public IEnumerable<StringPair> GetKeyMapping()
		{
			return Commands.Select((KeyboardCommand kc) => new StringPair(kc.Id, kc.KeyList));
		}

		public object Clone()
		{
			return new KeyboardShortcuts(this);
		}
	}
}

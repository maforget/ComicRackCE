using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer
{
	public class CommandMapper : Component
	{
		private class HandleItem
		{
			public readonly object Sender;

			public readonly CommandHandler Command;

			public readonly UpdateHandler Update;

			public readonly UpdateHandler Check;

			private readonly Func<bool> checkVisibility;

			public bool IdleUpdate;

			public bool ForcedUpdate;

			public bool IsVisible => checkVisibility();

			public Keys ShortcutKeys => (Sender as ToolStripMenuItem)?.ShortcutKeys ?? Keys.None;

			public HandleItem(object sender, CommandHandler command, UpdateHandler update, UpdateHandler check)
			{
				Sender = sender;
				Command = command;
				Update = update;
				Check = check;
				IdleUpdate = true;
				if (Sender is ToolStripItem)
				{
					ToolStripItem tsi = (ToolStripItem)Sender;
					checkVisibility = () => tsi.Visible;
				}
				else if (Sender is ButtonBase)
				{
					ButtonBase bb = (ButtonBase)Sender;
					checkVisibility = () => bb.Visible;
				}
				else
				{
					checkVisibility = () => false;
				}
			}
		}

		private readonly Dictionary<object, HandleItem> ht = new Dictionary<object, HandleItem>();

		private bool enable = true;

		private bool handleShield;

		public bool Enable
		{
			get
			{
				return enable;
			}
			set
			{
				enable = value;
			}
		}

		public CommandMapper(bool enable)
		{
			this.enable = enable;
			IdleProcess.Idle += ApplicationIdle;
		}

		public CommandMapper()
			: this(enable: true)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IdleProcess.Idle -= ApplicationIdle;
			}
			base.Dispose(disposing);
		}

		public void Add(CommandHandler clickHandler, UpdateHandler enabledHandler, UpdateHandler checkedHandler, params object[] senders)
		{
			foreach (object obj in senders)
			{
				HandleItem hi = new HandleItem(obj, clickHandler, enabledHandler, checkedHandler);
				ht[obj] = hi;
				ToolStripItem toolStripItem;
				ButtonBase buttonBase;
				if ((toolStripItem = obj as ToolStripItem) != null)
				{
					ToolStripMenuItem toolStripMenuItem = toolStripItem as ToolStripMenuItem;
					if (toolStripMenuItem != null && toolStripMenuItem.ShortcutKeys != 0)
					{
						hi.ForcedUpdate = true;
					}
					else if (!toolStripItem.IsOnOverflow)
					{
						ContextMenuStrip contextMenuStrip = toolStripItem.GetCurrentParent() as ContextMenuStrip;
						if (contextMenuStrip != null)
						{
							contextMenuStrip.Opening += delegate
							{
								IdleUpdate(hi, forced: true);
							};
							hi.IdleUpdate = false;
						}
						else
						{
							ToolStripDropDown toolStripDropDown = toolStripItem.GetCurrentParent() as ToolStripDropDown;
							if (toolStripDropDown != null)
							{
								toolStripDropDown.Opening += delegate
								{
									IdleUpdate(hi, forced: true);
								};
								hi.IdleUpdate = false;
							}
						}
					}
					ToolStripSplitButton toolStripSplitButton = toolStripItem as ToolStripSplitButton;
					if (toolStripSplitButton != null)
					{
						toolStripSplitButton.ButtonClick += CommandMapperClick;
					}
					else
					{
						toolStripItem.Click += CommandMapperClick;
					}
				}
				else if ((buttonBase = obj as ButtonBase) != null)
				{
					buttonBase.Click += CommandMapperClick;
				}
			}
		}

		public void Add(CommandHandler clickHandler, bool isCheckedHandler, UpdateHandler updateHandler, params object[] senders)
		{
			if (isCheckedHandler)
			{
				Add(clickHandler, null, updateHandler, senders);
			}
			else
			{
				Add(clickHandler, updateHandler, null, senders);
			}
		}

		public void Add(CommandHandler clickHandler, UpdateHandler enableHandler, params object[] senders)
		{
			Add(clickHandler, enableHandler, null, senders);
		}

		public void Add(CommandHandler ch, params object[] senders)
		{
			Add(ch, null, senders);
		}

		public bool Handle(object sender)
		{
			if (!ht.ContainsKey(sender))
			{
				return false;
			}
			HandleCommandItem(ht[sender]);
			return true;
		}

		public bool InvokeKey(Keys shortcutKeys)
		{
			foreach (HandleItem value in ht.Values)
			{
				if (value.ShortcutKeys == shortcutKeys)
				{
					HandleCommandItem(value);
					return true;
				}
			}
			return false;
		}

		public void AddService<T>(Control c, ServiceCommandHandler<T> clickHandler, ServiceUpdateHandler<T> enabledHandler, ServiceUpdateHandler<T> checkedHandler, params object[] senders) where T : class
		{
			CommandHandler clickHandler2 = null;
			UpdateHandler enabledHandler2 = null;
			UpdateHandler checkedHandler2 = null;
			if (clickHandler != null)
			{
				clickHandler2 = delegate
				{
					T val3 = c.FindActiveService<T>();
					if (val3 != null)
					{
						clickHandler(val3);
					}
				};
			}
			if (enabledHandler != null)
			{
				enabledHandler2 = delegate
				{
					T val2 = c.FindActiveService<T>();
					return val2 != null && enabledHandler(val2);
				};
			}
			if (checkedHandler != null)
			{
				checkedHandler2 = delegate
				{
					T val = c.FindActiveService<T>();
					return val != null && checkedHandler(val);
				};
			}
			Add(clickHandler2, enabledHandler2, checkedHandler2, senders);
		}

		public void AddService<T>(Control c, ServiceCommandHandler<T> clickHandler, bool isCheckedHandler, ServiceUpdateHandler<T> updateHandler, params object[] senders) where T : class
		{
			if (isCheckedHandler)
			{
				AddService(c, clickHandler, null, updateHandler, senders);
			}
			else
			{
				AddService(c, clickHandler, updateHandler, null, senders);
			}
		}

		public void AddService<T>(Control c, ServiceCommandHandler<T> clickHandler, ServiceUpdateHandler<T> enableHandler, params object[] senders) where T : class
		{
			AddService(c, clickHandler, enableHandler, null, senders);
		}

		public void AddService<T>(Control c, ServiceCommandHandler<T> ch, params object[] senders) where T : class
		{
			AddService(c, ch, null, senders);
		}

		private void HandleCommandItem(HandleItem item)
		{
			if (item.Update == null || item.Update())
			{
				item.Command?.Invoke();
			}
		}

		private void CommandMapperClick(object sender, EventArgs e)
		{
			if (!handleShield)
			{
				handleShield = true;
				try
				{
					Handle(sender);
				}
				finally
				{
					handleShield = false;
				}
			}
		}

		private void IdleUpdate(HandleItem hi, bool forced = false)
		{
			forced |= hi.ForcedUpdate;
			if (!forced && !hi.IsVisible)
			{
				return;
			}
			if (hi.Update != null)
			{
				bool enabled = hi.Update();
				ToolStripItem toolStripItem;
				ButtonBase buttonBase;
				if ((toolStripItem = hi.Sender as ToolStripItem) != null)
				{
					toolStripItem.Enabled = enabled;
				}
				else if ((buttonBase = hi.Sender as ButtonBase) != null)
				{
					buttonBase.Enabled = enabled;
				}
			}
			if (hi.Check != null)
			{
				bool @checked = hi.Check();
				ToolStripButton toolStripButton;
				if ((toolStripButton = hi.Sender as ToolStripButton) != null)
				{
					toolStripButton.Checked = @checked;
				}
				ToolStripMenuItem toolStripMenuItem;
				if ((toolStripMenuItem = hi.Sender as ToolStripMenuItem) != null)
				{
					toolStripMenuItem.Checked = @checked;
				}
			}
		}

		private void ApplicationIdle(object sender, EventArgs e)
		{
			if (!enable)
			{
				return;
			}
			foreach (HandleItem value in ht.Values)
			{
				if (value.IdleUpdate)
				{
					IdleUpdate(value);
				}
			}
		}
	}
}

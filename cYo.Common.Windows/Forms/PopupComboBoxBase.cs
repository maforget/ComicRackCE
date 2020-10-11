using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	[ToolboxBitmap(typeof(ComboBox))]
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[Description("Displays an editable text box with a drop-down list of permitted values.")]
	public class PopupComboBoxBase : ComboBox
	{
		private static Type modalMenuFilter;

		private static MethodInfo suspendMenuModeMethodInfo;

		private static MethodInfo resumeMenuModeMethodInfo;

		private static Type ModalMenuFilter
		{
			get
			{
				if (modalMenuFilter == null)
				{
					modalMenuFilter = Type.GetType("System.Windows.Forms.ToolStripManager+ModalMenuFilter");
				}
				if (modalMenuFilter == null)
				{
					modalMenuFilter = new List<Type>(typeof(ToolStripManager).Assembly.GetTypes()).Find((Type type) => type.FullName == "System.Windows.Forms.ToolStripManager+ModalMenuFilter");
				}
				return modalMenuFilter;
			}
		}

		private static MethodInfo SuspendMenuModeMethodInfo
		{
			get
			{
				if (suspendMenuModeMethodInfo == null)
				{
					Type type = ModalMenuFilter;
					if (type != null)
					{
						suspendMenuModeMethodInfo = type.GetMethod("SuspendMenuMode", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					}
				}
				return suspendMenuModeMethodInfo;
			}
		}

		private static MethodInfo ResumeMenuModeMethodInfo
		{
			get
			{
				if (resumeMenuModeMethodInfo == null)
				{
					Type type = ModalMenuFilter;
					if (type != null)
					{
						resumeMenuModeMethodInfo = type.GetMethod("ResumeMenuMode", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					}
				}
				return resumeMenuModeMethodInfo;
			}
		}

		private static void SuspendMenuMode()
		{
			MethodInfo methodInfo = SuspendMenuModeMethodInfo;
			if (methodInfo != null)
			{
				methodInfo.Invoke(null, null);
			}
		}

		private static void ResumeMenuMode()
		{
			MethodInfo methodInfo = ResumeMenuModeMethodInfo;
			if (methodInfo != null)
			{
				methodInfo.Invoke(null, null);
			}
		}

		protected override void OnDropDown(EventArgs e)
		{
			base.OnDropDown(e);
			SuspendMenuMode();
		}

		protected override void OnDropDownClosed(EventArgs e)
		{
			ResumeMenuMode();
			base.OnDropDownClosed(e);
		}
	}
}

using System;
using System.Reflection;
using System.Windows.Forms;

namespace cYo.Common.Windows
{
	public static class ControlExtensions
	{
		private static bool IsValid(Control c)
		{
			if (c != null)
			{
				return !c.IsDisposed;
			}
			return false;
		}

		public static void Invoke(this Control control, Action action)
		{
			if (!IsValid(control))
			{
				return;
			}
			control.Invoke((MethodInvoker)delegate
			{
				if (IsValid(control))
				{
					action();
				}
			});
		}

		public static void BeginInvoke(this Control control, Action action, bool catchErrors = true)
		{
			if (!IsValid(control))
			{
				return;
			}
			control.BeginInvoke((MethodInvoker)delegate
			{
				try
				{
					if (IsValid(control))
					{
						action();
					}
				}
				catch (Exception)
				{
					if (!catchErrors)
					{
						throw;
					}
				}
			});
		}

		public static bool InvokeIfRequired(this Control control, Action action)
		{
			if (!IsValid(control) || !control.InvokeRequired)
			{
				return false;
			}
			Invoke(control, action);
			return true;
		}

		public static bool BeginInvokeIfRequired(this Control control, Action action, bool catchErrors = true)
		{
			if (!IsValid(control) || !control.InvokeRequired)
			{
				return false;
			}
			BeginInvoke(control, action, catchErrors);
			return true;
		}

		public static Control TopParent(this Control c)
		{
			Control result = c;
			while ((c = c.Parent) != null)
			{
				result = c;
			}
			return result;
		}

		public static Control Clone(this Control controlToClone)
		{
			Type type = controlToClone.GetType();
			Control control = (Control)Activator.CreateInstance(type);
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				if (propertyInfo.CanWrite && propertyInfo.Name != "WindowTarget")
				{
					propertyInfo.SetValue(control, propertyInfo.GetValue(controlToClone, null), null);
				}
			}
			return control;
		}
	}
}

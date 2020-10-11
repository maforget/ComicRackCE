using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Viewer.Controls;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public class SubView : CaptionControl
	{
		private IMain mainForm;

		private IContainer components;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IMain Main
		{
			get
			{
				return mainForm;
			}
			set
			{
				if (mainForm != value)
				{
					mainForm = value;
					OnMainFormChanged();
				}
			}
		}

		public SubView()
		{
			InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IdleProcess.Idle -= Application_Idle;
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void Application_Idle(object sender, EventArgs e)
		{
			OnIdle();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!base.DesignMode)
			{
				IdleProcess.Idle += Application_Idle;
			}
		}

		protected virtual void OnIdle()
		{
		}

		protected virtual void OnMainFormChanged()
		{
			SetMain(base.Controls, Main);
		}

		private static void SetMain(ControlCollection cc, IMain main)
		{
			foreach (Control item in cc)
			{
				SubView subView = item as SubView;
				if (subView != null)
				{
					subView.Main = main;
				}
				else
				{
					SetMain(item.Controls, main);
				}
			}
		}

		protected static void TranslateColumns(IEnumerable<IColumn> itemViewColumnCollection)
		{
			ComicListField.TranslateColumns(itemViewColumnCollection);
		}

		private void InitializeComponent()
		{
			SuspendLayout();
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.Name = "SubView";
			ResumeLayout(false);
		}
	}
}

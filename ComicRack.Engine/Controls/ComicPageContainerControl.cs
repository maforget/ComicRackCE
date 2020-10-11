using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine.Controls
{
	public class ComicPageContainerControl : ContainerControl
	{
		private IContainer components;

		private TabBar tabBar;

		public IEnumerable<ComicPageControl> Pages => base.Controls.OfType<ComicPageControl>();

		public ComicPageContainerControl()
		{
			InitializeComponent();
		}

		public void ShowInfo(IEnumerable<ComicBook> books)
		{
			Pages.ForEach(delegate(ComicPageControl p)
			{
				p.ShowInfo(books);
			});
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			Control c = e.Control;
			if (!(c is TabBar))
			{
				string text = c.Text;
				Image image = null;
				ComicPageControl comicPageControl = e.Control as ComicPageControl;
				if (comicPageControl != null)
				{
					image = comicPageControl.Icon;
				}
				TabBar.TabBarItem tbi = new TabBar.TabBarItem
				{
					Text = text,
					Image = image,
					Tag = c
				};
				c.TextChanged += delegate
				{
					tbi.Text = c.Text;
				};
				c.Visible = false;
				c.Dock = DockStyle.Fill;
				tabBar.Items.Add(tbi);
				tabBar.Visible = tabBar.Items.Count > 1;
				if (tabBar.Items.Count == 1)
				{
					tabBar.SelectedTab = tbi;
				}
				base.Controls.SetChildIndex(tabBar, 1000);
			}
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			ComicPageControl c = e.Control as ComicPageControl;
			TabBar.TabBarItem tabBarItem = tabBar.Items.FirstOrDefault((TabBar.TabBarItem t) => t.Tag == c);
			if (tabBarItem != null)
			{
				tabBar.Items.Remove(tabBarItem);
				tabBar.Visible = tabBar.Items.Count > 1;
			}
			base.OnControlRemoved(e);
		}

		private void tabBar_SelectedTabChanged(object sender, TabBar.SelectedTabChangedEventArgs e)
		{
			foreach (TabBar.TabBarItem item in tabBar.Items)
			{
				((Control)item.Tag).Visible = item == e.NewItem;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			tabBar = new cYo.Common.Windows.Forms.TabBar();
			SuspendLayout();
			tabBar.AllowDrop = true;
			tabBar.Dock = System.Windows.Forms.DockStyle.Top;
			tabBar.Location = new System.Drawing.Point(0, 0);
			tabBar.MinimumTabWidth = 100;
			tabBar.Name = "tabBar";
			tabBar.Size = new System.Drawing.Size(487, 31);
			tabBar.TabIndex = 0;
			tabBar.Text = "tabBar1";
			tabBar.SelectedTabChanged += new System.EventHandler<cYo.Common.Windows.Forms.TabBar.SelectedTabChangedEventArgs>(tabBar_SelectedTabChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(tabBar);
			base.Name = "ComicInfoControl";
			base.Size = new System.Drawing.Size(487, 352);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}

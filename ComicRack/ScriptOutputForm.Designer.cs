using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer
{
    public partial class ScriptOutputForm
	{
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
			Log = new System.Windows.Forms.TextBox();
			SuspendLayout();
			Log.Dock = System.Windows.Forms.DockStyle.Fill;
			Log.Font = new System.Drawing.Font("Lucida Console", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			Log.Location = new System.Drawing.Point(0, 0);
			Log.MaxLength = 1000000;
			Log.Multiline = true;
			Log.Name = "Log";
			Log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			Log.Size = new System.Drawing.Size(606, 471);
			Log.TabIndex = 0;
			Log.WordWrap = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(606, 471);
			base.Controls.Add(Log);
			base.Name = "ScriptOutputForm";
			base.ShowIcon = false;
			Text = "Script Output";
			ResumeLayout(false);
			PerformLayout();
		}

		private IContainer components;

		public TextBox Log;

	}
}

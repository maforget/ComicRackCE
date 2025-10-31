using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public partial class QuestionDialog : FormEx
	{
		public string Question
		{
			get;
			set;
		}

		public string OkButtonText
		{
			get;
			set;
		}

		public string CancelButtonText
		{
			get;
			set;
		}

		public string OptionText
		{
			get;
			set;
		}

		public string Option2Text
		{
			get;
			set;
		}

		public bool Option2Independent
		{
			get;
			set;
		}

		public Image Image
		{
			get;
			set;
		}

		public bool ShowCancel
		{
			get;
			set;
		}

		public QuestionDialog()
		{
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
			Text = Application.ProductName;
			iconBox.BackgroundImage = SystemIcons.Question.ToBitmap();
		}

		public QuestionResult Ask(IWin32Window owner)
		{
			string[] array = Question.Split('\n');
			imageBox.Image = Image;
			imageBox.Visible = Image != null;
			lblQuestion.Text = array[0];
			lblDescription.Text = string.Empty;
			for (int i = 1; i < array.Length; i++)
			{
				Label label = lblDescription;
				label.Text = label.Text + ((i > 1) ? "\n" : string.Empty) + array[i];
			}
			if (OkButtonText != null)
			{
				btOK.Text = OkButtonText;
			}
			if (CancelButtonText != null)
			{
				btCancel.Text = CancelButtonText;
			}
			lblDescription.Visible = array.Length > 1;
			if (!string.IsNullOrEmpty(OptionText))
			{
				chkOption.Checked = OptionText.StartsWith("!");
				if (chkOption.Checked)
				{
					OptionText = OptionText.Substring(1);
				}
			}
			if (!string.IsNullOrEmpty(Option2Text))
			{
				chkOption2.Checked = Option2Text.StartsWith("!");
				if (chkOption2.Checked)
				{
					Option2Text = Option2Text.Substring(1);
				}
			}
			chkOption.Text = OptionText;
			chkOption.Visible = !string.IsNullOrEmpty(OptionText);
			chkOption2.Text = Option2Text;
			chkOption2.Visible = (Option2Independent || chkOption.Checked) && !string.IsNullOrEmpty(Option2Text);
			if (!Option2Independent)
			{
				chkOption2.Margin = new Padding(32, 0, 0, 0);
			}
			btCancel.Visible = ShowCancel;
			if (owner == null)
			{
				base.StartPosition = FormStartPosition.CenterScreen;
			}
			QuestionResult questionResult = ShowDialog(owner) != DialogResult.OK ? QuestionResult.Cancel : QuestionResult.Ok;
			if (chkOption.Checked)
			{
				questionResult |= QuestionResult.Option;
			}
			if (chkOption2.Checked)
			{
				questionResult |= QuestionResult.Option2;
			}
			return questionResult;
		}

		public static QuestionResult AskQuestion(IWin32Window owner, string question, string okText, string optionText = null, Image image = null, bool showCancel = true, string cancelText = null, string option2Text = null)
		{
			return AskQuestion(owner, question, okText, delegate(QuestionDialog qd)
			{
				qd.CancelButtonText = cancelText;
				qd.OptionText = optionText;
				qd.Option2Text = option2Text;
				qd.Image = image;
				qd.ShowCancel = showCancel;
			});
		}

		public static QuestionResult AskQuestion(IWin32Window owner, string question, string okButtonText = null, Action<QuestionDialog> setParameters = null)
		{
			using (QuestionDialog questionDialog = new QuestionDialog())
			{
				questionDialog.Question = question;
				questionDialog.OkButtonText = okButtonText;
				setParameters?.Invoke(questionDialog);
				return questionDialog.Ask(owner);
			}
		}

		public static bool Ask(IWin32Window owner, string question, string okButtonText)
		{
			return AskQuestion(owner, question, okButtonText, (Action<QuestionDialog>)null) == QuestionResult.Ok;
		}

		private void chkOption_CheckedChanged(object sender, EventArgs e)
		{
			if (!Option2Independent)
			{
				chkOption2.Visible = chkOption.Checked && !string.IsNullOrEmpty(chkOption2.Text);
			}
		}
	}
}

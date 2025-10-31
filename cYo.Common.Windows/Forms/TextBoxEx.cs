using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class TextBoxEx : TextBox, IPromptText, IDelayedAutoCompleteList
	{
		private bool quickSelectAll;

		private string promptText;

		private bool autoCompleteInitialized;

		private Func<AutoCompleteStringCollection> autoCompletePredicate;

		private bool hasEdited;

		private bool emptyText;

		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Category("Appearance")]
		[Description("The prompt text to display when there is nothing in the Text property.")]
		[DefaultValue(null)]
		public string PromptText
		{
			get
			{
				return promptText;
			}
			set
			{
				promptText = value;
				if (base.IsHandleCreated)
				{
					SetPromptText();
				}
			}
		}

		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Category("Behavior")]
		[Description("Automatically select the text when control receives the focus.")]
		[DefaultValue(true)]
		public bool FocusSelect
		{
			get;
			set;
		}

		public TextBoxEx()
		{
			FocusSelect = true;
        }

		protected override bool IsInputKey(Keys keyData)
		{
			if (!Multiline && (keyData == Keys.Down || keyData == Keys.Up))
			{
				return true;
			}
			return base.IsInputKey(keyData);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			SetPromptText();
        }

		protected override void OnEnter(EventArgs e)
		{
			DoEnter();
			base.OnEnter(e);
		}

		private void DoEnter()
		{
			emptyText = false;
			InitializeAutoComplete(withoutFocus: true);
			if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(PromptText))
			{
				emptyText = true;
				Text = PromptText;
			}
			if (Text.Length > 0 && FocusSelect)
			{
				SelectAll();
				quickSelectAll = true;
			}
			hasEdited = false;
		}

		private void InitializeAutoComplete(bool withoutFocus = false, bool delayed = false)
		{
			if (autoCompletePredicate != null && !autoCompleteInitialized && (Focused || withoutFocus))
			{
				autoCompleteInitialized = true;
				base.AutoCompleteCustomSource = autoCompletePredicate();
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			hasEdited = true;
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			DoLeave();
		}

		private void DoLeave()
		{
			if (!string.IsNullOrEmpty(PromptText) && !hasEdited && emptyText && SelectedText == PromptText)
			{
				Text = string.Empty;
			}
			quickSelectAll = false;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (quickSelectAll)
			{
				SelectAll();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			quickSelectAll = false;
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			quickSelectAll = false;
		}

		private void SetPromptText()
		{
			if (Focused)
			{
				DoLeave();
			}
			this.SetCueText(promptText);
			if (Focused)
			{
				DoEnter();
			}
		}

		public void SetLazyAutoComplete(Func<AutoCompleteStringCollection> autoCompletePredicate)
		{
			this.autoCompletePredicate = autoCompletePredicate;
			base.AutoCompleteMode = AutoCompleteMode.Append;
			base.AutoCompleteSource = AutoCompleteSource.CustomSource;
			base.AutoCompleteCustomSource = null;
			ResetLazyAutoComplete();
		}

		public void ResetLazyAutoComplete()
		{
			autoCompleteInitialized = false;
			InitializeAutoComplete();
		}

		public void BuildAutoComplete()
		{
			InitializeAutoComplete(withoutFocus: true);
		}
	}
}

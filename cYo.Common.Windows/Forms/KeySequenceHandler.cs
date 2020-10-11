using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class KeySequenceHandler : Component
	{
		private class SequenceState
		{
			private int position;

			private DateTime lastKeyEntered = DateTime.Now;

			public KeySequence Sequence
			{
				get;
				set;
			}

			public int Position
			{
				get
				{
					return position;
				}
				set
				{
					if (position != value)
					{
						position = value;
						lastKeyEntered = DateTime.Now;
					}
				}
			}

			public DateTime LastKeyEntered => lastKeyEntered;

			public Keys NextKey => Sequence.Sequence[position];

			public bool Completed => position >= Sequence.Sequence.Count;

			public SequenceState(KeySequence sequence)
			{
				Sequence = sequence;
			}

			public bool Parse(Keys key, int maximumIntervall)
			{
				if ((DateTime.Now - LastKeyEntered).TotalMilliseconds > (double)maximumIntervall || NextKey != key)
				{
					return false;
				}
				Position++;
				return true;
			}
		}

		private Control control;

		private readonly KeySequenceCollection sequences = new KeySequenceCollection();

		private int intervallTime = 1000;

		private readonly List<SequenceState> activeSequences = new List<SequenceState>();

		private Keys keyState;

		private IContainer components;

		[DefaultValue(null)]
		public Control Control
		{
			get
			{
				return control;
			}
			set
			{
				if (control != value)
				{
					if (control != null)
					{
						control.KeyDown -= control_KeyDown;
						control.KeyUp -= control_KeyUp;
					}
					control = value;
					if (control != null)
					{
						control.KeyDown += control_KeyDown;
						control.KeyUp += control_KeyUp;
					}
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public KeySequenceCollection Sequences => sequences;

		public KeySequenceCollection ActiveSequences
		{
			get
			{
				KeySequenceCollection keySequenceCollection = new KeySequenceCollection();
				foreach (SequenceState activeSequence in activeSequences)
				{
					keySequenceCollection.Add(activeSequence.Sequence);
				}
				return keySequenceCollection;
			}
		}

		[DefaultValue(1000)]
		public int IntervallTime
		{
			get
			{
				return intervallTime;
			}
			set
			{
				intervallTime = value;
			}
		}

		public event KeyEventHandler KeyDown;

		public event KeyEventHandler KeyUp;

		public event EventHandler<KeySequenceEventArgs> SequenceCompleted;

		public KeySequenceHandler()
		{
			InitializeComponent();
		}

		public KeySequenceHandler(IContainer container)
			: this()
		{
			container.Add(this);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
				Control = null;
			}
			base.Dispose(disposing);
		}

		private void ParseKey(Keys key)
		{
			for (int num = activeSequences.Count - 1; num >= 0; num--)
			{
				SequenceState sequenceState = activeSequences[num];
				if (!sequenceState.Parse(key, intervallTime))
				{
					activeSequences.RemoveAt(num);
				}
				else if (sequenceState.Completed)
				{
					activeSequences.Clear();
					OnSequenceCompleted(sequenceState.Sequence);
					return;
				}
			}
			foreach (KeySequence sequence in Sequences)
			{
				KeySequence ksrun = sequence;
				if (activeSequences.Find((SequenceState ss) => ss.Sequence == ksrun) == null)
				{
					SequenceState sequenceState2 = new SequenceState(sequence);
					if (sequenceState2.Parse(key, intervallTime))
					{
						activeSequences.Add(sequenceState2);
					}
				}
			}
		}

		public void Reset()
		{
			activeSequences.Clear();
		}

		private void FireSequenceEvent(EventHandler<KeySequenceEventArgs> eventHandler, KeySequence keySequence)
		{
			eventHandler?.Invoke(this, new KeySequenceEventArgs(keySequence));
		}

		protected virtual void OnSequenceCompleted(KeySequence sequence)
		{
			FireSequenceEvent(this.SequenceCompleted, sequence);
		}

		protected virtual void OnKeyDown(KeyEventArgs e)
		{
			if (this.KeyDown != null)
			{
				this.KeyDown(this, e);
			}
		}

		protected virtual void OnKeyUp(KeyEventArgs e)
		{
			ParseKey(e.KeyCode | keyState);
			if (this.KeyUp != null)
			{
				this.KeyUp(this, e);
			}
		}

		private void control_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
			case Keys.ControlKey:
			case Keys.LControlKey:
				keyState |= Keys.Control;
				break;
			case Keys.Menu:
			case Keys.LMenu:
				keyState |= Keys.Menu;
				break;
			case Keys.ShiftKey:
			case Keys.LShiftKey:
				keyState |= Keys.Shift;
				break;
			default:
				OnKeyDown(e);
				break;
			}
		}

		private void control_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
			case Keys.ControlKey:
			case Keys.LControlKey:
				keyState &= ~Keys.Control;
				break;
			case Keys.Menu:
			case Keys.LMenu:
				keyState &= ~Keys.Menu;
				break;
			case Keys.ShiftKey:
			case Keys.LShiftKey:
				keyState &= ~Keys.Shift;
				break;
			default:
				OnKeyUp(e);
				break;
			}
		}

		private void InitializeComponent()
		{
			components = new Container();
		}
	}
}

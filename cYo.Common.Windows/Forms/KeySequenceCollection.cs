using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	[Serializable]
	public class KeySequenceCollection : List<KeySequence>
	{
		public KeySequence Add(string name, params Keys[] keys)
		{
			KeySequence keySequence = new KeySequence(name, keys);
			Add(keySequence);
			return keySequence;
		}
	}
}

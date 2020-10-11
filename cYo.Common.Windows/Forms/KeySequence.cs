using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	[Serializable]
	[TypeConverter(typeof(KeySequenceConverter))]
	public class KeySequence
	{
		internal class KeySequenceConverter : TypeConverter
		{
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				if (destinationType == typeof(InstanceDescriptor))
				{
					return true;
				}
				return base.CanConvertTo(context, destinationType);
			}

			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				KeySequence keySequence = value as KeySequence;
				if (keySequence != null && destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo constructor = typeof(KeySequence).GetConstructor(new Type[2]
					{
						typeof(string),
						typeof(IEnumerable<Keys>)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[2]
						{
							keySequence.Name,
							keySequence.Sequence
						});
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		private readonly List<Keys> sequence = new List<Keys>();

		public string Name
		{
			get;
			set;
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public List<Keys> Sequence => sequence;

		public KeySequence(string name, IEnumerable<Keys> sequence)
		{
			Name = name;
			this.sequence = new List<Keys>(sequence);
		}

		public KeySequence(string name, params Keys[] keys)
			: this(name, (IEnumerable<Keys>)keys)
		{
		}

		public KeySequence()
			: this(null, null)
		{
		}

		public override string ToString()
		{
			return Name;
		}
	}
}

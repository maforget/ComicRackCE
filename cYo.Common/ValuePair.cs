using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace cYo.Common
{
	[Serializable]
	public class ValuePair<K, T>
	{
		[XmlAttribute]
		[DefaultValue(null)]
		public K Key
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(null)]
		public T Value
		{
			get;
			set;
		}

		public ValuePair()
		{
		}

		public ValuePair(K key, T value)
		{
			Key = key;
			Value = value;
		}
	}
}

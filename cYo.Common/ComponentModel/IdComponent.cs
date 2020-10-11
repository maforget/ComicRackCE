using System;
using System.Xml.Serialization;

namespace cYo.Common.ComponentModel
{
	[Serializable]
	public class IdComponent : LiteComponent
	{
		private Guid id = Guid.NewGuid();

		[XmlAttribute]
		public Guid Id
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}
	}
}

using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace cYo.Common.Runtime
{
	public sealed class VersionNeutralBinder : SerializationBinder
	{
		private static readonly Regex rxVersion = new Regex(", Version=.*?PublicKeyToken=([a-f0-9]|null)*", RegexOptions.Compiled);

		public override Type BindToType(string assemblyName, string typeName)
		{
			typeName = rxVersion.Replace(typeName, string.Empty);
			return Type.GetType(typeName) ?? Type.GetType($"{typeName}, {assemblyName}");
		}
	}
}

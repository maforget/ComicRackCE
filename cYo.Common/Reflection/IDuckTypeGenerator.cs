using System;

namespace cYo.Common.Reflection
{
	internal interface IDuckTypeGenerator
	{
		Type[] CreateDuckTypes(Type interfaceType, Type[] duckedTypes);
	}
}

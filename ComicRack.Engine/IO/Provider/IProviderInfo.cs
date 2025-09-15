using System;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public interface IProviderInfo
	{
		Type ProviderType { get; }
	}
}
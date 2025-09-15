using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Presentation.Ceco;
using cYo.Common.Reflection;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo
{
	public class XmlInfoProviderFactory : ProviderFactoryBase<XmlInfoProvider>
	{
		public void RegisterProvider(Type pt, IEnumerable<XmlInfoFile> xmlInfoFile, bool withLocking = true)
		{
			using (withLocking ? rwLock.UpgradeableReadLock() : null)
			{
				if (!providerDict.Any(pi => pi.ProviderType == pt))
				{
					using (withLocking ? rwLock.WriteLock() : null)
					{
						xmlInfoFile.ForEach(x => providerDict.Add(new XmlInfoProviderInfo(pt, x)));
					}
				}
			}
		}

		public override void RegisterProvider(Type pt, bool withLocking = true)
		{
			IValidateProvider validateProvider = Activator.CreateInstance(pt) as IValidateProvider;
			if (validateProvider == null || validateProvider.IsValid)
			{
				IEnumerable<XmlInfoFile> xmlInfos = pt.GetAttributes<XmlInfoFileAttribute>().Select(ffa => new XmlInfoFile(ffa.XmlInfoFile, ffa.Order));
				RegisterProvider(pt, xmlInfos, withLocking);
			}
		}

		public IEnumerable<XmlInfoProviderInfo> GetProviderInfos()
		{
			return GetProviderInfos<XmlInfoProviderInfo>().OrderBy(x => x.XmlInfoFile.Order);
		}

		public IEnumerable<XmlInfoFile> GetXmlInfoFiles()
		{
			return GetProviderInfos().Select(p => p.XmlInfoFile);
		}

		public IEnumerable<XmlInfoProvider> CreateProviders()
		{
			return base.CreateProviders<XmlInfoProviderInfo>();
		}

		public ComicInfo DeserializeAll(Func<string, Stream> getDataDelegate)
		{
			foreach (var providerInfo in GetProviderInfos())
			{
				Type type = providerInfo.ProviderType;
				XmlInfoProvider providerObject = Activator.CreateInstance(type) as XmlInfoProvider;
				ComicInfo comicInfo = providerObject.Deserialize(getDataDelegate, providerInfo.XmlInfoFile.FileName);

				if (comicInfo is not null)
					return comicInfo;
			}
			return null;
		}
	}
}
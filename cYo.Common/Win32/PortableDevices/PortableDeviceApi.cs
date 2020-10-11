using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace cYo.Common.Win32.PortableDevices
{
	internal static class PortableDeviceApi
	{
		[ComImport]
		[Guid("0000000C-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IStream
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoteRead([Out][MarshalAs(UnmanagedType.LPArray)] byte[] pv, [In] uint cb, out uint pcbRead);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoteWrite([In][MarshalAs(UnmanagedType.LPArray)] byte[] pv, [In] uint cb, out uint pcbWritten);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoteSeek([In] long dlibMove, [In] uint dwOrigin, out ulong plibNewPosition);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetSize([In] ulong libNewSize);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoteCopyTo([In][MarshalAs(UnmanagedType.Interface)] IStream pstm, [In] ulong cb, out ulong pcbRead, out ulong pcbWritten);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Commit([In] uint grfCommitFlags);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Revert();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void LockRegion([In] ulong libOffset, [In] ulong cb, [In] uint dwLockType);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void UnlockRegion([In] ulong libOffset, [In] ulong cb, [In] uint dwLockType);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Stat(out tagSTATSTG pstatstg, [In] uint grfStatFlag);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Clone([MarshalAs(UnmanagedType.Interface)] out IStream ppstm);
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct _FILETIME
		{
			public uint dwLowDateTime;

			public uint dwHighDateTime;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct tagSTATSTG
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwcsName;

			public uint type;

			public ulong cbSize;

			public _FILETIME mtime;

			public _FILETIME ctime;

			public _FILETIME atime;

			public uint grfMode;

			public uint grfLocksSupported;

			public Guid clsid;

			public uint grfStateBits;

			public uint reserved;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct tag_inner_PROPVARIANT
		{
			public ushort vt;

			public ushort wReserved1;

			public ushort wReserved2;

			public ushort wReserved3;

			public IntPtr p;

			public int p2;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct _tagpropertykey
		{
			public Guid fmtid;

			public uint pid;
		}

		[ComImport]
		[Guid("A1567595-4C2F-4574-A6FA-ECEF917B9A40")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPortableDeviceManager
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetDevices([In][Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] pPnPDeviceIDs, [In][Out] ref uint pcPnPDeviceIDs);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RefreshDeviceList();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetDeviceFriendlyName([In][MarshalAs(UnmanagedType.LPWStr)] string pszPnPDeviceID, [In][Out] ref ushort pDeviceFriendlyName, [In][Out] ref uint pcchDeviceFriendlyName);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetDeviceDescription([In][MarshalAs(UnmanagedType.LPWStr)] string pszPnPDeviceID, [In][Out] ref ushort pDeviceDescription, [In][Out] ref uint pcchDeviceDescription);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetDeviceManufacturer([In][MarshalAs(UnmanagedType.LPWStr)] string pszPnPDeviceID, [In][Out] ref ushort pDeviceManufacturer, [In][Out] ref uint pcchDeviceManufacturer);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetDeviceProperty([In][MarshalAs(UnmanagedType.LPWStr)] string pszPnPDeviceID, [In][MarshalAs(UnmanagedType.LPWStr)] string pszDevicePropertyName, [In][Out] ref byte pData, [In][Out] ref uint pcbData, [In][Out] ref uint pdwType);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetPrivateDevices([In][Out][MarshalAs(UnmanagedType.LPWStr)] ref string pPnPDeviceIDs, [In][Out] ref uint pcPnPDeviceIDs);
		}

		[ComImport]
		[Guid("6E3F2D79-4E07-48C4-8208-D8C2E5AF4A99")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPortableDeviceValuesCollection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetCount([In] ref uint pcElems);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetAt([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppValues);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Add([In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pValues);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Clear();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoveAt([In] uint dwIndex);
		}

		[ComImport]
		[Guid("DADA2357-E0AD-492E-98DB-DD61C53BA353")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPortableDeviceKeyCollection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetCount([In] ref uint pcElems);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetAt([In] uint dwIndex, [In] ref _tagpropertykey pKey);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Add([In] ref _tagpropertykey key);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Clear();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoveAt([In] uint dwIndex);
		}

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
		public interface IPropertyStore
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetCount(out uint cProps);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetAt([In] uint iProp, out _tagpropertykey pKey);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetValue([In] ref _tagpropertykey key, out tag_inner_PROPVARIANT pv);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetValue([In] ref _tagpropertykey key, [In] ref tag_inner_PROPVARIANT propvar);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Commit();
		}

		[ComImport]
		[Guid("6848F6F2-3155-4F86-B6F5-263EEEAB3143")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComConversionLoss]
		public interface IPortableDeviceValues
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetCount([In] ref uint pcelt);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetAt([In] uint index, [In][Out] ref _tagpropertykey pKey, [In][Out] ref tag_inner_PROPVARIANT pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetValue([In] ref _tagpropertykey key, [In] ref tag_inner_PROPVARIANT pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetValue([In] ref _tagpropertykey key, out tag_inner_PROPVARIANT pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetStringValue([In] ref _tagpropertykey key, [In][MarshalAs(UnmanagedType.LPWStr)] string Value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetStringValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.LPWStr)] out string pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetUnsignedIntegerValue([In] ref _tagpropertykey key, [In] uint Value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetUnsignedIntegerValue([In] ref _tagpropertykey key, out uint pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetSignedIntegerValue([In] ref _tagpropertykey key, [In] int Value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetSignedIntegerValue([In] ref _tagpropertykey key, out int pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetUnsignedLargeIntegerValue([In] ref _tagpropertykey key, [In] ulong Value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetUnsignedLargeIntegerValue([In] ref _tagpropertykey key, out ulong pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetSignedLargeIntegerValue([In] ref _tagpropertykey key, [In] long Value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetSignedLargeIntegerValue([In] ref _tagpropertykey key, out long pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetFloatValue([In] ref _tagpropertykey key, [In] float Value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetFloatValue([In] ref _tagpropertykey key, out float pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetErrorValue([In] ref _tagpropertykey key, [In][MarshalAs(UnmanagedType.Error)] int Value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetErrorValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Error)] out int pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetKeyValue([In] ref _tagpropertykey key, [In] ref _tagpropertykey Value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetKeyValue([In] ref _tagpropertykey key, out _tagpropertykey pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetBoolValue([In] ref _tagpropertykey key, [In] int Value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetBoolValue([In] ref _tagpropertykey key, out int pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetIUnknownValue([In] ref _tagpropertykey key, [In][MarshalAs(UnmanagedType.IUnknown)] object pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetIUnknownValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.IUnknown)] out object ppValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetGuidValue([In] ref _tagpropertykey key, [In] ref Guid Value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetGuidValue([In] ref _tagpropertykey key, out Guid pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetBufferValue([In] ref _tagpropertykey key, [In] ref byte pValue, [In] uint cbValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetBufferValue([In] ref _tagpropertykey key, [Out] IntPtr ppValue, out uint pcbValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetIPortableDeviceValuesValue([In] ref _tagpropertykey key, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetIPortableDeviceValuesValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetIPortableDevicePropVariantCollectionValue([In] ref _tagpropertykey key, [In][MarshalAs(UnmanagedType.Interface)] IPortableDevicePropVariantCollection pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetIPortableDevicePropVariantCollectionValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Interface)] out IPortableDevicePropVariantCollection ppValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetIPortableDeviceKeyCollectionValue([In] ref _tagpropertykey key, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceKeyCollection pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetIPortableDeviceKeyCollectionValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceKeyCollection ppValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetIPortableDeviceValuesCollectionValue([In] ref _tagpropertykey key, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValuesCollection pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetIPortableDeviceValuesCollectionValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValuesCollection ppValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoveValue([In] ref _tagpropertykey key);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void CopyValuesFromPropertyStore([In][MarshalAs(UnmanagedType.Interface)] IPropertyStore pStore);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void CopyValuesToPropertyStore([In][MarshalAs(UnmanagedType.Interface)] IPropertyStore pStore);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Clear();
		}

		[ComImport]
		[Guid("89B2E422-4F1B-4316-BCEF-A44AFEA83EB3")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPortableDevicePropVariantCollection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetCount([In] ref uint pcElems);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetAt([In] uint dwIndex, [In] ref tag_inner_PROPVARIANT pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Add([In] ref tag_inner_PROPVARIANT pValue);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetType(out ushort pvt);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void ChangeType([In] ushort vt);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Clear();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoveAt([In] uint dwIndex);
		}

		[ComImport]
		[Guid("FD8878AC-D841-4D17-891C-E6829CDB6934")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPortableDeviceResources
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetSupportedResources([In][MarshalAs(UnmanagedType.LPWStr)] string pszObjectID, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceKeyCollection ppKeys);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetResourceAttributes([In][MarshalAs(UnmanagedType.LPWStr)] string pszObjectID, [In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppResourceAttributes);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetStream([In][MarshalAs(UnmanagedType.LPWStr)] string pszObjectID, [In] ref _tagpropertykey key, [In] uint dwMode, [In][Out] ref uint pdwOptimalBufferSize, [MarshalAs(UnmanagedType.Interface)] out IStream ppStream);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Delete([In][MarshalAs(UnmanagedType.LPWStr)] string pszObjectID, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceKeyCollection pKeys);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Cancel();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void CreateResource([In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pResourceAttributes, [MarshalAs(UnmanagedType.Interface)] out IStream ppData, [In][Out] ref uint pdwOptimalWriteBufferSize, [In][Out][MarshalAs(UnmanagedType.LPWStr)] ref string ppszCookie);
		}

		[ComImport]
		[Guid("6A96ED84-7C73-4480-9938-BF5AF477D426")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPortableDeviceContent
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void EnumObjects([In] uint dwFlags, [In][MarshalAs(UnmanagedType.LPWStr)] string pszParentObjectID, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pFilter, [MarshalAs(UnmanagedType.Interface)] out IEnumPortableDeviceObjectIDs ppenum);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Properties([MarshalAs(UnmanagedType.Interface)] out IPortableDeviceProperties ppProperties);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Transfer([MarshalAs(UnmanagedType.Interface)] out IPortableDeviceResources ppResources);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void CreateObjectWithPropertiesOnly([In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pValues, [MarshalAs(UnmanagedType.LPWStr)] out string ppszObjectID);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void CreateObjectWithPropertiesAndData([In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pValues, [MarshalAs(UnmanagedType.Interface)] out IStream ppData, [In][Out] ref uint pdwOptimalWriteBufferSize, [In][Out][MarshalAs(UnmanagedType.LPWStr)] ref string ppszCookie);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Delete([In] uint dwOptions, [In][MarshalAs(UnmanagedType.Interface)] IPortableDevicePropVariantCollection pObjectIDs, [In][Out][MarshalAs(UnmanagedType.Interface)] ref IPortableDevicePropVariantCollection ppResults);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetObjectIDsFromPersistentUniqueIDs([In][MarshalAs(UnmanagedType.Interface)] IPortableDevicePropVariantCollection pPersistentUniqueIDs, [MarshalAs(UnmanagedType.Interface)] out IPortableDevicePropVariantCollection ppObjectIDs);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Cancel();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Move([In][MarshalAs(UnmanagedType.Interface)] IPortableDevicePropVariantCollection pObjectIDs, [In][MarshalAs(UnmanagedType.LPWStr)] string pszDestinationFolderObjectID, [In][Out][MarshalAs(UnmanagedType.Interface)] ref IPortableDevicePropVariantCollection ppResults);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Copy([In][MarshalAs(UnmanagedType.Interface)] IPortableDevicePropVariantCollection pObjectIDs, [In][MarshalAs(UnmanagedType.LPWStr)] string pszDestinationFolderObjectID, [In][Out][MarshalAs(UnmanagedType.Interface)] ref IPortableDevicePropVariantCollection ppResults);
		}

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("A8792A31-F385-493C-A893-40F64EB45F6E")]
		public interface IPortableDeviceEventCallback
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void OnEvent([In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pEventParameters);
		}

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("2C8C6DBF-E3DC-4061-BECC-8542E810D126")]
		public interface IPortableDeviceCapabilities
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetSupportedCommands([MarshalAs(UnmanagedType.Interface)] out IPortableDeviceKeyCollection ppCommands);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetCommandOptions([In] ref _tagpropertykey Command, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppOptions);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetFunctionalCategories([MarshalAs(UnmanagedType.Interface)] out IPortableDevicePropVariantCollection ppCategories);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetFunctionalObjects([In] ref Guid Category, [MarshalAs(UnmanagedType.Interface)] out IPortableDevicePropVariantCollection ppObjectIDs);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetSupportedContentTypes([In] ref Guid Category, [MarshalAs(UnmanagedType.Interface)] out IPortableDevicePropVariantCollection ppContentTypes);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetSupportedFormats([In] ref Guid ContentType, [MarshalAs(UnmanagedType.Interface)] out IPortableDevicePropVariantCollection ppFormats);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetSupportedFormatProperties([In] ref Guid Format, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceKeyCollection ppKeys);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetFixedPropertyAttributes([In] ref Guid Format, [In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppAttributes);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Cancel();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetSupportedEvents([MarshalAs(UnmanagedType.Interface)] out IPortableDevicePropVariantCollection ppEvents);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetEventOptions([In] ref Guid Event, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppOptions);
		}

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("625E2DF8-6392-4CF0-9AD1-3CFA5F17775C")]
		public interface IPortableDevice
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Open([In][MarshalAs(UnmanagedType.LPWStr)] string pszPnPDeviceID, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pClientInfo);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SendCommand([In] uint dwFlags, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pParameters, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppResults);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Content([MarshalAs(UnmanagedType.Interface)] out IPortableDeviceContent ppContent);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Capabilities([MarshalAs(UnmanagedType.Interface)] out IPortableDeviceCapabilities ppCapabilities);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Cancel();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Close();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Advise([In] uint dwFlags, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceEventCallback pCallback, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pParameters, [MarshalAs(UnmanagedType.LPWStr)] out string ppszCookie);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Unadvise([In][MarshalAs(UnmanagedType.LPWStr)] string pszCookie);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetPnPDeviceID([MarshalAs(UnmanagedType.LPWStr)] out string ppszPnPDeviceID);
		}

		[ComImport]
		[Guid("7F6D695C-03DF-4439-A809-59266BEEE3A6")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPortableDeviceProperties
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetSupportedProperties([In][MarshalAs(UnmanagedType.LPWStr)] string pszObjectID, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceKeyCollection ppKeys);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetPropertyAttributes([In][MarshalAs(UnmanagedType.LPWStr)] string pszObjectID, [In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppAttributes);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetValues([In][MarshalAs(UnmanagedType.LPWStr)] string pszObjectID, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceKeyCollection pKeys, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppValues);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetValues([In][MarshalAs(UnmanagedType.LPWStr)] string pszObjectID, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceValues pValues, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppResults);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Delete([In][MarshalAs(UnmanagedType.LPWStr)] string pszObjectID, [In][MarshalAs(UnmanagedType.Interface)] IPortableDeviceKeyCollection pKeys);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Cancel();
		}

		[ComImport]
		[Guid("10ECE955-CF41-4728-BFA0-41EEDF1BBF19")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IEnumPortableDeviceObjectIDs
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Next([In] uint cObjects, [In][Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] pObjIDs, [In][Out] ref uint pcFetched);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Skip([In] uint cObjects);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Reset();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumPortableDeviceObjectIDs ppenum);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Cancel();
		}

		[ComImport]
		[Guid("88E04DB3-1012-4D64-9996-F703A950D3F4")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPortableDeviceDataStream
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoteRead([Out][MarshalAs(UnmanagedType.LPArray)] byte[] pv, [In] uint cb, out uint pcbRead);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoteWrite([In][MarshalAs(UnmanagedType.LPArray)] byte[] pv, [In] uint cb, out uint pcbWritten);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoteSeek([In] long dlibMove, [In] uint dwOrigin, out ulong plibNewPosition);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void SetSize([In] ulong libNewSize);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoteCopyTo([In][MarshalAs(UnmanagedType.Interface)] IStream pstm, [In] ulong cb, out ulong pcbRead, out ulong pcbWritten);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Commit([In] uint grfCommitFlags);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Revert();

			[MethodImpl(MethodImplOptions.InternalCall)]
			void LockRegion([In] ulong libOffset, [In] ulong cb, [In] uint dwLockType);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void UnlockRegion([In] ulong libOffset, [In] ulong cb, [In] uint dwLockType);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Stat(out tagSTATSTG pstatstg, [In] uint grfStatFlag);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Clone([MarshalAs(UnmanagedType.Interface)] out IStream ppstm);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void GetObjectID([MarshalAs(UnmanagedType.LPWStr)] out string ppszObjectID);

			[MethodImpl(MethodImplOptions.InternalCall)]
			void Cancel();
		}

		[ComImport]
		[Guid("728A21C5-3D9E-48D7-9810-864848F0F404")]
		public class PortableDevice
		{
            //Decompile Error
            //[MethodImpl(MethodImplOptions.InternalCall)]
            //public extern PortableDevice();
        }

		[ComImport]
		[Guid("0C15D503-D017-47CE-9016-7B3F978721CC")]
		public class PortableDeviceValues
		{
			//Decompile Error
			//[MethodImpl(MethodImplOptions.InternalCall)]
			//public extern PortableDeviceValues();
		}

		[ComImport]
		[Guid("08A99E2F-6D6D-4B80-AF5A-BAF2BCBE4CB9")]
		public class PortableDevicePropVariantCollection
		{
			//Decompile Error
			//[MethodImpl(MethodImplOptions.InternalCall)]
			//public extern PortableDevicePropVariantCollection();
		}

		[ComImport]
		[Guid("0AF10CEC-2ECD-4B92-9581-34F6AE0637F3")]
		public class PortableDeviceManager
		{
			//Decompile Error
			//[MethodImpl(MethodImplOptions.InternalCall)]
			//public extern PortableDeviceManager();
		}

		public static Guid GUID_DEVINTERFACE_WPD;

		public static Guid GUID_DEVINTERFACE_WPD_PRIVATE;

		public static Guid GUID_DEVINTERFACE_WPD_SERVICE;

		public static Guid WPD_EVENT_NOTIFICATION;

		public static Guid WPD_EVENT_OBJECT_ADDED;

		public static Guid WPD_EVENT_OBJECT_REMOVED;

		public static Guid WPD_EVENT_OBJECT_UPDATED;

		public static Guid WPD_EVENT_DEVICE_RESET;

		public static Guid WPD_EVENT_DEVICE_CAPABILITIES_UPDATED;

		public static Guid WPD_EVENT_STORAGE_FORMAT;

		public static Guid WPD_EVENT_OBJECT_TRANSFER_REQUESTED;

		public static Guid WPD_EVENT_DEVICE_REMOVED;

		public static Guid WPD_EVENT_SERVICE_METHOD_COMPLETE;

		public static Guid WPD_CONTENT_TYPE_FUNCTIONAL_OBJECT;

		public static Guid WPD_CONTENT_TYPE_FOLDER;

		public static Guid WPD_CONTENT_TYPE_IMAGE;

		public static Guid WPD_CONTENT_TYPE_DOCUMENT;

		public static Guid WPD_CONTENT_TYPE_CONTACT;

		public static Guid WPD_CONTENT_TYPE_CONTACT_GROUP;

		public static Guid WPD_CONTENT_TYPE_AUDIO;

		public static Guid WPD_CONTENT_TYPE_VIDEO;

		public static Guid WPD_CONTENT_TYPE_TELEVISION;

		public static Guid WPD_CONTENT_TYPE_PLAYLIST;

		public static Guid WPD_CONTENT_TYPE_MIXED_CONTENT_ALBUM;

		public static Guid WPD_CONTENT_TYPE_AUDIO_ALBUM;

		public static Guid WPD_CONTENT_TYPE_IMAGE_ALBUM;

		public static Guid WPD_CONTENT_TYPE_VIDEO_ALBUM;

		public static Guid WPD_CONTENT_TYPE_MEMO;

		public static Guid WPD_CONTENT_TYPE_EMAIL;

		public static Guid WPD_CONTENT_TYPE_APPOINTMENT;

		public static Guid WPD_CONTENT_TYPE_TASK;

		public static Guid WPD_CONTENT_TYPE_PROGRAM;

		public static Guid WPD_CONTENT_TYPE_GENERIC_FILE;

		public static Guid WPD_CONTENT_TYPE_CALENDAR;

		public static Guid WPD_CONTENT_TYPE_GENERIC_MESSAGE;

		public static Guid WPD_CONTENT_TYPE_NETWORK_ASSOCIATION;

		public static Guid WPD_CONTENT_TYPE_CERTIFICATE;

		public static Guid WPD_CONTENT_TYPE_WIRELESS_PROFILE;

		public static Guid WPD_CONTENT_TYPE_MEDIA_CAST;

		public static Guid WPD_CONTENT_TYPE_SECTION;

		public static Guid WPD_CONTENT_TYPE_UNSPECIFIED;

		public static Guid WPD_CONTENT_TYPE_ALL;

		public static Guid WPD_FUNCTIONAL_CATEGORY_DEVICE;

		public static Guid WPD_FUNCTIONAL_CATEGORY_STORAGE;

		public static Guid WPD_FUNCTIONAL_CATEGORY_STILL_IMAGE_CAPTURE;

		public static Guid WPD_FUNCTIONAL_CATEGORY_AUDIO_CAPTURE;

		public static Guid WPD_FUNCTIONAL_CATEGORY_VIDEO_CAPTURE;

		public static Guid WPD_FUNCTIONAL_CATEGORY_SMS;

		public static Guid WPD_FUNCTIONAL_CATEGORY_RENDERING_INFORMATION;

		public static Guid WPD_FUNCTIONAL_CATEGORY_NETWORK_CONFIGURATION;

		public static Guid WPD_FUNCTIONAL_CATEGORY_ALL;

		public static Guid WPD_OBJECT_FORMAT_PROPERTIES_ONLY;

		public static Guid WPD_OBJECT_FORMAT_UNSPECIFIED;

		public static Guid WPD_OBJECT_FORMAT_SCRIPT;

		public static Guid WPD_OBJECT_FORMAT_EXECUTABLE;

		public static Guid WPD_OBJECT_FORMAT_TEXT;

		public static Guid WPD_OBJECT_FORMAT_HTML;

		public static Guid WPD_OBJECT_FORMAT_DPOF;

		public static Guid WPD_OBJECT_FORMAT_AIFF;

		public static Guid WPD_OBJECT_FORMAT_WAVE;

		public static Guid WPD_OBJECT_FORMAT_MP3;

		public static Guid WPD_OBJECT_FORMAT_AVI;

		public static Guid WPD_OBJECT_FORMAT_MPEG;

		public static Guid WPD_OBJECT_FORMAT_ASF;

		public static Guid WPD_OBJECT_FORMAT_EXIF;

		public static Guid WPD_OBJECT_FORMAT_TIFFEP;

		public static Guid WPD_OBJECT_FORMAT_FLASHPIX;

		public static Guid WPD_OBJECT_FORMAT_BMP;

		public static Guid WPD_OBJECT_FORMAT_CIFF;

		public static Guid WPD_OBJECT_FORMAT_GIF;

		public static Guid WPD_OBJECT_FORMAT_JFIF;

		public static Guid WPD_OBJECT_FORMAT_PCD;

		public static Guid WPD_OBJECT_FORMAT_PICT;

		public static Guid WPD_OBJECT_FORMAT_PNG;

		public static Guid WPD_OBJECT_FORMAT_TIFF;

		public static Guid WPD_OBJECT_FORMAT_TIFFIT;

		public static Guid WPD_OBJECT_FORMAT_JP2;

		public static Guid WPD_OBJECT_FORMAT_JPX;

		public static Guid WPD_OBJECT_FORMAT_WINDOWSIMAGEFORMAT;

		public static Guid WPD_OBJECT_FORMAT_WMA;

		public static Guid WPD_OBJECT_FORMAT_WMV;

		public static Guid WPD_OBJECT_FORMAT_WPLPLAYLIST;

		public static Guid WPD_OBJECT_FORMAT_M3UPLAYLIST;

		public static Guid WPD_OBJECT_FORMAT_MPLPLAYLIST;

		public static Guid WPD_OBJECT_FORMAT_ASXPLAYLIST;

		public static Guid WPD_OBJECT_FORMAT_PLSPLAYLIST;

		public static Guid WPD_OBJECT_FORMAT_ABSTRACT_CONTACT_GROUP;

		public static Guid WPD_OBJECT_FORMAT_ABSTRACT_MEDIA_CAST;

		public static Guid WPD_OBJECT_FORMAT_VCALENDAR1;

		public static Guid WPD_OBJECT_FORMAT_ICALENDAR;

		public static Guid WPD_OBJECT_FORMAT_ABSTRACT_CONTACT;

		public static Guid WPD_OBJECT_FORMAT_VCARD2;

		public static Guid WPD_OBJECT_FORMAT_VCARD3;

		public static Guid WPD_OBJECT_FORMAT_ICON;

		public static Guid WPD_OBJECT_FORMAT_XML;

		public static Guid WPD_OBJECT_FORMAT_AAC;

		public static Guid WPD_OBJECT_FORMAT_AUDIBLE;

		public static Guid WPD_OBJECT_FORMAT_FLAC;

		public static Guid WPD_OBJECT_FORMAT_OGG;

		public static Guid WPD_OBJECT_FORMAT_MP4;

		public static Guid WPD_OBJECT_FORMAT_M4A;

		public static Guid WPD_OBJECT_FORMAT_MP2;

		public static Guid WPD_OBJECT_FORMAT_MICROSOFT_WORD;

		public static Guid WPD_OBJECT_FORMAT_MHT_COMPILED_HTML;

		public static Guid WPD_OBJECT_FORMAT_MICROSOFT_EXCEL;

		public static Guid WPD_OBJECT_FORMAT_MICROSOFT_POWERPOINT;

		public static Guid WPD_OBJECT_FORMAT_NETWORK_ASSOCIATION;

		public static Guid WPD_OBJECT_FORMAT_X509V3CERTIFICATE;

		public static Guid WPD_OBJECT_FORMAT_MICROSOFT_WFC;

		public static Guid WPD_OBJECT_FORMAT_3GP;

		public static Guid WPD_OBJECT_FORMAT_3GPA;

		public static Guid WPD_OBJECT_FORMAT_ALL;

		public static _tagpropertykey WPD_OBJECT_ID;

		public static _tagpropertykey WPD_OBJECT_PARENT_ID;

		public static _tagpropertykey WPD_OBJECT_NAME;

		public static _tagpropertykey WPD_OBJECT_PERSISTENT_UNIQUE_ID;

		public static _tagpropertykey WPD_OBJECT_FORMAT;

		public static _tagpropertykey WPD_OBJECT_CONTENT_TYPE;

		public static _tagpropertykey WPD_OBJECT_ISHIDDEN;

		public static _tagpropertykey WPD_OBJECT_ISSYSTEM;

		public static _tagpropertykey WPD_OBJECT_SIZE;

		public static _tagpropertykey WPD_OBJECT_ORIGINAL_FILE_NAME;

		public static _tagpropertykey WPD_OBJECT_NON_CONSUMABLE;

		public static _tagpropertykey WPD_OBJECT_REFERENCES;

		public static _tagpropertykey WPD_OBJECT_KEYWORDS;

		public static _tagpropertykey WPD_OBJECT_SYNC_ID;

		public static _tagpropertykey WPD_OBJECT_IS_DRM_PROTECTED;

		public static _tagpropertykey WPD_OBJECT_DATE_CREATED;

		public static _tagpropertykey WPD_OBJECT_DATE_MODIFIED;

		public static _tagpropertykey WPD_OBJECT_DATE_AUTHORED;

		public static _tagpropertykey WPD_OBJECT_BACK_REFERENCES;

		public static _tagpropertykey WPD_OBJECT_CONTAINER_FUNCTIONAL_OBJECT_ID;

		public static _tagpropertykey WPD_OBJECT_GENERATE_THUMBNAIL_FROM_RESOURCE;

		public static _tagpropertykey WPD_OBJECT_HINT_LOCATION_DISPLAY_NAME;

		public static _tagpropertykey WPD_OBJECT_CAN_DELETE;

		public static _tagpropertykey WPD_OBJECT_LANGUAGE_LOCALE;

		public static _tagpropertykey WPD_FOLDER_CONTENT_TYPES_ALLOWED;

		public static _tagpropertykey WPD_IMAGE_BITDEPTH;

		public static _tagpropertykey WPD_IMAGE_CROPPED_STATUS;

		public static _tagpropertykey WPD_IMAGE_COLOR_CORRECTED_STATUS;

		public static _tagpropertykey WPD_IMAGE_FNUMBER;

		public static _tagpropertykey WPD_IMAGE_EXPOSURE_TIME;

		public static _tagpropertykey WPD_IMAGE_EXPOSURE_INDEX;

		public static _tagpropertykey WPD_IMAGE_HORIZONTAL_RESOLUTION;

		public static _tagpropertykey WPD_IMAGE_VERTICAL_RESOLUTION;

		public static _tagpropertykey WPD_MEDIA_TOTAL_BITRATE;

		public static _tagpropertykey WPD_MEDIA_BITRATE_TYPE;

		public static _tagpropertykey WPD_MEDIA_COPYRIGHT;

		public static _tagpropertykey WPD_MEDIA_SUBSCRIPTION_CONTENT_ID;

		public static _tagpropertykey WPD_MEDIA_USE_COUNT;

		public static _tagpropertykey WPD_MEDIA_SKIP_COUNT;

		public static _tagpropertykey WPD_MEDIA_LAST_ACCESSED_TIME;

		public static _tagpropertykey WPD_MEDIA_PARENTAL_RATING;

		public static _tagpropertykey WPD_MEDIA_META_GENRE;

		public static _tagpropertykey WPD_MEDIA_COMPOSER;

		public static _tagpropertykey WPD_MEDIA_EFFECTIVE_RATING;

		public static _tagpropertykey WPD_MEDIA_SUB_TITLE;

		public static _tagpropertykey WPD_MEDIA_RELEASE_DATE;

		public static _tagpropertykey WPD_MEDIA_SAMPLE_RATE;

		public static _tagpropertykey WPD_MEDIA_STAR_RATING;

		public static _tagpropertykey WPD_MEDIA_USER_EFFECTIVE_RATING;

		public static _tagpropertykey WPD_MEDIA_TITLE;

		public static _tagpropertykey WPD_MEDIA_DURATION;

		public static _tagpropertykey WPD_MEDIA_BUY_NOW;

		public static _tagpropertykey WPD_MEDIA_ENCODING_PROFILE;

		public static _tagpropertykey WPD_MEDIA_WIDTH;

		public static _tagpropertykey WPD_MEDIA_HEIGHT;

		public static _tagpropertykey WPD_MEDIA_ARTIST;

		public static _tagpropertykey WPD_MEDIA_ALBUM_ARTIST;

		public static _tagpropertykey WPD_MEDIA_OWNER;

		public static _tagpropertykey WPD_MEDIA_MANAGING_EDITOR;

		public static _tagpropertykey WPD_MEDIA_WEBMASTER;

		public static _tagpropertykey WPD_MEDIA_SOURCE_URL;

		public static _tagpropertykey WPD_MEDIA_DESTINATION_URL;

		public static _tagpropertykey WPD_MEDIA_DESCRIPTION;

		public static _tagpropertykey WPD_MEDIA_GENRE;

		public static _tagpropertykey WPD_MEDIA_TIME_BOOKMARK;

		public static _tagpropertykey WPD_MEDIA_OBJECT_BOOKMARK;

		public static _tagpropertykey WPD_MEDIA_LAST_BUILD_DATE;

		public static _tagpropertykey WPD_MEDIA_BYTE_BOOKMARK;

		public static _tagpropertykey WPD_MEDIA_TIME_TO_LIVE;

		public static _tagpropertykey WPD_MEDIA_GUID;

		public static _tagpropertykey WPD_MEDIA_SUB_DESCRIPTION;

		public static _tagpropertykey WPD_CONTACT_DISPLAY_NAME;

		public static _tagpropertykey WPD_CONTACT_FIRST_NAME;

		public static _tagpropertykey WPD_CONTACT_MIDDLE_NAMES;

		public static _tagpropertykey WPD_CONTACT_LAST_NAME;

		public static _tagpropertykey WPD_CONTACT_PREFIX;

		public static _tagpropertykey WPD_CONTACT_SUFFIX;

		public static _tagpropertykey WPD_CONTACT_PHONETIC_FIRST_NAME;

		public static _tagpropertykey WPD_CONTACT_PHONETIC_LAST_NAME;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_FULL_POSTAL_ADDRESS;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_LINE1;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_LINE2;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_CITY;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_REGION;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_POSTAL_CODE;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_COUNTRY;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_FULL_POSTAL_ADDRESS;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_LINE1;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_LINE2;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_CITY;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_REGION;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_POSTAL_CODE;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_COUNTRY;

		public static _tagpropertykey WPD_CONTACT_OTHER_FULL_POSTAL_ADDRESS;

		public static _tagpropertykey WPD_CONTACT_OTHER_POSTAL_ADDRESS_LINE1;

		public static _tagpropertykey WPD_CONTACT_OTHER_POSTAL_ADDRESS_LINE2;

		public static _tagpropertykey WPD_CONTACT_OTHER_POSTAL_ADDRESS_CITY;

		public static _tagpropertykey WPD_CONTACT_OTHER_POSTAL_ADDRESS_REGION;

		public static _tagpropertykey WPD_CONTACT_OTHER_POSTAL_ADDRESS_POSTAL_CODE;

		public static _tagpropertykey WPD_CONTACT_OTHER_POSTAL_ADDRESS_POSTAL_COUNTRY;

		public static _tagpropertykey WPD_CONTACT_PRIMARY_EMAIL_ADDRESS;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_EMAIL;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_EMAIL2;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_EMAIL;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_EMAIL2;

		public static _tagpropertykey WPD_CONTACT_OTHER_EMAILS;

		public static _tagpropertykey WPD_CONTACT_PRIMARY_PHONE;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_PHONE;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_PHONE2;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_PHONE;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_PHONE2;

		public static _tagpropertykey WPD_CONTACT_MOBILE_PHONE;

		public static _tagpropertykey WPD_CONTACT_MOBILE_PHONE2;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_FAX;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_FAX;

		public static _tagpropertykey WPD_CONTACT_PAGER;

		public static _tagpropertykey WPD_CONTACT_OTHER_PHONES;

		public static _tagpropertykey WPD_CONTACT_PRIMARY_WEB_ADDRESS;

		public static _tagpropertykey WPD_CONTACT_PERSONAL_WEB_ADDRESS;

		public static _tagpropertykey WPD_CONTACT_BUSINESS_WEB_ADDRESS;

		public static _tagpropertykey WPD_CONTACT_INSTANT_MESSENGER;

		public static _tagpropertykey WPD_CONTACT_INSTANT_MESSENGER2;

		public static _tagpropertykey WPD_CONTACT_INSTANT_MESSENGER3;

		public static _tagpropertykey WPD_CONTACT_COMPANY_NAME;

		public static _tagpropertykey WPD_CONTACT_PHONETIC_COMPANY_NAME;

		public static _tagpropertykey WPD_CONTACT_ROLE;

		public static _tagpropertykey WPD_CONTACT_BIRTHDATE;

		public static _tagpropertykey WPD_CONTACT_PRIMARY_FAX;

		public static _tagpropertykey WPD_CONTACT_SPOUSE;

		public static _tagpropertykey WPD_CONTACT_CHILDREN;

		public static _tagpropertykey WPD_CONTACT_ASSISTANT;

		public static _tagpropertykey WPD_CONTACT_ANNIVERSARY_DATE;

		public static _tagpropertykey WPD_CONTACT_RINGTONE;

		public static _tagpropertykey WPD_MUSIC_ALBUM;

		public static _tagpropertykey WPD_MUSIC_TRACK;

		public static _tagpropertykey WPD_MUSIC_LYRICS;

		public static _tagpropertykey WPD_MUSIC_MOOD;

		public static _tagpropertykey WPD_AUDIO_BITRATE;

		public static _tagpropertykey WPD_AUDIO_CHANNEL_COUNT;

		public static _tagpropertykey WPD_AUDIO_FORMAT_CODE;

		public static _tagpropertykey WPD_AUDIO_BIT_DEPTH;

		public static _tagpropertykey WPD_AUDIO_BLOCK_ALIGNMENT;

		public static _tagpropertykey WPD_VIDEO_AUTHOR;

		public static _tagpropertykey WPD_VIDEO_RECORDEDTV_STATION_NAME;

		public static _tagpropertykey WPD_VIDEO_RECORDEDTV_CHANNEL_NUMBER;

		public static _tagpropertykey WPD_VIDEO_RECORDEDTV_REPEAT;

		public static _tagpropertykey WPD_VIDEO_BUFFER_SIZE;

		public static _tagpropertykey WPD_VIDEO_CREDITS;

		public static _tagpropertykey WPD_VIDEO_KEY_FRAME_DISTANCE;

		public static _tagpropertykey WPD_VIDEO_QUALITY_SETTING;

		public static _tagpropertykey WPD_VIDEO_SCAN_TYPE;

		public static _tagpropertykey WPD_VIDEO_BITRATE;

		public static _tagpropertykey WPD_VIDEO_FOURCC_CODE;

		public static _tagpropertykey WPD_VIDEO_FRAMERATE;

		public static _tagpropertykey WPD_COMMON_INFORMATION_SUBJECT;

		public static _tagpropertykey WPD_COMMON_INFORMATION_BODY_TEXT;

		public static _tagpropertykey WPD_COMMON_INFORMATION_PRIORITY;

		public static _tagpropertykey WPD_COMMON_INFORMATION_START_DATETIME;

		public static _tagpropertykey WPD_COMMON_INFORMATION_END_DATETIME;

		public static _tagpropertykey WPD_COMMON_INFORMATION_NOTES;

		public static _tagpropertykey WPD_EMAIL_TO_LINE;

		public static _tagpropertykey WPD_EMAIL_CC_LINE;

		public static _tagpropertykey WPD_EMAIL_BCC_LINE;

		public static _tagpropertykey WPD_EMAIL_HAS_BEEN_READ;

		public static _tagpropertykey WPD_EMAIL_RECEIVED_TIME;

		public static _tagpropertykey WPD_EMAIL_HAS_ATTACHMENTS;

		public static _tagpropertykey WPD_EMAIL_SENDER_ADDRESS;

		public static _tagpropertykey WPD_APPOINTMENT_LOCATION;

		public static _tagpropertykey WPD_APPOINTMENT_TYPE;

		public static _tagpropertykey WPD_APPOINTMENT_REQUIRED_ATTENDEES;

		public static _tagpropertykey WPD_APPOINTMENT_OPTIONAL_ATTENDEES;

		public static _tagpropertykey WPD_APPOINTMENT_ACCEPTED_ATTENDEES;

		public static _tagpropertykey WPD_APPOINTMENT_RESOURCES;

		public static _tagpropertykey WPD_APPOINTMENT_TENTATIVE_ATTENDEES;

		public static _tagpropertykey WPD_APPOINTMENT_DECLINED_ATTENDEES;

		public static _tagpropertykey WPD_TASK_STATUS;

		public static _tagpropertykey WPD_TASK_PERCENT_COMPLETE;

		public static _tagpropertykey WPD_TASK_REMINDER_DATE;

		public static _tagpropertykey WPD_TASK_OWNER;

		public static _tagpropertykey WPD_NETWORK_ASSOCIATION_HOST_NETWORK_IDENTIFIERS;

		public static _tagpropertykey WPD_NETWORK_ASSOCIATION_X509V3SEQUENCE;

		public static _tagpropertykey WPD_STILL_IMAGE_CAPTURE_RESOLUTION;

		public static _tagpropertykey WPD_STILL_IMAGE_CAPTURE_FORMAT;

		public static _tagpropertykey WPD_STILL_IMAGE_COMPRESSION_SETTING;

		public static _tagpropertykey WPD_STILL_IMAGE_WHITE_BALANCE;

		public static _tagpropertykey WPD_STILL_IMAGE_RGB_GAIN;

		public static _tagpropertykey WPD_STILL_IMAGE_FNUMBER;

		public static _tagpropertykey WPD_STILL_IMAGE_FOCAL_LENGTH;

		public static _tagpropertykey WPD_STILL_IMAGE_FOCUS_DISTANCE;

		public static _tagpropertykey WPD_STILL_IMAGE_FOCUS_MODE;

		public static _tagpropertykey WPD_STILL_IMAGE_EXPOSURE_METERING_MODE;

		public static _tagpropertykey WPD_STILL_IMAGE_FLASH_MODE;

		public static _tagpropertykey WPD_STILL_IMAGE_EXPOSURE_TIME;

		public static _tagpropertykey WPD_STILL_IMAGE_EXPOSURE_PROGRAM_MODE;

		public static _tagpropertykey WPD_STILL_IMAGE_EXPOSURE_INDEX;

		public static _tagpropertykey WPD_STILL_IMAGE_EXPOSURE_BIAS_COMPENSATION;

		public static _tagpropertykey WPD_STILL_IMAGE_CAPTURE_DELAY;

		public static _tagpropertykey WPD_STILL_IMAGE_CAPTURE_MODE;

		public static _tagpropertykey WPD_STILL_IMAGE_CONTRAST;

		public static _tagpropertykey WPD_STILL_IMAGE_SHARPNESS;

		public static _tagpropertykey WPD_STILL_IMAGE_DIGITAL_ZOOM;

		public static _tagpropertykey WPD_STILL_IMAGE_EFFECT_MODE;

		public static _tagpropertykey WPD_STILL_IMAGE_BURST_NUMBER;

		public static _tagpropertykey WPD_STILL_IMAGE_BURST_INTERVAL;

		public static _tagpropertykey WPD_STILL_IMAGE_TIMELAPSE_NUMBER;

		public static _tagpropertykey WPD_STILL_IMAGE_TIMELAPSE_INTERVAL;

		public static _tagpropertykey WPD_STILL_IMAGE_FOCUS_METERING_MODE;

		public static _tagpropertykey WPD_STILL_IMAGE_UPLOAD_URL;

		public static _tagpropertykey WPD_STILL_IMAGE_ARTIST;

		public static _tagpropertykey WPD_STILL_IMAGE_CAMERA_MODEL;

		public static _tagpropertykey WPD_STILL_IMAGE_CAMERA_MANUFACTURER;

		public static _tagpropertykey WPD_SMS_PROVIDER;

		public static _tagpropertykey WPD_SMS_TIMEOUT;

		public static _tagpropertykey WPD_SMS_MAX_PAYLOAD;

		public static _tagpropertykey WPD_SMS_ENCODING;

		public static _tagpropertykey WPD_SECTION_DATA_OFFSET;

		public static _tagpropertykey WPD_SECTION_DATA_LENGTH;

		public static _tagpropertykey WPD_SECTION_DATA_UNITS;

		public static _tagpropertykey WPD_SECTION_DATA_REFERENCED_OBJECT_RESOURCE;

		public static _tagpropertykey WPD_RENDERING_INFORMATION_PROFILES;

		public static _tagpropertykey WPD_RENDERING_INFORMATION_PROFILE_ENTRY_TYPE;

		public static _tagpropertykey WPD_RENDERING_INFORMATION_PROFILE_ENTRY_CREATABLE_RESOURCES;

		public static _tagpropertykey WPD_COMMAND_STORAGE_FORMAT;

		public static _tagpropertykey WPD_COMMAND_STORAGE_EJECT;

		public static _tagpropertykey WPD_PROPERTY_STORAGE_OBJECT_ID;

		public static _tagpropertykey WPD_PROPERTY_STORAGE_DESTINATION_OBJECT_ID;

		public static _tagpropertykey WPD_COMMAND_SMS_SEND;

		public static _tagpropertykey WPD_PROPERTY_SMS_RECIPIENT;

		public static _tagpropertykey WPD_PROPERTY_SMS_MESSAGE_TYPE;

		public static _tagpropertykey WPD_PROPERTY_SMS_TEXT_MESSAGE;

		public static _tagpropertykey WPD_PROPERTY_SMS_BINARY_MESSAGE;

		public static _tagpropertykey WPD_OPTION_SMS_BINARY_MESSAGE_SUPPORTED;

		public static _tagpropertykey WPD_COMMAND_STILL_IMAGE_CAPTURE_INITIATE;

		public static _tagpropertykey WPD_COMMAND_MEDIA_CAPTURE_START;

		public static _tagpropertykey WPD_COMMAND_MEDIA_CAPTURE_STOP;

		public static _tagpropertykey WPD_COMMAND_MEDIA_CAPTURE_PAUSE;

		public static _tagpropertykey WPD_COMMAND_DEVICE_HINTS_GET_CONTENT_LOCATION;

		public static _tagpropertykey WPD_PROPERTY_DEVICE_HINTS_CONTENT_TYPE;

		public static _tagpropertykey WPD_PROPERTY_DEVICE_HINTS_CONTENT_LOCATIONS;

		public static _tagpropertykey WPD_COMMAND_GENERATE_KEYPAIR;

		public static _tagpropertykey WPD_COMMAND_COMMIT_KEYPAIR;

		public static _tagpropertykey WPD_COMMAND_PROCESS_WIRELESS_PROFILE;

		public static _tagpropertykey WPD_PROPERTY_PUBLIC_KEY;

		public static _tagpropertykey WPD_RESOURCE_DEFAULT;

		public static _tagpropertykey WPD_RESOURCE_CONTACT_PHOTO;

		public static _tagpropertykey WPD_RESOURCE_THUMBNAIL;

		public static _tagpropertykey WPD_RESOURCE_ICON;

		public static _tagpropertykey WPD_RESOURCE_AUDIO_CLIP;

		public static _tagpropertykey WPD_RESOURCE_ALBUM_ART;

		public static _tagpropertykey WPD_RESOURCE_GENERIC;

		public static _tagpropertykey WPD_RESOURCE_VIDEO_CLIP;

		public static _tagpropertykey WPD_RESOURCE_BRANDING_ART;

		public static _tagpropertykey WPD_PROPERTY_NULL;

		public static _tagpropertykey WPD_FUNCTIONAL_OBJECT_CATEGORY;

		public static _tagpropertykey WPD_STORAGE_TYPE;

		public static _tagpropertykey WPD_STORAGE_FILE_SYSTEM_TYPE;

		public static _tagpropertykey WPD_STORAGE_CAPACITY;

		public static _tagpropertykey WPD_STORAGE_FREE_SPACE_IN_BYTES;

		public static _tagpropertykey WPD_STORAGE_FREE_SPACE_IN_OBJECTS;

		public static _tagpropertykey WPD_STORAGE_DESCRIPTION;

		public static _tagpropertykey WPD_STORAGE_SERIAL_NUMBER;

		public static _tagpropertykey WPD_STORAGE_MAX_OBJECT_SIZE;

		public static _tagpropertykey WPD_STORAGE_CAPACITY_IN_OBJECTS;

		public static _tagpropertykey WPD_STORAGE_ACCESS_CAPABILITY;

		public static _tagpropertykey WPD_CLIENT_NAME;

		public static _tagpropertykey WPD_CLIENT_MAJOR_VERSION;

		public static _tagpropertykey WPD_CLIENT_MINOR_VERSION;

		public static _tagpropertykey WPD_CLIENT_REVISION;

		public static _tagpropertykey WPD_CLIENT_WMDRM_APPLICATION_PRIVATE_KEY;

		public static _tagpropertykey WPD_CLIENT_WMDRM_APPLICATION_CERTIFICATE;

		public static _tagpropertykey WPD_CLIENT_SECURITY_QUALITY_OF_SERVICE;

		public static _tagpropertykey WPD_CLIENT_DESIRED_ACCESS;

		public static _tagpropertykey WPD_CLIENT_SHARE_MODE;

		public static _tagpropertykey WPD_CLIENT_EVENT_COOKIE;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_FORM;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_CAN_READ;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_CAN_WRITE;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_CAN_DELETE;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_DEFAULT_VALUE;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_FAST_PROPERTY;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_RANGE_MIN;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_RANGE_MAX;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_RANGE_STEP;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_ENUMERATION_ELEMENTS;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_REGULAR_EXPRESSION;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_MAX_SIZE;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_NAME;

		public static _tagpropertykey WPD_PROPERTY_ATTRIBUTE_VARTYPE;

		public static _tagpropertykey WPD_CLASS_EXTENSION_OPTIONS_SUPPORTED_CONTENT_TYPES;

		public static _tagpropertykey WPD_CLASS_EXTENSION_OPTIONS_DONT_REGISTER_WPD_DEVICE_INTERFACE;

		public static _tagpropertykey WPD_CLASS_EXTENSION_OPTIONS_REGISTER_WPD_PRIVATE_DEVICE_INTERFACE;

		public static _tagpropertykey WPD_CLASS_EXTENSION_OPTIONS_MULTITRANSPORT_MODE;

		public static _tagpropertykey WPD_CLASS_EXTENSION_OPTIONS_DEVICE_IDENTIFICATION_VALUES;

		public static _tagpropertykey WPD_CLASS_EXTENSION_OPTIONS_TRANSPORT_BANDWIDTH;

		public static _tagpropertykey WPD_RESOURCE_ATTRIBUTE_TOTAL_SIZE;

		public static _tagpropertykey WPD_RESOURCE_ATTRIBUTE_CAN_READ;

		public static _tagpropertykey WPD_RESOURCE_ATTRIBUTE_CAN_WRITE;

		public static _tagpropertykey WPD_RESOURCE_ATTRIBUTE_CAN_DELETE;

		public static _tagpropertykey WPD_RESOURCE_ATTRIBUTE_OPTIMAL_READ_BUFFER_SIZE;

		public static _tagpropertykey WPD_RESOURCE_ATTRIBUTE_OPTIMAL_WRITE_BUFFER_SIZE;

		public static _tagpropertykey WPD_RESOURCE_ATTRIBUTE_FORMAT;

		public static _tagpropertykey WPD_RESOURCE_ATTRIBUTE_RESOURCE_KEY;

		public static _tagpropertykey WPD_DEVICE_SYNC_PARTNER;

		public static _tagpropertykey WPD_DEVICE_FIRMWARE_VERSION;

		public static _tagpropertykey WPD_DEVICE_POWER_LEVEL;

		public static _tagpropertykey WPD_DEVICE_POWER_SOURCE;

		public static _tagpropertykey WPD_DEVICE_PROTOCOL;

		public static _tagpropertykey WPD_DEVICE_MANUFACTURER;

		public static _tagpropertykey WPD_DEVICE_MODEL;

		public static _tagpropertykey WPD_DEVICE_SERIAL_NUMBER;

		public static _tagpropertykey WPD_DEVICE_SUPPORTS_NON_CONSUMABLE;

		public static _tagpropertykey WPD_DEVICE_DATETIME;

		public static _tagpropertykey WPD_DEVICE_FRIENDLY_NAME;

		public static _tagpropertykey WPD_DEVICE_SUPPORTED_DRM_SCHEMES;

		public static _tagpropertykey WPD_DEVICE_SUPPORTED_FORMATS_ARE_ORDERED;

		public static _tagpropertykey WPD_DEVICE_TYPE;

		public static _tagpropertykey WPD_DEVICE_NETWORK_IDENTIFIER;

		public static _tagpropertykey WPD_DEVICE_FUNCTIONAL_UNIQUE_ID;

		public static _tagpropertykey WPD_DEVICE_MODEL_UNIQUE_ID;

		public static _tagpropertykey WPD_DEVICE_TRANSPORT;

		public static _tagpropertykey WPD_DEVICE_USE_DEVICE_STAGE;

		public static _tagpropertykey WPD_SERVICE_VERSION;

		public static _tagpropertykey WPD_EVENT_PARAMETER_PNP_DEVICE_ID;

		public static _tagpropertykey WPD_EVENT_PARAMETER_EVENT_ID;

		public static _tagpropertykey WPD_EVENT_PARAMETER_OPERATION_STATE;

		public static _tagpropertykey WPD_EVENT_PARAMETER_OPERATION_PROGRESS;

		public static _tagpropertykey WPD_EVENT_PARAMETER_OBJECT_PARENT_PERSISTENT_UNIQUE_ID;

		public static _tagpropertykey WPD_EVENT_PARAMETER_OBJECT_CREATION_COOKIE;

		public static _tagpropertykey WPD_EVENT_PARAMETER_CHILD_HIERARCHY_CHANGED;

		public static _tagpropertykey WPD_EVENT_PARAMETER_SERVICE_METHOD_CONTEXT;

		public static _tagpropertykey WPD_EVENT_OPTION_IS_BROADCAST_EVENT;

		public static _tagpropertykey WPD_EVENT_OPTION_IS_AUTOPLAY_EVENT;

		public static _tagpropertykey WPD_EVENT_ATTRIBUTE_NAME;

		public static _tagpropertykey WPD_EVENT_ATTRIBUTE_PARAMETERS;

		public static _tagpropertykey WPD_EVENT_ATTRIBUTE_OPTIONS;

		public static _tagpropertykey WPD_API_OPTION_USE_CLEAR_DATA_STREAM;

		public static _tagpropertykey WPD_API_OPTION_IOCTL_ACCESS;

		public static _tagpropertykey WPD_FORMAT_ATTRIBUTE_NAME;

		public static _tagpropertykey WPD_FORMAT_ATTRIBUTE_MIMETYPE;

		public static _tagpropertykey WPD_METHOD_ATTRIBUTE_NAME;

		public static _tagpropertykey WPD_METHOD_ATTRIBUTE_ASSOCIATED_FORMAT;

		public static _tagpropertykey WPD_METHOD_ATTRIBUTE_ACCESS;

		public static _tagpropertykey WPD_METHOD_ATTRIBUTE_PARAMETERS;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_ORDER;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_USAGE;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_FORM;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_DEFAULT_VALUE;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_RANGE_MIN;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_RANGE_MAX;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_RANGE_STEP;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_ENUMERATION_ELEMENTS;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_REGULAR_EXPRESSION;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_MAX_SIZE;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_VARTYPE;

		public static _tagpropertykey WPD_PARAMETER_ATTRIBUTE_NAME;

		public static _tagpropertykey WPD_COMMAND_COMMON_RESET_DEVICE;

		public static _tagpropertykey WPD_COMMAND_COMMON_GET_OBJECT_IDS_FROM_PERSISTENT_UNIQUE_IDS;

		public static _tagpropertykey WPD_COMMAND_COMMON_SAVE_CLIENT_INFORMATION;

		public static _tagpropertykey WPD_PROPERTY_COMMON_COMMAND_CATEGORY;

		public static _tagpropertykey WPD_PROPERTY_COMMON_COMMAND_ID;

		public static _tagpropertykey WPD_PROPERTY_COMMON_HRESULT;

		public static _tagpropertykey WPD_PROPERTY_COMMON_DRIVER_ERROR_CODE;

		public static _tagpropertykey WPD_PROPERTY_COMMON_COMMAND_TARGET;

		public static _tagpropertykey WPD_PROPERTY_COMMON_PERSISTENT_UNIQUE_IDS;

		public static _tagpropertykey WPD_PROPERTY_COMMON_OBJECT_IDS;

		public static _tagpropertykey WPD_PROPERTY_COMMON_CLIENT_INFORMATION;

		public static _tagpropertykey WPD_PROPERTY_COMMON_CLIENT_INFORMATION_CONTEXT;

		public static _tagpropertykey WPD_OPTION_VALID_OBJECT_IDS;

		public static _tagpropertykey WPD_COMMAND_OBJECT_ENUMERATION_START_FIND;

		public static _tagpropertykey WPD_COMMAND_OBJECT_ENUMERATION_FIND_NEXT;

		public static _tagpropertykey WPD_COMMAND_OBJECT_ENUMERATION_END_FIND;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_ENUMERATION_PARENT_ID;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_ENUMERATION_FILTER;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_ENUMERATION_OBJECT_IDS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_ENUMERATION_CONTEXT;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_ENUMERATION_NUM_OBJECTS_REQUESTED;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_GET_SUPPORTED;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_GET_ATTRIBUTES;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_GET;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_SET;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_GET_ALL;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_DELETE;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_OBJECT_ID;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_KEYS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_ATTRIBUTES;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_VALUES;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_WRITE_RESULTS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_DELETE_RESULTS;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_START;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_NEXT;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_END;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_START;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_NEXT;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_END;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_START;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_NEXT;

		public static _tagpropertykey WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_END;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_BULK_OBJECT_IDS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_BULK_CONTEXT;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_BULK_VALUES;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_BULK_PROPERTY_KEYS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_BULK_DEPTH;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_BULK_PARENT_OBJECT_ID;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_BULK_OBJECT_FORMAT;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_PROPERTIES_BULK_WRITE_RESULTS;

		public static _tagpropertykey WPD_COMMAND_OBJECT_RESOURCES_GET_SUPPORTED;

		public static _tagpropertykey WPD_COMMAND_OBJECT_RESOURCES_GET_ATTRIBUTES;

		public static _tagpropertykey WPD_COMMAND_OBJECT_RESOURCES_OPEN;

		public static _tagpropertykey WPD_COMMAND_OBJECT_RESOURCES_READ;

		public static _tagpropertykey WPD_COMMAND_OBJECT_RESOURCES_WRITE;

		public static _tagpropertykey WPD_COMMAND_OBJECT_RESOURCES_CLOSE;

		public static _tagpropertykey WPD_COMMAND_OBJECT_RESOURCES_DELETE;

		public static _tagpropertykey WPD_COMMAND_OBJECT_RESOURCES_CREATE_RESOURCE;

		public static _tagpropertykey WPD_COMMAND_OBJECT_RESOURCES_REVERT;

		public static _tagpropertykey WPD_COMMAND_OBJECT_RESOURCES_SEEK;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_OBJECT_ID;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_ACCESS_MODE;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_RESOURCE_KEYS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_RESOURCE_ATTRIBUTES;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_CONTEXT;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_TO_READ;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_READ;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_TO_WRITE;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_WRITTEN;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_DATA;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_OPTIMAL_TRANSFER_BUFFER_SIZE;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_SEEK_OFFSET;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_SEEK_ORIGIN_FLAG;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_RESOURCES_POSITION_FROM_START;

		public static _tagpropertykey WPD_OPTION_OBJECT_RESOURCES_SEEK_ON_READ_SUPPORTED;

		public static _tagpropertykey WPD_OPTION_OBJECT_RESOURCES_SEEK_ON_WRITE_SUPPORTED;

		public static _tagpropertykey WPD_COMMAND_OBJECT_MANAGEMENT_CREATE_OBJECT_WITH_PROPERTIES_ONLY;

		public static _tagpropertykey WPD_COMMAND_OBJECT_MANAGEMENT_CREATE_OBJECT_WITH_PROPERTIES_AND_DATA;

		public static _tagpropertykey WPD_COMMAND_OBJECT_MANAGEMENT_WRITE_OBJECT_DATA;

		public static _tagpropertykey WPD_COMMAND_OBJECT_MANAGEMENT_COMMIT_OBJECT;

		public static _tagpropertykey WPD_COMMAND_OBJECT_MANAGEMENT_REVERT_OBJECT;

		public static _tagpropertykey WPD_COMMAND_OBJECT_MANAGEMENT_DELETE_OBJECTS;

		public static _tagpropertykey WPD_COMMAND_OBJECT_MANAGEMENT_MOVE_OBJECTS;

		public static _tagpropertykey WPD_COMMAND_OBJECT_MANAGEMENT_COPY_OBJECTS;

		public static _tagpropertykey WPD_COMMAND_OBJECT_MANAGEMENT_UPDATE_OBJECT_WITH_PROPERTIES_AND_DATA;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_CREATION_PROPERTIES;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_CONTEXT;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_NUM_BYTES_TO_WRITE;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_NUM_BYTES_WRITTEN;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_DATA;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_ID;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_DELETE_OPTIONS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_OPTIMAL_TRANSFER_BUFFER_SIZE;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_IDS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_DELETE_RESULTS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_DESTINATION_FOLDER_OBJECT_ID;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_MOVE_RESULTS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_COPY_RESULTS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_UPDATE_PROPERTIES;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_PROPERTY_KEYS;

		public static _tagpropertykey WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_FORMAT;

		public static _tagpropertykey WPD_OPTION_OBJECT_MANAGEMENT_RECURSIVE_DELETE_SUPPORTED;

		public static _tagpropertykey WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_COMMANDS;

		public static _tagpropertykey WPD_COMMAND_CAPABILITIES_GET_COMMAND_OPTIONS;

		public static _tagpropertykey WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FUNCTIONAL_CATEGORIES;

		public static _tagpropertykey WPD_COMMAND_CAPABILITIES_GET_FUNCTIONAL_OBJECTS;

		public static _tagpropertykey WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_CONTENT_TYPES;

		public static _tagpropertykey WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FORMATS;

		public static _tagpropertykey WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FORMAT_PROPERTIES;

		public static _tagpropertykey WPD_COMMAND_CAPABILITIES_GET_FIXED_PROPERTY_ATTRIBUTES;

		public static _tagpropertykey WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_EVENTS;

		public static _tagpropertykey WPD_COMMAND_CAPABILITIES_GET_EVENT_OPTIONS;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_SUPPORTED_COMMANDS;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_COMMAND;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_COMMAND_OPTIONS;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_CATEGORIES;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_CATEGORY;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_OBJECTS;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_CONTENT_TYPES;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_CONTENT_TYPE;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_FORMATS;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_FORMAT;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_PROPERTY_KEYS;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_PROPERTY_ATTRIBUTES;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_SUPPORTED_EVENTS;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_EVENT;

		public static _tagpropertykey WPD_PROPERTY_CAPABILITIES_EVENT_OPTIONS;

		public static _tagpropertykey WPD_COMMAND_CLASS_EXTENSION_WRITE_DEVICE_INFORMATION;

		public static _tagpropertykey WPD_PROPERTY_CLASS_EXTENSION_DEVICE_INFORMATION_VALUES;

		public static _tagpropertykey WPD_PROPERTY_CLASS_EXTENSION_DEVICE_INFORMATION_WRITE_RESULTS;

		public static _tagpropertykey WPD_COMMAND_CLASS_EXTENSION_REGISTER_SERVICE_INTERFACES;

		public static _tagpropertykey WPD_COMMAND_CLASS_EXTENSION_UNREGISTER_SERVICE_INTERFACES;

		public static _tagpropertykey WPD_PROPERTY_CLASS_EXTENSION_SERVICE_OBJECT_ID;

		public static _tagpropertykey WPD_PROPERTY_CLASS_EXTENSION_SERVICE_INTERFACES;

		public static _tagpropertykey WPD_PROPERTY_CLASS_EXTENSION_SERVICE_REGISTRATION_RESULTS;

		public static _tagpropertykey WPD_COMMAND_SERVICE_COMMON_GET_SERVICE_OBJECT_ID;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_OBJECT_ID;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_METHODS;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_METHODS_BY_FORMAT;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_METHOD_ATTRIBUTES;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_METHOD_PARAMETER_ATTRIBUTES;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_FORMATS;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_ATTRIBUTES;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_FORMAT_PROPERTIES;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_PROPERTY_ATTRIBUTES;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_EVENTS;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_EVENT_ATTRIBUTES;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_EVENT_PARAMETER_ATTRIBUTES;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_INHERITED_SERVICES;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_RENDERING_PROFILES;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_COMMANDS;

		public static _tagpropertykey WPD_COMMAND_SERVICE_CAPABILITIES_GET_COMMAND_OPTIONS;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_METHODS;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_FORMAT;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_METHOD;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_METHOD_ATTRIBUTES;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_PARAMETER;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_PARAMETER_ATTRIBUTES;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_FORMATS;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_FORMAT_ATTRIBUTES;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_PROPERTY_KEYS;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_PROPERTY_ATTRIBUTES;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_EVENTS;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_EVENT;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_EVENT_ATTRIBUTES;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_INHERITANCE_TYPE;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_INHERITED_SERVICES;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_RENDERING_PROFILES;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_COMMANDS;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_COMMAND;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_CAPABILITIES_COMMAND_OPTIONS;

		public static _tagpropertykey WPD_COMMAND_SERVICE_METHODS_START_INVOKE;

		public static _tagpropertykey WPD_COMMAND_SERVICE_METHODS_CANCEL_INVOKE;

		public static _tagpropertykey WPD_COMMAND_SERVICE_METHODS_END_INVOKE;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_METHOD;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_METHOD_PARAMETER_VALUES;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_METHOD_RESULT_VALUES;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_METHOD_CONTEXT;

		public static _tagpropertykey WPD_PROPERTY_SERVICE_METHOD_HRESULT;

		static PortableDeviceApi()
		{
			GUID_DEVINTERFACE_WPD = new Guid(1791129720u, 42746, 16725, 186, 133, 249, 143, 73, 29, 79, 51);
			GUID_DEVINTERFACE_WPD_PRIVATE = new Guid(3121377679u, 19949, 18871, 189, 211, 250, 190, 40, 102, 18, 17);
			GUID_DEVINTERFACE_WPD_SERVICE = new Guid(2666811264u, 15716, 16966, 166, 170, 32, 111, 50, 141, 30, 220);
			WPD_EVENT_NOTIFICATION = new Guid(732095498, 27468, 17045, 187, 67, 38, 50, 43, 153, 174, 178);
			WPD_EVENT_OBJECT_ADDED = new Guid(2804341397u, 57863, 19202, 141, 68, 190, 242, 232, 108, 191, 252);
			WPD_EVENT_OBJECT_REMOVED = new Guid(3196234632u, 42284, 18467, 150, 229, 208, 39, 38, 113, 252, 56);
			WPD_EVENT_OBJECT_UPDATED = new Guid(340109145, 11777, 18525, 159, 39, byte.MaxValue, 7, 218, 230, 151, 171);
			WPD_EVENT_DEVICE_RESET = new Guid(2002112339u, 49645, 17651, 181, 162, 69, 30, 44, 55, 107, 39);
			WPD_EVENT_DEVICE_CAPABILITIES_UPDATED = new Guid(914905761u, 52564, 19882, 179, 208, 175, 179, 224, 63, 89, 153);
			WPD_EVENT_STORAGE_FORMAT = new Guid(931291499, 8892, 17524, 162, 81, 48, 112, 248, 211, 136, 87);
			WPD_EVENT_OBJECT_TRANSFER_REQUESTED = new Guid(2367070369u, 62150, 16858, 143, 25, 94, 83, 114, 26, 219, 242);
			WPD_EVENT_DEVICE_REMOVED = new Guid(3838560795u, 26904, 18617, 133, 238, 2, 190, 124, 133, 10, 249);
			WPD_EVENT_SERVICE_METHOD_COMPLETE = new Guid(2318661112u, 2764, 19867, 156, 196, 17, 45, 53, 59, 134, 202);
			WPD_CONTENT_TYPE_FUNCTIONAL_OBJECT = new Guid(2582446432u, 6143, 19524, 157, 152, 29, 122, 111, 148, 25, 33);
			WPD_CONTENT_TYPE_FOLDER = new Guid(669180818u, 41233, 18656, 171, 12, 225, 119, 5, 160, 95, 133);
			WPD_CONTENT_TYPE_IMAGE = new Guid(4011919317u, 42282, 16963, 162, 107, 98, 212, 23, 109, 118, 3);
			WPD_CONTENT_TYPE_DOCUMENT = new Guid(1745542994u, 38154, 16449, 155, 65, 101, 227, 147, 100, 129, 85);
			WPD_CONTENT_TYPE_CONTACT = new Guid(3938091795u, 17701, 18183, 159, 14, 135, 198, 128, 142, 148, 53);
			WPD_CONTENT_TYPE_CONTACT_GROUP = new Guid(879462706, 19510, 16600, 148, 21, 24, 40, 41, 31, 157, 233);
			WPD_CONTENT_TYPE_AUDIO = new Guid(1255327838, 24109, 17893, 136, 100, 79, 34, 158, 60, 108, 240);
			WPD_CONTENT_TYPE_VIDEO = new Guid(2455875644u, 15736, 17689, 133, 227, 2, 197, 225, 245, 11, 185);
			WPD_CONTENT_TYPE_TELEVISION = new Guid(1621191119u, 62126, 20001, 147, 117, 150, 119, 241, 28, 28, 110);
			WPD_CONTENT_TYPE_PLAYLIST = new Guid(439613412u, 44819, 18677, 153, 78, 119, 54, 157, 254, 4, 163);
			WPD_CONTENT_TYPE_MIXED_CONTENT_ALBUM = new Guid(15778732u, 42387, 18860, 146, 25, 36, 171, 202, 90, 37, 99);
			WPD_CONTENT_TYPE_AUDIO_ALBUM = new Guid(2853729150u, 20489, 18682, 174, 33, 133, 242, 67, 131, 180, 230);
			WPD_CONTENT_TYPE_IMAGE_ALBUM = new Guid(1970876744, 5621, 18992, 168, 19, 84, 237, 138, 55, 226, 38);
			WPD_CONTENT_TYPE_VIDEO_ALBUM = new Guid(19598775u, 54465, 17878, 176, 129, 148, 184, 119, 121, 97, 79);
			WPD_CONTENT_TYPE_MEMO = new Guid(2631012047u, 15184, 16719, 166, 65, 228, 115, byte.MaxValue, 228, 87, 81);
			WPD_CONTENT_TYPE_EMAIL = new Guid(2151154762u, 32337, 20367, 136, 61, 29, 6, 35, 209, 69, 51);
			WPD_CONTENT_TYPE_APPOINTMENT = new Guid(267191822u, 34707, 19230, 144, 201, 72, 172, 56, 154, 198, 49);
			WPD_CONTENT_TYPE_TASK = new Guid(1663381292u, 34943, 19638, 177, 172, 210, 152, 85, 220, 239, 108);
			WPD_CONTENT_TYPE_PROGRAM = new Guid(3530160490u, 9340, 19455, 152, 251, 151, 243, 196, 146, 32, 230);
			WPD_CONTENT_TYPE_GENERIC_FILE = new Guid(8773798u, 36148, 17879, 188, 92, 68, 126, 89, 199, 61, 72);
			WPD_CONTENT_TYPE_CALENDAR = new Guid(2717735271u, 24611, 18848, 157, 241, 248, 6, 11, 231, 81, 176);
			WPD_CONTENT_TYPE_GENERIC_MESSAGE = new Guid(3893275384u, 45787, 16691, 182, 126, 27, 239, 75, 74, 110, 95);
			WPD_CONTENT_TYPE_NETWORK_ASSOCIATION = new Guid(52275182, 6344, 16901, 132, 126, 137, 161, 18, 97, 208, 243);
			WPD_CONTENT_TYPE_CERTIFICATE = new Guid(3694687976u, 43336, 16480, 144, 80, 203, 215, 126, 138, 61, 135);
			WPD_CONTENT_TYPE_WIRELESS_PROFILE = new Guid(195823370u, 40799, 19876, 168, 246, 61, 228, 77, 104, 253, 108);
			WPD_CONTENT_TYPE_MEDIA_CAST = new Guid(1586017228, 15973, 20066, 191, byte.MaxValue, 34, 148, 149, 37, 58, 176);
			WPD_CONTENT_TYPE_SECTION = new Guid(2182121973u, 7569, 19913, 190, 60, 187, 177, 179, 91, 24, 206);
			WPD_CONTENT_TYPE_UNSPECIFIED = new Guid(685298462, 9372, 17742, 170, 188, 52, 136, 49, 104, 230, 52);
			WPD_CONTENT_TYPE_ALL = new Guid(2162258130u, 4181, 19006, 185, 82, 130, 204, 79, 138, 134, 137);
			WPD_FUNCTIONAL_CATEGORY_DEVICE = new Guid(149571179u, 58276, 17206, 161, 243, 164, 77, 43, 92, 67, 140);
			WPD_FUNCTIONAL_CATEGORY_STORAGE = new Guid(602954684, 5598, 19498, 165, 91, 169, 175, 92, 228, 18, 239);
			WPD_FUNCTIONAL_CATEGORY_STILL_IMAGE_CAPTURE = new Guid(1631363879u, 43923, 18688, 180, 250, 137, 91, 181, 135, 75, 121);
			WPD_FUNCTIONAL_CATEGORY_AUDIO_CAPTURE = new Guid(1059723545u, 51138, 18944, 133, 93, 245, 124, 240, 109, 235, 187);
			WPD_FUNCTIONAL_CATEGORY_VIDEO_CAPTURE = new Guid(3795738475u, 29251, 17322, 141, 241, 14, 179, 217, 104, 169, 24);
			WPD_FUNCTIONAL_CATEGORY_SMS = new Guid(4497585u, 49641, 19197, 179, 88, 166, 44, 97, 23, 201, 207);
			WPD_FUNCTIONAL_CATEGORY_RENDERING_INFORMATION = new Guid(140512164u, 42938, 18945, 171, 14, 0, 101, 208, 163, 86, 211);
			WPD_FUNCTIONAL_CATEGORY_NETWORK_CONFIGURATION = new Guid(1224006514, 31850, 19120, 158, 26, 71, 14, 60, 219, 242, 106);
			WPD_FUNCTIONAL_CATEGORY_ALL = new Guid(764044562u, 42828, 17550, 186, 138, 244, 172, 7, 196, 147, 153);
			WPD_OBJECT_FORMAT_PROPERTIES_ONLY = new Guid(805371904u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_UNSPECIFIED = new Guid(805306368u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_SCRIPT = new Guid(805437440u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_EXECUTABLE = new Guid(805502976u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_TEXT = new Guid(805568512u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_HTML = new Guid(805634048u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_DPOF = new Guid(805699584u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_AIFF = new Guid(805765120u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_WAVE = new Guid(805830656u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_MP3 = new Guid(805896192u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_AVI = new Guid(805961728u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_MPEG = new Guid(806027264u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_ASF = new Guid(806092800u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_EXIF = new Guid(939589632u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_TIFFEP = new Guid(939655168u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_FLASHPIX = new Guid(939720704u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_BMP = new Guid(939786240u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_CIFF = new Guid(939851776u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_GIF = new Guid(939982848u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_JFIF = new Guid(940048384u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_PCD = new Guid(940113920u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_PICT = new Guid(940179456u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_PNG = new Guid(940244992u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_TIFF = new Guid(940376064u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_TIFFIT = new Guid(940441600u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_JP2 = new Guid(940507136u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_JPX = new Guid(940572672u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_WINDOWSIMAGEFORMAT = new Guid(3095461888u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_WMA = new Guid(3103850496u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_WMV = new Guid(3112239104u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_WPLPLAYLIST = new Guid(3121610752u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_M3UPLAYLIST = new Guid(3121676288u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_MPLPLAYLIST = new Guid(3121741824u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_ASXPLAYLIST = new Guid(3121807360u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_PLSPLAYLIST = new Guid(3121872896u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_ABSTRACT_CONTACT_GROUP = new Guid(3120955392u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_ABSTRACT_MEDIA_CAST = new Guid(3121283072u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_VCALENDAR1 = new Guid(3187802112u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_ICALENDAR = new Guid(3187867648u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_ABSTRACT_CONTACT = new Guid(3145793536u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_VCARD2 = new Guid(3145859072u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_VCARD3 = new Guid(3145924608u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_ICON = new Guid(124924653, 4140, 17976, 156, 34, 131, 241, 66, 191, 200, 34);
			WPD_OBJECT_FORMAT_XML = new Guid(3129081856u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_AAC = new Guid(3103981568u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_AUDIBLE = new Guid(3104047104u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_FLAC = new Guid(3104178176u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_OGG = new Guid(3103916032u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_MP4 = new Guid(3112304640u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_M4A = new Guid(816555948, 28669, 19491, 163, 89, 62, 155, 82, 243, 241, 200);
			WPD_OBJECT_FORMAT_MP2 = new Guid(3112370176u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_MICROSOFT_WORD = new Guid(3129147392u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_MHT_COMPILED_HTML = new Guid(3129212928u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_MICROSOFT_EXCEL = new Guid(3129278464u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_MICROSOFT_POWERPOINT = new Guid(3129344000u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_NETWORK_ASSOCIATION = new Guid(2969698304u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_X509V3CERTIFICATE = new Guid(2969763840u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_MICROSOFT_WFC = new Guid(2969829376u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_3GP = new Guid(3112435712u, 44652, 18436, 152, 186, 197, 123, 70, 150, 95, 231);
			WPD_OBJECT_FORMAT_3GPA = new Guid(3843499824u, 63857, 16879, 161, 11, 34, 113, 160, 1, 157, 122);
			WPD_OBJECT_FORMAT_ALL = new Guid(3254136498u, 19379, 18332, 156, 250, 5, 181, 243, 165, 123, 34);
			InitializePropertyKeys();
		}

		private static void InitializePropertyKeys()
		{
			WPD_OBJECT_ID.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_ID.pid = 2u;
			WPD_OBJECT_PARENT_ID.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_PARENT_ID.pid = 3u;
			WPD_OBJECT_NAME.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_NAME.pid = 4u;
			WPD_OBJECT_PERSISTENT_UNIQUE_ID.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_PERSISTENT_UNIQUE_ID.pid = 5u;
			WPD_OBJECT_FORMAT.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_FORMAT.pid = 6u;
			WPD_OBJECT_CONTENT_TYPE.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_CONTENT_TYPE.pid = 7u;
			WPD_OBJECT_ISHIDDEN.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_ISHIDDEN.pid = 9u;
			WPD_OBJECT_ISSYSTEM.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_ISSYSTEM.pid = 10u;
			WPD_OBJECT_SIZE.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_SIZE.pid = 11u;
			WPD_OBJECT_ORIGINAL_FILE_NAME.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_ORIGINAL_FILE_NAME.pid = 12u;
			WPD_OBJECT_NON_CONSUMABLE.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_NON_CONSUMABLE.pid = 13u;
			WPD_OBJECT_REFERENCES.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_REFERENCES.pid = 14u;
			WPD_OBJECT_KEYWORDS.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_KEYWORDS.pid = 15u;
			WPD_OBJECT_SYNC_ID.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_SYNC_ID.pid = 16u;
			WPD_OBJECT_IS_DRM_PROTECTED.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_IS_DRM_PROTECTED.pid = 17u;
			WPD_OBJECT_DATE_CREATED.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_DATE_CREATED.pid = 18u;
			WPD_OBJECT_DATE_MODIFIED.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_DATE_MODIFIED.pid = 19u;
			WPD_OBJECT_DATE_AUTHORED.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_DATE_AUTHORED.pid = 20u;
			WPD_OBJECT_BACK_REFERENCES.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_BACK_REFERENCES.pid = 21u;
			WPD_OBJECT_CONTAINER_FUNCTIONAL_OBJECT_ID.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_CONTAINER_FUNCTIONAL_OBJECT_ID.pid = 23u;
			WPD_OBJECT_GENERATE_THUMBNAIL_FROM_RESOURCE.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_GENERATE_THUMBNAIL_FROM_RESOURCE.pid = 24u;
			WPD_OBJECT_HINT_LOCATION_DISPLAY_NAME.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_HINT_LOCATION_DISPLAY_NAME.pid = 25u;
			WPD_OBJECT_CAN_DELETE.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_CAN_DELETE.pid = 26u;
			WPD_OBJECT_LANGUAGE_LOCALE.fmtid = new Guid(4016785677u, 23768, 17274, 175, 252, 218, 139, 96, 238, 74, 60);
			WPD_OBJECT_LANGUAGE_LOCALE.pid = 27u;
			WPD_FOLDER_CONTENT_TYPES_ALLOWED.fmtid = new Guid(2124053183u, 58728, 19252, 170, 47, 19, 187, 18, 171, 23, 125);
			WPD_FOLDER_CONTENT_TYPES_ALLOWED.pid = 2u;
			WPD_IMAGE_BITDEPTH.fmtid = new Guid(1674987784u, 40865, 18335, 133, 186, 153, 82, 33, 100, 71, 219);
			WPD_IMAGE_BITDEPTH.pid = 3u;
			WPD_IMAGE_CROPPED_STATUS.fmtid = new Guid(1674987784u, 40865, 18335, 133, 186, 153, 82, 33, 100, 71, 219);
			WPD_IMAGE_CROPPED_STATUS.pid = 4u;
			WPD_IMAGE_COLOR_CORRECTED_STATUS.fmtid = new Guid(1674987784u, 40865, 18335, 133, 186, 153, 82, 33, 100, 71, 219);
			WPD_IMAGE_COLOR_CORRECTED_STATUS.pid = 5u;
			WPD_IMAGE_FNUMBER.fmtid = new Guid(1674987784u, 40865, 18335, 133, 186, 153, 82, 33, 100, 71, 219);
			WPD_IMAGE_FNUMBER.pid = 6u;
			WPD_IMAGE_EXPOSURE_TIME.fmtid = new Guid(1674987784u, 40865, 18335, 133, 186, 153, 82, 33, 100, 71, 219);
			WPD_IMAGE_EXPOSURE_TIME.pid = 7u;
			WPD_IMAGE_EXPOSURE_INDEX.fmtid = new Guid(1674987784u, 40865, 18335, 133, 186, 153, 82, 33, 100, 71, 219);
			WPD_IMAGE_EXPOSURE_INDEX.pid = 8u;
			WPD_IMAGE_HORIZONTAL_RESOLUTION.fmtid = new Guid(1674987784u, 40865, 18335, 133, 186, 153, 82, 33, 100, 71, 219);
			WPD_IMAGE_HORIZONTAL_RESOLUTION.pid = 9u;
			WPD_IMAGE_VERTICAL_RESOLUTION.fmtid = new Guid(1674987784u, 40865, 18335, 133, 186, 153, 82, 33, 100, 71, 219);
			WPD_IMAGE_VERTICAL_RESOLUTION.pid = 10u;
			WPD_MEDIA_TOTAL_BITRATE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_TOTAL_BITRATE.pid = 2u;
			WPD_MEDIA_BITRATE_TYPE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_BITRATE_TYPE.pid = 3u;
			WPD_MEDIA_COPYRIGHT.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_COPYRIGHT.pid = 4u;
			WPD_MEDIA_SUBSCRIPTION_CONTENT_ID.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_SUBSCRIPTION_CONTENT_ID.pid = 5u;
			WPD_MEDIA_USE_COUNT.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_USE_COUNT.pid = 6u;
			WPD_MEDIA_SKIP_COUNT.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_SKIP_COUNT.pid = 7u;
			WPD_MEDIA_LAST_ACCESSED_TIME.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_LAST_ACCESSED_TIME.pid = 8u;
			WPD_MEDIA_PARENTAL_RATING.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_PARENTAL_RATING.pid = 9u;
			WPD_MEDIA_META_GENRE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_META_GENRE.pid = 10u;
			WPD_MEDIA_COMPOSER.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_COMPOSER.pid = 11u;
			WPD_MEDIA_EFFECTIVE_RATING.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_EFFECTIVE_RATING.pid = 12u;
			WPD_MEDIA_SUB_TITLE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_SUB_TITLE.pid = 13u;
			WPD_MEDIA_RELEASE_DATE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_RELEASE_DATE.pid = 14u;
			WPD_MEDIA_SAMPLE_RATE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_SAMPLE_RATE.pid = 15u;
			WPD_MEDIA_STAR_RATING.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_STAR_RATING.pid = 16u;
			WPD_MEDIA_USER_EFFECTIVE_RATING.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_USER_EFFECTIVE_RATING.pid = 17u;
			WPD_MEDIA_TITLE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_TITLE.pid = 18u;
			WPD_MEDIA_DURATION.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_DURATION.pid = 19u;
			WPD_MEDIA_BUY_NOW.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_BUY_NOW.pid = 20u;
			WPD_MEDIA_ENCODING_PROFILE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_ENCODING_PROFILE.pid = 21u;
			WPD_MEDIA_WIDTH.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_WIDTH.pid = 22u;
			WPD_MEDIA_HEIGHT.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_HEIGHT.pid = 23u;
			WPD_MEDIA_ARTIST.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_ARTIST.pid = 24u;
			WPD_MEDIA_ALBUM_ARTIST.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_ALBUM_ARTIST.pid = 25u;
			WPD_MEDIA_OWNER.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_OWNER.pid = 26u;
			WPD_MEDIA_MANAGING_EDITOR.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_MANAGING_EDITOR.pid = 27u;
			WPD_MEDIA_WEBMASTER.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_WEBMASTER.pid = 28u;
			WPD_MEDIA_SOURCE_URL.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_SOURCE_URL.pid = 29u;
			WPD_MEDIA_DESTINATION_URL.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_DESTINATION_URL.pid = 30u;
			WPD_MEDIA_DESCRIPTION.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_DESCRIPTION.pid = 31u;
			WPD_MEDIA_GENRE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_GENRE.pid = 32u;
			WPD_MEDIA_TIME_BOOKMARK.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_TIME_BOOKMARK.pid = 33u;
			WPD_MEDIA_OBJECT_BOOKMARK.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_OBJECT_BOOKMARK.pid = 34u;
			WPD_MEDIA_LAST_BUILD_DATE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_LAST_BUILD_DATE.pid = 35u;
			WPD_MEDIA_BYTE_BOOKMARK.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_BYTE_BOOKMARK.pid = 36u;
			WPD_MEDIA_TIME_TO_LIVE.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_TIME_TO_LIVE.pid = 37u;
			WPD_MEDIA_GUID.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_GUID.pid = 38u;
			WPD_MEDIA_SUB_DESCRIPTION.fmtid = new Guid(785955333, 2771, 17116, 176, 208, 188, 149, 172, 57, 106, 200);
			WPD_MEDIA_SUB_DESCRIPTION.pid = 39u;
			WPD_CONTACT_DISPLAY_NAME.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_DISPLAY_NAME.pid = 2u;
			WPD_CONTACT_FIRST_NAME.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_FIRST_NAME.pid = 3u;
			WPD_CONTACT_MIDDLE_NAMES.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_MIDDLE_NAMES.pid = 4u;
			WPD_CONTACT_LAST_NAME.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_LAST_NAME.pid = 5u;
			WPD_CONTACT_PREFIX.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PREFIX.pid = 6u;
			WPD_CONTACT_SUFFIX.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_SUFFIX.pid = 7u;
			WPD_CONTACT_PHONETIC_FIRST_NAME.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PHONETIC_FIRST_NAME.pid = 8u;
			WPD_CONTACT_PHONETIC_LAST_NAME.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PHONETIC_LAST_NAME.pid = 9u;
			WPD_CONTACT_PERSONAL_FULL_POSTAL_ADDRESS.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_FULL_POSTAL_ADDRESS.pid = 10u;
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_LINE1.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_LINE1.pid = 11u;
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_LINE2.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_LINE2.pid = 12u;
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_CITY.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_CITY.pid = 13u;
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_REGION.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_REGION.pid = 14u;
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_POSTAL_CODE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_POSTAL_CODE.pid = 15u;
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_COUNTRY.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_POSTAL_ADDRESS_COUNTRY.pid = 16u;
			WPD_CONTACT_BUSINESS_FULL_POSTAL_ADDRESS.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_FULL_POSTAL_ADDRESS.pid = 17u;
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_LINE1.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_LINE1.pid = 18u;
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_LINE2.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_LINE2.pid = 19u;
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_CITY.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_CITY.pid = 20u;
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_REGION.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_REGION.pid = 21u;
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_POSTAL_CODE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_POSTAL_CODE.pid = 22u;
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_COUNTRY.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_POSTAL_ADDRESS_COUNTRY.pid = 23u;
			WPD_CONTACT_OTHER_FULL_POSTAL_ADDRESS.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_OTHER_FULL_POSTAL_ADDRESS.pid = 24u;
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_LINE1.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_LINE1.pid = 25u;
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_LINE2.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_LINE2.pid = 26u;
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_CITY.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_CITY.pid = 27u;
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_REGION.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_REGION.pid = 28u;
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_POSTAL_CODE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_POSTAL_CODE.pid = 29u;
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_POSTAL_COUNTRY.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_OTHER_POSTAL_ADDRESS_POSTAL_COUNTRY.pid = 30u;
			WPD_CONTACT_PRIMARY_EMAIL_ADDRESS.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PRIMARY_EMAIL_ADDRESS.pid = 31u;
			WPD_CONTACT_PERSONAL_EMAIL.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_EMAIL.pid = 32u;
			WPD_CONTACT_PERSONAL_EMAIL2.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_EMAIL2.pid = 33u;
			WPD_CONTACT_BUSINESS_EMAIL.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_EMAIL.pid = 34u;
			WPD_CONTACT_BUSINESS_EMAIL2.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_EMAIL2.pid = 35u;
			WPD_CONTACT_OTHER_EMAILS.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_OTHER_EMAILS.pid = 36u;
			WPD_CONTACT_PRIMARY_PHONE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PRIMARY_PHONE.pid = 37u;
			WPD_CONTACT_PERSONAL_PHONE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_PHONE.pid = 38u;
			WPD_CONTACT_PERSONAL_PHONE2.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_PHONE2.pid = 39u;
			WPD_CONTACT_BUSINESS_PHONE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_PHONE.pid = 40u;
			WPD_CONTACT_BUSINESS_PHONE2.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_PHONE2.pid = 41u;
			WPD_CONTACT_MOBILE_PHONE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_MOBILE_PHONE.pid = 42u;
			WPD_CONTACT_MOBILE_PHONE2.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_MOBILE_PHONE2.pid = 43u;
			WPD_CONTACT_PERSONAL_FAX.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_FAX.pid = 44u;
			WPD_CONTACT_BUSINESS_FAX.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_FAX.pid = 45u;
			WPD_CONTACT_PAGER.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PAGER.pid = 46u;
			WPD_CONTACT_OTHER_PHONES.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_OTHER_PHONES.pid = 47u;
			WPD_CONTACT_PRIMARY_WEB_ADDRESS.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PRIMARY_WEB_ADDRESS.pid = 48u;
			WPD_CONTACT_PERSONAL_WEB_ADDRESS.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PERSONAL_WEB_ADDRESS.pid = 49u;
			WPD_CONTACT_BUSINESS_WEB_ADDRESS.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BUSINESS_WEB_ADDRESS.pid = 50u;
			WPD_CONTACT_INSTANT_MESSENGER.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_INSTANT_MESSENGER.pid = 51u;
			WPD_CONTACT_INSTANT_MESSENGER2.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_INSTANT_MESSENGER2.pid = 52u;
			WPD_CONTACT_INSTANT_MESSENGER3.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_INSTANT_MESSENGER3.pid = 53u;
			WPD_CONTACT_COMPANY_NAME.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_COMPANY_NAME.pid = 54u;
			WPD_CONTACT_PHONETIC_COMPANY_NAME.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PHONETIC_COMPANY_NAME.pid = 55u;
			WPD_CONTACT_ROLE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_ROLE.pid = 56u;
			WPD_CONTACT_BIRTHDATE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_BIRTHDATE.pid = 57u;
			WPD_CONTACT_PRIMARY_FAX.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_PRIMARY_FAX.pid = 58u;
			WPD_CONTACT_SPOUSE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_SPOUSE.pid = 59u;
			WPD_CONTACT_CHILDREN.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_CHILDREN.pid = 60u;
			WPD_CONTACT_ASSISTANT.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_ASSISTANT.pid = 61u;
			WPD_CONTACT_ANNIVERSARY_DATE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_ANNIVERSARY_DATE.pid = 62u;
			WPD_CONTACT_RINGTONE.fmtid = new Guid(4225039787u, 39037, 18295, 179, 249, 114, 97, 133, 169, 49, 43);
			WPD_CONTACT_RINGTONE.pid = 63u;
			WPD_MUSIC_ALBUM.fmtid = new Guid(3005543786u, 56413, 18149, 182, 223, 210, 234, 65, 72, 136, 198);
			WPD_MUSIC_ALBUM.pid = 3u;
			WPD_MUSIC_TRACK.fmtid = new Guid(3005543786u, 56413, 18149, 182, 223, 210, 234, 65, 72, 136, 198);
			WPD_MUSIC_TRACK.pid = 4u;
			WPD_MUSIC_LYRICS.fmtid = new Guid(3005543786u, 56413, 18149, 182, 223, 210, 234, 65, 72, 136, 198);
			WPD_MUSIC_LYRICS.pid = 6u;
			WPD_MUSIC_MOOD.fmtid = new Guid(3005543786u, 56413, 18149, 182, 223, 210, 234, 65, 72, 136, 198);
			WPD_MUSIC_MOOD.pid = 8u;
			WPD_AUDIO_BITRATE.fmtid = new Guid(3005543786u, 56413, 18149, 182, 223, 210, 234, 65, 72, 136, 198);
			WPD_AUDIO_BITRATE.pid = 9u;
			WPD_AUDIO_CHANNEL_COUNT.fmtid = new Guid(3005543786u, 56413, 18149, 182, 223, 210, 234, 65, 72, 136, 198);
			WPD_AUDIO_CHANNEL_COUNT.pid = 10u;
			WPD_AUDIO_FORMAT_CODE.fmtid = new Guid(3005543786u, 56413, 18149, 182, 223, 210, 234, 65, 72, 136, 198);
			WPD_AUDIO_FORMAT_CODE.pid = 11u;
			WPD_AUDIO_BIT_DEPTH.fmtid = new Guid(3005543786u, 56413, 18149, 182, 223, 210, 234, 65, 72, 136, 198);
			WPD_AUDIO_BIT_DEPTH.pid = 12u;
			WPD_AUDIO_BLOCK_ALIGNMENT.fmtid = new Guid(3005543786u, 56413, 18149, 182, 223, 210, 234, 65, 72, 136, 198);
			WPD_AUDIO_BLOCK_ALIGNMENT.pid = 13u;
			WPD_VIDEO_AUTHOR.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_AUTHOR.pid = 2u;
			WPD_VIDEO_RECORDEDTV_STATION_NAME.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_RECORDEDTV_STATION_NAME.pid = 4u;
			WPD_VIDEO_RECORDEDTV_CHANNEL_NUMBER.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_RECORDEDTV_CHANNEL_NUMBER.pid = 5u;
			WPD_VIDEO_RECORDEDTV_REPEAT.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_RECORDEDTV_REPEAT.pid = 7u;
			WPD_VIDEO_BUFFER_SIZE.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_BUFFER_SIZE.pid = 8u;
			WPD_VIDEO_CREDITS.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_CREDITS.pid = 9u;
			WPD_VIDEO_KEY_FRAME_DISTANCE.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_KEY_FRAME_DISTANCE.pid = 10u;
			WPD_VIDEO_QUALITY_SETTING.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_QUALITY_SETTING.pid = 11u;
			WPD_VIDEO_SCAN_TYPE.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_SCAN_TYPE.pid = 12u;
			WPD_VIDEO_BITRATE.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_BITRATE.pid = 13u;
			WPD_VIDEO_FOURCC_CODE.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_FOURCC_CODE.pid = 14u;
			WPD_VIDEO_FRAMERATE.fmtid = new Guid(879698275u, 63896, 16710, 139, 1, 209, 155, 76, 0, 222, 154);
			WPD_VIDEO_FRAMERATE.pid = 15u;
			WPD_COMMON_INFORMATION_SUBJECT.fmtid = new Guid(2995448139u, 1444, 20110, 190, 1, 114, 204, 126, 9, 157, 143);
			WPD_COMMON_INFORMATION_SUBJECT.pid = 2u;
			WPD_COMMON_INFORMATION_BODY_TEXT.fmtid = new Guid(2995448139u, 1444, 20110, 190, 1, 114, 204, 126, 9, 157, 143);
			WPD_COMMON_INFORMATION_BODY_TEXT.pid = 3u;
			WPD_COMMON_INFORMATION_PRIORITY.fmtid = new Guid(2995448139u, 1444, 20110, 190, 1, 114, 204, 126, 9, 157, 143);
			WPD_COMMON_INFORMATION_PRIORITY.pid = 4u;
			WPD_COMMON_INFORMATION_START_DATETIME.fmtid = new Guid(2995448139u, 1444, 20110, 190, 1, 114, 204, 126, 9, 157, 143);
			WPD_COMMON_INFORMATION_START_DATETIME.pid = 5u;
			WPD_COMMON_INFORMATION_END_DATETIME.fmtid = new Guid(2995448139u, 1444, 20110, 190, 1, 114, 204, 126, 9, 157, 143);
			WPD_COMMON_INFORMATION_END_DATETIME.pid = 6u;
			WPD_COMMON_INFORMATION_NOTES.fmtid = new Guid(2995448139u, 1444, 20110, 190, 1, 114, 204, 126, 9, 157, 143);
			WPD_COMMON_INFORMATION_NOTES.pid = 7u;
			WPD_EMAIL_TO_LINE.fmtid = new Guid(1106835034, 21636, 18306, 177, 61, 71, 64, 221, 124, 55, 197);
			WPD_EMAIL_TO_LINE.pid = 2u;
			WPD_EMAIL_CC_LINE.fmtid = new Guid(1106835034, 21636, 18306, 177, 61, 71, 64, 221, 124, 55, 197);
			WPD_EMAIL_CC_LINE.pid = 3u;
			WPD_EMAIL_BCC_LINE.fmtid = new Guid(1106835034, 21636, 18306, 177, 61, 71, 64, 221, 124, 55, 197);
			WPD_EMAIL_BCC_LINE.pid = 4u;
			WPD_EMAIL_HAS_BEEN_READ.fmtid = new Guid(1106835034, 21636, 18306, 177, 61, 71, 64, 221, 124, 55, 197);
			WPD_EMAIL_HAS_BEEN_READ.pid = 7u;
			WPD_EMAIL_RECEIVED_TIME.fmtid = new Guid(1106835034, 21636, 18306, 177, 61, 71, 64, 221, 124, 55, 197);
			WPD_EMAIL_RECEIVED_TIME.pid = 8u;
			WPD_EMAIL_HAS_ATTACHMENTS.fmtid = new Guid(1106835034, 21636, 18306, 177, 61, 71, 64, 221, 124, 55, 197);
			WPD_EMAIL_HAS_ATTACHMENTS.pid = 9u;
			WPD_EMAIL_SENDER_ADDRESS.fmtid = new Guid(1106835034, 21636, 18306, 177, 61, 71, 64, 221, 124, 55, 197);
			WPD_EMAIL_SENDER_ADDRESS.pid = 10u;
			WPD_APPOINTMENT_LOCATION.fmtid = new Guid(4187946243u, 17181, 16600, 161, 201, 78, 34, 13, 156, 136, 211);
			WPD_APPOINTMENT_LOCATION.pid = 3u;
			WPD_APPOINTMENT_TYPE.fmtid = new Guid(4187946243u, 17181, 16600, 161, 201, 78, 34, 13, 156, 136, 211);
			WPD_APPOINTMENT_TYPE.pid = 7u;
			WPD_APPOINTMENT_REQUIRED_ATTENDEES.fmtid = new Guid(4187946243u, 17181, 16600, 161, 201, 78, 34, 13, 156, 136, 211);
			WPD_APPOINTMENT_REQUIRED_ATTENDEES.pid = 8u;
			WPD_APPOINTMENT_OPTIONAL_ATTENDEES.fmtid = new Guid(4187946243u, 17181, 16600, 161, 201, 78, 34, 13, 156, 136, 211);
			WPD_APPOINTMENT_OPTIONAL_ATTENDEES.pid = 9u;
			WPD_APPOINTMENT_ACCEPTED_ATTENDEES.fmtid = new Guid(4187946243u, 17181, 16600, 161, 201, 78, 34, 13, 156, 136, 211);
			WPD_APPOINTMENT_ACCEPTED_ATTENDEES.pid = 10u;
			WPD_APPOINTMENT_RESOURCES.fmtid = new Guid(4187946243u, 17181, 16600, 161, 201, 78, 34, 13, 156, 136, 211);
			WPD_APPOINTMENT_RESOURCES.pid = 11u;
			WPD_APPOINTMENT_TENTATIVE_ATTENDEES.fmtid = new Guid(4187946243u, 17181, 16600, 161, 201, 78, 34, 13, 156, 136, 211);
			WPD_APPOINTMENT_TENTATIVE_ATTENDEES.pid = 12u;
			WPD_APPOINTMENT_DECLINED_ATTENDEES.fmtid = new Guid(4187946243u, 17181, 16600, 161, 201, 78, 34, 13, 156, 136, 211);
			WPD_APPOINTMENT_DECLINED_ATTENDEES.pid = 13u;
			WPD_TASK_STATUS.fmtid = new Guid(3813992798u, 55456, 17975, 160, 58, 12, 178, 104, 56, 219, 199);
			WPD_TASK_STATUS.pid = 6u;
			WPD_TASK_PERCENT_COMPLETE.fmtid = new Guid(3813992798u, 55456, 17975, 160, 58, 12, 178, 104, 56, 219, 199);
			WPD_TASK_PERCENT_COMPLETE.pid = 8u;
			WPD_TASK_REMINDER_DATE.fmtid = new Guid(3813992798u, 55456, 17975, 160, 58, 12, 178, 104, 56, 219, 199);
			WPD_TASK_REMINDER_DATE.pid = 10u;
			WPD_TASK_OWNER.fmtid = new Guid(3813992798u, 55456, 17975, 160, 58, 12, 178, 104, 56, 219, 199);
			WPD_TASK_OWNER.pid = 11u;
			WPD_NETWORK_ASSOCIATION_HOST_NETWORK_IDENTIFIERS.fmtid = new Guid(3838393375u, 45571, 17393, 161, 0, 90, 7, 209, 27, 2, 116);
			WPD_NETWORK_ASSOCIATION_HOST_NETWORK_IDENTIFIERS.pid = 2u;
			WPD_NETWORK_ASSOCIATION_X509V3SEQUENCE.fmtid = new Guid(3838393375u, 45571, 17393, 161, 0, 90, 7, 209, 27, 2, 116);
			WPD_NETWORK_ASSOCIATION_X509V3SEQUENCE.pid = 3u;
			WPD_STILL_IMAGE_CAPTURE_RESOLUTION.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_CAPTURE_RESOLUTION.pid = 2u;
			WPD_STILL_IMAGE_CAPTURE_FORMAT.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_CAPTURE_FORMAT.pid = 3u;
			WPD_STILL_IMAGE_COMPRESSION_SETTING.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_COMPRESSION_SETTING.pid = 4u;
			WPD_STILL_IMAGE_WHITE_BALANCE.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_WHITE_BALANCE.pid = 5u;
			WPD_STILL_IMAGE_RGB_GAIN.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_RGB_GAIN.pid = 6u;
			WPD_STILL_IMAGE_FNUMBER.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_FNUMBER.pid = 7u;
			WPD_STILL_IMAGE_FOCAL_LENGTH.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_FOCAL_LENGTH.pid = 8u;
			WPD_STILL_IMAGE_FOCUS_DISTANCE.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_FOCUS_DISTANCE.pid = 9u;
			WPD_STILL_IMAGE_FOCUS_MODE.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_FOCUS_MODE.pid = 10u;
			WPD_STILL_IMAGE_EXPOSURE_METERING_MODE.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_EXPOSURE_METERING_MODE.pid = 11u;
			WPD_STILL_IMAGE_FLASH_MODE.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_FLASH_MODE.pid = 12u;
			WPD_STILL_IMAGE_EXPOSURE_TIME.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_EXPOSURE_TIME.pid = 13u;
			WPD_STILL_IMAGE_EXPOSURE_PROGRAM_MODE.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_EXPOSURE_PROGRAM_MODE.pid = 14u;
			WPD_STILL_IMAGE_EXPOSURE_INDEX.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_EXPOSURE_INDEX.pid = 15u;
			WPD_STILL_IMAGE_EXPOSURE_BIAS_COMPENSATION.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_EXPOSURE_BIAS_COMPENSATION.pid = 16u;
			WPD_STILL_IMAGE_CAPTURE_DELAY.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_CAPTURE_DELAY.pid = 17u;
			WPD_STILL_IMAGE_CAPTURE_MODE.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_CAPTURE_MODE.pid = 18u;
			WPD_STILL_IMAGE_CONTRAST.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_CONTRAST.pid = 19u;
			WPD_STILL_IMAGE_SHARPNESS.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_SHARPNESS.pid = 20u;
			WPD_STILL_IMAGE_DIGITAL_ZOOM.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_DIGITAL_ZOOM.pid = 21u;
			WPD_STILL_IMAGE_EFFECT_MODE.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_EFFECT_MODE.pid = 22u;
			WPD_STILL_IMAGE_BURST_NUMBER.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_BURST_NUMBER.pid = 23u;
			WPD_STILL_IMAGE_BURST_INTERVAL.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_BURST_INTERVAL.pid = 24u;
			WPD_STILL_IMAGE_TIMELAPSE_NUMBER.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_TIMELAPSE_NUMBER.pid = 25u;
			WPD_STILL_IMAGE_TIMELAPSE_INTERVAL.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_TIMELAPSE_INTERVAL.pid = 26u;
			WPD_STILL_IMAGE_FOCUS_METERING_MODE.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_FOCUS_METERING_MODE.pid = 27u;
			WPD_STILL_IMAGE_UPLOAD_URL.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_UPLOAD_URL.pid = 28u;
			WPD_STILL_IMAGE_ARTIST.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_ARTIST.pid = 29u;
			WPD_STILL_IMAGE_CAMERA_MODEL.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_CAMERA_MODEL.pid = 30u;
			WPD_STILL_IMAGE_CAMERA_MANUFACTURER.fmtid = new Guid(1489334764, 7115, 17063, 138, 197, 187, 41, 21, 115, 162, 96);
			WPD_STILL_IMAGE_CAMERA_MANUFACTURER.pid = 31u;
			WPD_SMS_PROVIDER.fmtid = new Guid(2115007692, 20735, 19921, 167, 66, 83, 190, 111, 9, 58, 13);
			WPD_SMS_PROVIDER.pid = 2u;
			WPD_SMS_TIMEOUT.fmtid = new Guid(2115007692, 20735, 19921, 167, 66, 83, 190, 111, 9, 58, 13);
			WPD_SMS_TIMEOUT.pid = 3u;
			WPD_SMS_MAX_PAYLOAD.fmtid = new Guid(2115007692, 20735, 19921, 167, 66, 83, 190, 111, 9, 58, 13);
			WPD_SMS_MAX_PAYLOAD.pid = 4u;
			WPD_SMS_ENCODING.fmtid = new Guid(2115007692, 20735, 19921, 167, 66, 83, 190, 111, 9, 58, 13);
			WPD_SMS_ENCODING.pid = 5u;
			WPD_SECTION_DATA_OFFSET.fmtid = new Guid(1365966123u, 50766, 17648, 152, 220, 190, 225, 200, 143, 125, 102);
			WPD_SECTION_DATA_OFFSET.pid = 2u;
			WPD_SECTION_DATA_LENGTH.fmtid = new Guid(1365966123u, 50766, 17648, 152, 220, 190, 225, 200, 143, 125, 102);
			WPD_SECTION_DATA_LENGTH.pid = 3u;
			WPD_SECTION_DATA_UNITS.fmtid = new Guid(1365966123u, 50766, 17648, 152, 220, 190, 225, 200, 143, 125, 102);
			WPD_SECTION_DATA_UNITS.pid = 4u;
			WPD_SECTION_DATA_REFERENCED_OBJECT_RESOURCE.fmtid = new Guid(1365966123u, 50766, 17648, 152, 220, 190, 225, 200, 143, 125, 102);
			WPD_SECTION_DATA_REFERENCED_OBJECT_RESOURCE.pid = 5u;
			WPD_RENDERING_INFORMATION_PROFILES.fmtid = new Guid(3309110175u, 60963, 18993, 133, 144, 118, 57, 135, 152, 112, 180);
			WPD_RENDERING_INFORMATION_PROFILES.pid = 2u;
			WPD_RENDERING_INFORMATION_PROFILE_ENTRY_TYPE.fmtid = new Guid(3309110175u, 60963, 18993, 133, 144, 118, 57, 135, 152, 112, 180);
			WPD_RENDERING_INFORMATION_PROFILE_ENTRY_TYPE.pid = 3u;
			WPD_RENDERING_INFORMATION_PROFILE_ENTRY_CREATABLE_RESOURCES.fmtid = new Guid(3309110175u, 60963, 18993, 133, 144, 118, 57, 135, 152, 112, 180);
			WPD_RENDERING_INFORMATION_PROFILE_ENTRY_CREATABLE_RESOURCES.pid = 4u;
			WPD_COMMAND_STORAGE_FORMAT.fmtid = new Guid(3640199078u, 13516, 17914, 151, 251, 208, 7, 250, 71, 236, 148);
			WPD_COMMAND_STORAGE_FORMAT.pid = 2u;
			WPD_COMMAND_STORAGE_EJECT.fmtid = new Guid(3640199078u, 13516, 17914, 151, 251, 208, 7, 250, 71, 236, 148);
			WPD_COMMAND_STORAGE_EJECT.pid = 4u;
			WPD_PROPERTY_STORAGE_OBJECT_ID.fmtid = new Guid(3640199078u, 13516, 17914, 151, 251, 208, 7, 250, 71, 236, 148);
			WPD_PROPERTY_STORAGE_OBJECT_ID.pid = 1001u;
			WPD_PROPERTY_STORAGE_DESTINATION_OBJECT_ID.fmtid = new Guid(3640199078u, 13516, 17914, 151, 251, 208, 7, 250, 71, 236, 148);
			WPD_PROPERTY_STORAGE_DESTINATION_OBJECT_ID.pid = 1002u;
			WPD_COMMAND_SMS_SEND.fmtid = new Guid(2948750694u, 65037, 16660, 144, 151, 151, 12, 147, 233, 32, 209);
			WPD_COMMAND_SMS_SEND.pid = 2u;
			WPD_PROPERTY_SMS_RECIPIENT.fmtid = new Guid(2948750694u, 65037, 16660, 144, 151, 151, 12, 147, 233, 32, 209);
			WPD_PROPERTY_SMS_RECIPIENT.pid = 1001u;
			WPD_PROPERTY_SMS_MESSAGE_TYPE.fmtid = new Guid(2948750694u, 65037, 16660, 144, 151, 151, 12, 147, 233, 32, 209);
			WPD_PROPERTY_SMS_MESSAGE_TYPE.pid = 1002u;
			WPD_PROPERTY_SMS_TEXT_MESSAGE.fmtid = new Guid(2948750694u, 65037, 16660, 144, 151, 151, 12, 147, 233, 32, 209);
			WPD_PROPERTY_SMS_TEXT_MESSAGE.pid = 1003u;
			WPD_PROPERTY_SMS_BINARY_MESSAGE.fmtid = new Guid(2948750694u, 65037, 16660, 144, 151, 151, 12, 147, 233, 32, 209);
			WPD_PROPERTY_SMS_BINARY_MESSAGE.pid = 1004u;
			WPD_OPTION_SMS_BINARY_MESSAGE_SUPPORTED.fmtid = new Guid(2948750694u, 65037, 16660, 144, 151, 151, 12, 147, 233, 32, 209);
			WPD_OPTION_SMS_BINARY_MESSAGE_SUPPORTED.pid = 5001u;
			WPD_COMMAND_STILL_IMAGE_CAPTURE_INITIATE.fmtid = new Guid(1338861954, 8866, 19205, 164, 139, 98, 211, 139, 242, 123, 50);
			WPD_COMMAND_STILL_IMAGE_CAPTURE_INITIATE.pid = 2u;
			WPD_COMMAND_MEDIA_CAPTURE_START.fmtid = new Guid(1504981946u, 65092, 19853, 128, 140, 107, 203, 155, 15, 21, 232);
			WPD_COMMAND_MEDIA_CAPTURE_START.pid = 2u;
			WPD_COMMAND_MEDIA_CAPTURE_STOP.fmtid = new Guid(1504981946u, 65092, 19853, 128, 140, 107, 203, 155, 15, 21, 232);
			WPD_COMMAND_MEDIA_CAPTURE_STOP.pid = 3u;
			WPD_COMMAND_MEDIA_CAPTURE_PAUSE.fmtid = new Guid(1504981946u, 65092, 19853, 128, 140, 107, 203, 155, 15, 21, 232);
			WPD_COMMAND_MEDIA_CAPTURE_PAUSE.pid = 4u;
			WPD_COMMAND_DEVICE_HINTS_GET_CONTENT_LOCATION.fmtid = new Guid(224377131u, 52038, 19535, 131, 67, 11, 195, 211, 241, 124, 132);
			WPD_COMMAND_DEVICE_HINTS_GET_CONTENT_LOCATION.pid = 2u;
			WPD_PROPERTY_DEVICE_HINTS_CONTENT_TYPE.fmtid = new Guid(224377131u, 52038, 19535, 131, 67, 11, 195, 211, 241, 124, 132);
			WPD_PROPERTY_DEVICE_HINTS_CONTENT_TYPE.pid = 1001u;
			WPD_PROPERTY_DEVICE_HINTS_CONTENT_LOCATIONS.fmtid = new Guid(224377131u, 52038, 19535, 131, 67, 11, 195, 211, 241, 124, 132);
			WPD_PROPERTY_DEVICE_HINTS_CONTENT_LOCATIONS.pid = 1002u;
			WPD_COMMAND_GENERATE_KEYPAIR.fmtid = new Guid(2029635324, 31160, 18236, 144, 96, 107, 210, 61, 208, 114, 196);
			WPD_COMMAND_GENERATE_KEYPAIR.pid = 2u;
			WPD_COMMAND_COMMIT_KEYPAIR.fmtid = new Guid(2029635324, 31160, 18236, 144, 96, 107, 210, 61, 208, 114, 196);
			WPD_COMMAND_COMMIT_KEYPAIR.pid = 3u;
			WPD_COMMAND_PROCESS_WIRELESS_PROFILE.fmtid = new Guid(2029635324, 31160, 18236, 144, 96, 107, 210, 61, 208, 114, 196);
			WPD_COMMAND_PROCESS_WIRELESS_PROFILE.pid = 4u;
			WPD_PROPERTY_PUBLIC_KEY.fmtid = new Guid(2029635324, 31160, 18236, 144, 96, 107, 210, 61, 208, 114, 196);
			WPD_PROPERTY_PUBLIC_KEY.pid = 1001u;
			WPD_RESOURCE_DEFAULT.fmtid = new Guid(3894311358u, 13552, 16831, 181, 63, 241, 160, 106, 232, 120, 66);
			WPD_RESOURCE_DEFAULT.pid = 0u;
			WPD_RESOURCE_CONTACT_PHOTO.fmtid = new Guid(743270403u, 33002, 17792, 175, 154, 91, 225, 162, 62, 221, 203);
			WPD_RESOURCE_CONTACT_PHOTO.pid = 0u;
			WPD_RESOURCE_THUMBNAIL.fmtid = new Guid(3351513018u, 39162, 18101, 153, 96, 35, 254, 193, 36, 207, 222);
			WPD_RESOURCE_THUMBNAIL.pid = 0u;
			WPD_RESOURCE_ICON.fmtid = new Guid(4053139160u, 43560, 20195, 177, 83, 225, 130, 221, 94, 220, 57);
			WPD_RESOURCE_ICON.pid = 0u;
			WPD_RESOURCE_AUDIO_CLIP.fmtid = new Guid(1002518914u, 34225, 18656, 149, 166, 141, 58, 208, 107, 225, 23);
			WPD_RESOURCE_AUDIO_CLIP.pid = 0u;
			WPD_RESOURCE_ALBUM_ART.fmtid = new Guid(4029326164u, 8960, 20013, 161, 185, 59, 103, 48, 247, 250, 33);
			WPD_RESOURCE_ALBUM_ART.pid = 0u;
			WPD_RESOURCE_GENERIC.fmtid = new Guid(3115971861u, 47728, 17991, 148, 220, 250, 73, 37, 233, 90, 7);
			WPD_RESOURCE_GENERIC.pid = 0u;
			WPD_RESOURCE_VIDEO_CLIP.fmtid = new Guid(3043421762u, 25448, 17040, 134, 98, 112, 24, 47, 183, 159, 32);
			WPD_RESOURCE_VIDEO_CLIP.pid = 0u;
			WPD_RESOURCE_BRANDING_ART.fmtid = new Guid(3056841134u, 27823, 19079, 149, 137, 34, 222, 214, 221, 88, 153);
			WPD_RESOURCE_BRANDING_ART.pid = 0u;
			WPD_PROPERTY_NULL.fmtid = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			WPD_PROPERTY_NULL.pid = 0u;
			WPD_FUNCTIONAL_OBJECT_CATEGORY.fmtid = new Guid(2399481235u, 43978, 20421, 165, 172, 176, 29, 244, 219, 229, 152);
			WPD_FUNCTIONAL_OBJECT_CATEGORY.pid = 2u;
			WPD_STORAGE_TYPE.fmtid = new Guid(27460986, 29910, 20096, 190, 167, 220, 76, 33, 44, 229, 10);
			WPD_STORAGE_TYPE.pid = 2u;
			WPD_STORAGE_FILE_SYSTEM_TYPE.fmtid = new Guid(27460986, 29910, 20096, 190, 167, 220, 76, 33, 44, 229, 10);
			WPD_STORAGE_FILE_SYSTEM_TYPE.pid = 3u;
			WPD_STORAGE_CAPACITY.fmtid = new Guid(27460986, 29910, 20096, 190, 167, 220, 76, 33, 44, 229, 10);
			WPD_STORAGE_CAPACITY.pid = 4u;
			WPD_STORAGE_FREE_SPACE_IN_BYTES.fmtid = new Guid(27460986, 29910, 20096, 190, 167, 220, 76, 33, 44, 229, 10);
			WPD_STORAGE_FREE_SPACE_IN_BYTES.pid = 5u;
			WPD_STORAGE_FREE_SPACE_IN_OBJECTS.fmtid = new Guid(27460986, 29910, 20096, 190, 167, 220, 76, 33, 44, 229, 10);
			WPD_STORAGE_FREE_SPACE_IN_OBJECTS.pid = 6u;
			WPD_STORAGE_DESCRIPTION.fmtid = new Guid(27460986, 29910, 20096, 190, 167, 220, 76, 33, 44, 229, 10);
			WPD_STORAGE_DESCRIPTION.pid = 7u;
			WPD_STORAGE_SERIAL_NUMBER.fmtid = new Guid(27460986, 29910, 20096, 190, 167, 220, 76, 33, 44, 229, 10);
			WPD_STORAGE_SERIAL_NUMBER.pid = 8u;
			WPD_STORAGE_MAX_OBJECT_SIZE.fmtid = new Guid(27460986, 29910, 20096, 190, 167, 220, 76, 33, 44, 229, 10);
			WPD_STORAGE_MAX_OBJECT_SIZE.pid = 9u;
			WPD_STORAGE_CAPACITY_IN_OBJECTS.fmtid = new Guid(27460986, 29910, 20096, 190, 167, 220, 76, 33, 44, 229, 10);
			WPD_STORAGE_CAPACITY_IN_OBJECTS.pid = 10u;
			WPD_STORAGE_ACCESS_CAPABILITY.fmtid = new Guid(27460986, 29910, 20096, 190, 167, 220, 76, 33, 44, 229, 10);
			WPD_STORAGE_ACCESS_CAPABILITY.pid = 11u;
			WPD_CLIENT_NAME.fmtid = new Guid(541957900, 8850, 16512, 159, 66, 64, 102, 78, 112, 248, 89);
			WPD_CLIENT_NAME.pid = 2u;
			WPD_CLIENT_MAJOR_VERSION.fmtid = new Guid(541957900, 8850, 16512, 159, 66, 64, 102, 78, 112, 248, 89);
			WPD_CLIENT_MAJOR_VERSION.pid = 3u;
			WPD_CLIENT_MINOR_VERSION.fmtid = new Guid(541957900, 8850, 16512, 159, 66, 64, 102, 78, 112, 248, 89);
			WPD_CLIENT_MINOR_VERSION.pid = 4u;
			WPD_CLIENT_REVISION.fmtid = new Guid(541957900, 8850, 16512, 159, 66, 64, 102, 78, 112, 248, 89);
			WPD_CLIENT_REVISION.pid = 5u;
			WPD_CLIENT_WMDRM_APPLICATION_PRIVATE_KEY.fmtid = new Guid(541957900, 8850, 16512, 159, 66, 64, 102, 78, 112, 248, 89);
			WPD_CLIENT_WMDRM_APPLICATION_PRIVATE_KEY.pid = 6u;
			WPD_CLIENT_WMDRM_APPLICATION_CERTIFICATE.fmtid = new Guid(541957900, 8850, 16512, 159, 66, 64, 102, 78, 112, 248, 89);
			WPD_CLIENT_WMDRM_APPLICATION_CERTIFICATE.pid = 7u;
			WPD_CLIENT_SECURITY_QUALITY_OF_SERVICE.fmtid = new Guid(541957900, 8850, 16512, 159, 66, 64, 102, 78, 112, 248, 89);
			WPD_CLIENT_SECURITY_QUALITY_OF_SERVICE.pid = 8u;
			WPD_CLIENT_DESIRED_ACCESS.fmtid = new Guid(541957900, 8850, 16512, 159, 66, 64, 102, 78, 112, 248, 89);
			WPD_CLIENT_DESIRED_ACCESS.pid = 9u;
			WPD_CLIENT_SHARE_MODE.fmtid = new Guid(541957900, 8850, 16512, 159, 66, 64, 102, 78, 112, 248, 89);
			WPD_CLIENT_SHARE_MODE.pid = 10u;
			WPD_CLIENT_EVENT_COOKIE.fmtid = new Guid(541957900, 8850, 16512, 159, 66, 64, 102, 78, 112, 248, 89);
			WPD_CLIENT_EVENT_COOKIE.pid = 11u;
			WPD_PROPERTY_ATTRIBUTE_FORM.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_FORM.pid = 2u;
			WPD_PROPERTY_ATTRIBUTE_CAN_READ.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_CAN_READ.pid = 3u;
			WPD_PROPERTY_ATTRIBUTE_CAN_WRITE.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_CAN_WRITE.pid = 4u;
			WPD_PROPERTY_ATTRIBUTE_CAN_DELETE.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_CAN_DELETE.pid = 5u;
			WPD_PROPERTY_ATTRIBUTE_DEFAULT_VALUE.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_DEFAULT_VALUE.pid = 6u;
			WPD_PROPERTY_ATTRIBUTE_FAST_PROPERTY.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_FAST_PROPERTY.pid = 7u;
			WPD_PROPERTY_ATTRIBUTE_RANGE_MIN.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_RANGE_MIN.pid = 8u;
			WPD_PROPERTY_ATTRIBUTE_RANGE_MAX.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_RANGE_MAX.pid = 9u;
			WPD_PROPERTY_ATTRIBUTE_RANGE_STEP.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_RANGE_STEP.pid = 10u;
			WPD_PROPERTY_ATTRIBUTE_ENUMERATION_ELEMENTS.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_ENUMERATION_ELEMENTS.pid = 11u;
			WPD_PROPERTY_ATTRIBUTE_REGULAR_EXPRESSION.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_REGULAR_EXPRESSION.pid = 12u;
			WPD_PROPERTY_ATTRIBUTE_MAX_SIZE.fmtid = new Guid(2876851160u, 25394, 17503, 160, 13, 141, 94, 241, 233, 111, 55);
			WPD_PROPERTY_ATTRIBUTE_MAX_SIZE.pid = 13u;
			WPD_PROPERTY_ATTRIBUTE_NAME.fmtid = new Guid(1570611552, 29870, 17356, 133, 169, 254, 85, 90, 128, 121, 142);
			WPD_PROPERTY_ATTRIBUTE_NAME.pid = 2u;
			WPD_PROPERTY_ATTRIBUTE_VARTYPE.fmtid = new Guid(1570611552, 29870, 17356, 133, 169, 254, 85, 90, 128, 121, 142);
			WPD_PROPERTY_ATTRIBUTE_VARTYPE.pid = 3u;
			WPD_CLASS_EXTENSION_OPTIONS_SUPPORTED_CONTENT_TYPES.fmtid = new Guid(1661599727u, 43132, 19623, 132, 52, 121, 117, 118, 228, 10, 150);
			WPD_CLASS_EXTENSION_OPTIONS_SUPPORTED_CONTENT_TYPES.pid = 2u;
			WPD_CLASS_EXTENSION_OPTIONS_DONT_REGISTER_WPD_DEVICE_INTERFACE.fmtid = new Guid(1661599727u, 43132, 19623, 132, 52, 121, 117, 118, 228, 10, 150);
			WPD_CLASS_EXTENSION_OPTIONS_DONT_REGISTER_WPD_DEVICE_INTERFACE.pid = 3u;
			WPD_CLASS_EXTENSION_OPTIONS_REGISTER_WPD_PRIVATE_DEVICE_INTERFACE.fmtid = new Guid(1661599727u, 43132, 19623, 132, 52, 121, 117, 118, 228, 10, 150);
			WPD_CLASS_EXTENSION_OPTIONS_REGISTER_WPD_PRIVATE_DEVICE_INTERFACE.pid = 4u;
			WPD_CLASS_EXTENSION_OPTIONS_MULTITRANSPORT_MODE.fmtid = new Guid(1043699162, 19825, 18942, 160, 180, 212, 64, 108, 58, 233, 63);
			WPD_CLASS_EXTENSION_OPTIONS_MULTITRANSPORT_MODE.pid = 2u;
			WPD_CLASS_EXTENSION_OPTIONS_DEVICE_IDENTIFICATION_VALUES.fmtid = new Guid(1043699162, 19825, 18942, 160, 180, 212, 64, 108, 58, 233, 63);
			WPD_CLASS_EXTENSION_OPTIONS_DEVICE_IDENTIFICATION_VALUES.pid = 3u;
			WPD_CLASS_EXTENSION_OPTIONS_TRANSPORT_BANDWIDTH.fmtid = new Guid(1043699162, 19825, 18942, 160, 180, 212, 64, 108, 58, 233, 63);
			WPD_CLASS_EXTENSION_OPTIONS_TRANSPORT_BANDWIDTH.pid = 4u;
			WPD_RESOURCE_ATTRIBUTE_TOTAL_SIZE.fmtid = new Guid(515307012u, 37496, 17055, 147, 204, 91, 184, 192, 102, 86, 182);
			WPD_RESOURCE_ATTRIBUTE_TOTAL_SIZE.pid = 2u;
			WPD_RESOURCE_ATTRIBUTE_CAN_READ.fmtid = new Guid(515307012u, 37496, 17055, 147, 204, 91, 184, 192, 102, 86, 182);
			WPD_RESOURCE_ATTRIBUTE_CAN_READ.pid = 3u;
			WPD_RESOURCE_ATTRIBUTE_CAN_WRITE.fmtid = new Guid(515307012u, 37496, 17055, 147, 204, 91, 184, 192, 102, 86, 182);
			WPD_RESOURCE_ATTRIBUTE_CAN_WRITE.pid = 4u;
			WPD_RESOURCE_ATTRIBUTE_CAN_DELETE.fmtid = new Guid(515307012u, 37496, 17055, 147, 204, 91, 184, 192, 102, 86, 182);
			WPD_RESOURCE_ATTRIBUTE_CAN_DELETE.pid = 5u;
			WPD_RESOURCE_ATTRIBUTE_OPTIMAL_READ_BUFFER_SIZE.fmtid = new Guid(515307012u, 37496, 17055, 147, 204, 91, 184, 192, 102, 86, 182);
			WPD_RESOURCE_ATTRIBUTE_OPTIMAL_READ_BUFFER_SIZE.pid = 6u;
			WPD_RESOURCE_ATTRIBUTE_OPTIMAL_WRITE_BUFFER_SIZE.fmtid = new Guid(515307012u, 37496, 17055, 147, 204, 91, 184, 192, 102, 86, 182);
			WPD_RESOURCE_ATTRIBUTE_OPTIMAL_WRITE_BUFFER_SIZE.pid = 7u;
			WPD_RESOURCE_ATTRIBUTE_FORMAT.fmtid = new Guid(515307012u, 37496, 17055, 147, 204, 91, 184, 192, 102, 86, 182);
			WPD_RESOURCE_ATTRIBUTE_FORMAT.pid = 8u;
			WPD_RESOURCE_ATTRIBUTE_RESOURCE_KEY.fmtid = new Guid(515307012u, 37496, 17055, 147, 204, 91, 184, 192, 102, 86, 182);
			WPD_RESOURCE_ATTRIBUTE_RESOURCE_KEY.pid = 9u;
			WPD_DEVICE_SYNC_PARTNER.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_SYNC_PARTNER.pid = 2u;
			WPD_DEVICE_FIRMWARE_VERSION.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_FIRMWARE_VERSION.pid = 3u;
			WPD_DEVICE_POWER_LEVEL.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_POWER_LEVEL.pid = 4u;
			WPD_DEVICE_POWER_SOURCE.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_POWER_SOURCE.pid = 5u;
			WPD_DEVICE_PROTOCOL.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_PROTOCOL.pid = 6u;
			WPD_DEVICE_MANUFACTURER.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_MANUFACTURER.pid = 7u;
			WPD_DEVICE_MODEL.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_MODEL.pid = 8u;
			WPD_DEVICE_SERIAL_NUMBER.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_SERIAL_NUMBER.pid = 9u;
			WPD_DEVICE_SUPPORTS_NON_CONSUMABLE.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_SUPPORTS_NON_CONSUMABLE.pid = 10u;
			WPD_DEVICE_DATETIME.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_DATETIME.pid = 11u;
			WPD_DEVICE_FRIENDLY_NAME.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_FRIENDLY_NAME.pid = 12u;
			WPD_DEVICE_SUPPORTED_DRM_SCHEMES.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_SUPPORTED_DRM_SCHEMES.pid = 13u;
			WPD_DEVICE_SUPPORTED_FORMATS_ARE_ORDERED.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_SUPPORTED_FORMATS_ARE_ORDERED.pid = 14u;
			WPD_DEVICE_TYPE.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_TYPE.pid = 15u;
			WPD_DEVICE_NETWORK_IDENTIFIER.fmtid = new Guid(651466650u, 58947, 17958, 158, 43, 115, 109, 192, 201, 47, 220);
			WPD_DEVICE_NETWORK_IDENTIFIER.pid = 16u;
			WPD_DEVICE_FUNCTIONAL_UNIQUE_ID.fmtid = new Guid(1178457698, 32708, 17041, 145, 28, 127, 76, 156, 202, 151, 153);
			WPD_DEVICE_FUNCTIONAL_UNIQUE_ID.pid = 2u;
			WPD_DEVICE_MODEL_UNIQUE_ID.fmtid = new Guid(1178457698, 32708, 17041, 145, 28, 127, 76, 156, 202, 151, 153);
			WPD_DEVICE_MODEL_UNIQUE_ID.pid = 3u;
			WPD_DEVICE_TRANSPORT.fmtid = new Guid(1178457698, 32708, 17041, 145, 28, 127, 76, 156, 202, 151, 153);
			WPD_DEVICE_TRANSPORT.pid = 4u;
			WPD_DEVICE_USE_DEVICE_STAGE.fmtid = new Guid(1178457698, 32708, 17041, 145, 28, 127, 76, 156, 202, 151, 153);
			WPD_DEVICE_USE_DEVICE_STAGE.pid = 5u;
			WPD_SERVICE_VERSION.fmtid = new Guid(1964009866u, 52052, 18460, 184, 219, 13, 117, 201, 63, 28, 6);
			WPD_SERVICE_VERSION.pid = 2u;
			WPD_EVENT_PARAMETER_PNP_DEVICE_ID.fmtid = new Guid(363534675u, 63511, 20463, 169, 33, 86, 118, 232, 56, 246, 224);
			WPD_EVENT_PARAMETER_PNP_DEVICE_ID.pid = 2u;
			WPD_EVENT_PARAMETER_EVENT_ID.fmtid = new Guid(363534675u, 63511, 20463, 169, 33, 86, 118, 232, 56, 246, 224);
			WPD_EVENT_PARAMETER_EVENT_ID.pid = 3u;
			WPD_EVENT_PARAMETER_OPERATION_STATE.fmtid = new Guid(363534675u, 63511, 20463, 169, 33, 86, 118, 232, 56, 246, 224);
			WPD_EVENT_PARAMETER_OPERATION_STATE.pid = 4u;
			WPD_EVENT_PARAMETER_OPERATION_PROGRESS.fmtid = new Guid(363534675u, 63511, 20463, 169, 33, 86, 118, 232, 56, 246, 224);
			WPD_EVENT_PARAMETER_OPERATION_PROGRESS.pid = 5u;
			WPD_EVENT_PARAMETER_OBJECT_PARENT_PERSISTENT_UNIQUE_ID.fmtid = new Guid(363534675u, 63511, 20463, 169, 33, 86, 118, 232, 56, 246, 224);
			WPD_EVENT_PARAMETER_OBJECT_PARENT_PERSISTENT_UNIQUE_ID.pid = 6u;
			WPD_EVENT_PARAMETER_OBJECT_CREATION_COOKIE.fmtid = new Guid(363534675u, 63511, 20463, 169, 33, 86, 118, 232, 56, 246, 224);
			WPD_EVENT_PARAMETER_OBJECT_CREATION_COOKIE.pid = 7u;
			WPD_EVENT_PARAMETER_CHILD_HIERARCHY_CHANGED.fmtid = new Guid(363534675u, 63511, 20463, 169, 33, 86, 118, 232, 56, 246, 224);
			WPD_EVENT_PARAMETER_CHILD_HIERARCHY_CHANGED.pid = 8u;
			WPD_EVENT_PARAMETER_SERVICE_METHOD_CONTEXT.fmtid = new Guid(1384151946, 18708, 17187, 155, 154, 116, 246, 84, 178, 184, 70);
			WPD_EVENT_PARAMETER_SERVICE_METHOD_CONTEXT.pid = 2u;
			WPD_EVENT_OPTION_IS_BROADCAST_EVENT.fmtid = new Guid(3017333463u, 41825, 19331, 138, 72, 91, 2, 206, 16, 113, 59);
			WPD_EVENT_OPTION_IS_BROADCAST_EVENT.pid = 2u;
			WPD_EVENT_OPTION_IS_AUTOPLAY_EVENT.fmtid = new Guid(3017333463u, 41825, 19331, 138, 72, 91, 2, 206, 16, 113, 59);
			WPD_EVENT_OPTION_IS_AUTOPLAY_EVENT.pid = 3u;
			WPD_EVENT_ATTRIBUTE_NAME.fmtid = new Guid(281634168, 11905, 16657, 173, 222, 224, 140, 166, 19, 143, 109);
			WPD_EVENT_ATTRIBUTE_NAME.pid = 2u;
			WPD_EVENT_ATTRIBUTE_PARAMETERS.fmtid = new Guid(281634168, 11905, 16657, 173, 222, 224, 140, 166, 19, 143, 109);
			WPD_EVENT_ATTRIBUTE_PARAMETERS.pid = 3u;
			WPD_EVENT_ATTRIBUTE_OPTIONS.fmtid = new Guid(281634168, 11905, 16657, 173, 222, 224, 140, 166, 19, 143, 109);
			WPD_EVENT_ATTRIBUTE_OPTIONS.pid = 4u;
			WPD_API_OPTION_USE_CLEAR_DATA_STREAM.fmtid = new Guid(283462206, 1325, 18295, 161, 60, 222, 118, 20, 190, 43, 196);
			WPD_API_OPTION_USE_CLEAR_DATA_STREAM.pid = 2u;
			WPD_API_OPTION_IOCTL_ACCESS.fmtid = new Guid(283462206, 1325, 18295, 161, 60, 222, 118, 20, 190, 43, 196);
			WPD_API_OPTION_IOCTL_ACCESS.pid = 3u;
			WPD_FORMAT_ATTRIBUTE_NAME.fmtid = new Guid(2694848512u, 48303, 19432, 179, 245, 35, 63, 35, 28, 245, 143);
			WPD_FORMAT_ATTRIBUTE_NAME.pid = 2u;
			WPD_FORMAT_ATTRIBUTE_MIMETYPE.fmtid = new Guid(2694848512u, 48303, 19432, 179, 245, 35, 63, 35, 28, 245, 143);
			WPD_FORMAT_ATTRIBUTE_MIMETYPE.pid = 3u;
			WPD_METHOD_ATTRIBUTE_NAME.fmtid = new Guid(4051325041u, 61497, 17583, 142, 254, 67, 44, 243, 46, 67, 42);
			WPD_METHOD_ATTRIBUTE_NAME.pid = 2u;
			WPD_METHOD_ATTRIBUTE_ASSOCIATED_FORMAT.fmtid = new Guid(4051325041u, 61497, 17583, 142, 254, 67, 44, 243, 46, 67, 42);
			WPD_METHOD_ATTRIBUTE_ASSOCIATED_FORMAT.pid = 3u;
			WPD_METHOD_ATTRIBUTE_ACCESS.fmtid = new Guid(4051325041u, 61497, 17583, 142, 254, 67, 44, 243, 46, 67, 42);
			WPD_METHOD_ATTRIBUTE_ACCESS.pid = 4u;
			WPD_METHOD_ATTRIBUTE_PARAMETERS.fmtid = new Guid(4051325041u, 61497, 17583, 142, 254, 67, 44, 243, 46, 67, 42);
			WPD_METHOD_ATTRIBUTE_PARAMETERS.pid = 5u;
			WPD_PARAMETER_ATTRIBUTE_ORDER.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_ORDER.pid = 2u;
			WPD_PARAMETER_ATTRIBUTE_USAGE.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_USAGE.pid = 3u;
			WPD_PARAMETER_ATTRIBUTE_FORM.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_FORM.pid = 4u;
			WPD_PARAMETER_ATTRIBUTE_DEFAULT_VALUE.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_DEFAULT_VALUE.pid = 5u;
			WPD_PARAMETER_ATTRIBUTE_RANGE_MIN.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_RANGE_MIN.pid = 6u;
			WPD_PARAMETER_ATTRIBUTE_RANGE_MAX.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_RANGE_MAX.pid = 7u;
			WPD_PARAMETER_ATTRIBUTE_RANGE_STEP.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_RANGE_STEP.pid = 8u;
			WPD_PARAMETER_ATTRIBUTE_ENUMERATION_ELEMENTS.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_ENUMERATION_ELEMENTS.pid = 9u;
			WPD_PARAMETER_ATTRIBUTE_REGULAR_EXPRESSION.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_REGULAR_EXPRESSION.pid = 10u;
			WPD_PARAMETER_ATTRIBUTE_MAX_SIZE.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_MAX_SIZE.pid = 11u;
			WPD_PARAMETER_ATTRIBUTE_VARTYPE.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_VARTYPE.pid = 12u;
			WPD_PARAMETER_ATTRIBUTE_NAME.fmtid = new Guid(3867561431u, 62245, 17898, 161, 213, 151, 207, 115, 182, 202, 88);
			WPD_PARAMETER_ATTRIBUTE_NAME.pid = 13u;
			WPD_COMMAND_COMMON_RESET_DEVICE.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_COMMAND_COMMON_RESET_DEVICE.pid = 2u;
			WPD_COMMAND_COMMON_GET_OBJECT_IDS_FROM_PERSISTENT_UNIQUE_IDS.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_COMMAND_COMMON_GET_OBJECT_IDS_FROM_PERSISTENT_UNIQUE_IDS.pid = 3u;
			WPD_COMMAND_COMMON_SAVE_CLIENT_INFORMATION.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_COMMAND_COMMON_SAVE_CLIENT_INFORMATION.pid = 4u;
			WPD_PROPERTY_COMMON_COMMAND_CATEGORY.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_PROPERTY_COMMON_COMMAND_CATEGORY.pid = 1001u;
			WPD_PROPERTY_COMMON_COMMAND_ID.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_PROPERTY_COMMON_COMMAND_ID.pid = 1002u;
			WPD_PROPERTY_COMMON_HRESULT.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_PROPERTY_COMMON_HRESULT.pid = 1003u;
			WPD_PROPERTY_COMMON_DRIVER_ERROR_CODE.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_PROPERTY_COMMON_DRIVER_ERROR_CODE.pid = 1004u;
			WPD_PROPERTY_COMMON_COMMAND_TARGET.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_PROPERTY_COMMON_COMMAND_TARGET.pid = 1006u;
			WPD_PROPERTY_COMMON_PERSISTENT_UNIQUE_IDS.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_PROPERTY_COMMON_PERSISTENT_UNIQUE_IDS.pid = 1007u;
			WPD_PROPERTY_COMMON_OBJECT_IDS.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_PROPERTY_COMMON_OBJECT_IDS.pid = 1008u;
			WPD_PROPERTY_COMMON_CLIENT_INFORMATION.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_PROPERTY_COMMON_CLIENT_INFORMATION.pid = 1009u;
			WPD_PROPERTY_COMMON_CLIENT_INFORMATION_CONTEXT.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_PROPERTY_COMMON_CLIENT_INFORMATION_CONTEXT.pid = 1010u;
			WPD_OPTION_VALID_OBJECT_IDS.fmtid = new Guid(4030868124u, 24008, 17472, 181, 189, 93, 242, 136, 53, 101, 138);
			WPD_OPTION_VALID_OBJECT_IDS.pid = 5001u;
			WPD_COMMAND_OBJECT_ENUMERATION_START_FIND.fmtid = new Guid(3074903697u, 59384, 19161, 180, 0, 173, 26, 75, 88, 238, 236);
			WPD_COMMAND_OBJECT_ENUMERATION_START_FIND.pid = 2u;
			WPD_COMMAND_OBJECT_ENUMERATION_FIND_NEXT.fmtid = new Guid(3074903697u, 59384, 19161, 180, 0, 173, 26, 75, 88, 238, 236);
			WPD_COMMAND_OBJECT_ENUMERATION_FIND_NEXT.pid = 3u;
			WPD_COMMAND_OBJECT_ENUMERATION_END_FIND.fmtid = new Guid(3074903697u, 59384, 19161, 180, 0, 173, 26, 75, 88, 238, 236);
			WPD_COMMAND_OBJECT_ENUMERATION_END_FIND.pid = 4u;
			WPD_PROPERTY_OBJECT_ENUMERATION_PARENT_ID.fmtid = new Guid(3074903697u, 59384, 19161, 180, 0, 173, 26, 75, 88, 238, 236);
			WPD_PROPERTY_OBJECT_ENUMERATION_PARENT_ID.pid = 1001u;
			WPD_PROPERTY_OBJECT_ENUMERATION_FILTER.fmtid = new Guid(3074903697u, 59384, 19161, 180, 0, 173, 26, 75, 88, 238, 236);
			WPD_PROPERTY_OBJECT_ENUMERATION_FILTER.pid = 1002u;
			WPD_PROPERTY_OBJECT_ENUMERATION_OBJECT_IDS.fmtid = new Guid(3074903697u, 59384, 19161, 180, 0, 173, 26, 75, 88, 238, 236);
			WPD_PROPERTY_OBJECT_ENUMERATION_OBJECT_IDS.pid = 1003u;
			WPD_PROPERTY_OBJECT_ENUMERATION_CONTEXT.fmtid = new Guid(3074903697u, 59384, 19161, 180, 0, 173, 26, 75, 88, 238, 236);
			WPD_PROPERTY_OBJECT_ENUMERATION_CONTEXT.pid = 1004u;
			WPD_PROPERTY_OBJECT_ENUMERATION_NUM_OBJECTS_REQUESTED.fmtid = new Guid(3074903697u, 59384, 19161, 180, 0, 173, 26, 75, 88, 238, 236);
			WPD_PROPERTY_OBJECT_ENUMERATION_NUM_OBJECTS_REQUESTED.pid = 1005u;
			WPD_COMMAND_OBJECT_PROPERTIES_GET_SUPPORTED.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_COMMAND_OBJECT_PROPERTIES_GET_SUPPORTED.pid = 2u;
			WPD_COMMAND_OBJECT_PROPERTIES_GET_ATTRIBUTES.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_COMMAND_OBJECT_PROPERTIES_GET_ATTRIBUTES.pid = 3u;
			WPD_COMMAND_OBJECT_PROPERTIES_GET.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_COMMAND_OBJECT_PROPERTIES_GET.pid = 4u;
			WPD_COMMAND_OBJECT_PROPERTIES_SET.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_COMMAND_OBJECT_PROPERTIES_SET.pid = 5u;
			WPD_COMMAND_OBJECT_PROPERTIES_GET_ALL.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_COMMAND_OBJECT_PROPERTIES_GET_ALL.pid = 6u;
			WPD_COMMAND_OBJECT_PROPERTIES_DELETE.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_COMMAND_OBJECT_PROPERTIES_DELETE.pid = 7u;
			WPD_PROPERTY_OBJECT_PROPERTIES_OBJECT_ID.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_PROPERTY_OBJECT_PROPERTIES_OBJECT_ID.pid = 1001u;
			WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_KEYS.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_KEYS.pid = 1002u;
			WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_ATTRIBUTES.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_ATTRIBUTES.pid = 1003u;
			WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_VALUES.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_VALUES.pid = 1004u;
			WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_WRITE_RESULTS.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_WRITE_RESULTS.pid = 1005u;
			WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_DELETE_RESULTS.fmtid = new Guid(2656404196u, 2068, 17638, 152, 26, 178, 153, 141, 88, 56, 4);
			WPD_PROPERTY_OBJECT_PROPERTIES_PROPERTY_DELETE_RESULTS.pid = 1006u;
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_START.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_START.pid = 2u;
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_NEXT.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_NEXT.pid = 3u;
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_END.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_LIST_END.pid = 4u;
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_START.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_START.pid = 5u;
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_NEXT.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_NEXT.pid = 6u;
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_END.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_GET_VALUES_BY_OBJECT_FORMAT_END.pid = 7u;
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_START.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_START.pid = 8u;
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_NEXT.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_NEXT.pid = 9u;
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_END.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_COMMAND_OBJECT_PROPERTIES_BULK_SET_VALUES_BY_OBJECT_LIST_END.pid = 10u;
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_OBJECT_IDS.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_OBJECT_IDS.pid = 1001u;
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_CONTEXT.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_CONTEXT.pid = 1002u;
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_VALUES.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_VALUES.pid = 1003u;
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_PROPERTY_KEYS.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_PROPERTY_KEYS.pid = 1004u;
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_DEPTH.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_DEPTH.pid = 1005u;
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_PARENT_OBJECT_ID.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_PARENT_OBJECT_ID.pid = 1006u;
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_OBJECT_FORMAT.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_OBJECT_FORMAT.pid = 1007u;
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_WRITE_RESULTS.fmtid = new Guid(298329309, 1229, 20046, 140, 123, 246, 239, 183, 148, 216, 78);
			WPD_PROPERTY_OBJECT_PROPERTIES_BULK_WRITE_RESULTS.pid = 1008u;
			WPD_COMMAND_OBJECT_RESOURCES_GET_SUPPORTED.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_COMMAND_OBJECT_RESOURCES_GET_SUPPORTED.pid = 2u;
			WPD_COMMAND_OBJECT_RESOURCES_GET_ATTRIBUTES.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_COMMAND_OBJECT_RESOURCES_GET_ATTRIBUTES.pid = 3u;
			WPD_COMMAND_OBJECT_RESOURCES_OPEN.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_COMMAND_OBJECT_RESOURCES_OPEN.pid = 4u;
			WPD_COMMAND_OBJECT_RESOURCES_READ.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_COMMAND_OBJECT_RESOURCES_READ.pid = 5u;
			WPD_COMMAND_OBJECT_RESOURCES_WRITE.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_COMMAND_OBJECT_RESOURCES_WRITE.pid = 6u;
			WPD_COMMAND_OBJECT_RESOURCES_CLOSE.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_COMMAND_OBJECT_RESOURCES_CLOSE.pid = 7u;
			WPD_COMMAND_OBJECT_RESOURCES_DELETE.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_COMMAND_OBJECT_RESOURCES_DELETE.pid = 8u;
			WPD_COMMAND_OBJECT_RESOURCES_CREATE_RESOURCE.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_COMMAND_OBJECT_RESOURCES_CREATE_RESOURCE.pid = 9u;
			WPD_COMMAND_OBJECT_RESOURCES_REVERT.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_COMMAND_OBJECT_RESOURCES_REVERT.pid = 10u;
			WPD_COMMAND_OBJECT_RESOURCES_SEEK.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_COMMAND_OBJECT_RESOURCES_SEEK.pid = 11u;
			WPD_PROPERTY_OBJECT_RESOURCES_OBJECT_ID.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_OBJECT_ID.pid = 1001u;
			WPD_PROPERTY_OBJECT_RESOURCES_ACCESS_MODE.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_ACCESS_MODE.pid = 1002u;
			WPD_PROPERTY_OBJECT_RESOURCES_RESOURCE_KEYS.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_RESOURCE_KEYS.pid = 1003u;
			WPD_PROPERTY_OBJECT_RESOURCES_RESOURCE_ATTRIBUTES.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_RESOURCE_ATTRIBUTES.pid = 1004u;
			WPD_PROPERTY_OBJECT_RESOURCES_CONTEXT.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_CONTEXT.pid = 1005u;
			WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_TO_READ.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_TO_READ.pid = 1006u;
			WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_READ.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_READ.pid = 1007u;
			WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_TO_WRITE.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_TO_WRITE.pid = 1008u;
			WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_WRITTEN.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_NUM_BYTES_WRITTEN.pid = 1009u;
			WPD_PROPERTY_OBJECT_RESOURCES_DATA.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_DATA.pid = 1010u;
			WPD_PROPERTY_OBJECT_RESOURCES_OPTIMAL_TRANSFER_BUFFER_SIZE.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_OPTIMAL_TRANSFER_BUFFER_SIZE.pid = 1011u;
			WPD_PROPERTY_OBJECT_RESOURCES_SEEK_OFFSET.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_SEEK_OFFSET.pid = 1012u;
			WPD_PROPERTY_OBJECT_RESOURCES_SEEK_ORIGIN_FLAG.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_SEEK_ORIGIN_FLAG.pid = 1013u;
			WPD_PROPERTY_OBJECT_RESOURCES_POSITION_FROM_START.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_PROPERTY_OBJECT_RESOURCES_POSITION_FROM_START.pid = 1014u;
			WPD_OPTION_OBJECT_RESOURCES_SEEK_ON_READ_SUPPORTED.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_OPTION_OBJECT_RESOURCES_SEEK_ON_READ_SUPPORTED.pid = 5001u;
			WPD_OPTION_OBJECT_RESOURCES_SEEK_ON_WRITE_SUPPORTED.fmtid = new Guid(3013784109u, 42389, 16648, 190, 10, 252, 60, 150, 95, 61, 74);
			WPD_OPTION_OBJECT_RESOURCES_SEEK_ON_WRITE_SUPPORTED.pid = 5002u;
			WPD_COMMAND_OBJECT_MANAGEMENT_CREATE_OBJECT_WITH_PROPERTIES_ONLY.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_COMMAND_OBJECT_MANAGEMENT_CREATE_OBJECT_WITH_PROPERTIES_ONLY.pid = 2u;
			WPD_COMMAND_OBJECT_MANAGEMENT_CREATE_OBJECT_WITH_PROPERTIES_AND_DATA.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_COMMAND_OBJECT_MANAGEMENT_CREATE_OBJECT_WITH_PROPERTIES_AND_DATA.pid = 3u;
			WPD_COMMAND_OBJECT_MANAGEMENT_WRITE_OBJECT_DATA.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_COMMAND_OBJECT_MANAGEMENT_WRITE_OBJECT_DATA.pid = 4u;
			WPD_COMMAND_OBJECT_MANAGEMENT_COMMIT_OBJECT.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_COMMAND_OBJECT_MANAGEMENT_COMMIT_OBJECT.pid = 5u;
			WPD_COMMAND_OBJECT_MANAGEMENT_REVERT_OBJECT.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_COMMAND_OBJECT_MANAGEMENT_REVERT_OBJECT.pid = 6u;
			WPD_COMMAND_OBJECT_MANAGEMENT_DELETE_OBJECTS.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_COMMAND_OBJECT_MANAGEMENT_DELETE_OBJECTS.pid = 7u;
			WPD_COMMAND_OBJECT_MANAGEMENT_MOVE_OBJECTS.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_COMMAND_OBJECT_MANAGEMENT_MOVE_OBJECTS.pid = 8u;
			WPD_COMMAND_OBJECT_MANAGEMENT_COPY_OBJECTS.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_COMMAND_OBJECT_MANAGEMENT_COPY_OBJECTS.pid = 9u;
			WPD_COMMAND_OBJECT_MANAGEMENT_UPDATE_OBJECT_WITH_PROPERTIES_AND_DATA.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_COMMAND_OBJECT_MANAGEMENT_UPDATE_OBJECT_WITH_PROPERTIES_AND_DATA.pid = 10u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_CREATION_PROPERTIES.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_CREATION_PROPERTIES.pid = 1001u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_CONTEXT.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_CONTEXT.pid = 1002u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_NUM_BYTES_TO_WRITE.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_NUM_BYTES_TO_WRITE.pid = 1003u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_NUM_BYTES_WRITTEN.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_NUM_BYTES_WRITTEN.pid = 1004u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_DATA.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_DATA.pid = 1005u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_ID.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_ID.pid = 1006u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_DELETE_OPTIONS.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_DELETE_OPTIONS.pid = 1007u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_OPTIMAL_TRANSFER_BUFFER_SIZE.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_OPTIMAL_TRANSFER_BUFFER_SIZE.pid = 1008u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_IDS.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_IDS.pid = 1009u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_DELETE_RESULTS.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_DELETE_RESULTS.pid = 1010u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_DESTINATION_FOLDER_OBJECT_ID.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_DESTINATION_FOLDER_OBJECT_ID.pid = 1011u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_MOVE_RESULTS.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_MOVE_RESULTS.pid = 1012u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_COPY_RESULTS.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_COPY_RESULTS.pid = 1013u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_UPDATE_PROPERTIES.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_UPDATE_PROPERTIES.pid = 1014u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_PROPERTY_KEYS.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_PROPERTY_KEYS.pid = 1015u;
			WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_FORMAT.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_PROPERTY_OBJECT_MANAGEMENT_OBJECT_FORMAT.pid = 1016u;
			WPD_OPTION_OBJECT_MANAGEMENT_RECURSIVE_DELETE_SUPPORTED.fmtid = new Guid(4011738077u, 43501, 17217, 139, 204, 24, 97, 146, 174, 160, 137);
			WPD_OPTION_OBJECT_MANAGEMENT_RECURSIVE_DELETE_SUPPORTED.pid = 5001u;
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_COMMANDS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_COMMANDS.pid = 2u;
			WPD_COMMAND_CAPABILITIES_GET_COMMAND_OPTIONS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_COMMAND_CAPABILITIES_GET_COMMAND_OPTIONS.pid = 3u;
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FUNCTIONAL_CATEGORIES.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FUNCTIONAL_CATEGORIES.pid = 4u;
			WPD_COMMAND_CAPABILITIES_GET_FUNCTIONAL_OBJECTS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_COMMAND_CAPABILITIES_GET_FUNCTIONAL_OBJECTS.pid = 5u;
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_CONTENT_TYPES.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_CONTENT_TYPES.pid = 6u;
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FORMATS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FORMATS.pid = 7u;
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FORMAT_PROPERTIES.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_FORMAT_PROPERTIES.pid = 8u;
			WPD_COMMAND_CAPABILITIES_GET_FIXED_PROPERTY_ATTRIBUTES.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_COMMAND_CAPABILITIES_GET_FIXED_PROPERTY_ATTRIBUTES.pid = 9u;
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_EVENTS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_COMMAND_CAPABILITIES_GET_SUPPORTED_EVENTS.pid = 10u;
			WPD_COMMAND_CAPABILITIES_GET_EVENT_OPTIONS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_COMMAND_CAPABILITIES_GET_EVENT_OPTIONS.pid = 11u;
			WPD_PROPERTY_CAPABILITIES_SUPPORTED_COMMANDS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_SUPPORTED_COMMANDS.pid = 1001u;
			WPD_PROPERTY_CAPABILITIES_COMMAND.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_COMMAND.pid = 1002u;
			WPD_PROPERTY_CAPABILITIES_COMMAND_OPTIONS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_COMMAND_OPTIONS.pid = 1003u;
			WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_CATEGORIES.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_CATEGORIES.pid = 1004u;
			WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_CATEGORY.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_CATEGORY.pid = 1005u;
			WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_OBJECTS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_FUNCTIONAL_OBJECTS.pid = 1006u;
			WPD_PROPERTY_CAPABILITIES_CONTENT_TYPES.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_CONTENT_TYPES.pid = 1007u;
			WPD_PROPERTY_CAPABILITIES_CONTENT_TYPE.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_CONTENT_TYPE.pid = 1008u;
			WPD_PROPERTY_CAPABILITIES_FORMATS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_FORMATS.pid = 1009u;
			WPD_PROPERTY_CAPABILITIES_FORMAT.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_FORMAT.pid = 1010u;
			WPD_PROPERTY_CAPABILITIES_PROPERTY_KEYS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_PROPERTY_KEYS.pid = 1011u;
			WPD_PROPERTY_CAPABILITIES_PROPERTY_ATTRIBUTES.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_PROPERTY_ATTRIBUTES.pid = 1012u;
			WPD_PROPERTY_CAPABILITIES_SUPPORTED_EVENTS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_SUPPORTED_EVENTS.pid = 1013u;
			WPD_PROPERTY_CAPABILITIES_EVENT.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_EVENT.pid = 1014u;
			WPD_PROPERTY_CAPABILITIES_EVENT_OPTIONS.fmtid = new Guid(212593784, 27508, 16838, 146, 22, 38, 57, 209, 252, 227, 86);
			WPD_PROPERTY_CAPABILITIES_EVENT_OPTIONS.pid = 1015u;
			WPD_COMMAND_CLASS_EXTENSION_WRITE_DEVICE_INFORMATION.fmtid = new Guid(872090897, 25763, 20396, 180, 199, 61, 254, 170, 153, 176, 81);
			WPD_COMMAND_CLASS_EXTENSION_WRITE_DEVICE_INFORMATION.pid = 2u;
			WPD_PROPERTY_CLASS_EXTENSION_DEVICE_INFORMATION_VALUES.fmtid = new Guid(872090897, 25763, 20396, 180, 199, 61, 254, 170, 153, 176, 81);
			WPD_PROPERTY_CLASS_EXTENSION_DEVICE_INFORMATION_VALUES.pid = 1001u;
			WPD_PROPERTY_CLASS_EXTENSION_DEVICE_INFORMATION_WRITE_RESULTS.fmtid = new Guid(872090897, 25763, 20396, 180, 199, 61, 254, 170, 153, 176, 81);
			WPD_PROPERTY_CLASS_EXTENSION_DEVICE_INFORMATION_WRITE_RESULTS.pid = 1002u;
			WPD_COMMAND_CLASS_EXTENSION_REGISTER_SERVICE_INTERFACES.fmtid = new Guid(2131196341u, 64043, 18278, 156, 178, 247, 59, 163, 11, 103, 88);
			WPD_COMMAND_CLASS_EXTENSION_REGISTER_SERVICE_INTERFACES.pid = 2u;
			WPD_COMMAND_CLASS_EXTENSION_UNREGISTER_SERVICE_INTERFACES.fmtid = new Guid(2131196341u, 64043, 18278, 156, 178, 247, 59, 163, 11, 103, 88);
			WPD_COMMAND_CLASS_EXTENSION_UNREGISTER_SERVICE_INTERFACES.pid = 3u;
			WPD_PROPERTY_CLASS_EXTENSION_SERVICE_OBJECT_ID.fmtid = new Guid(2131196341u, 64043, 18278, 156, 178, 247, 59, 163, 11, 103, 88);
			WPD_PROPERTY_CLASS_EXTENSION_SERVICE_OBJECT_ID.pid = 1001u;
			WPD_PROPERTY_CLASS_EXTENSION_SERVICE_INTERFACES.fmtid = new Guid(2131196341u, 64043, 18278, 156, 178, 247, 59, 163, 11, 103, 88);
			WPD_PROPERTY_CLASS_EXTENSION_SERVICE_INTERFACES.pid = 1002u;
			WPD_PROPERTY_CLASS_EXTENSION_SERVICE_REGISTRATION_RESULTS.fmtid = new Guid(2131196341u, 64043, 18278, 156, 178, 247, 59, 163, 11, 103, 88);
			WPD_PROPERTY_CLASS_EXTENSION_SERVICE_REGISTRATION_RESULTS.pid = 1003u;
			WPD_COMMAND_SERVICE_COMMON_GET_SERVICE_OBJECT_ID.fmtid = new Guid(841942813, 14063, 18303, 180, 181, 111, 82, 215, 52, 186, 238);
			WPD_COMMAND_SERVICE_COMMON_GET_SERVICE_OBJECT_ID.pid = 2u;
			WPD_PROPERTY_SERVICE_OBJECT_ID.fmtid = new Guid(841942813, 14063, 18303, 180, 181, 111, 82, 215, 52, 186, 238);
			WPD_PROPERTY_SERVICE_OBJECT_ID.pid = 1001u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_METHODS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_METHODS.pid = 2u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_METHODS_BY_FORMAT.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_METHODS_BY_FORMAT.pid = 3u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_METHOD_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_METHOD_ATTRIBUTES.pid = 4u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_METHOD_PARAMETER_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_METHOD_PARAMETER_ATTRIBUTES.pid = 5u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_FORMATS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_FORMATS.pid = 6u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_ATTRIBUTES.pid = 7u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_FORMAT_PROPERTIES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_FORMAT_PROPERTIES.pid = 8u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_PROPERTY_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_PROPERTY_ATTRIBUTES.pid = 9u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_EVENTS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_EVENTS.pid = 10u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_EVENT_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_EVENT_ATTRIBUTES.pid = 11u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_EVENT_PARAMETER_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_EVENT_PARAMETER_ATTRIBUTES.pid = 12u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_INHERITED_SERVICES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_INHERITED_SERVICES.pid = 13u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_RENDERING_PROFILES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_FORMAT_RENDERING_PROFILES.pid = 14u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_COMMANDS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_SUPPORTED_COMMANDS.pid = 15u;
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_COMMAND_OPTIONS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_COMMAND_SERVICE_CAPABILITIES_GET_COMMAND_OPTIONS.pid = 16u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_METHODS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_METHODS.pid = 1001u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_FORMAT.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_FORMAT.pid = 1002u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_METHOD.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_METHOD.pid = 1003u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_METHOD_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_METHOD_ATTRIBUTES.pid = 1004u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_PARAMETER.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_PARAMETER.pid = 1005u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_PARAMETER_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_PARAMETER_ATTRIBUTES.pid = 1006u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_FORMATS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_FORMATS.pid = 1007u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_FORMAT_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_FORMAT_ATTRIBUTES.pid = 1008u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_PROPERTY_KEYS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_PROPERTY_KEYS.pid = 1009u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_PROPERTY_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_PROPERTY_ATTRIBUTES.pid = 1010u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_EVENTS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_EVENTS.pid = 1011u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_EVENT.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_EVENT.pid = 1012u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_EVENT_ATTRIBUTES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_EVENT_ATTRIBUTES.pid = 1013u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_INHERITANCE_TYPE.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_INHERITANCE_TYPE.pid = 1014u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_INHERITED_SERVICES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_INHERITED_SERVICES.pid = 1015u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_RENDERING_PROFILES.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_RENDERING_PROFILES.pid = 1016u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_COMMANDS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_SUPPORTED_COMMANDS.pid = 1017u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_COMMAND.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_COMMAND.pid = 1018u;
			WPD_PROPERTY_SERVICE_CAPABILITIES_COMMAND_OPTIONS.fmtid = new Guid(608534132, 11935, 17657, 140, 87, 29, 27, 203, 23, 11, 137);
			WPD_PROPERTY_SERVICE_CAPABILITIES_COMMAND_OPTIONS.pid = 1019u;
			WPD_COMMAND_SERVICE_METHODS_START_INVOKE.fmtid = new Guid(760356008u, 49584, 17000, 163, 66, 207, 25, 50, 21, 105, 188);
			WPD_COMMAND_SERVICE_METHODS_START_INVOKE.pid = 2u;
			WPD_COMMAND_SERVICE_METHODS_CANCEL_INVOKE.fmtid = new Guid(760356008u, 49584, 17000, 163, 66, 207, 25, 50, 21, 105, 188);
			WPD_COMMAND_SERVICE_METHODS_CANCEL_INVOKE.pid = 3u;
			WPD_COMMAND_SERVICE_METHODS_END_INVOKE.fmtid = new Guid(760356008u, 49584, 17000, 163, 66, 207, 25, 50, 21, 105, 188);
			WPD_COMMAND_SERVICE_METHODS_END_INVOKE.pid = 4u;
			WPD_PROPERTY_SERVICE_METHOD.fmtid = new Guid(760356008u, 49584, 17000, 163, 66, 207, 25, 50, 21, 105, 188);
			WPD_PROPERTY_SERVICE_METHOD.pid = 1001u;
			WPD_PROPERTY_SERVICE_METHOD_PARAMETER_VALUES.fmtid = new Guid(760356008u, 49584, 17000, 163, 66, 207, 25, 50, 21, 105, 188);
			WPD_PROPERTY_SERVICE_METHOD_PARAMETER_VALUES.pid = 1002u;
			WPD_PROPERTY_SERVICE_METHOD_RESULT_VALUES.fmtid = new Guid(760356008u, 49584, 17000, 163, 66, 207, 25, 50, 21, 105, 188);
			WPD_PROPERTY_SERVICE_METHOD_RESULT_VALUES.pid = 1003u;
			WPD_PROPERTY_SERVICE_METHOD_CONTEXT.fmtid = new Guid(760356008u, 49584, 17000, 163, 66, 207, 25, 50, 21, 105, 188);
			WPD_PROPERTY_SERVICE_METHOD_CONTEXT.pid = 1004u;
			WPD_PROPERTY_SERVICE_METHOD_HRESULT.fmtid = new Guid(760356008u, 49584, 17000, 163, 66, 207, 25, 50, 21, 105, 188);
			WPD_PROPERTY_SERVICE_METHOD_HRESULT.pid = 1005u;
		}
	}
}

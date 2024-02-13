using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace cYo.Common.Win32.PortableDevices
{
	public class Device
	{
		public const string RootNode = "DEVICE";

		private PortableDeviceApi.IPortableDevice portableDevice;

		private PortableDeviceApi.IPortableDeviceValues portableDeviceValues;

		private PortableDeviceApi.IPortableDeviceContent portableContent;

		public string Id
		{
			get;
			private set;
		}

		public string Name
		{
			get
			{
				string stringValue = GetStringValue(PortableDeviceApi.WPD_DEVICE_FRIENDLY_NAME);
				if (!string.IsNullOrEmpty(stringValue))
				{
					return stringValue;
				}
				return Model;
			}
		}

		public string Model => GetStringValue(PortableDeviceApi.WPD_DEVICE_MODEL);

		public string Manufacturer => GetStringValue(PortableDeviceApi.WPD_DEVICE_MANUFACTURER);

		public string Key => Model + ":" + SerialNumber;

		public string SerialNumber => GetStringValue(PortableDeviceApi.WPD_DEVICE_SERIAL_NUMBER);

		public bool IsConnected => portableDevice != null;

		public DeviceFolder Root
		{
			get
			{
				Connect();
				return new DeviceFolder(this);
			}
		}

		public Device(string id)
		{
			Id = id;
		}

		public void Connect()
		{
			if (portableDevice == null)
			{
				portableDevice = (PortableDeviceApi.IPortableDevice)new PortableDeviceApi.PortableDevice();
				portableDeviceValues = (PortableDeviceApi.IPortableDeviceValues)new PortableDeviceApi.PortableDeviceValues();
				portableDevice.Open(Id, portableDeviceValues);
				portableDevice.Content(out portableContent);
			}
		}

		public void Disconnect()
		{
			if (portableDevice != null)
			{
				portableDevice.Close();
				portableDevice = null;
			}
		}

		public IEnumerable<DeviceItem> EnumerateItems(DeviceFolder parent)
		{
			portableContent.Properties(out var properties);
			PortableDeviceApi.IEnumPortableDeviceObjectIDs objectIds = null;
			portableContent.EnumObjects(0u, parent.Id, null, out objectIds);
			string[] ids = new string[100];
			uint fetched = 0u;
			do
			{
				objectIds.Next((uint)ids.Length, ids, ref fetched);
				for (int i = 0; i < fetched; i++)
				{
					yield return WrapObject(parent, properties, ids[i]);
				}
			}
			while (fetched != 0);
		}

		public Stream ReadFile(DeviceFile file)
		{
			try
			{
				uint pdwOptimalBufferSize = 0u;
				portableContent.Transfer(out var ppResources);
				ppResources.GetStream(file.Id, ref PortableDeviceApi.WPD_RESOURCE_DEFAULT, 0u, ref pdwOptimalBufferSize, out var ppStream);
				byte[] array = new byte[pdwOptimalBufferSize];
				MemoryStream memoryStream = new MemoryStream();
				uint pcbRead;
				do
				{
					ppStream.RemoteRead(array, pdwOptimalBufferSize, out pcbRead);
					memoryStream.Write(array, 0, (int)pcbRead);
				}
				while (pcbRead != 0);
				Marshal.ReleaseComObject(ppStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public DeviceFolder CreateFolder(DeviceFolder parent, string folderName)
		{
			folderName = Path.GetFileName(folderName);
			PortableDeviceApi.IPortableDeviceValues portableDeviceValues = (PortableDeviceApi.IPortableDeviceValues)new PortableDeviceApi.PortableDeviceValues();
			portableDeviceValues.SetStringValue(ref PortableDeviceApi.WPD_OBJECT_PARENT_ID, parent.Id);
			portableDeviceValues.SetStringValue(ref PortableDeviceApi.WPD_OBJECT_NAME, folderName);
			portableDeviceValues.SetGuidValue(ref PortableDeviceApi.WPD_OBJECT_CONTENT_TYPE, ref PortableDeviceApi.WPD_CONTENT_TYPE_FOLDER);
			portableContent.CreateObjectWithPropertiesOnly(portableDeviceValues, out var ppszObjectID);
			return new DeviceFolder(parent, ppszObjectID, folderName);
		}

		public DeviceFile WriteFile(DeviceFolder folder, string fileName, Stream dataStream, int size = -1)
		{
			fileName = Path.GetFileName(fileName);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
			string text = Path.GetExtension(fileName).ToLower();
			DeviceItem deviceItem = folder.Items.FirstOrDefault((DeviceItem item) => item.Name == fileName);
			if (deviceItem != null)
			{
				if (!(deviceItem is DeviceFile))
				{
					throw new IOException("Can not create file with same name as folder");
				}
				deviceItem.Delete();
			}
			PortableDeviceApi.IPortableDeviceValues portableDeviceValues = (PortableDeviceApi.IPortableDeviceValues)new PortableDeviceApi.PortableDeviceValues();
			if (size == -1)
			{
				size = (int)dataStream.Length;
			}
			portableDeviceValues.SetStringValue(ref PortableDeviceApi.WPD_OBJECT_PARENT_ID, folder.Id);
			portableDeviceValues.SetUnsignedLargeIntegerValue(ref PortableDeviceApi.WPD_OBJECT_SIZE, (ulong)size);
			portableDeviceValues.SetStringValue(ref PortableDeviceApi.WPD_OBJECT_NAME, fileNameWithoutExtension);
			portableDeviceValues.SetStringValue(ref PortableDeviceApi.WPD_OBJECT_ORIGINAL_FILE_NAME, fileName);
			portableDeviceValues.SetGuidValue(ref PortableDeviceApi.WPD_OBJECT_CONTENT_TYPE, ref PortableDeviceApi.WPD_CONTENT_TYPE_GENERIC_FILE);
			Guid Value;
			switch (text)
			{
			case ".xml":
				Value = PortableDeviceApi.WPD_OBJECT_FORMAT_XML;
				break;
			case ".jpg":
			case ".jpeg":
			case ".jiff":
				Value = PortableDeviceApi.WPD_OBJECT_FORMAT_JFIF;
				break;
			case ".txt":
				Value = PortableDeviceApi.WPD_OBJECT_FORMAT_TEXT;
				break;
			case ".tif":
			case ".tiff":
				Value = PortableDeviceApi.WPD_OBJECT_FORMAT_TIFF;
				break;
			default:
				Value = PortableDeviceApi.WPD_OBJECT_FORMAT_UNSPECIFIED;
				break;
			}
			portableDeviceValues.SetGuidValue(ref PortableDeviceApi.WPD_OBJECT_FORMAT, ref Value);
			uint pdwOptimalWriteBufferSize = 0u;
			string ppszCookie = null;
			portableContent.CreateObjectWithPropertiesAndData(portableDeviceValues, out var ppData, ref pdwOptimalWriteBufferSize, ref ppszCookie);
			PortableDeviceApi.IPortableDeviceDataStream portableDeviceDataStream = (PortableDeviceApi.IPortableDeviceDataStream)ppData;
			uint num = 0u;
			byte[] array = new byte[pdwOptimalWriteBufferSize];
			while (true)
			{
				int num2 = dataStream.Read(array, 0, Math.Min((int)pdwOptimalWriteBufferSize, size));
				if (num2 == 0)
				{
					break;
				}
				size -= num2;
				ppData.RemoteWrite(array, (uint)num2, out var pcbWritten);
				num += pcbWritten;
			}
			ppData.Commit(0u);
			portableDeviceDataStream.GetObjectID(out var ppszObjectID);
			return new DeviceFile(folder, ppszObjectID, fileNameWithoutExtension, num);
		}

		public void Delete(IEnumerable<DeviceItem> portableDeviceItems)
		{
			PortableDeviceApi.IPortableDevicePropVariantCollection portableDevicePropVariantCollection = (PortableDeviceApi.IPortableDevicePropVariantCollection)new PortableDeviceApi.PortableDevicePropVariantCollection();
			PortableDeviceApi.IPortableDevicePropVariantCollection ppResults = (PortableDeviceApi.IPortableDevicePropVariantCollection)new PortableDeviceApi.PortableDevicePropVariantCollection();
			foreach (DeviceItem portableDeviceItem in portableDeviceItems)
			{
				StringToPropVariant(portableDeviceItem.Id, out var propvarValue);
				portableDevicePropVariantCollection.Add(ref propvarValue);
			}
			portableContent.Delete(0u, portableDevicePropVariantCollection, ref ppResults);
		}

		public void Delete(params DeviceItem[] items)
		{
			Delete(items.AsEnumerable());
		}

		public long GetFreeSpace(DeviceFolder folder)
		{
			DeviceStorageFolder storageParent = folder.StorageParent;
			if (storageParent == null)
			{
				return 0L;
			}
			ulong pValue = 0uL;
			try
			{
				GetValues(storageParent.Id).GetUnsignedLargeIntegerValue(ref PortableDeviceApi.WPD_STORAGE_FREE_SPACE_IN_BYTES, out pValue);
				return (long)pValue;
			}
			catch
			{
				return (long)pValue;
			}
		}

		public DeviceItem Find(string regex, int maxLevel = -1)
		{
			return Root.Find(regex, maxLevel);
		}

		public DeviceFolder GetFolder(string path)
		{
			return GetItem(path) as DeviceFolder;
		}

		public DeviceFile GetFile(string path)
		{
			return GetItem(path) as DeviceFile;
		}

		public void DeleteFile(string path)
		{
			DeviceFile file = GetFile(path);
			if (file != null)
			{
				Delete(file);
			}
		}

		public void DeleteFolder(string path)
		{
			DeviceFolder folder = GetFolder(path);
			if (folder != null)
			{
				Delete(folder);
			}
		}

		public bool FileExists(string path)
		{
			return GetFile(path) != null;
		}

		public bool FolderExists(string path)
		{
			return GetFolder(path) != null;
		}

		public DeviceFile WriteFile(string path, Stream dataStream, int size = -1, bool autoCreateFolders = true)
		{
			string directoryName = Path.GetDirectoryName(path);
			string fileName = Path.GetFileName(path);
			return WriteFile(CreateFolder(directoryName), fileName, dataStream, size);
		}

		public DeviceFolder CreateFolder(string path)
		{
			return GetItem(path, createFolders: true) as DeviceFolder;
		}

		private DeviceItem GetItem(string absolutePath, bool createFolders = false)
		{
			DeviceFolder deviceFolder = Root;
			string[] array = GetPathParts(absolutePath).ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				bool flag = false;
				bool flag2 = i == array.Length - 1;
				foreach (DeviceItem item in EnumerateItems(deviceFolder))
				{
					if (item.Name != text)
					{
						continue;
					}
					flag = true;
					if (item is DeviceFile)
					{
						return flag2 ? item : null;
					}
					if (item is DeviceFolder)
					{
						deviceFolder = item as DeviceFolder;
						if (flag2)
						{
							return deviceFolder;
						}
					}
				}
				if (!flag)
				{
					if (!createFolders)
					{
						break;
					}
					deviceFolder = deviceFolder.CreateFolder(text);
					if (flag2)
					{
						return deviceFolder;
					}
				}
			}
			return null;
		}

		private string GetStringValue(PortableDeviceApi._tagpropertykey id)
		{
			GetValues(RootNode).GetStringValue(ref id, out var pValue);
			return pValue;
		}

		private PortableDeviceApi.IPortableDeviceValues GetValues(string id)
		{
			Connect();
			portableContent.Properties(out var ppProperties);
			ppProperties.GetValues(id, null, out var ppValues);
			return ppValues;
		}

		private static IEnumerable<string> GetPathParts(string absolutePath)
		{
			return from s in absolutePath.Split('\\', '/')
				select s.Trim() into s
				where !string.IsNullOrEmpty(s)
				select s;
		}

		private static DeviceItem WrapObject(DeviceFolder folder, PortableDeviceApi.IPortableDeviceProperties properties, string objectId)
		{
			properties.GetSupportedProperties(objectId, out var ppKeys);
			properties.GetValues(objectId, ppKeys, out var ppValues);
			ppValues.GetStringValue(ref PortableDeviceApi.WPD_OBJECT_NAME, out var pValue);
			ppValues.GetGuidValue(ref PortableDeviceApi.WPD_OBJECT_CONTENT_TYPE, out var pValue2);
			if (pValue2 == PortableDeviceApi.WPD_CONTENT_TYPE_FUNCTIONAL_OBJECT)
			{
				ppValues.GetGuidValue(ref PortableDeviceApi.WPD_FUNCTIONAL_OBJECT_CATEGORY, out var pValue3);
				if (pValue3 == PortableDeviceApi.WPD_FUNCTIONAL_CATEGORY_STORAGE)
				{
					string pValue4 = "Unknown";
					try
					{
						ppValues.GetStringValue(ref PortableDeviceApi.WPD_STORAGE_FILE_SYSTEM_TYPE, out pValue4);
					}
					catch
					{
					}
					return new DeviceStorageFolder(folder, objectId, pValue, pValue4);
				}
			}
			if (pValue2 == PortableDeviceApi.WPD_CONTENT_TYPE_FOLDER)
			{
				return new DeviceFolder(folder, objectId, pValue);
			}
			ulong pValue5;
			try
			{
				ppValues.GetUnsignedLargeIntegerValue(ref PortableDeviceApi.WPD_OBJECT_SIZE, out pValue5);
			}
			catch (Exception)
			{
				pValue5 = 0uL;
			}
			try
			{
				ppValues.GetStringValue(ref PortableDeviceApi.WPD_OBJECT_ORIGINAL_FILE_NAME, out var pValue6);
				pValue = pValue6;
			}
			catch
			{
			}
			return new DeviceFile(folder, objectId, pValue, (long)pValue5);
		}

		private static void StringToPropVariant(string value, out PortableDeviceApi.tag_inner_PROPVARIANT propvarValue)
		{
			PortableDeviceApi.IPortableDeviceValues portableDeviceValues = (PortableDeviceApi.IPortableDeviceValues)new PortableDeviceApi.PortableDeviceValues();
			portableDeviceValues.SetStringValue(ref PortableDeviceApi.WPD_OBJECT_ID, value);
			portableDeviceValues.GetValue(ref PortableDeviceApi.WPD_OBJECT_ID, out propvarValue);
		}
	}
}

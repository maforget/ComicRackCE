using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace cYo.Common.Compression.SevenZip
{
	[StructLayout(LayoutKind.Explicit, Size = 64)]
	public struct PropVariant
	{
		[FieldOffset(0)]
		public short vt;

		[FieldOffset(8)]
		public IntPtr pointerValue;

		[FieldOffset(8)]
		public byte byteValue;

		[FieldOffset(8)]
		public short shortValue;

		[FieldOffset(8)]
		public int intValue;

		[FieldOffset(8)]
		public long longValue;

		[FieldOffset(8)]
		public float floatValue;

		[FieldOffset(8)]
		public double doubleValue;

		[FieldOffset(8)]
		public System.Runtime.InteropServices.ComTypes.FILETIME filetime;

		public VarEnum VarType => (VarEnum)vt;

		[DllImport("ole32.dll")]
		private static extern int PropVariantClear(ref PropVariant pvar);

		public void Clear()
		{
			switch (VarType)
			{
			case VarEnum.VT_NULL:
			case VarEnum.VT_I2:
			case VarEnum.VT_I4:
			case VarEnum.VT_R4:
			case VarEnum.VT_R8:
			case VarEnum.VT_CY:
			case VarEnum.VT_DATE:
			case VarEnum.VT_ERROR:
			case VarEnum.VT_BOOL:
			case VarEnum.VT_I1:
			case VarEnum.VT_UI1:
			case VarEnum.VT_UI2:
			case VarEnum.VT_UI4:
			case VarEnum.VT_I8:
			case VarEnum.VT_UI8:
			case VarEnum.VT_INT:
			case VarEnum.VT_UINT:
			case VarEnum.VT_HRESULT:
			case VarEnum.VT_FILETIME:
				vt = 0;
				break;
			case VarEnum.VT_BSTR:
				Marshal.FreeBSTR(pointerValue);
				vt = 0;
				break;
			default:
				PropVariantClear(ref this);
				break;
			case VarEnum.VT_EMPTY:
				break;
			}
		}

		public object GetObject()
		{
			switch (VarType)
			{
			case VarEnum.VT_EMPTY:
				return null;
			case VarEnum.VT_FILETIME:
				return DateTime.FromFileTime(longValue);
			default:
			{
				GCHandle gCHandle = GCHandle.Alloc(this, GCHandleType.Pinned);
				try
				{
					return Marshal.GetObjectForNativeVariant(gCHandle.AddrOfPinnedObject());
				}
				finally
				{
					gCHandle.Free();
				}
			}
			}
		}

		public void SetObject(object value)
		{
			if (value == null)
			{
				vt = 0;
				return;
			}
			switch (Type.GetTypeCode(value.GetType()))
			{
			case TypeCode.DBNull:
				vt = 1;
				break;
			case TypeCode.Boolean:
				shortValue = Convert.ToInt16(value);
				vt = 11;
				break;
			case TypeCode.SByte:
				byteValue = (byte)value;
				vt = 16;
				break;
			case TypeCode.Byte:
				byteValue = (byte)value;
				vt = 17;
				break;
			case TypeCode.Int16:
				shortValue = (short)value;
				vt = 2;
				break;
			case TypeCode.UInt16:
				shortValue = (short)value;
				vt = 18;
				break;
			case TypeCode.Int32:
				intValue = (int)value;
				vt = 3;
				break;
			case TypeCode.UInt32:
				intValue = (int)value;
				vt = 19;
				break;
			case TypeCode.Int64:
				longValue = (long)value;
				vt = 20;
				break;
			case TypeCode.UInt64:
				longValue = (long)value;
				vt = 21;
				break;
			case TypeCode.Single:
				floatValue = (float)value;
				vt = 4;
				break;
			case TypeCode.Double:
				doubleValue = (double)value;
				vt = 5;
				break;
			case TypeCode.String:
				pointerValue = Marshal.StringToBSTR((string)value);
				vt = 8;
				break;
			case TypeCode.Char:
			case TypeCode.Decimal:
			case TypeCode.DateTime:
			case (TypeCode)17:
				break;
			}
		}
	}
}

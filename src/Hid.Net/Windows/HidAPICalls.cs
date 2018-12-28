﻿using Device.Net;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Hid.Net.Windows
{
    public static class HidAPICalls 
    {
        #region Constants
        public const int DigcfDeviceinterface = 16;
        public const int DigcfPresent = 2;
        public const uint FileShareRead = 1;
        public const uint FileShareWrite = 2;
        public const uint GenericRead = 2147483648;
        public const uint GenericWrite = 1073741824;
        public const uint OpenExisting = 3;
        public const int HIDP_STATUS_SUCCESS = 0x110000;
        public const int HIDP_STATUS_INVALID_PREPARSED_DATA = -0x3FEF0000;
        #endregion        

        #region API Calls

        #region Hid
        [DllImport("hid.dll", SetLastError = true)]
        internal static extern bool HidD_FreePreparsedData(ref IntPtr pointerToPreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern bool HidD_GetAttributes(SafeFileHandle hidDeviceObject, ref HidAttributes attributes);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern void HidD_GetHidGuid(ref Guid hidGuid);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool HidD_GetManufacturerString(SafeFileHandle hidDeviceObject, IntPtr pointerToBuffer, uint bufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern bool HidD_GetPreparsedData(SafeFileHandle hidDeviceObject, out IntPtr pointerToPreparsedData);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool HidD_GetProductString(SafeFileHandle hidDeviceObject, IntPtr pointerToBuffer, uint bufferLength);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool HidD_GetSerialNumberString(SafeFileHandle hidDeviceObject, IntPtr pointerToBuffer, uint bufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern int HidP_GetCaps(IntPtr pointerToPreparsedData, out HidCollectionCapabilities hidCollectionCapabilities);
        #endregion

        #endregion

        #region Helper Methods
        public static HidCollectionCapabilities GetHidCapabilities(SafeFileHandle readSafeFileHandle)
        {
            var isSuccess = HidD_GetPreparsedData(readSafeFileHandle, out var pointerToPreParsedData);
            WindowsDeviceBase.HandleError(isSuccess, "Could not get pre parsed data");

            var result = HidP_GetCaps(pointerToPreParsedData, out var hidCollectionCapabilities);
            if (result != HIDP_STATUS_SUCCESS)
            {
                throw new Exception($"Could not get Hid capabilities. Return code: {result}");
            }

            return hidCollectionCapabilities;
        }
        #endregion
    }
}

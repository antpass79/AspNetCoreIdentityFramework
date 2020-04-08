﻿using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Globe.Client.Platform.Extensions
{
    public static class StringStringExtensions
    {
        public static string ToPlainString(this SecureString secureString)
        {
            if (secureString == null)
                return string.Empty;

            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}

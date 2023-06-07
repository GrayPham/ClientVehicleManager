using Connect.Common;
using Connect.Common.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.RemoteDataProvider.Common
{
    public class ConnectionHelper
    {
        public const byte InvalidCommand = 0x80;
        public const byte InvalidSerialNumber = 0x81;
        public const byte InvalidLicenseKey = 0x82;
        public const byte MacError = 0x83;
        public const byte Unknown = 0xFF;

        public const byte FunctionCreateSession = 0x1;
        public const byte FunctionReconnectSession = 0x2;
        public const byte FunctionCloseSession = 0x3;
        public const byte FunctionSessionError = 0x4;
        public const byte FunctionClientList = 0x5;
        public static String ToString(int function)
        {
            if (function == FunctionCreateSession) return @"FunctionCreateSession";
            if (function == FunctionReconnectSession) return @"FunctionReconnectSession";
            if (function == FunctionCloseSession) return @"FunctionCloseSession";
            if (function == FunctionSessionError) return @"FunctionSessionError";
            if (function == FunctionClientList) return @"FunctionClientList";
            return "";
        }

        public static ConnectionFailedReason GetFailedReason(byte reason)
        {
            if (reason == InvalidSerialNumber) return ConnectionFailedReason.InvalidSerialNumber;
            if (reason == InvalidLicenseKey) return ConnectionFailedReason.InvalidLicenseKey;
            if (reason == MacError) return ConnectionFailedReason.MacError;
            return ConnectionFailedReason.Unknown;
        }

        public static byte GetFailedCode(ConnectionFailedReason reason)
        {
            switch (reason)
            {
                case ConnectionFailedReason.InvalidSerialNumber:
                    return InvalidSerialNumber;
                case ConnectionFailedReason.InvalidLicenseKey:
                    return InvalidLicenseKey;
                case ConnectionFailedReason.MacError:
                    return MacError;
                default:
                    return Unknown;
            }
        }
    }
}

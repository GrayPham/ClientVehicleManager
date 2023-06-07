using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.Common
{
    public enum EConnectType { LinQToSQL, WebAPI, TCPIP, Provider }
    public class ConnectDatas
    {
        public const int TimeOut = 20 * 1000;
        public const int TimeOutAPI = 20 * 1000;

        public static EConnectType ConnectType = EConnectType.TCPIP;
        public static EConnectType UpdateType = EConnectType.TCPIP;
    }
}

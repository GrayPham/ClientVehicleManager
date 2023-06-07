using Connect.Common.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Connect.Common.Helper
{
    public class ProviderHelper
    {
        //**-------------------------------------------------------------------------------
        public static bool IsNumeric(object type)
        {
            if (type == null) return false;
            TypeCode typeCode = Type.GetTypeCode(type.GetType());
            switch (typeCode)
            {
                case TypeCode.Byte:

                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
            return false;
        }

        //**-------------------------------------------------------------------------------
        public static int S2Int(object value)
        {
            try
            {
                return int.Parse("0" + value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static DateTime S2Date(object value)
        {
            try
            {
                return DateTime.Parse("" + value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static double S2Double(object value)
        {
            try
            {
                return double.Parse("" + value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static double ToDouble(double? value)
        {
            if (value == null) return 0;
            else
                return value.Value;
        }
        public static int OToInt(object value)
        {
            if (value == null) return 0;
            else
                try
                {
                    return int.Parse("0" + value);
                }
                catch (Exception)
                {
                    return 0;
                }

        }
        public static Int64 OToInt64(object value)
        {
            if (value == null) return 0;
            else
                try
                {
                    return Int64.Parse("0" + value);
                }
                catch (Exception)
                {
                    return 0;
                }

        }
        public static double OToDouble(object value)
        {
            if (value == null) return 0;
            else
                try
                {
                    return Double.Parse("" + value);
                }
                catch (Exception)
                {
                    return 0;
                }
        }
        public static decimal ToDecimal(decimal? value)
        {
            if (value == null) return 0;
            else
                return value.Value;
        }
        public static decimal DToDecimal(double? value)
        {
            if (value == null) return 0;
            else
                return (decimal)value.Value;
        }
        public static double DToDouble(decimal? value)
        {
            if (value == null) return 0;
            else
                return (double)value.Value;
        }
        public static bool S2Bool(object value)
        {
            try
            {

                if (("" + value).ToUpper() == "TRUE") return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool S2Bool(bool? value)
        {
            try
            {
                if (value == null) return false;
                return value.Value;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //**-------------------------------------------------------------------------------
        public static void ConvertEPlatform(EPlatform senum, out String st)
        {
            st = "";
            switch (senum)
            {
                case EPlatform.None:
                    st = nameof(EPlatform.None);
                    break;
                case EPlatform.Windows:
                    st = nameof(EPlatform.Windows);
                    break;
                case EPlatform.Web:
                    st = nameof(EPlatform.Web);
                    break;
                case EPlatform.Android:
                    st = nameof(EPlatform.Android);
                    break;
                case EPlatform.IOS:
                    st = nameof(EPlatform.IOS);
                    break;
                case EPlatform.WindowPhone:
                    st = nameof(EPlatform.WindowPhone);
                    break;
                default:
                    st = nameof(EPlatform.None);
                    break;
            }
        }
        public static EPlatform S2EPlatform(string key)
        {
            switch (key)
            {
                case nameof(EPlatform.None):
                    return EPlatform.None;
                case nameof(EPlatform.Windows):
                    return EPlatform.Windows;
                case nameof(EPlatform.Web):
                    return EPlatform.Web;
                case nameof(EPlatform.Android):
                    return EPlatform.Android;
                case nameof(EPlatform.IOS):
                    return EPlatform.IOS;
                case nameof(EPlatform.WindowPhone):
                    return EPlatform.WindowPhone;
                default:
                    return EPlatform.None;
            }
        }
        //**-------------------------------------------------------------------------------
        public static void ConvertEClientType(EClientType senum, out String st)
        {
            st = "";
            switch (senum)
            {
                case EClientType.None:
                    st = "Không xác định";
                    break;
                case EClientType.Replication:
                    st = "Replication";
                    break;
                case EClientType.Admin:
                    st = "Admin";
                    break;
                case EClientType.Manager:
                    st = "Manager";
                    break;
                case EClientType.Supervisor:
                    st = "Supervisor";
                    break;
                case EClientType.ClientNormal:
                    st = "ClientNormal";
                    break;
                case EClientType.ClientDevice:
                    st = "ClientDevice";
                    break;
                default:
                    st = "Không xác định";
                    break;
            }
        }
        public static EClientType S2EClientType(string key)
        {
            switch (key)
            {
                case nameof(EClientType.None):
                    return EClientType.None;
                case nameof(EClientType.Admin):
                    return EClientType.Admin;
                case nameof(EClientType.Replication):
                    return EClientType.Replication;
                case nameof(EClientType.Manager):
                    return EClientType.Manager;
                case nameof(EClientType.Supervisor):
                    return EClientType.Supervisor;
                case nameof(EClientType.ClientNormal):
                    return EClientType.ClientNormal;
                case nameof(EClientType.ClientDevice):
                    return EClientType.ClientDevice;
                default:
                    return EClientType.None;
            }
        }
        public static EClientType I2EClientType(int key)
        {
            switch (key)
            {
                case (int)(EClientType.None):
                    return EClientType.None;
                case (int)(EClientType.Admin):
                    return EClientType.Admin;
                case (int)(EClientType.Replication):
                    return EClientType.Replication;
                case (int)(EClientType.Manager):
                    return EClientType.Manager;
                case (int)(EClientType.Supervisor):
                    return EClientType.Supervisor;
                case (int)(EClientType.ClientNormal):
                    return EClientType.ClientNormal;
                case (int)(EClientType.ClientDevice):
                    return EClientType.ClientDevice;
                default:
                    return EClientType.None;
            }
        }

        //**-------------------------------------------------------------------------------
        static decimal logID = 0;
        public static string CreateNewID(int length)
        {
            //DateTime.Now.ToString("yyyyMMddhhmmss")
            long longNumber = DateTime.Now.ToFileTime();
            longNumber = longNumber * (new Random(DateTime.Now.Millisecond)).Next(2, 9999);
            decimal result = decimal.Parse(Math.Abs(longNumber).ToString("00000000000000000000"));

            if (result == logID)
                return CreateNewID(length);
            else
            {
                logID = result;
                return ("" + result).Substring(0, length);
            }
        }
        public static int ICreateNewID(int length)
        {
            //DateTime.Now.ToString("yyyyMMddhhmmss")
            long longNumber = DateTime.Now.ToFileTime();
            longNumber = longNumber * (new Random(DateTime.Now.Millisecond)).Next(2, 9999);
            decimal result = decimal.Parse(Math.Abs(longNumber).ToString("00000000000000000000"));

            return int.Parse(("" + result).Substring(0, length));


        }

        public static bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }
            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
        public static bool PingIP(string ip)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send(ip);
                    if (reply != null)
                    {
                        return reply.Status == IPStatus.Success;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool CheckHostIsAvailable(string hostUri, int portNumber)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(hostUri, portNumber, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
                    return success;
                }
            }
            catch 
            {
                return false;
            }
        }
    }
}

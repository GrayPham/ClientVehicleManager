using System;
using System.Security.Cryptography;
using System.Text;

namespace Connect.Common.Helper
{
    public class HexHelper
    {
        public static string CalculateMd5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static String ToHexString(byte value)
        {
            return (Convert.ToString(value / 16, 16).ToUpper() + Convert.ToString(value % 16, 16).ToUpper());
        }

        public static String ToHexString(UInt16 value)
        {
            var barrValue = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                return (ToHexString(barrValue[1]) + ToHexString(barrValue[0]));
            }
            return (ToHexString(barrValue[0]) + ToHexString(barrValue[1]));
        }

        public static String ToHexString(int value)
        {
            var barrValue = BitConverter.GetBytes(value);
            return (ToHexString(barrValue[0]) + ToHexString(barrValue[1]) + ToHexString(barrValue[2]) + ToHexString(barrValue[3]));
        }

        public static String ToHexString(UInt32 value)
        {
            var barrValue = BitConverter.GetBytes(value);
            return (ToHexString(barrValue[0]) + ToHexString(barrValue[1]) + ToHexString(barrValue[2]) + ToHexString(barrValue[3]));
        }

        public static String ToHexString(Byte[] value)
        {
            if (value == null) return null;
            //* Address
            var sb = new StringBuilder();
            foreach (var v in value)
            {
                sb.Append(ToHexString(v));
            }
            return sb.ToString();
        }

        public static bool IsEqual(Byte[] s1, Byte[] s2)
        {
            if (s1 == null && s2 == null) return true;
            if (s1 == null) return false;
            if (s2 == null) return false;
            if (s1.Length != s2.Length) return false;
            for (int i = 0; i < s1.Length; i++) if (s1[i] != s2[i]) return false;
            return true;
        }

        public static bool IsHexChar(char c)
        {
            if (Char.IsNumber(c)) return true;
            var upper = Char.ToUpper(c);
            if (upper >= 'A' && upper <= 'F') return true;
            return false;
        }

        public static bool IsHexString(String st)
        {
            foreach (var c in st)
            {
                if (!IsHexChar(c)) return false;
            }
            return true;
        }

        public static Byte ToByte(Char c)
        {
            if (c >= '0' && c <= '9') return (Byte)(c - '0');
            var upper = Char.ToUpper(c);
            if (upper >= 'A' && upper <= 'F') return (Byte)(upper - 'A' + 10);
            throw new ArgumentOutOfRangeException();
        }

        public static Byte ToByte(String st)
        {
            if (st.Length > 2) throw new ArgumentOutOfRangeException();
            Byte ret = 0;
            for (int i = 0; i < st.Length; i++)
            {
                ret *= 16;
                ret += ToByte(st[i]);
            }
            return ret;
        }

        public static Byte[] ToBytes(String st)
        {
            var builder = new StringBuilder(st);
            if (st.Length % 2 != 0) builder.Insert(0, '0');
            var ret = new byte[builder.Length / 2];
            for (int i = 0; i < st.Length; i++)
            {
                ret[i / 2] += ToByte(st[i]);
                if (i % 2 == 0)
                {
                    ret[i / 2] *= 16;
                }
            }
            return ret;
        }

        public static UInt16 ToUInt16(String st)
        {
            if (st.Length > 4) throw new ArgumentOutOfRangeException();
            UInt16 ret = 0;
            for (int i = 0; i < st.Length; i++)
            {
                ret *= 16;
                ret += ToByte(st[i]);
            }
            return ret;
        }

        public static int InsertToBytes(byte[] array, int start, int val)
        {
            var bytes = BitConverter.GetBytes(val);
            for (int i = 0; i < bytes.Length; i++) array[start + i] = bytes[i];
            return start + 4;
        }

        public static int InsertToBytes(byte[] array, int start, UInt32 val)
        {
            var bytes = BitConverter.GetBytes(val);
            for (int i = 0; i < bytes.Length; i++) array[start + i] = bytes[i];
            return start + 4;
        }

        public static int InsertToBytes(byte[] array, int start, UInt16 val)
        {
            var bytes = BitConverter.GetBytes(val);
            for (int i = 0; i < bytes.Length; i++) array[start + i] = bytes[i];
            return start + 2;
        }

        public static int InsertToBytes(byte[] array, int start, byte[] val)
        {
            for (int i = 0; i < val.Length; i++) array[start + i] = val[i];
            return start + val.Length;
        }
    }
}

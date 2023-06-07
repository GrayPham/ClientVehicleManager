using Connect.Common.Interface;
using Newtonsoft.Json;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.NetworkInformation;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.Reflection;
using System.Linq;

namespace Connect.RemoteDataProvider.Common
{
    public class SerializationHelper
    {
        public static void RegisterType<T>(ILog log)
        {
            var type = typeof(T);
            // log.Info(type.ToString());
            var protobufObject = RuntimeTypeModel.Default.Add(type, false);
            RuntimeTypeModel.Default[type].EnumPassthru = true;
            var props = type.GetProperties();
            var i = 1;
            foreach (var prop in props)
            {
                //    log.Info(prop.Name);
                protobufObject.Add(i++, prop.Name);
            }
        }

        public static void Compile(ILog log)
        {
            // RuntimeTypeModel.Default.Compile();
        }

        static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// Compresses byte array to new byte array.
        /// </summary>
        public static byte[] Compress(byte[] raw)
        {
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory,
                CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        public static byte[] Decompress(byte[] gzip)
        {

            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (var stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        try
                        {
                            count = stream.Read(buffer, 0, size);
                            if (count > 0)
                            {
                                memory.Write(buffer, 0, count);
                            }
                        }
                        catch (Exception)
                        {
                            count = 0;
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public static byte[] Serialize<T>(T obj)
        {
            try
            {
                var st = JsonConvert.SerializeObject(obj);
                return Compress(GetBytes(st));
            }
            catch (Exception)
            {
                return null;
            }

            /*
            var ms = new MemoryStream();
            using (var writer = new BsonWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, obj);
            }
            return ms.ToArray();

            var st = JsonConvert.SerializeObject(obj);
            return GetBytes(st);
            byte[] b = null;
            using (var ms = new MemoryStream(0))
            {
                Serializer.Serialize<T>(ms, obj);
                b = new byte[ms.Position];
                Array.Copy(ms.GetBuffer(), b, b.Length);
            }
            return b;
            */
        }

        public static T Deserialize<T>(byte[] data)
        {
            try
            {
                var decopmress = Decompress(data);
                var st = GetString(decopmress);
                return JsonConvert.DeserializeObject<T>(st);
            }
            catch (Exception)
            {
                return default(T);
            }
            /*
            var ms = new MemoryStream(data);
            using (var reader = new BsonReader(ms))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(reader);
            }

            var st = GetString(data);
            return JsonConvert.DeserializeObject<T>(st);
            // if (data == null) return;
            using (ms = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(ms);
            }
             */
        }
        public static T ODeserialize<T>(object data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>("" + data);
            }
            catch (Exception)
            {
                return default(T);
            }

            /*
            var ms = new MemoryStream(data);
            using (var reader = new BsonReader(ms))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(reader);
            }

            var st = GetString(data);
            return JsonConvert.DeserializeObject<T>(st);
            // if (data == null) return;
            using (ms = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(ms);
            }
             */
        }
        /// <summary>
        /// Finds the MAC address of the first operation NIC found.
        /// </summary>
        /// <returns>The MAC address.</returns>
        public static string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }

        /// <summary>
        /// Encrypts a given password and returns the encrypted data
        /// as a base64 string.
        /// </summary>
        /// <param name="plainText">An unencrypted string that needs
        /// to be secured.</param>
        /// <returns>A base64 encoded string that represents the encrypted
        /// binary data.
        /// </returns>
        /// <remarks>This solution is not really secure as we are
        /// keeping strings in memory. If runtime protection is essential,
        /// <see cref="SecureString"/> should be used.</remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="plainText"/>
        /// is a null reference.</exception>
        public string Encrypt(string plainText)
        {
            if (plainText == null) throw new ArgumentNullException("plainText");

            //encrypt data
            var data = Encoding.Unicode.GetBytes(plainText);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.LocalMachine);

            //return as base64 string
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Decrypts a given string.
        /// </summary>
        /// <param name="cipher">A base64 encoded string that was created
        /// through the <see cref="Encrypt(string)"/> or
        /// <see cref="Encrypt(string)"/> extension methods.</param>
        /// <returns>The decrypted string.</returns>
        /// <remarks>Keep in mind that the decrypted string remains in memory
        /// and makes your application vulnerable per se. If runtime protection
        /// is essential, <see cref="SecureString"/> should be used.</remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="cipher"/>
        /// is a null reference.</exception>
        public string Decrypt(string cipher)
        {
            if (cipher == null) throw new ArgumentNullException("cipher");

            //parse base64 string
            byte[] data = Convert.FromBase64String(cipher);

            //decrypt data
            byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.LocalMachine);
            return Encoding.Unicode.GetString(decrypted);
        }

        public static string EncryptText(string openText, string cKey, string cIv)
        {
            var rc2Csp = new RC2CryptoServiceProvider();
            var encryptor = rc2Csp.CreateEncryptor(Convert.FromBase64String(cKey),
                Convert.FromBase64String(cIv));
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    byte[] toEncrypt = Encoding.Unicode.GetBytes(openText);

                    csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                    csEncrypt.FlushFinalBlock();

                    byte[] encrypted = msEncrypt.ToArray();

                    return Convert.ToBase64String(encrypted);
                }
            }
        }
        public static string DecryptText(string encryptedText, string cKey, string cIv)
        {
            var rc2Csp = new RC2CryptoServiceProvider();
            var decryptor = rc2Csp.CreateDecryptor(Convert.FromBase64String(cKey),
                Convert.FromBase64String(cIv));
            using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    var bytes = new List<byte>();
                    int b;
                    do
                    {
                        b = csDecrypt.ReadByte();
                        if (b != -1)
                        {
                            bytes.Add(Convert.ToByte(b));
                        }
                    } while (b != -1);

                    return Encoding.Unicode.GetString(bytes.ToArray());
                }
            }
        }
        public static string ListInfoToJson(object value)
        {
            try
            {
                if (value == null) return "[]";
                return Newtonsoft.Json.JsonConvert.SerializeObject(value);
            }
            catch (Exception)
            {
                return "[]";
            }
      
        }
        public static List<T> JsonToListInfo<T>(string value)
        {
            try
            {
                return (Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(value ?? (value = ""))) ?? (new List<T>());
            }
            catch 
            {
                return new List<T>();
            }

        }
        public static DataTable JsonToDataTable(string value)
        {
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(value, (typeof(DataTable)));
            return dt;
        }
        public static List<T> JsonToDataTable2Info<T>(string value)
        {
            try
            {
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(value, (typeof(DataTable)));
                if (dt != null)
                {
                    return MappingToListInfo<T>(dt);
                }
                else
                {
                    return new List<T>();
                }
            }
            catch (Exception)
            {
                return new List<T>();
            }

        }
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        public static string InfoToJson(object value)
        {

            if (value == null) return "";
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
        public static T JsonToInfo<T>(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value ?? (value = ""));
        }
        public static List<T> MappingToListInfo<T>(DataTable dt)
        {
            try
            {
                var lst = new List<T>();
                var tClass = typeof(T);
                PropertyInfo[] proInModel = tClass.GetProperties();
                List<DataColumn> proInDataColumns = dt.Columns.Cast<DataColumn>().ToList();
                T cn;
                foreach (DataRow item in dt.Rows)
                {
                    cn = (T)Activator.CreateInstance(tClass);
                    foreach (var pc in proInModel)
                    {
                        try
                        {
                            var d = proInDataColumns.Find(c => string.Equals(c.ColumnName.ToLower().Trim(), pc.Name.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase));
                            if (d != null)
                            {

                                Type type = pc.PropertyType.GetTypeInfo();
                                if (type.Name == "Nullable`1")
                                {
                                    type = type.GetGenericArguments()[0];
                                }
                                var value = item[pc.Name] == DBNull.Value ? null : Convert.ChangeType(item[pc.Name], type);
                                pc.SetValue(cn, item[pc.Name] == DBNull.Value ? null : value, null);
                            }
                        }
                        catch (Exception)
                        {

                        }

                    }
                    lst.Add(cn);
                }
                return lst;
            }
            catch (Exception e)
            {
                throw e;
                // return new List<T>();
            }
        }
        public static T MappingToListInfo<T>(DataRow row)
        {
            var tClass = typeof(T);
            T cn = cn = (T)Activator.CreateInstance(tClass);
            try
            {

                if (row == null) return cn;
                var lst = new List<T>();
                PropertyInfo[] proInModel = tClass.GetProperties();
                List<DataColumn> proInDataColumns = row.Table.Columns.Cast<DataColumn>().ToList();


                foreach (var pc in proInModel)
                {
                    var d = proInDataColumns.Find(c => string.Equals(c.ColumnName.ToLower().Trim(), pc.Name.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase));
                    if (d != null)
                        pc.SetValue(cn, row[pc.Name], null);

                }
                return cn;
            }
            catch 
            {
            }
            return cn;
        }
    }
}

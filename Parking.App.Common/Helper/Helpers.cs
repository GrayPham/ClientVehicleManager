using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Color = System.Drawing.Color;
using Brush = System.Drawing.Brush;
using Newtonsoft.Json;
using System.Collections;
using System.Media;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using Parking.App.Common.Helper;

namespace Parking.App.Common.Helper
{
    public class Helpers
    {
        public static string fullPathMainForm = GetFullPathOfMainForm();
        public static CultureInfo cultureInfo = new CultureInfo("en-US") { NumberFormat = { CurrencySymbol = "₫ " } };

        public static bool IsChangeAd = false;
        private static SoundPlayer player = new SoundPlayer();
        public static string DateToString_ddMMyyyy(byte? day, byte? month, int? year)
        {
            string dateFull = "";
            if (day != null && day.Value > 0) dateFull = day.Value.ToString("00");
            if (month != null && month.Value > 0) dateFull += @"/" + month.Value.ToString("00");
            if (year != null && year.Value > 0) dateFull += @"/" + year.Value.ToString("0000");
            dateFull.Replace(@"//", @"/");
            if (dateFull.Length > 0)
            {
                if (dateFull.Substring(0, 1) == @"/") dateFull = dateFull.Substring(1, dateFull.Length - 2);
                if (dateFull.Substring(0, dateFull.Length - 1) == @"/") dateFull = dateFull.Substring(0, dateFull.Length - 2);
            }

            return dateFull;
        }
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
        public static string Double2SVND(double value)
        {
            try
            {
                return value.ToString("#,##0", cultureInfo);
            }
            catch (Exception)
            {
                return "";
            }

        }
        public static string Dou2SVND(double value)
        {
            try
            {
                return value.ToString("#,##0", cultureInfo) + " " + cultureInfo.NumberFormat.CurrencySymbol;
            }
            catch (Exception)
            {
                return "";
            }
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
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
            catch (Exception)
            {
                return null;
            }


        }

        public static byte[] ImageToBinary(string imagePath)
        {
            FileStream fS = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] b = new byte[fS.Length];
            fS.Read(b, 0, (int)fS.Length);
            fS.Close();
            return b;
        }

        public static DateTime? FormatTo_ddMMyyyy(DateTime? date)
        {
            if (date == null) return null;
            string str = date.Value.ToString("dd/MM/yyyy hh:mm:ss");
            return DateTime.Parse(str);
        }
        public static DateTime S2Dtime(string value, string format = "dd/MM/yyyy")
        {
            if (value == null) return DateTime.MinValue;
            try
            {
                return DateTime.ParseExact(value, format, cultureInfo);
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }
        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static void SetDefaultValue<T>(T info) where T : new()
        {
            //Read Attribute Names and Types
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var objFieldNames = info.GetType().GetProperties(flags).Cast<PropertyInfo>().
                Select(item => new
                {
                    Name = item.Name,
                    Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType,
                    value = item.GetValue(info, null)
                }).ToList();

            foreach (var item in objFieldNames)
            {
                PropertyInfo propertyInfos = info.GetType().GetProperty(item.Name);
                if (propertyInfos.PropertyType == typeof(Nullable<DateTime>))
                {
                    propertyInfos.SetValue(info, ConvertDateTime(item.value), null);
                }
                if (propertyInfos.PropertyType == typeof(Nullable<int>))
                {
                    propertyInfos.SetValue(info, ConvertInt(item.value), null);
                }
            }

        }

        private static DateTime? ConvertDateTime(object value)
        {
            DateTime? _result = null;
            if (value != null)
            {
                if (((DateTime?)value != DateTime.MinValue) && ((DateTime?)value != DateTime.MaxValue)) _result = (DateTime?)value;

            }

            return _result;
        }

        private static int? ConvertInt(object value)
        {

            int? _result = null;
            if (value != null) if ((int)value > 0) _result = (int)value;
            return _result;
        }

        public static void Copy<T>(T source, T infoOut) where T : new()
        {
            //Read Attribute Names and Types
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var objFieldNames = infoOut.GetType().GetProperties(flags).Cast<PropertyInfo>().
                Select(item => new
                {
                    Name = item.Name,
                    Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType,
                    value = item.GetValue(infoOut, null)
                }).ToList();

            foreach (var item in objFieldNames)
            {
                PropertyInfo propertyInfos = source.GetType().GetProperty(item.Name);
                propertyInfos.SetValue(source, item.value, null);
            }

        }
        public static IList<string> ConvertStringToList(string pstring)
        {
            IList<string> _result = new List<string>(32);
            if (pstring != null)
            {
                try
                {
                    _result = pstring.Split(',');
                }
                catch (Exception)
                {
                    return _result;
                }

            }
            return _result;
        }
        //public static void ConvertMonthPeriod(MonthPeriod period, out String st)
        //{
        //    switch (period)
        //    {
        //        case MonthPeriod.FirstHalf:
        //            st = LAttendance.MonthPeriodFirstHalf; return;
        //        case MonthPeriod.SecondHalf:
        //            st = LAttendance.MonthPeriodSecondHalf; return;
        //        default:
        //            st = LAttendance.MonthPeriodFull; return;
        //    }
        //}
        public DateTime GetDateTimeByFormat(string value)
        {
            DateTime _result = DateTime.MinValue;
            try
            {
                _result = DateTime.ParseExact(value, Formats.FDate, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                _result = DateTime.MinValue;
            }
            return _result;
        }
        public static string ConvertStringVN(string s)
        {
            if (s == null) return "";
            if (s.Length == 0) return "";
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToUpper().Trim();
        }
        public static bool DoubleEmptyOrZero(double? value)
        {
            try
            {
                if (value > 0) return false;
                return true;
            }
            catch (Exception)
            {
                return true;
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
        public static double DToDouble(decimal value)
        {
            if (value == null) return 0;
            else
                return (double)value;
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
            return "[]";
        }
        public static List<T> JsonToListInfo<T>(string value)
        {
            try
            {
                return (Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(value ?? (value = ""))) ?? (new List<T>());
            }
            catch (Exception)
            {
                return new List<T>();
            }

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
        public static decimal SToDecimal(string value)
        {
            if (value == null) return 0;
            else
                return decimal.Parse(value);
        }
        public static double SToDouble(string value)
        {
            if (value == null) return 0;
            else
                return double.Parse(value);
        }


        //public double CaculatorTaxableIncome(double salary)
        //{
        //    double _result = 0;
        //switch (switch_on)
        //{
        //    default:
        //}
        //}

        //public static void ConverPaymentModel(PaymentModel type, out String st)
        //{
        //    switch (type)
        //    {
        //        case PaymentModel.TransferPercent:
        //            st = LPayroll.LPaymentModel_TransferPercent;
        //            break;
        //        case PaymentModel.FixedTransfer:
        //            st = LPayroll.LPaymentModel_FixedTransfer;
        //            break;
        //        case PaymentModel.CashPercent:
        //            st = LPayroll.LPaymentModel_CashPercent;
        //            break;
        //        case PaymentModel.FixedCash:
        //            st = LPayroll.LPaymentModel_FixedCash;
        //            break;
        //        default:
        //            st = LPayroll.LPaymentModel_TransferPercent;
        //            break;
        //    }
        //}

        public static Image RoundCorners(Image StartImage, int CornerRadius, Color BackgroundColor)
        {
            CornerRadius *= 2;
            Bitmap RoundedImage = new Bitmap(StartImage.Width, StartImage.Height);

            using (Graphics g = Graphics.FromImage(RoundedImage))
            {
                g.Clear(BackgroundColor);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                using (Brush brush = new TextureBrush(StartImage))
                {
                    using (GraphicsPath gp = new GraphicsPath())
                    {
                        gp.AddArc(-1, -1, CornerRadius, CornerRadius, 180, 90);
                        gp.AddArc(0 + RoundedImage.Width - CornerRadius, -1, CornerRadius, CornerRadius, 270, 90);
                        gp.AddArc(0 + RoundedImage.Width - CornerRadius, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
                        gp.AddArc(-1, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);

                        g.FillPath(brush, gp);
                    }
                }

                return RoundedImage;
            }
        }

        //public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {

        //        Image imageTemp = ResizeToDefault(image);
        //        // Convert Image to byte[]
        //        imageTemp.Save(ms, format);
        //        byte[] imageBytes = ms.ToArray();

        //        // Convert byte[] to Base64 String
        //        string base64String = Convert.ToBase64String(imageBytes);
        //        ms.Close();
        //        return base64String;
        //    }
        //}

        public static string ImageToBase64_2(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {

                    Image imageTemp = ResizeToDefault2(image);
                    // Convert Image to byte[]
                    imageTemp.Save(ms, format);
                    byte[] imageBytes = ms.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    ms.Close();
                    return base64String;
                }
            }
            catch (Exception ex)
            {
                return null;
                Debug.WriteLine(ex);
            }
        }
        public static string ImageToBase64(string path)
        {
            try
            {
                using (Image image = Image.FromFile(path))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Helper ImageToBase64 Expcetion" + ex.Message);
                return "";
            }

        }
        public static string ImageToBase64(string path, int width, int height)
        {
            try
            {
                using (Image image = Image.FromFile(path))
                {
                    var bitmap = ResizeImage(image, width, height);
                    using (MemoryStream m = new MemoryStream())
                    {
                        bitmap.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static Image Base64ToImage(string base64String)
        {
            if (base64String == null) return null;
            if (base64String.Length == 0) return null;
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            ms.Close();
            return image;
        }

        public static Image ResizeToDefault(Image image)
        {
            if (image != null && image.Width > 90)
            {
                var width = 700;
                var heigth = 370;
                return ResizeImage(image, width, heigth);
            }
            return image;
        }

        public static Image ResizeToDefault2(Image image)
        {
            try
            {
                if (image != null && image.Width > 90)
                {
                    var width = 320;
                    var heigth = 400;
                    return ResizeImage(image, width, heigth);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return image;
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            try
            {
                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
            catch (Exception ex)
            {
                return null;
                Debug.WriteLine(ex.Message);
            }
        }
        public static bool IsUnicode(string input)
        {
            var asciiBytesCount = Encoding.ASCII.GetByteCount(input);
            var unicodBytesCount = Encoding.UTF8.GetByteCount(input);
            return asciiBytesCount != unicodBytesCount;
        }

        public static string UpperFirstDigit(string value)
        {
            var arr = value.ToCharArray();
            arr[0] = Char.ToUpperInvariant(arr[0]);
            return new String(arr);
        }



        public static void CropImage(System.Drawing.Image image, bool isDisplayPhotoBig = false)
        {
            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Bitmap flippedImage = new Bitmap(image);

            Rectangle rec = new Rectangle(130, 164, 580, 385);
            Bitmap bmpImage = new Bitmap(flippedImage);

            if (isDisplayPhotoBig == false)
            {
                using (var kk = bmpImage.Clone(rec, image.PixelFormat))
                {
                    kk.Save(GetFullPathOfMainForm() + @"IdCardImage\CardImageBig.JPG");
                }
            }
            else
            {
                using (var kk = bmpImage.Clone(rec, image.PixelFormat))
                {
                    kk.Save(GetFullPathOfMainForm() + @"IdCardImage\DisplayPhotoBig.JPG");
                }
            }
        }

        public static List<T> ConvertObjectToListModel<T>(object Data)
        {
            List<T> t = new List<T>();

            try
            {
                IEnumerable enumerable = Data as IEnumerable;
                if (enumerable != null)
                {
                    foreach (object element in enumerable)
                    {
                        if (element != null)
                        {
                            string x = JsonConvert.SerializeObject(element);
                            T deviceData = JsonConvert.DeserializeObject<T>(x);
                            t.Add(deviceData);

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            return t;
        }

        //covert hex of sql to string


        public static string HashCodePassword(string pass)
        {
            try
            {
                var bytes = new UTF8Encoding().GetBytes(pass);
                var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                return null;
            }
        }


        public static string removeCharacter(string text)
        {
            try
            {
                if (!String.IsNullOrEmpty(text))
                    text = text.Remove(text.Length - 1);
                return text;
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                return text;
            }
        }




        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void HideWindow()
        {
            // Find the window associated with the process
            IntPtr hWnd = FindWindow(null, "Notepad");

            if (hWnd != IntPtr.Zero)
            {
                // Hide the window
                ShowWindow(hWnd, 0); // SW_HIDE = 0
            }
        }


        public static string GetFullPathOfMainForm()
        {
            //Assembly bundleAssembly = AppDomain.CurrentDomain.GetAssemblies()
            //      .First(x => x.FullName.Contains("Shell"));
            //string FullName = bundleAssembly.Location;
            //string FullName2 = FullName.Substring(0, FullName.IndexOf("bin"));
            //return FullName2;
            return System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath) + @"\";
        }

        public static int GetMinorsAndNotify(string dob)
        {
            try
            {

                var DateBorn = DateTime.ParseExact(dob, "yyMMdd", CultureInfo.InvariantCulture);
                var dateNow = DateTime.Now;
                var minors = (dateNow - DateBorn).TotalDays;
                return (int)minors / 365;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }

        }

        public static void StopSound()
        {
            try
            {
                player.Stop();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void PlaySound(string SoundName)
        {
            try
            {
                if (string.IsNullOrEmpty(SoundName))
                    return;

                var fullPathMainForm = GetFullPathOfMainForm();
                player.SoundLocation = (fullPathMainForm + SoundName);
                player.Load();
                player.Play();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Helper Play Sound Exception" + ex.Message);
            }
        }

        public static string CutTheTextToTwoLine(string originalString)
        {

            if (originalString.Length > 14)
            {
                originalString = string.Concat(originalString.Substring(0, 14), Environment.NewLine, originalString.Substring(14));
            }
            return originalString;
        }

        public static string RemoveCharacterInNameUser(string name)
        {
            int index = name.IndexOf("(");

            if (index != -1)
            {
                name = name.Substring(0, index);
            }

            return name;

        }


        public static string GetPhysicalAddress()
        {
            try
            {
                String firstMacAddress = NetworkInterface
        .GetAllNetworkInterfaces()
        .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
        .Select(nic => nic.GetPhysicalAddress().ToString())
        .FirstOrDefault();
                return firstMacAddress;
            }
            catch (Exception ex)
            {
                return null;
                Debug.WriteLine("Mac Address Exception" + ex.Message);
            }


        }


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);
        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);

        private static Process _process;
        private readonly static object _locker = new object();

        private static Process KeyBoardProcess
        {
            get
            {
                lock (_locker)
                {
                    if (_process == null)
                    {
                        _process = new Process();
                        _process.StartInfo.FileName = @"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe";
                        _process.StartInfo.UseShellExecute = true;

                    }
                    return _process;
                }
            }
        }
        public static void OpenKeyBoard()
        {
            try
            {
                KeyBoardProcess.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static string GetPublicIp()
        {
            try
            {
                string ipAddress = "";
                using (WebClient client = new WebClient())
                {
                    ipAddress = client.DownloadString("http://icanhazip.com/");
                }
                return ipAddress;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public static void closeKeyboard()
        {
            try
            {
                //  KillKeyBoardProcess();
                uint WM_SYSCOMMAND = 274;
                uint SC_CLOSE = 61536;
                IntPtr KeyboardWnd = FindWindow("IPTip_Main_Window", null);
                PostMessage(KeyboardWnd.ToInt32(), WM_SYSCOMMAND, (int)SC_CLOSE, 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void SendToTelegram(string message)
        {
            try
            {
                string urlString = $"https://api.telegram.org/bot{Constants.Constants.TokenTelegram}/sendMessage?chat_id={Constants.Constants.IdUserTelegram}&text={message}";
                WebClient webclient = new WebClient();
                webclient.DownloadStringAsync(new Uri(urlString));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static string FindNameDictionaryByKey(string key)
        {
            try
            {
                var dict = SoundRegulations.ListSoundType;
                string value = "";
                if (dict.TryGetValue(key, out value))
                {
                    return value;
                }
                return value;
            }
            catch (Exception ex)
            {
                return "";
                Debug.WriteLine("Cannot convert");
            }

        }




    }
}

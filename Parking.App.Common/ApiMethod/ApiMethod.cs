using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Connect.Common.Contract;
using RestSharp;
using Connect.Common.Helper;
using System.IO;
using Parking.App.Common.ViewModels;
namespace Parking.App.Common.ApiMethod
{
    public class ApiMethod
    {
        public async static Task<HttpResponseMessage> PostCall<T>(T model) where T : class
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = Constants.Constants.ApiServerURL;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(model);
                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, stringContent);
                    // response.Wait();
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }
        public async static Task<HttpResponseMessage> PostCallQR<T>(T model) where T : class
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = Constants.Constants.ApiQrURL;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(model);
                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, stringContent);
                    // response.Wait();
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }
        public async static Task<HttpResponseMessage> PostCallOcr<T>(T model) where T : class
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = Constants.Constants.ApiOcrURL;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-OCR-SECRET", Constants.Constants.OcrSecretCode);


                    var json = JsonConvert.SerializeObject(model);
                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, stringContent);
                    // response.Wait();
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }
        public async static Task<HttpResponseMessage> PostCallSound(int soundNo) 
        {
            try
            {
                var url = Constants.Constants.PostCallUri;
                var url2 = $"{url}={soundNo}";

                using (var httpClient = new HttpClient())
                {
                    // Call the API and get the response
                    var response = await httpClient.GetAsync(url2);

                    // Get the content of the response as a string
                 //   string content = await response.Content.ReadAsStringAsync();

                    // Print the content to the console
                    return response;
                }
            }
            catch
            {
                throw;
                
            }
        }
        public static ResultInfo DownFileAdmgt(object key,string filePath)
        {
            var result = new ResultInfo();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var test = string.Format(Constants.Constants.PostCallUriAdmgt, key);
                var options = new RestClientOptions(test)
                {
                    ThrowOnAnyError = true,
                    Timeout = Constants.Constants.RequestTimeOut
                };

                var client = new RestClient(options);
                var request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                request.Timeout = Constants.Constants.RequestTimeOut;
                var fileBytes = client.DownloadDataAsync(request).Result;
                File.WriteAllBytes(filePath, fileBytes);
                result.Status = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.ErrorMessage = ex.Message;
                return result;
            }
        }
        public static Task<ResultInfo> DownFileAdmgtAsync(object key, string filePath)
        {
            return Task.Run<ResultInfo>(() => {
                return DownFileAdmgt(key, filePath);
            });
        }
        public static ResultInfo RequestPost(UInt32 signature, UInt16 functionCode, RequestInfo req)
        {
            var result = new ResultInfo();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                DataRequest dataSend = new DataRequest()
                {
                    Signature = signature,
                    FrameID = 0,
                    DataLength = 0,
                    FunctionCode = functionCode,
                    Data = req
                };
                var options = new RestClientOptions(Constants.Constants.ApiServerURL)
                {
                    ThrowOnAnyError = true,
                    Timeout = Constants.Constants.RequestTimeOut
                };

                var client = new RestClient(options);
                var request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                request.Timeout = Constants.Constants.RequestTimeOut;
                var json = JsonConvert.SerializeObject(dataSend);
                request.AddJsonBody(dataSend);
                var resp = client.ExecutePostAsync(request).Result;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var infoRep = JsonHelper.JsonToInfo<ResultInfo>(resp.Content);
                    if (infoRep != null) return infoRep;
                }
                else
                {
                    result.Status = false;
                    result.ErrorMessage = resp.Content;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.ErrorMessage = ex.Message;
                return result;
            }

            return result;
        }
        public static Task<ResultInfo> RequestPostAsync(UInt32 signature, UInt16 functionCode, RequestInfo req)
        {
            return Task.Run<ResultInfo>(() => {
                return RequestPost(signature, functionCode, req);
            });
        }

        //**---------------------------------------------------------

        public static ResultInfo CheckNewVersion(string versionCode,string type)
        {
            var result = new ResultInfo();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var options = new RestClientOptions(string.Format(Constants.Constants.ApiVersionURL, type, versionCode))
                {
                    ThrowOnAnyError = true,
                    Timeout = Constants.Constants.RequestTimeOut
                };

                var client = new RestClient(options);
                var request = new RestRequest();
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                request.Timeout = Constants.Constants.RequestTimeOut;
                

                var resp = client.ExecuteGetAsync(request).Result;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var infoRep = JsonHelper.JsonToInfo<ResultInfo>(resp.Content);
                    if (infoRep != null) return infoRep;
                }
                else
                {
                    result.Status = false;
                    result.ErrorMessage = resp.Content;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.ErrorMessage = ex.Message;
                return result;
            }

            return result;
        }
        public static Task<ResultInfo> CheckNewVersionAsync(string versionCode, string type)
        {
            return Task.Run<ResultInfo>(() => {
                return CheckNewVersion(versionCode, type);
            });
        }


        //**---------------------------------------------------------
    }
}

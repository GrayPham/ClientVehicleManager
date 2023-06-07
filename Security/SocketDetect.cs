using Newtonsoft.Json;
using Security.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yolov5Net.Scorer;

namespace Security
{
    public class SocketDetect
    {
        private ClientWebSocket webSocket1 = new ClientWebSocket();
        private string Id;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public SocketDetect()
        {

        }
        private byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
        public async Task<bool> OpenConnectAsync()
        {
            webSocket1 = new ClientWebSocket();
            try
            {
                // Connect FastAPI
                Uri uri = new Uri(Config.socketFastAPI);
                await webSocket1.ConnectAsync(uri, cancellationTokenSource.Token);
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }
        public async Task CloseWebSocket(ClientWebSocket webSocket)
        {
            if (webSocket.State != WebSocketState.Closed)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
            }
        }
        private async Task<bool> ReopenWebSocket()
        {
            try
            {
                //// Close the previous WebSocket connection (if any)
                //if (webSocket1 != null)
                //{
                //    await webSocket1.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                //    webSocket1.Dispose();
                //    webSocket1 = null;
                //}

                // Create a new WebSocket connection
                webSocket1 = new ClientWebSocket();
                await webSocket1.ConnectAsync(new Uri(Config.socketFastAPI), CancellationToken.None);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reopening WebSocket connection: {ex.Message}");
                return false;
            }
        }
        public async Task<string> request(Image imagebase, List<YoloPrediction> predictions)
        {
            var imageBaseBytes = ImageToByteArray(imagebase);

            var data = new
            {
                Image = imageBaseBytes,
                Predictions = predictions
            };
            // Serialize the JSON object to a string
            var json = JsonConvert.SerializeObject(data);
            var cancellationTokenSource = new CancellationTokenSource();
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
            try
            {
                // Reopen
                if(webSocket1.State == WebSocketState.Closed || webSocket1.State == WebSocketState.Aborted)
                {
                    bool check = await OpenConnectAsync();
                }
                //var message = JsonConvert.SerializeObject(dto);
                //var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                if (webSocket1.State == WebSocketState.Open)
                {
                    //// Gửi dữ liệu lên server
                    //var sendBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes("Hello, FastAPI!"));
                    await webSocket1.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationTokenSource.Token);

                    //// Nhận dữ liệu từ server
                    var bufferGet = new ArraySegment<byte>(new byte[1024]);

                    var receivedResult = await webSocket1.ReceiveAsync(bufferGet, cancellationTokenSource.Token);
                    //if (receivedResult.MessageType == WebSocketMessageType.Close)
                    //{
                    //    await webSocket1.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationTokenSource.Token);
                    //}
                    var message = Encoding.UTF8.GetString(bufferGet.Array, bufferGet.Offset, receivedResult.Count);

                    return message.ToString();


                }
            }

            catch (WebSocketException ex)
            {
                return "Exception:" + ex.Message;
            }
            // Trả về giá trị mặc định cho kiểu Task<string>
            return default;
        }
        public bool SocketStatus()
        {
            if (webSocket1.State == WebSocketState.Open || webSocket1.State == WebSocketState.Connecting)
            {
                return  true ;
            }
            return false;
            
        }

    }
}

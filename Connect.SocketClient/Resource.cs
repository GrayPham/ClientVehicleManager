using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.SocketClient
{
    public enum L2LCode
    {
        OpenSucceed = 0,
        AccessDenied,
        InvalidState,
        AlreadyOpen,
        InvalidParameter,
        UnknownError,
        BufferNull,
        PortIsNotOpen,
        WriteTimeOut,
        WriteInvalidParameter
    }

    public static class Resource
    {
        public static String PortOpenSucceed = @"Mở cổng thành công";
        public static String PortCloseSucceed = @"Mở cổng thành công";
        public static String PortWriteFailed = @"Lỗi ghi dữ liệu";
        public static String UnknownError = @"Lỗi không xác định";

        public static String WriteTimeOut = @"Thao tác ghi không thực hiện được (time-out)";
        public static String PortIsNotOpen = @"Cố gắng ghi vào một cổng chưa mở";
        public static String BufferNull = @"Bộ đệm chưa được khởi tạo";
        public static String AccessDenied = @"Cổng đã bị chiếm bởi ứng dụng khác hoặc bị khóa bởi quản trị";
        public static String InvalidState = @"Cổng không tồn tại trong hệ thống hoặc có lỗi thiết bị";
        public static String AlreadyOpen = @"Cố gắng mở một cổng đã được mở trước đó";
        public static String InvalidParameter = @"Thông số Parity, DataBits, Handshaking, Baudrate hoặc một thông số khác không chính xác";
        public static String WriteInvalidParameter = @"Offset + Count lớn hơn kích thước bộ đệm hoặc nhỏ hơn 0";

        public static String GetCodeText(L2LCode code)
        {
            switch (code)
            {
                case L2LCode.OpenSucceed: return PortOpenSucceed;
                case L2LCode.AccessDenied: return AccessDenied;
                case L2LCode.InvalidState: return InvalidState;
                case L2LCode.AlreadyOpen: return AlreadyOpen;
                case L2LCode.InvalidParameter: return InvalidParameter;
                case L2LCode.UnknownError: return UnknownError;
                case L2LCode.BufferNull: return BufferNull;
                case L2LCode.PortIsNotOpen: return PortIsNotOpen;
                case L2LCode.WriteTimeOut: return WriteTimeOut;
                case L2LCode.WriteInvalidParameter: return WriteInvalidParameter;
            }
            return "";
        }
    }
}

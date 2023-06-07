using Connect.Common.Helper;
using System;

namespace Connect.RemoteDataProvider.Protocol
{
    public class Frame
    {
        public static readonly byte[] StartBytes = { 0x01, 0xFF, 0xF0, 0xF0 };

        public UInt32 Signature { get; set; }
        public int FrameID { get; set; }
        public UInt16 FunctionCode { get; set; }
        public UInt32 DataLength { get; set; }
        public Byte[] Data { get; set; }

        public Byte[] GetFrame()
        {
            var output = new byte[DataLength + 18];
            var pos = 0;
            pos = HexHelper.InsertToBytes(output, pos, StartBytes);
            pos = HexHelper.InsertToBytes(output, pos, Signature);
            pos = HexHelper.InsertToBytes(output, pos, FrameID);
            pos = HexHelper.InsertToBytes(output, pos, FunctionCode);
            pos = HexHelper.InsertToBytes(output, pos, DataLength);
            HexHelper.InsertToBytes(output, pos, Data);
            return output;
        }

        public String ToHexString()
        {
            return HexHelper.ToHexString(Signature) + HexHelper.ToHexString(FrameID)
                   + HexHelper.ToHexString(FunctionCode) + HexHelper.ToHexString(DataLength) +
                   HexHelper.ToHexString(Data);
        }
    }
}

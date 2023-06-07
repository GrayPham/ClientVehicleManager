namespace Connect.Common.Contract
{
    public partial class UnknownUpdatedInfo
    {
        public int TypeCode { get; set; }
        public int RecordChanged { get; set; }
        public UnknownUpdatedInfo() { TypeCode = 0; RecordChanged = 0; }
    }
}

namespace Connect.Common.Contract
{
    public partial class ServiceErrorInfo
    {
        public int ErrorID { get; set; }
        public string ClassName { get; set; }
        public string Message { get; set; }
        public string ActionName { get; set; }
        public int Dedicated { get; set; }
        public bool Status { get; set; }
        public int IDNew { get; set; }
        public ServiceErrorInfo()
        {
            ErrorID = 0;
            ClassName = "";
            Message = "";
            ActionName = "";
            Dedicated = 0;
            Status = true;
            IDNew = 0;
        }
        public ServiceErrorInfo(string className, string message, string actionName)
        {
            ErrorID = 0;
            ClassName = className;
            Message = message;
            ActionName = actionName;
            Dedicated = 0;
            Status = false;
            IDNew = 0;
        }
    }
}

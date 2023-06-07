namespace Connect.Common.Contract
{
    public partial class ExceptionInfo
    {
        public int ErrorID { get; set; }
        public string ClassName { get; set; }
        public string Message { get; set; }
        public string ActionName { get; set; }
        public int Dedicated { get; set; }
        public ExceptionInfo()
        {
            ErrorID = 0;
            ClassName = "";
            Message = "";
            ActionName = "";
            Dedicated = 0;
        }
        public ExceptionInfo(string className, string message, string actionName)
        {
            ErrorID = 0;
            ClassName = className;
            Message = message;
            ActionName = actionName;
            Dedicated = 0;
        }

    }
}

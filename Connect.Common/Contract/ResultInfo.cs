using System;

namespace Connect.Common.Contract
{
    public partial class ResultInfo
    {
        public Guid KeyID { get; set; }
        public Guid RequestKeyID { get; set; }
        public object Info { get; set; }
        public string ErrorMessage { get; set; }
        public bool Status { get; set; }
        public string Code { get; set; }
        public int Record { get; set; }
        public object Data { get; set; }
        public int FrameNumber { get; set; }
        public UInt16 FunctionCode { get; set; }
        //---------------------------------------------------------
        public string ModelName { get; set; }
        public int ClientServiceID { get; set; }
        public Guid AppKey { get; set; }
        public string TSql { get; set; }
        //---------------------------------------------------------
        public ResultInfo()
        {
            ReSet();
        }
        public void ReSet()
        {
            KeyID = Guid.NewGuid();
            ErrorMessage = "";
            Status = false;
            Code = "";
            Info = null;
            Record = 0;
            Data = null;
            FrameNumber = 0;
            FunctionCode = 0;
            ModelName = "";
            ClientServiceID = 0;
            AppKey = Guid.Empty;
            TSql = "";
        }
        public void Set(ResultInfo info)
        {
            if(info == null)
            {
                ReSet();
            }
            KeyID = info.KeyID;
            RequestKeyID = info.RequestKeyID;
            ErrorMessage = info.ErrorMessage;
            Status = info.Status;
            Code = info.Code;
            Info = info.Info;
            Record = info.Record;
            Data = info.Data;
            FrameNumber = info.FrameNumber;
            FunctionCode = info.FunctionCode;
            ModelName = info.ModelName;
            ClientServiceID = info.ClientServiceID;
            AppKey = info.AppKey;
            TSql = info.TSql;
        }
    }
}

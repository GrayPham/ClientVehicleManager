using Connect.Common.Contract;
using Server.Contract.Session;
using System;
using System.Threading;

namespace nsConnect.RemoteDataProvider.Client
{
    public class DataRequestInfo
    {
        public int ClientID { get; set; }
        public ManualResetEvent SemaWaiting { get; set; }
        public RequestInfo Request { get; set; }
        public ResultInfo Result { get; set; }
        public DataRequestInfo()
        {
            ClientID = 0;
            Request = new RequestInfo();
            Result = new ResultInfo();
            SemaWaiting = new ManualResetEvent(false);
        }
        public DataRequestInfo(RequestInfo info)
        {
            ClientID = 0;
            Request = info;
            Result = new ResultInfo();
            SemaWaiting = new ManualResetEvent(false);
        }
        public DataRequestInfo(int clientID, RequestInfo info)
        {
            ClientID = clientID;
            Request = info;
            Result = new ResultInfo();
            SemaWaiting = new ManualResetEvent(false);
        }
        public DataRequestInfo(RequestInfo info, ResultInfo result)
        {
            ClientID = 0;
            Request = info;
            Result = result;
            SemaWaiting = new ManualResetEvent(false);
        }
        public DataRequestInfo(int clientID)
        {
            ClientID = clientID;
            Request = new RequestInfo();
            Result = new ResultInfo();
            SemaWaiting = new ManualResetEvent(false);
            Request.Owner = SessionDatas.GetOwner();
            Request.UserName = SessionDatas.GetLoginUser();
            Request.UserID = "" + SessionDatas.GetUserID();
            Request.BranchID = SessionDatas.GetBranchID();
            Request.BranchCode = SessionDatas.GetBranchCode();
        }
        public Guid GetKeyID()
        {
            return Request.KeyID;
        }
    }
}

using Connect.Common;
using Connect.Common.Contract;
using Parking.App.Contract.Setting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parking.App.Service.Handlers
{
    public interface ILoginService
    {
        Task<ResultInfo> RequestClientList();
        void RequestLogOut(SessionInfo info);
        void RequestLogin(SessionInfo info);
        void RequestReLogin(SessionInfo info);
        //**--------------------------------------------------------------------------
        ResultInfo ChangePassWord(AccountInfo account);
        ResultInfo ChangePassWordAsyn(AccountInfo info);
        //**--------------------------------------------------------------------------
        Task<ResultInfo> RequestClientClose(int ClientID);
        Task<ResultInfo> RequestClientCloseAsyn(int ClientID);
        //**--------------------------------------------------------------------------
        event ResponsedEventHandler<SessionInfo> ResponseLogin;
        event ResponsedEventHandler<SessionInfo> ResponseReLogin;
        event GeneralEventHandler<string> ResponseSessionCompleted;
        event GeneralEventHandler<ResultInfo> MessageReceived;
        //**--------------------------------------------------------------------------
        ResultInfo RequestSend2Client(int clientID, RequestInfo info);
        Task<ResultInfo> RequestSend2ClientAsyn(int clientID, RequestInfo info);
        //**--------------------------------------------------------------------------
    }
}

using Connect.Common.Contract;
using Parking.App.Contract.Common;
namespace Parking.App.Interface.Common
{
    public interface IProgramController
    {
        void Close();
        void LoginSuccess(SessionInfo info);
        void ConnectSuccess(ServerInfo info);
        void SetStatus(string description);
    }
}

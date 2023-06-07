using Connect.Common.Interface;
using Parking.Contract.Common;
using nsConnect.RemoteDataProvider.Client;

namespace Parking.App.Service.Common
{
    public class tblDeviceRequestConnectService : RemoteCacheDataService<tblDeviceRequestConnectInfo>
    {
        public tblDeviceRequestConnectService(ILog log, bool isDataSync, uint signature, string name) : base(log, isDataSync, signature, name)
        {
        }
        protected override void DefaultInsert(tblDeviceRequestConnectInfo info)
        {
            
        }

        protected override void DefaultModified(tblDeviceRequestConnectInfo info)
        {
            
        }
    }
}

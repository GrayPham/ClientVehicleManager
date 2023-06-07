using Connect.Common.Interface;
using Parking.App.Contract.Common;
using nsConnect.RemoteDataProvider.Client;
namespace Parking.App.Service.Common
{
    public class tblStoreDeviceService : RemoteCacheDataService<tblStoreDeviceInfo>
    {
        public tblStoreDeviceService(ILog log, bool isDataSync, uint signature, string name) : base(log, isDataSync, signature, name)
        {
        }

        protected override void DefaultInsert(tblStoreDeviceInfo info)
        {

        }

        protected override void DefaultModified(tblStoreDeviceInfo info)
        {

        }  
    }
}

using nsConnect.RemoteDataProvider.Client;
using Connect.Common.Interface;
using Parking.App.Contract.Common;

namespace Parking.App.Service.Common
{
    public class tblStoreEnvironmentSettingService : RemoteCacheDataService<tblStoreEnvironmentSettingInfo>
    {
        public tblStoreEnvironmentSettingService(ILog log, bool isDataSync, uint signature, string name) : base(log, isDataSync, signature, name)
        {
        }

        protected override void DefaultInsert(tblStoreEnvironmentSettingInfo info)
        {
            
        }

        protected override void DefaultModified(tblStoreEnvironmentSettingInfo info)
        {
            
        }
    }
}
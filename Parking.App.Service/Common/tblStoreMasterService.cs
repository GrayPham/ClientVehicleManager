using Connect.Common.Interface;
using Parking.App.Contract.Common;
using nsConnect.RemoteDataProvider.Client;

namespace Parking.App.Service.Common
{
    public class tblStoreMasterService : RemoteCacheDataService<tblStoreMasterInfo>
    {
        public tblStoreMasterService(ILog log, bool isDataSync, uint signature, string name) : base(log, isDataSync, signature, name)
        {
        }

        protected override void DefaultInsert(tblStoreMasterInfo info)
        {

        }

        protected override void DefaultModified(tblStoreMasterInfo info)
        {

        }
    }
}
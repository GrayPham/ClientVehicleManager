using Connect.Common.Interface;
using Parking.App.Contract.Common;
using nsConnect.RemoteDataProvider.Client;

namespace Parking.App.Service.Common
{
    public class tblUserMgtStoreService : RemoteCacheDataService<tblUserMgtStoreInfo>
    {
        public tblUserMgtStoreService(ILog log, bool isDataSync, uint signature, string name) : base(log, isDataSync, signature, name)
        {
        }

        protected override void DefaultInsert(tblUserMgtStoreInfo info)
        {

        }

        protected override void DefaultModified(tblUserMgtStoreInfo info)
        {

        }
    }
  
}

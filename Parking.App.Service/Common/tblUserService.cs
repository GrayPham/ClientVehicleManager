using Connect.Common.Interface;
using Parking.App.Contract.Common;
using nsConnect.RemoteDataProvider.Client;

namespace Parking.App.Service.Common
{
    public class tblUserService : RemoteCacheDataService<tblUserInfo>
    {
        public tblUserService(ILog log, bool isDataSync, uint signature, string name) : base(log, isDataSync, signature, name)
        {
        }

        protected override void DefaultInsert(tblUserInfo info)
        {

        }

        protected override void DefaultModified(tblUserInfo info)
        {
          
        }
    }
}

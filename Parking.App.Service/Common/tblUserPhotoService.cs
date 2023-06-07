using Connect.Common.Interface;
using Parking.App.Contract.Common;
using nsConnect.RemoteDataProvider.Client;
namespace Parking.App.Service.Common
{
    public class tblUserPhotoService : RemoteCacheDataService<tblUserPhotoInfo>
    {
        public tblUserPhotoService(ILog log, bool isDataSync, uint signature, string name) : base(log, isDataSync, signature, name)
        {
        }

        protected override void DefaultInsert(tblUserPhotoInfo info)
        {

        }

        protected override void DefaultModified(tblUserPhotoInfo info)
        {

        }
    }
}

using nsConnect.RemoteDataProvider.Client;
using Server.Contract.Session;
using System;
using Parking.Contract.Common;
using Connect.Common.Interface;

namespace Parking.App.Service.Common
{
    public class tblClientSoundMgtService : RemoteCacheDataService<tblClientSoundMgtInfo>
    {
        public tblClientSoundMgtService(ILog log, bool isDataSync, uint signature, string name) : base(log, isDataSync, signature, name)
        {
        }

        protected override void DefaultInsert(tblClientSoundMgtInfo info)
        {
            
        }

        protected override void DefaultModified(tblClientSoundMgtInfo info)
        {
            info.LastModified = DateTime.Now;
            info.LastModifiedBy = SessionDatas.GetLoginUser();
        }
    }
}
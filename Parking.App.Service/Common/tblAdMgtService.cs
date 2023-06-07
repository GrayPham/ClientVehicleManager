using Connect.Common;
using Connect.Common.Interface;
using Parking.App.Contract.Common;
using Parking.Contract.Common;
using nsConnect.RemoteDataProvider.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Parking.App.Service.Common
{
    public class tblAdMgtService : RemoteCacheDataService<tblAdMgtInfo>
    {
        public event GeneralEventHandler<tblAdMgtInfo> UpdateCompleted;
        public event GeneralEventHandler<object> RemoveCompleted;
        public event GeneralEventHandler<tblAdMgtInfo> AddCompleted;
        public void Set_UpdateCompleted(tblAdMgtInfo info)
        {
            if (UpdateCompleted != null) UpdateCompleted(this, new EventArgs<tblAdMgtInfo>(info));
        }
        public void Set_RemoveCompleted(object info)
        {
            if (RemoveCompleted != null) RemoveCompleted(this, new EventArgs<object>(info));
        }
        public void Set_AddCompleted(tblAdMgtInfo info)
        {
            if (AddCompleted != null) AddCompleted(this, new EventArgs<tblAdMgtInfo>(info));
        }


        public tblAdMgtService(ILog log, bool isDataSync, uint signature, string name) : base(log, isDataSync, signature, name)
        {
        }
        protected override void DefaultInsert(tblAdMgtInfo info)
        {
            
        }

        protected override void DefaultModified(tblAdMgtInfo info)
        {
            
        }
    }
}

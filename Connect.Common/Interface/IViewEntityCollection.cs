using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Connect.Common.Interface
{
    public interface IViewEntityCollection<TInfo> : IBindingList where TInfo : IInfo<TInfo>
    {
        void SetBinding(IInfoCollection<TInfo> infos);
    }
}

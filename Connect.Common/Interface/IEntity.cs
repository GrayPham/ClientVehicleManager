using System;
using System.ComponentModel;

namespace Connect.Common.Interface
{
    public interface IEntity : ICloneable, INotifyPropertyChanged, IIdentify
    {

    }
}

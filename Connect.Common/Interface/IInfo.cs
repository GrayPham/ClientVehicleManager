using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.Common.Interface
{
    public interface IInfo<in T> : IIdentify
    {
        void Copy(T info);
        string SQLData();
        string PrimaryKey();

    }
}

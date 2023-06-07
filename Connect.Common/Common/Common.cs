using DevExpress.Xpo;
using System;

namespace Connect.Common.Common
{
    public enum ConnectionFailedReason
    {
        Unknown,
        InvalidSerialNumber,
        InvalidLicenseKey,
        MacError,
    }
    public enum EPlatform { None, Windows, Web, Android, IOS, WindowPhone }
    public enum EValidateType { None, Add, Update, Remote, AddList, UpdateList, RemoteList }
    public enum EClientType { None, Replication, Admin, Manager, Supervisor, ClientNormal, ClientDevice }
    public enum EFormBy { Me, All }
    public enum EDataType { List, Database }
    public enum ETypeFilter { Equal, NotEqual, StartsWith, Contains, NotContain, EndsWith, IsNull, NotNull, Empty, NotEmpty, Greater, GreaterOrEqual, Less, LessOrEqual, Between, In, NotIn }


}
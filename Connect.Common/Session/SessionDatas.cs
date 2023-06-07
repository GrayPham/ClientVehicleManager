using Connect.Common;
using Connect.Common.Contract;

namespace Server.Contract.Session
{
    public partial class SessionDatas
    {
        public static int BranchSelectID { get; set; }
        public static event GeneralEventHandler<SessionInfo> ReTryLogin;
        public static event GeneralEventHandler<SessionInfo> LoginOuted;
        public static void OnReTryLogin(object view)
        {
            if (ReTryLogin != null) ReTryLogin(view, new EventArgs<SessionInfo>(SessionDatas.Sessioninfo));
        }
        public static void OnUserLogOut(object view)
        {
            if (LoginOuted != null) LoginOuted(view, new EventArgs<SessionInfo>(SessionDatas.Sessioninfo));
        }
        public static bool IsLogined { get; set; } = false;
        public static SessionInfo Sessioninfo { get; set; }
        public static string GetLoginUser()
        {
            return Sessioninfo == null ? "" : (""+ Sessioninfo.Login_Username).ToLower();
        }
        public static int GetUserID()
        {
            return ( Sessioninfo == null ? 0 : (Sessioninfo.Login_LoginID));
        }
        public static int GetBranchID()
        {
            return Sessioninfo == null ? 0 : Sessioninfo.BranchID;
        }
        public static object GetBranchCode()
        {
            return Sessioninfo == null ? "" : Sessioninfo.BranchCode;
        }
       
        public static string GetOwner()
        {
            return Sessioninfo == null ? "" : Sessioninfo.Owner;
        }

        //-------------------------------------------------------------------
    }
}

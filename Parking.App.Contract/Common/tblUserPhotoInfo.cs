using Connect.Common.Interface;
namespace Parking.App.Contract.Common
{
    public class tblUserPhotoInfo : IInfo<tblUserPhotoInfo>
    {
        //-------------------------------------------------------------

        private object _id;

        public int UserPhotoNo { get; set; }
        public string UserID { get; set; }
        public byte[] TakenPhoto { get; set; }
        public byte[] IdCardPhoto { get; set; }


        public tblUserPhotoInfo()
        {
            UserID = "";
            TakenPhoto = new byte[0];
            IdCardPhoto =null;
        }

        public void Copy(tblUserPhotoInfo info)
        {
            UserPhotoNo = info.UserPhotoNo;
            UserID = info.UserID;
            TakenPhoto = info.TakenPhoto;
            IdCardPhoto = info.IdCardPhoto;
        }

        //-------------------------------------------------------------  
        public object ValueID { get { return UserPhotoNo; } set { _id = value; } }

        public override string ToString() { return UserPhotoNo.ToString(); }

        public string PrimaryKey()
        {
            return @" UserPhotoNo";
        }

        public string SQLData()
        {
            return @"";
        }
        //-------------------------------------------------------------  
    }
   
    
}

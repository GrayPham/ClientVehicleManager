using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.App.Contract.Entity
{
    public class UserEntity
    {
        public string UserID { get; set; }
        public string UserType { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public bool ApproveReject { get; set; }

        public string UserStatus { get; set; }

        public DateTime RegistDate { get; set; }

        public string Desc { get; set; }


        public UserEntity()
        {
            UserID = String.Empty;
            UserType = String.Empty;
            Password = String.Empty;
            UserName = String.Empty;
            PhoneNumber = String.Empty;
            Birthday = DateTime.Now;
            Email = String.Empty;
            Gender = false;
            ApproveReject = false;
            UserStatus = String.Empty;
            RegistDate = DateTime.Now;
            Desc = String.Empty;
        }
    }
}

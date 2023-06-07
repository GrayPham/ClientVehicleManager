using Connect.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connect.Common.Contract
{
    public class InfoBase<T> : IInfo<T>
    {
        //-------------------------------------------------------------

        #region Member

        private object _id = 0;

        #endregion

        //-------------------------------------------------------------

        #region Propertise

        public virtual object ValueID
        {
            get
            {
                return _id;
            }
            set
            {

            }
        }

        public virtual void Copy(T info)
        {

        }


        #endregion


        public virtual string SQLData()
        {
            return "";
        }


        public string PrimaryKey()
        {
            return "";
        }
    }
}

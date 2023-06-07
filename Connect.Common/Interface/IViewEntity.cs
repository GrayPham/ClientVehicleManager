using System.ComponentModel;

namespace Connect.Common.Interface
{
    public interface IViewEntity<T> : INotifyPropertyChanged where T : new()
    {
        void Set(T info);
        void Copy(T info);
        T Info { get; set; }
        object ValueID { get; set; }
        //object ValueID { get; set; }
        //void CopyEntity(IViewEntity<T> entity);
    }
}

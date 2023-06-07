namespace Connect.Common.Interface
{
    /// <summary>
    /// Library 2 Libary message updated. The class support this interface will raise an event with Code, Message and Type for notice client library about event
    /// </summary>
    public interface IL2LMessageUpdated
    {
        event L2LMessageEventHandler L2LMessageUpdated;
    }
}

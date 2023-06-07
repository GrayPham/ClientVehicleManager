using Connect.Common.Logging;
using System;

namespace Connect.Common.Interface
{
    public interface ILog
    {
        void Info(String content);
        void Warn(String content);
        void Error(String content);
        void SError(string className, string content, string trac, string data);
        void Critical(String content);
        void In(String content);
        void Out(String content);
        void Trace(String content);
        void Print(String context, Exception ex);
        void ChangeFile(string path);
        event GeneralEventHandler<DLogInfo> Errored;
    }
}

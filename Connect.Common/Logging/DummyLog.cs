using Connect.Common.Interface;
using System;

namespace Connect.Common.Logging
{
    public class DummyLog : ILog
    {
        public event GeneralEventHandler<DLogInfo> Errored;

        public void Info(string content)
        {
            // Do nothing
        }

        public void Warn(string content)
        {
            // Do nothing
        }

        public void Error(string content)
        {
            // Do nothing
        }

        public void Critical(string content)
        {
            // Do nothing
        }

        public void In(string content)
        {
            // Do nothing
        }

        public void Out(string content)
        {
            // Do nothing
        }

        public void Trace(string content)
        {
            // Do nothing
        }

        public void Print(string context, Exception ex)
        {
            // Do nothing
        }
        public void ChangeFile(string path)
        {

        }

        public void SError(string className, string content, string trac, string data)
        {

        }
    }
}

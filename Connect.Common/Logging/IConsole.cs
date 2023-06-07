using System;

namespace Connect.Common.Logging
{
    public interface IConsole
    {
        void WriteLine(String format, params object[] args);
        void SetColor(ConsoleColor color);
    }
}
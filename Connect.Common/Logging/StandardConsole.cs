using System;

namespace Connect.Common.Logging
{
    public class StandardConsole : IConsole
    {
        public void WriteLine(string format, params object[] args)
        {
            try
            {
                Console.WriteLine(format, args);
            }
            catch
            {
            }
        }

        public void SetColor(ConsoleColor color)
        {
            try
            {
                if (Console.ForegroundColor != color)
                {
                    Console.ForegroundColor = color;
                }
            }
            catch
            {
            }
        }
    }
}

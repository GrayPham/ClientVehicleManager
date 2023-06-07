using Connect.Common.Interface;
using System;
using System.IO;
using System.Text;

namespace Connect.Common.Logging
{
    enum Category
    {
        Info,
        Warning,
        Error,
        Critical,
        In,
        Out,
        Trace
    }

    public class DirectLog : ILog
    {
        #region Private Member
        //-----------------------------------------------------------------------------------------------------

        private bool _isConsoleOutput = false;
        private bool _isFileOutput = true;
        private String _logFile;
        private readonly IConsole _console;
        private StreamWriter _writer;
        public event GeneralEventHandler<DLogInfo> Errored;

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Interface
        //-----------------------------------------------------------------------------------------------------

        public void Info(string content)
        {
            Log(Category.Info, content);
        }

        public void Warn(string content)
        {
            if (TraceEnable) Log(Category.Warning, TraceHelper.GetCurrentMethod() + ":" + content);
        }

        public void Error(string content)
        {
            if (TraceEnable) Log(Category.Error, TraceHelper.GetCurrentMethod() + ":" + content);
        }
        public void SError(string className, string content, string trac, string data)
        {
            var curent = TraceHelper.GetCurrentMethod();
            if (TraceEnable) Log(Category.Error, curent + ":" + content + "\n\t => " + trac);
            try
            {
                var info = new DLogInfo()
                {
                    ClassName = className,
                    CurrentMethod = curent,
                    ErrorMessage = content,
                    StackTrace = trac,
                    Data = data,
                };
                if (Errored != null) Errored(this, new EventArgs<DLogInfo>(info));
            }
            catch (Exception)
            {
            }
        }
        public void Critical(string content)
        {
            if (TraceEnable) Log(Category.Critical, TraceHelper.GetCurrentMethod() + ":" + content);
        }

        public void In(string content)
        {
            Log(Category.In, content);
        }

        public void Out(string content)
        {
            Log(Category.Out, content);
        }

        public void Trace(string content)
        {
            if (TraceEnable) Log(Category.Trace, TraceHelper.GetCurrentMethod() + ":" + content);
        }

        public void Print(string context, Exception ex)
        {
            if (ex == null) return;
            if (ex.InnerException != null)
            {
                Warn(context + ex.InnerException.Message);
            }
            else
            {
                Warn(context + ex.Message);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Properties/Public Method
        //-----------------------------------------------------------------------------------------------------

        public bool TraceEnable { get; set; }

        public bool ConsoleOutputEnable
        {
            get { return _isConsoleOutput; }
            set { _isConsoleOutput = value; }
        }

        public bool FileOutputEnable
        {
            get { return _isFileOutput; }
            set { _isFileOutput = value; }
        }

        public string LogFile
        {
            get { return _logFile; }
            set
            {
                Close();
                _logFile = value;
                Open(_logFile);
            }
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Construtor/Destructor
        //-----------------------------------------------------------------------------------------------------
        public DirectLog(String filename, IConsole console)
        {
            TraceEnable = true;

            if (filename != null)
            {
                _logFile = filename;
                _isFileOutput = Open(_logFile);
            }
            else
            {
                _writer = null;
                _logFile = null;
                _isFileOutput = false;
            }

            if (console != null)
            {
                _console = console;
                _isConsoleOutput = true;
            }
            else
            {
                _console = null;
                _isConsoleOutput = false;
            }
        }
        public DirectLog(string padth, String filename, IConsole console)
        {
            TraceEnable = true;
            if (filename == null)
            {
                filename = GetFileName(padth);
            }
            if (filename != null)
            {
                _logFile = filename;
                _isFileOutput = Open(_logFile);
            }
            else
            {
                _writer = null;
                _logFile = null;
                _isFileOutput = false;
            }

            if (console != null)
            {
                _console = console;
                _isConsoleOutput = true;
            }
            else
            {
                _console = null;
                _isConsoleOutput = false;
            }
        }
        public string GetFileName(string path)
        {
            string strdirectory = path + "\\Log\\";
            if (!Directory.Exists(strdirectory))
            {
                Directory.CreateDirectory(strdirectory);
            }
            string datenow = DateTime.Now.ToString("ddMMyy_HH");
            if (datenow.IndexOf("/") > 0)
            {
                datenow = datenow.Replace("/", "-");
            }
            string filename = strdirectory + "log_" + datenow;
            if (!Directory.Exists(strdirectory))
            {
                Directory.CreateDirectory(strdirectory);
            }
            return filename + ".txt";
        }
        public void ChangeFile(string path)
        {
            _logFile = GetFileName(path);
            _isFileOutput = Open(_logFile);
        }
        ~DirectLog()
        {
            Close();
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Private Method
        //-----------------------------------------------------------------------------------------------------

        private void Log(Category cat, string content)
        {
            var time = DateTime.Now;
            var builder = new StringBuilder();
            switch (cat)
            {
                case Category.Info:
                    builder.Append("Info ");
                    break;
                case Category.Warning:
                    builder.Append("Warning ");
                    break;
                case Category.Error:
                    builder.Append("Error ");
                    break;
                case Category.Critical:
                    builder.Append("Critical ");
                    break;
                case Category.In:
                    builder.Append("In ");
                    break;
                case Category.Out:
                    builder.Append("Out ");
                    break;
                case Category.Trace:
                    builder.Append("Trace ");
                    break;
                default:
                    break;
            }
            //if (cat == Category.In) builder.Append("In ");
            //if (cat == Category.Out) builder.Append("Out");
            builder.Append(": ");
            builder.Append(content);

            if ((_console != null) && _isConsoleOutput)
            {
                switch (cat)
                {
                    case Category.Warning:
                        _console.SetColor(ConsoleColor.Yellow);
                        break;
                    case Category.Error:
                        _console.SetColor(ConsoleColor.Yellow);
                        break;
                    case Category.Critical:
                        _console.SetColor(ConsoleColor.Red);
                        break;
                    case Category.In:
                        _console.SetColor(ConsoleColor.Magenta);
                        break;
                    case Category.Out:
                        _console.SetColor(ConsoleColor.Green);
                        break;
                    case Category.Trace:
                        _console.SetColor(ConsoleColor.DarkBlue);
                        break;
                    default:
                        _console.SetColor(ConsoleColor.White);
                        break;
                }
                _console.WriteLine("{0:H:mm:ss fff} {1}", time, builder.ToString());
            }

            if ((_writer != null) && _isFileOutput)
            {
                _writer.WriteLine("{0:H:mm:ss fff} {1}", time, builder);
            }
        }

        private bool Open(String file)
        {
            try
            {
                _writer = File.Exists(file) ? File.AppendText(file) : File.CreateText(file);
                _writer.AutoFlush = true;
            }
            catch (Exception e)
            {
                _writer = null;
                if (_console != null)
                {
                    var time = DateTime.Now;
                    _console.WriteLine("{0:H:mm:ss fff} Open [{1}] error [{2}]", time, file, e.Message);
                }
                return false;
            }
            return true;
        }

        private void Close()
        {
            if (_writer != null)
            {
                try
                {
                    _writer.Flush();
                    _writer.Close();
                }
                catch
                {
                    _console.SetColor(ConsoleColor.Red);
                    _console.WriteLine(@"Không ghi được text file");
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion
    }
}

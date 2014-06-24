using System;
using System.IO;

namespace Chinook.Utility
{
    public class FileLogWriter : ILogWriter
    {
        static object __lock = new object();

        string _logFile;

        public FileLogWriter(string logFile)
        {
            _logFile = logFile;
        }

        public void Write(string message)
        {
            Write(message, null);
        }

        public void Write(string message, string details)
        {
            lock (__lock)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_logFile));
                using (StreamWriter sw = File.AppendText(_logFile))
                {
                    sw.WriteLine(String.Format("[{0}] {1}", DateTime.Now, message));
                    if (!String.IsNullOrEmpty(details))
                    {
                        sw.WriteLine(details);
                    }
                }
            }
        }
    }
}

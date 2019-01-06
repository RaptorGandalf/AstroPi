using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroPi.Logging
{
    public static class Logger
    {
        public delegate void OnLog(string text);

        public static OnLog LogEvent { get; set; }

        public static void Log(string text)
        {
            LogEvent?.Invoke(text);
        }
    }
}

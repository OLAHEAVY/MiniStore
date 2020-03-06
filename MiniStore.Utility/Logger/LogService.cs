using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Utility.Logger
{
    public class LogService : ILog
    {
        public void LogException(string message, Exception ex)
        {
            Serilog.Log.Error(ex, message);
        }

        public void LogInformation(string message)
        {
            Serilog.Log.Information(message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MiniStore.Utility.Logger
{
    public class LogService : ILog
    {
        public void LogException(string message, Exception ex)
        {
            Serilog.Log.Error (ex, WebUtility.HtmlEncode(message));

            
        }

        public void LogInformation(string message)
        {
            Serilog.Log.Information(WebUtility.HtmlEncode(message));
        }
    }
}

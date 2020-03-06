using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Utility.Logger
{
    public interface ILog
    {
        void LogException(string message, Exception ex);
        void LogInformation(string message);
    }
}

using MiniStore.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.Interface
{
    public interface ICommonHelper
    {
        long? CurrentUserId();
        ResponseMessage OutputMessage(bool isSuccessful, string message);
        string CurrentUsername();
    }
}

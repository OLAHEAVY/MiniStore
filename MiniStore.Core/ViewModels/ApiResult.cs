using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.ViewModels
{
    public class ApiResult<T>
    {

        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public bool HasError { get; set; }
        public T Result { get; set; }
    }
}

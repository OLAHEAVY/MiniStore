using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.ViewModels
{
    public class UserResponseModel
    {
        public ResponseMessage ResponseMessage { get; set; }
        public ApplicationUser User { get; set; }
    }
}

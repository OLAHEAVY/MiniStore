using Microsoft.AspNetCore.Mvc.Rendering;
using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Core.Interface
{
    public interface IStateService
    {
        IEnumerable<SelectListItem> GetAllStatesDropDown();
    }
}

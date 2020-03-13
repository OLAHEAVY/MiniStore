using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Core.Interface
{
    public interface ILocalGovernmentService
    {
        IEnumerable<SelectListItem> GetLocalGovernmentDropdownList(string stateId);
    }
}

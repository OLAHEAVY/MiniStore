using Microsoft.AspNetCore.Mvc.Rendering;
using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Data.Repository.IRepository
{
    public interface ILocalGovernmentRepository: IBaseRepository<LocalGovernment>
    {
        IEnumerable<SelectListItem> GetLocalGovernmentListForDropDown(string StateId);
    }
}

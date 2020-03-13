using Microsoft.AspNetCore.Mvc.Rendering;
using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Data.Repository.IRepository
{
    public interface IStateRepository: IBaseRepository<State>
    {
        IEnumerable<SelectListItem> GetStateListForDropDown();
    }
}

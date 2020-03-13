using Microsoft.AspNetCore.Mvc.Rendering;
using MiniStore.Data.Entities;
using MiniStore.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniStore.Data.Repository
{
    public class StateRepository: BaseRepository<State>, IStateRepository
    {
        private MiniStoreDbContext _db;

        public StateRepository(MiniStoreDbContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetStateListForDropDown()
        {
            return _db.State.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()

            });
        }
    }
}

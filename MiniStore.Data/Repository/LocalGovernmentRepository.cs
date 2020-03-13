using Microsoft.AspNetCore.Mvc.Rendering;
using MiniStore.Data.Entities;
using MiniStore.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniStore.Data.Repository
{
    public class LocalGovernmentRepository: BaseRepository<LocalGovernment>, ILocalGovernmentRepository
    {
        private readonly MiniStoreDbContext _db;

        public LocalGovernmentRepository(MiniStoreDbContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetLocalGovernmentListForDropDown(string stateId)
        {
            return _db.LocalGovernment.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.StateId.ToString(),


            }).Where(c => c.Value == stateId);
        }
    }
}

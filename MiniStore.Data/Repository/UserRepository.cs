using MiniStore.Data.Entities;
using MiniStore.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniStore.Data.Repository
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly MiniStoreDbContext _db;

        public UserRepository(MiniStoreDbContext db) : base(db)
        {
            _db = db;
        }
        public void LockUser(long userId)
        {
            //retieve user from the database
            var userFromDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
        }

        public void UnlockUser(long userId)
        {
            //retieve user from the database
            var userFromDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            userFromDb.LockoutEnd = DateTime.Now;
        }
    }
}

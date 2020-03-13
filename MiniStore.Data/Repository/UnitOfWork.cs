using MiniStore.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        //inject the db context here
        private readonly MiniStoreDbContext _db;

        public UnitOfWork(MiniStoreDbContext db)
        {
            _db = db;

            User = new UserRepository(_db);

            State = new StateRepository(_db);

            LocalGovernment = new LocalGovernmentRepository(_db);

        }

        public IUserRepository User { get; private set; }

        public IStateRepository State { get; private set; }

        public ILocalGovernmentRepository LocalGovernment { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() > 0;
        }

    }
}

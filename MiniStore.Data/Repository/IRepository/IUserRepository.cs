using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Data.Repository.IRepository
{
    public interface IUserRepository :IBaseRepository<ApplicationUser>
    {
        void LockUser(long userId);

        void UnlockUser(long userId);
    }
}

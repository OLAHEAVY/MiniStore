using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Data.Repository.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        IUserRepository User { get; }

        IStateRepository State { get;}

        ILocalGovernmentRepository LocalGovernment { get;}

        Task<bool> Save();
    }
}

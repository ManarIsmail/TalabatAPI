using TalabatBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatBLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepositry<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
    }
}

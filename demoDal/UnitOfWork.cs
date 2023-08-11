using TalabatBLL.Entities;
using TalabatBLL.Interfaces;
using TalabatDAL.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatDAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;

        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
          => await _context.SaveChangesAsync();

        public void Dispose()
          => _context.Dispose();

        public IGenericRepositry<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories is null)
                _repositories= new Hashtable();

            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepositry<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepositry<TEntity>) _repositories[type];
        }
    }
}

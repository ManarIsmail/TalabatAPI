using TalabatBLL.Entities;
using TalabatBLL.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatBLL.Interfaces
{
    public interface IGenericRepositry<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync (int id);
        Task<T> GetEntityWithSpec(ISpecifications<T> spec);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecifications<T> spec);
        Task<int> CountAsync(ISpecifications<T> spec);
        
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

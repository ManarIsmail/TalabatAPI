using TalabatBLL.Entities;
using TalabatBLL.Interfaces;
using TalabatBLL.Specifications;
using TalabatDAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatDAL
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepositry(StoreContext context)
        {
            _context = context;
        }

        public void Add(T entity)
            => _context.Set<T>().Add(entity);

        public async Task<int> CountAsync(ISpecifications<T> spec)
             => await ApplySpecification(spec).CountAsync();



        public void Delete(T entity)
            => _context.Set<T>().Remove(entity);
        

        public async Task<T> GetByIdAsync(int id)
          => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetEntityWithSpec(ISpecifications<T> spec)
            => await ApplySpecification(spec).FirstOrDefaultAsync();


        public async Task<IReadOnlyList<T>> ListAllAsync()
             => await _context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecifications<T> spec)
            => await ApplySpecification(spec).ToListAsync();
       

        public void Update(T entity)
           => _context.Set<T>().Update(entity);

        private IQueryable<T> ApplySpecification(ISpecifications<T> specifications)
            => SpecificationEvaluatar<T>.GetQuery(_context.Set<T>(), specifications);
     
    }
}

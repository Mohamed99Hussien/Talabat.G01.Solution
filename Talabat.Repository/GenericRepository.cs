using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context)
        {
            _context= context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Mosakan
           // if (typeof(T) == typeof(Product)) 
             //   return (IEnumerable<T>) await _context.Set<Product>().Include(p =>p.productBrand).Include(p => p.productType).ToListAsync();
        
            return await _context.Set<T>().ToListAsync();
        }

       
        public async Task<T> GetByIdAsync(int Id)
        //=> await _context.Set<T>().FindAsync(Id);

      => await _context.Set<T>().FirstOrDefaultAsync(P => P.Id == Id); //=> take with database





        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification( ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }

        public async Task CreateAsync(T entity)
         => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity)
        => _context.Set<T>().Update(entity); 
        //=> _context.Entry(entity).State= EntityState.Modified; // make cheek and Update
        public void Delete(T entity)
        => _context.Set<T>().Remove(entity);

        public void Delete(IEnumerable<Order> existingOrder)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;


namespace Talabat.Repository.Data
{
    public class SpecificationEvaluator <TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery, ISpecification<TEntity> spec)
        {
             var query = InputQuery; // _context.set<Product>
            if (spec.Criteria != null) // P => p.ID ==10
                query = query.Where(spec.Criteria);

            // _context.set<TEntity>().where( P => p.ID ==10)

            if (spec.IsPaginationEnabled)
                query=query.Skip(spec.Skip).Take(spec.Take);

            // _context.set<Product>.Skip(5).Take(5)

            if (spec.OrderBy !=null)
                query= query.OrderBy(spec.OrderBy);

            if (spec.OrderByDescending !=null)
                query= query.OrderByDescending(spec.OrderByDescending);


            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression)); // Loop

            // _context.set<TEntity>().where( P => p.ID ==10).Include(p => p.PrudectBrand)

            // _context.set<TEntity>().where( P => p.ID ==10).Include(p => p.PrudectBrand).Include(p=> p.Producttype)
            return query;

        //    string[] Name = { "Mohamed", "Hussien", "Hassan" };
        //   string FullName = Name.Aggregate((N1, N2) => $"{N1} {N2}"); // Mohame Hussien Hassen 
            
        }
    }
}

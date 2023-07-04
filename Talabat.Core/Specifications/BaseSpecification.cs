using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        // automatic property =>  do bucking field 
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        // get All Products
        public BaseSpecification()
        {
           // Includes = new List<Expression<Func<T, object>>>();
        }

        // use to Get a specific product
        public BaseSpecification(Expression<Func<T, bool>> criteria) // criteria  => (P => p.id==10)
         {
            Criteria = criteria;
           //  Includes = new List<Expression<Func<T, object>>>();
         }

        public void AddOrderBy(Expression<Func<T, object>> OrderByExpression) 
        {
            OrderBy = OrderByExpression; // set data 
        }

        public void AddOrderByDecending(Expression<Func<T, object>> OrderByDecendingExpression)
        {
            OrderByDescending = OrderByDecendingExpression; // set data 
        }

        public void ApplyPagination(int skip, int take) 
        {
            IsPaginationEnabled = true;
            Skip= skip;
            Take= take;
        }
    }
}

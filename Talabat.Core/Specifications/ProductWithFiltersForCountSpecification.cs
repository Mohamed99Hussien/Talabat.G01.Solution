using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFiltersForCountSpecification: BaseSpecification <Product>
    {
        // cheek => If there is a filter
        public ProductWithFiltersForCountSpecification(ProductSpecParms productParms)
            :base(P =>

                     (string.IsNullOrEmpty(productParms.Search) || P.Name.ToLower().Contains(productParms.Search)) &&
                     (!productParms.BrandId.HasValue || P.ProductBrandId == productParms.BrandId.Value) &&
                     (!productParms.TypeId.HasValue || P.ProductTypeId == productParms.TypeId.Value)
                  )
        { }
    }
}

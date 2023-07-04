using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification :BaseSpecification<Product>
    {
        // this constructor is used for Get All Products
        public ProductWithBrandAndTypeSpecification(ProductSpecParms productParms) // go to constructor Criteria
            :base(P =>
                     (string.IsNullOrEmpty(productParms.Search) || P.Name.ToLower().Contains(productParms.Search)) &&
                     (!productParms.BrandId.HasValue || P.ProductBrandId == productParms.BrandId.Value) &&
                     (!productParms.TypeId.HasValue  || P.ProductTypeId  == productParms.TypeId.Value ) 
                  )
        {
            Includes.Add(p => p.productBrand);
            Includes.Add(p => p.productType);

            // totalProducts =18~20
            // pageSize      =5
            // pageIndex     =2

            ApplyPagination(productParms.PageSize * (productParms.PageIndex - 1), productParms.PageSize);

            if (!string.IsNullOrEmpty(productParms.Sort))
            {
                switch (productParms.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDecending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
        }

        // this constructor is used for Get a Specific Product
        public ProductWithBrandAndTypeSpecification(int  Id) : base(p => p.Id ==Id)// Criteria = (p => p.Id ==Id)
        {
            Includes.Add(p => p.productBrand);
            Includes.Add(p => p.productType);
        }
    }
}

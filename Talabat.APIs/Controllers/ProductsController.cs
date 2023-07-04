using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    
    public class ProductsController : BaseApiController
    {
        //private readonly IGenericRepository<Product> _productsRepo;
        //private readonly IGenericRepository<ProductBrand> _brandsRepo;
        //private readonly IGenericRepository<ProductType> _typesRepo;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsController(
            IMapper mapper,
                IUnitOfWork unitOfWork
            //IGenericRepository <Product> productsRepo,
            //IGenericRepository<ProductBrand> brandsRepo,
            //IGenericRepository<ProductType> typessRepo
            ) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

            //_productsRepo= productsRepo;
            //_brandsRepo= brandsRepo;
            //_typesRepo= typessRepo;
        }

        //  [Authorize]

        //[CachedAttribute(600)] // Action Filter
        [HttpGet] // Get : api/products
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParms productParms)
        {
            var spec = new ProductWithBrandAndTypeSpecification(productParms);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            //var products = await _productsRepo.GetAllAsync();

            var Data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);
            var countSpec = new ProductWithFiltersForCountSpecification(productParms);
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);
            return Ok(new Pagination<ProductToReturnDto>(productParms.PageIndex,productParms.PageSize,count,Data));
        }
       // [CachedAttribute(600)]
        [HttpGet ("{id}")] // Get : api/products/10
        public async Task<ActionResult<ProductToReturnDto>> GetProductsById( int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);
           
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            if (product == null) return NotFound(new ApiResponse(404));
                                            

            //var product = await _productsRepo.GetById(id);
            return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
        }
      //  [CachedAttribute(600)]
        [HttpGet("brands")] // GET : api/product/brands
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
        {
            var brands =await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
     //   [CachedAttribute(600)]
        [HttpGet("types")] // GET : api/product/types
        public async Task<ActionResult<IEnumerable<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }
    }
}

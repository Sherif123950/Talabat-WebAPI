using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<ProductType> _typeRepo;
        //private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork
            //IGenericRepository<Product> ProductRepo
            //, IGenericRepository<ProductType> TypeRepo,
            //IGenericRepository<ProductBrand> BrandRepo
            , IMapper mapper)
        {
            //_productRepo = ProductRepo;
            //_typeRepo = TypeRepo;
            //_brandRepo = BrandRepo;
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [CacheAttribute(500)]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetAllProducts([FromQuery]ProductSpecParams Params)
        {
            var ProductSpec = new ProductBrandAndTypeSpecification(Params);
            var Products = await _unitOfWork.Repository<Product>().GetAllAsyncWithSpec(ProductSpec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(Products);
            var CountSpec = new ProductWithFiltrationForCountSpecAsync(Params);
            var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDTO>(Params.PageSize, Params.PageIndex,MappedProducts,Count));
        }
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [CacheAttribute(500)]
        public async Task<ActionResult<Product>> GetProductById(int Id)
        {
            var ProductSpec = new ProductBrandAndTypeSpecification(Id);
            var Product = await _unitOfWork.Repository<Product>().GetByIdAsyncWithSpec(ProductSpec);
            if (Product is null)
                return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<Product, ProductToReturnDTO>(Product);
            return Ok(MappedProduct);
        }
        [CacheAttribute(500)]
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(Types);
        }
        [HttpGet("Brands")]
        [CacheAttribute(500)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }
    }
}

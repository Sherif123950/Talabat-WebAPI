using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductBrandAndTypeSpecification:BaseSpecifications<Product>
    {
        public ProductBrandAndTypeSpecification(ProductSpecParams Params) :
            base(P=>
            (string.IsNullOrEmpty(Params.Search)||P.Name.Trim().ToLower().Contains(Params.Search))
            &&
            (!Params.BrandId.HasValue || P.ProductBrandId==Params.BrandId)
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
                )
        {
            Includes.Add(P=>P.ProductType);
            Includes.Add(P => P.ProductBrand);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort) 
                {
                    case "PriceAsc" :
                        AddOrderBy(P => P.Price);
                            break;
                    case "PriceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default :
                        AddOrderBy(P => P.Name);
                        break;
                }               
            }
            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }
        public ProductBrandAndTypeSpecification(int id):base(P=>P.Id==id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
        }
    }
}

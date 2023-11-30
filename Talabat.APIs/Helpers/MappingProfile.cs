using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.APIs.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(S => S.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(S => S.ProductType.Name))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, Core.Entities.Order_Aggregation.Address>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>().ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                                               .ForMember(D => D.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>().ForMember(D=>D.ProductId,OI=>OI.MapFrom(S=>S.ProductItem.ProductId))
                                                .ForMember(D => D.ProductName, OI => OI.MapFrom(S => S.ProductItem.ProductName))
                                                .ForMember(D => D.PictureUrl, OI => OI.MapFrom(S => S.ProductItem.PictureUrl))
                                                .ForMember(D => D.PictureUrl, OI => OI.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}

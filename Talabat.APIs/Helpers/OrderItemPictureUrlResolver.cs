using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            
            if (!string.IsNullOrEmpty(source.ProductItem.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.ProductItem.PictureUrl}";
            }
            return string.Empty;
        }
    }
}

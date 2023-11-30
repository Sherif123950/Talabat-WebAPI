using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregation
{
    public class OrderItem :BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItem productItem, decimal price, int quantity)
        {
            ProductItem = productItem;
            Price = price;
            Quantity = quantity;
        }

        public ProductItem ProductItem { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}

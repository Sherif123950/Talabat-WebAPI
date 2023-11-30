using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Specifications.Order_Specs
{
    public class OrderSpecification:BaseSpecifications<Order>
    {
        public OrderSpecification(string Email):base(O=>O.BuyerEmail==Email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDesc(O=>O.OrderDate);
        }
        public OrderSpecification(string Email,int id) : base(O => O.BuyerEmail == Email&&O.Id==id)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}

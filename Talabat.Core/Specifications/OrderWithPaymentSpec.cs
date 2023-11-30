using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Specifications
{
    public class OrderWithPaymentSpec:BaseSpecifications<Order>
    {
        public OrderWithPaymentSpec(string PaymentIntentId):base(O=>O.PaymentIntentId==PaymentIntentId)
        {
            Includes.Add(O=>O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}

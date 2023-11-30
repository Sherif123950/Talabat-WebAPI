using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _prodRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IPaymentService paymentService
            //IGenericRepository<Product> ProdRepo,
            //IGenericRepository<DeliveryMethod> DeliveryRepo,
            //IGenericRepository<Order> OrderRepo
            )
        {
            _basketRepository = basketRepository;
            this._unitOfWork = unitOfWork;
            this._paymentService = paymentService;
            //_prodRepo = ProdRepo;
            //_deliveryRepo = DeliveryRepo;
            //_orderRepo = OrderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            //1.Get Basket From Basket Repo
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            var OrderList = new List<OrderItem>();
            //2.Get Selected Items at Basket From Product Repo
            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItem = new ProductItem(item.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItem, Product.Price, item.Quantity);
                    OrderList.Add(OrderItem);
                }
            }
            //3.Calculate SubTotal
            var SubTotal = OrderList.Sum(OL => OL.Price * OL.Quantity);
            //4.Get Delivery Method From DeliveryMethod Repo
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            //5.Create Order
            var Spec = new OrderWithPaymentSpec(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetByIdAsyncWithSpec(Spec);
            if (ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            }
            var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderList, SubTotal,Basket.PaymentIntentId);
            //6.Add Order Locally
            await _unitOfWork.Repository<Order>().AddAsync(Order);
            //7.Save Order To Database[ToDo]
            var Result=await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods()
        {
            var DeliveryMethods =await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethods;
        }

        public Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId)
        {
            var Spec = new OrderSpecification(buyerEmail,OrderId);
            var Order = _unitOfWork.Repository<Order>().GetByIdAsyncWithSpec(Spec);
            return Order;
        }

        public Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var OrderSpec = new OrderSpecification(buyerEmail);
            var Orders=_unitOfWork.Repository<Order>().GetAllAsyncWithSpec(OrderSpec);
            return Orders;
        }
    }
}

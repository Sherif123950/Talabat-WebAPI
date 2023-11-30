using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    
    public class PaymentsController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_829016043a5dc7c7cbcbd8c55d773e6b90a763477b7b70d740a3a7c61f8f35b1";

        public PaymentsController(IBasketRepository basketRepository,IPaymentService paymentService,IMapper mapper)
        {
            this._basketRepository = basketRepository;
            this._paymentService = paymentService;
            this._mapper = mapper;
        }
        [HttpPost("{BasketId}")]
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentId(string BasketId)
        {
            var Basket = await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            if (Basket is null) return BadRequest(new ApiResponse(400, "There Is A Problem With Your Basket"));
            var MappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDto>(Basket);
            return Ok(MappedBasket);
        }
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    await _paymentService.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _paymentService.UpdatePaymentIntentToSucceedOrFailed(PaymentIntent.Id, true);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }

        }
    }
}

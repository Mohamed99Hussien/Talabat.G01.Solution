using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IService;

namespace Talabat.APIs.Controllers
{

    
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _whSecret = " whsec_cc436d2a5f6be9dad47d82f0731276b0ffcb53b616f2beba9d9a9b5139f95bfe";

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("basketId")] //POST: /api/Payments/basketId
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent([FromBody]string basketId)
            {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null) return BadRequest(new ApiResponse(400,"a problem with your Basket"));
            return Ok(basket);
        }

        // take Stripe => To make sure that user make PaymentIntent

        [HttpPost("webhook")] // POST : https://localhost:44369/api/Payments/webhook 
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSecret);

                PaymentIntent intent;
                Order order;
                // Handle the event
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        intent = (PaymentIntent) stripeEvent.Data.Object;
                        order = await _paymentService.UpdatePaymentIntentToSucceedOrFailed(intent.Id, true);
                        _logger.LogInformation("Payment is Succeded", order.Id, intent.Id);
                        break;

                    case Events.PaymentIntentPaymentFailed:
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        order = await _paymentService.UpdatePaymentIntentToSucceedOrFailed(intent.Id, false);
                        _logger.LogInformation("Payment is Faild", order.Id, intent.Id);
                        break;
                }


                return new EmptyResult();
            
        }
    }
}

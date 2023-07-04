using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IService;

namespace Talabat.APIs.Controllers
{

    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [HttpPost] // POST: /api/Orders

        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email); // get it from Token

            var orderAddress = _mapper.Map<AddressDto,Address>(orderDto.ShippingAddress);

            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BaskedId,orderDto.DeliveryMethodId, orderAddress);

            if (order == null) return  BadRequest(new ApiResponse(400));

            return Ok(_mapper.Map<Core.Entities.Order_Aggregate.Order, OrderToReturnDto>(order));
        }

        [HttpGet] // GET : /api/Orders
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrderForUserAsync(buyerEmail);
            return Ok(_mapper.Map<IEnumerable<Core.Entities.Order_Aggregate.Order>,IEnumerable<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")] // GET : /api/Orders/id
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(id ,buyerEmail);
            if (order == null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Core.Entities.Order_Aggregate.Order, OrderToReturnDto>(order));
        }

        [HttpGet("deliveryMethods")] //GET : /api/Orders/deliveryMethods

        public async Task<ActionResult<IEquatable<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethod = await _orderService.GetDeliveryMethodAsync();
            return Ok(deliveryMethod);
        }
    }
}

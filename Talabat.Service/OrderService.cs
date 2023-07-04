using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepositories;
using Talabat.Core.IService;
using Talabat.Core.Specifications;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodsRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(
            IBasketRepository basketRepo,
            //IGenericRepository<Product> productRepo,
            //IGenericRepository<DeliveryMethod> deliveryMethodsRepo,
            //IGenericRepository<Order> orderRepo) 
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            )
        
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
           _paymentService = paymentService;
            //_productRepo = productRepo;
            //_deliveryMethodsRepo = deliveryMethodsRepo;
            //_orderRepo = orderRepo;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string baskedId, int deliveryMethodId, Address shippingAddress)
        {
            // PseudoCode
            //1. Get Basket From Baskets Repo
            var basket = await _basketRepo.GetBasketAsync(baskedId);

            //2. Get Selected Items at Baskets Repo
            var orderItems =new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemOrdered(product.Id,product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrdered,product.Price,item.Quantity); // i will take item Quantity from front end

                orderItems.Add(orderItem);
            }

            //3. Calculate SubTotal
            var subTotal =(orderItems.Sum(item =>(int.Parse(item.Price) * item.Quantity)).ToString());

            //4. Get DeliveryMethod from DeliveryMethodRepo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // 5. Create Order

            // because if I have Order with same PaymentIntentId
            var spec = new OrderByPaymentIntentIdSepcidication(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            if (existingOrder !=null )
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(baskedId); // Update Amount
            }

            var order = new Order(buyerEmail,shippingAddress,deliveryMethod,orderItems,subTotal,basket.PaymentIntentId); // builder design pattern
            await _unitOfWork.Repository<Order>().CreateAsync(order);

            // 6. Save to Database [TODO]
           var result= await _unitOfWork.Complete();
            if (result <= 0) return null; 
            return order;
        }

        public async Task<IEnumerable<DeliveryMethod>> GetDeliveryMethodAsync()
        {
           var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            return deliveryMethods;
        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpecifications(orderId, buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpecifications(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return orders;
        }
    }
}

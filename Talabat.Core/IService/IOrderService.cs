using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.IService
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, string baskedId, int deliveryMethodId, Address ShippingAddress); // Address of the order

        Task<IEnumerable<Order>> GetOrderForUserAsync(string buyerEmail);

        Task<Order> GetOrderByIdForUserAsync(int basketId,string buyerEmail);

        Task<IEnumerable<DeliveryMethod>> GetDeliveryMethodAsync();
    }
}

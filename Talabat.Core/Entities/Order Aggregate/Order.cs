using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order:BaseEntity
    {
        public Order()
        {
        }

        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, string subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            PaymentIntentId = paymentIntentId;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }= DateTimeOffset.Now; // specific AM Or PM

        public OrderStatus Status { get; set; } = OrderStatus.Panding;

        public Address ShippingAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; } // Navigation Property [OWN]

        public ICollection<OrderItem> Items { get; set; } // Navigation Property [Many]

        public string SubTotal { get; set; }
        public string PaymentIntentId { get; set; }

        public string GetTotal()
            => SubTotal + DeliveryMethod.Cost; 

    }
}

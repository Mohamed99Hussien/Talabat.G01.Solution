using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public string Status { get; set; } 
        public Address ShippingAddress { get; set; }

        // public DeliveryMethod DeliveryMethod { get; set; } // Navigation Property [OWN]

        public string DeliveryMethod { get; set; }
        public int DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } // Navigation Property [Many]

        public string SubTotal { get; set; }
        public string PaymentIntentId { get; set; }

        public string Total { get; set;} // take value from function "GetTotal" in Class Order
    }
}

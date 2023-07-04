using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.DTOs
{
    public class OrderDto
    {
        public string BaskedId { get; set; }
        public int  DeliveryMethodId { get; set;}

        public AddressDto ShippingAddress { get; set; }
    }
}

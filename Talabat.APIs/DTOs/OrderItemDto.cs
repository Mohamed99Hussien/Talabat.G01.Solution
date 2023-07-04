namespace Talabat.APIs.DTOs
{
    public class OrderItemDto
    {
        public int Id { get; set; } // Id of OrderItem

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
    }
}
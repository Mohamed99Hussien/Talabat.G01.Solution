using Talabat.Core.Entities;

namespace Talabat.APIs.DTOs
{
    //DTO : Data Transfer Object
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string PictureUrl { get; set; }

        public int ProductBrandId { get; set; } 

        public string productBrand { get; set; } 

      
        public int ProductTypeId { get; set; }
        public string productType { get; set; } 
    }
}

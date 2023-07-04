﻿using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        //[Required]
      //  public string Description { get; set; }
        [Required]
        [Range(1,int.MaxValue, ErrorMessage = "Quantity must be one item at least")] 
        public int Quantity { get; set; }
        [Required]
        [Range(0.1,double.MaxValue , ErrorMessage ="Price must be Greater than Zero !!")]
        public string Price { get; set; }
        [Required]            
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
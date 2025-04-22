using System.ComponentModel.DataAnnotations;

namespace ApiVersioning.Models
{
    public class Product
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
    }
}

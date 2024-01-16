using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApi.Models
{

    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between $0.01 and $10,000")]
        public float Price { get; set; }
    }
}






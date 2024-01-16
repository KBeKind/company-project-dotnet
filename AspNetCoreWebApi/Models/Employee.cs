using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApi.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Department must be between 3 and 30 characters")]
        public string Department { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "City must be between 3 and 30 characters")]
        public string City { get; set; }
    }
}

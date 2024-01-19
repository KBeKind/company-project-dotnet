using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace AspNetCoreWebApi.Models
{
    public class RegistrationModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The username name must be at least 3 and at max 30 characters long.", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The first name must be at least 3 and at max 30 characters long.", MinimumLength = 3)]
        public string FirstName { get; set; }


        [Required]
        [StringLength(30, ErrorMessage = "The last name must be at least 3 and at max 30 characters long.", MinimumLength = 3)]
        public string LastName { get; set; }

    }
}

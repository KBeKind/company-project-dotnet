using System.ComponentModel.DataAnnotations;

namespace AspNectCoreWebApiClientProject.Models
{
    public class FrontendLoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
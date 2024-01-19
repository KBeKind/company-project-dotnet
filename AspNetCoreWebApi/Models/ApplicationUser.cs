using Microsoft.AspNetCore.Identity;

namespace AspNetCoreWebApi.Models
{
    public class ApplicationUser: IdentityUser
    {
        // Id, UserName, Email, Password Management
        public string FirstName { get; set; }

        public string LastName { get; set; }


        //public ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();
    }
}

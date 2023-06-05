using Microsoft.AspNetCore.Identity;

namespace eCommerce.Data.Auth
{
    public class AppUser : IdentityUser
    {
        public DateTime AccountCreationDate { get; set; }
    }
}

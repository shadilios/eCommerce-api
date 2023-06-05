using eCommerce.Data.Auth;
using Microsoft.AspNetCore.Identity;

namespace eCommerce.Services.AuthService
{
    public interface IAuth
    {
        public Task<List<AppUserDto>> GetUsers();
        public Task<bool> UserExists(SignUpDto model);
        public Task<IdentityResult> RegisterUser(SignUpDto model);
        public Task<IdentityResult> RegisterEditor(SignUpDto model);
        public Task<IdentityResult> RegisterAdmin(SignUpDto model);
    }
}

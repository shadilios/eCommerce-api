using eCommerce.Data;
using eCommerce.Data.Auth;
using eCommerce.Data.Roles;
using eCommerce.Models.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Services.AuthService
{
    public class AuthService : IAuth
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        /// <summary>
        /// Gets all users and their roles
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppUserDto>> GetUsers()
        {
            var roles = _context.Roles;
            var userRoles = _context.UserRoles;

            var users = await _context.Users
                .Select(u => new AppUserDto
                {
                    Username = u.UserName,
                    AccountCreationDate = u.AccountCreationDate,
                    Roles = roles
            .Where(r => userRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == r.Id))
            .Select(r => r.Name)
            .ToList()
                })
                .ToListAsync();

            return users;
        }

        public async Task<bool> UserExists(SignUpDto model)
        {
            //check username exists
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return true;
            return false;
        }

        public async Task<IdentityResult> RegisterUser(SignUpDto model)
        {
            //create new user obj
            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                AccountCreationDate = DateTime.Now,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            //check if user role doesn't exist in db, then create it
            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));

            //if role exists, give role to user
            if (await _roleManager.RoleExistsAsync(Roles.User))
                await _userManager.AddToRoleAsync(user, Roles.User);

            //create user in database
            return result;
        }

        public async Task<IdentityResult> RegisterEditor(SignUpDto model)
        {
            //create new user obj
            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                AccountCreationDate = DateTime.Now,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            //check if user role doesn't exist in db, then create it
            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));

            //check if editor role doesn't exist in db, then create it
            if (!await _roleManager.RoleExistsAsync(Roles.Editor))
                await _roleManager.CreateAsync(new IdentityRole(Roles.Editor));

            // Multiple roles for Editor
            await _userManager.AddToRolesAsync(user, new[] { Roles.Editor, Roles.User });

            //create user in database
            return result;
        }

        public async Task<IdentityResult> RegisterAdmin(SignUpDto model)
        {
            //create new user obj
            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                AccountCreationDate = DateTime.Now,
            };

            //create user in database
            var result = await _userManager.CreateAsync(user, model.Password);

            //check if admin role doesn't exist in db, then create it
            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));

            //check if editor role doesn't exist in db, then create it
            if (!await _roleManager.RoleExistsAsync(Roles.Editor))
                await _roleManager.CreateAsync(new IdentityRole(Roles.Editor));


            //check if user role doesn't exist in db, then create it
            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));

            // Multiple roles for admin
            await _userManager.AddToRolesAsync(user, new[] { Roles.Admin, Roles.Editor, Roles.User });

            return result;
        }
    }
}

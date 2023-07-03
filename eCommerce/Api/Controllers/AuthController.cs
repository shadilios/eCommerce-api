using eCommerce.Data;
using eCommerce.Data.Auth;
using eCommerce.Data.Roles;
using eCommerce.Models.Utils;
using eCommerce.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eCommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        //private readonly AppDbContext _context;
        private readonly IAuth _auth;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AppDbContext context, IAuth auth)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            //_context = context;
            _auth = auth;
        }

        #region Register

        /// <summary>
        /// Register a normal user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] SignUpDto model)
        {
            //check username exists
            var userExists = await _auth.UserExists(model);
            if (userExists)
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User already exists!" });

            var result = await _auth.RegisterUser(model);

            //if error fails, throw error
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


        /// <summary>
        /// Register an admin user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("registerAdmin")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> RegisterAdmin([FromBody] SignUpDto model)
        {
            //check username exists
            var userExists = await _auth.UserExists(model);
            if (userExists)
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User already exists!" });
            var result = await _auth.RegisterAdmin(model);

            //if error fails, throw error
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        /// <summary>
        /// Register an editor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("registerEditor")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> RegisterEditor([FromBody] SignUpDto model)
        {
            //check username exists
            var userExists = await _auth.UserExists(model);
            if (userExists)
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User already exists!" });

            var result = await _auth.RegisterEditor(model);

            //if error fails, throw error
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        #endregion

        #region IDK

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var Roles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in Roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new AuthResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpiryDate = token.ValidTo,
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = Roles.ToList()

                });
            }
            return Unauthorized();
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtConfiguration:TokenSecret"]));

            var token = new JwtSecurityToken(
                //issuer: _configuration["iss"],
                //audience: _configuration["aud"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return token;
        }

        [HttpDelete]
        [Route("deleteUser")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteUser([FromBody] string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            var deleteRequest = await _userManager.DeleteAsync(user);

            if (deleteRequest.Succeeded)
            {
                return Ok("User Deleted successfully!");
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("getUsers")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Editor)]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsers()
        {
            var users = await _auth.GetUsers();

            if (users != null)
            {
                return Ok(users);
            }
            return BadRequest("No Users found");
        }

        #endregion

    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using _5thBridgeShop.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using _5thBridgeShop.Authentication;

namespace _5thBridgeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<Application> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private IConfiguration _config;

        public AuthenticationController(UserManager<Application> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _config = config;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if(user!= null && await userManager.CheckPasswordAsync(user,model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
               
                var authClaims = new List<Claim>
                {
                    new Claim("name",user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim("role", userRole));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
                   

            }
            return Unauthorized();

        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {

            var userExists = await userManager.FindByNameAsync(model.Username);
           // if (userExists != null)
            //{
           //     return StatusCode(StatusCode.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            //}
            Application user = new Application()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation Failed! please check user details  and try again." });
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully" });

        }
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Register model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
           // if (userExists != null)
            //{
               // return StatusCode(StatusCode.Status500InternalServerError, new Response { Status = "Error", Message = "user already exists!" });
            //}

            Application user = new Application()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
          
           // if (!result.Succeeded)
           //{
             //  return StatusCode(StatusCode.Status500InternalServerError, new Response { Status = "Error", Message = "User credentials failed please check again" });
            //}
            if(!await roleManager.RoleExistsAsync(UserRole.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
            }
            if(!await roleManager.RoleExistsAsync(UserRole.User))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.User));
            }
            if(await roleManager.RoleExistsAsync(UserRole.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRole.Admin);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully" });

        }
        
    }
}


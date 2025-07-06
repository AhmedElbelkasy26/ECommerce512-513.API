using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Stripe.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce512.API.Areas.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AccountsController(UserManager<ApplicationUser> userManager , IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {
            var user = new ApplicationUser
            {
                UserName = register.UserName,
                Email = register.Email

            };

            ApplicationUser? testUser = await _userManager.FindByEmailAsync(user.Email);
            if (testUser != null)
            {
                ModelStateDictionary keyValuePairs = new ModelStateDictionary();
                keyValuePairs.AddModelError("Email", "this account is already exist");
                //ModelState.AddModelError("Email", "this account is already exist...");
                return BadRequest(keyValuePairs);
            }
            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                var res = await _userManager.AddToRoleAsync(user, "Customer");
                if (res.Succeeded)
                {
                    return Ok("Done");
                }
                foreach (var item in res.Errors)
                {
                    ModelState.AddModelError(" ", item.Description);
                }
                return BadRequest(ModelState);
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(" ", item.Description);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            ApplicationUser? userByEmail = await _userManager.FindByEmailAsync(login.Account);
            ApplicationUser? userByName = await _userManager.FindByNameAsync(login.Account);

            ApplicationUser? appUser = userByEmail ?? userByName;
            if (appUser == null)
            {
                ModelState.AddModelError(" ", "Your Account doesn't exist");
                return BadRequest(ModelState);
            }

            var result = await _userManager.CheckPasswordAsync(appUser, login.Password);
            if (result)
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                List<Claim> claims = new List<Claim>()
                {
                    new Claim( ClaimTypes.NameIdentifier, appUser.Id),
                    new Claim(ClaimTypes.Name , appUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach (var item in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }

                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
                SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _config["JWT:Issuer"],
                    audience: _config["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds

                    );
                return Ok(new {Token = new JwtSecurityTokenHandler().WriteToken(token)} );
            }

            ModelState.AddModelError(" ", "the password is incorrect");

            return BadRequest(ModelState);
        }


    }
}

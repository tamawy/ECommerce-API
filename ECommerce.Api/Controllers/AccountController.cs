using ECommerce.Api.Data;
using ECommerce.Api.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<AppUser> _userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Email = userDto.Email,
                    UserName = userDto.Email,
                    PhoneNumber = userDto.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    return Ok("Success");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    claims.Add(new Claim(ClaimTypes.Email, user.Email!));
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    var securityKey = configuration["Jwt:SecurityKey"];
                    var token = new JwtSecurityToken(
                        issuer: configuration["Jwt:Issuer"],
                        audience: configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey!)),
                        SecurityAlgorithms.HmacSha256)
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    var tokenObject = new 
                    { 
                        Token = tokenString,
                        Expiration = token.ValidTo
                    };
                    return Ok(tokenObject);
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }
            return BadRequest(ModelState);
        }
    }
}

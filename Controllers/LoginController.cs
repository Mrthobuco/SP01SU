using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SP01.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SP01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private SignInManager<Account> _signInManager;
        private Sp01DbContext _dbContext;

        public LoginController(IConfiguration config, SignInManager<Account> signInManager, Sp01DbContext dbContext)
        {
            _config = config;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, false, false);
            if (!result.Succeeded) return BadRequest(new AuthenticateResponse { Successful = false, Error = "username and password are invalid" });
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, login.Username)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_config["JwtExpiryDays"]));
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: expiry,
              signingCredentials: credentials);

            return Ok(new AuthenticateResponse { Successful = true , Token = new JwtSecurityTokenHandler().WriteToken(token)});
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            // For demo: hardcoded users, replace with DB check for real apps
            if (IsValidUserCredentials(login.Username, login.Password, out string role))
            {
                var token = GenerateJwtToken(login.Username, role);
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials");
        }

        private bool IsValidUserCredentials(string username, string password, out string role)
        {
            role = string.Empty;

            // Example hardcoded users
            if (username == "admin" && password == "adminpass")
            {
                role = "Admin";
                return true;
            }
            else if (username == "user" && password == "userpass")
            {
                role = "User";
                return true;
            }

            return false;
        }

        private string GenerateJwtToken(string username, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

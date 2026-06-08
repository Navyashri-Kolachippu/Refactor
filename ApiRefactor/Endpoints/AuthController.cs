using ApiRefactor.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ApiRefactor.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            //Fake auth data replace with actual logic to validate credentials
            //any username and password will work but admin and password will only be able to write waves.

            var jwtSettings = _configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, request.Username)
            };

            // Only admin gets write permission
            if (request.Username == "admin" && request.Password == "password")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                claims.Add(new Claim("permission", "wave:write"));
            }
            else
            {
                // normal users readonly
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new AuthResponseDto
            {
                AccessToken = jwt,
                ExpiresIn = 3600
            });
        }
    }
}


using AutoMapper;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static DataAccess.DTO.AuthDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly string? _jwtkey;
        private string? _jwtIssuer;
        private string? _jwtAudience;
        private int _jwtExpiry;
        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtkey = configuration["Jwt:SecretKey"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
            _jwtExpiry = int.Parse(configuration["Jwt:ExpirationMinutes"]);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerModel)
        {
            if (registerModel == null
                 || string.IsNullOrEmpty(registerModel.FirstName)
                 || string.IsNullOrEmpty(registerModel.Email)
                 || string.IsNullOrEmpty(registerModel.Password)
                 || string.IsNullOrEmpty(registerModel.LastName))
            {
                return BadRequest("Invalid Register details");
            }

            var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
            if (existingUser != null)
            {
                return Conflict("Email already exists");
            }

            var user = new AppUser
            {
                Email = registerModel.Email,
                UserName = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            return Ok("User created successfully with role 'User'");
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Invalid username or password" });
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { success = false, message = "Invalid username or password" });
            }
            var token = GenerateJwtToken(user);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("AuthToken", token, cookieOptions);

            return Ok(new { success = true, token });
        }


        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("AuthToken", new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            return Ok(new { success = true, message = "Logged out successfully" });
        }
        private string GenerateJwtToken(AppUser user)
        {


            var userRoles = _userManager.GetRolesAsync(user).Result;
            Console.WriteLine($" User {user.Email} has roles: {string.Join(", ", userRoles)}");

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("FirstName", user.FirstName),
        new Claim("LastName", user.LastName)
    };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            Console.WriteLine($" Total Claims: {claims.Count}");

            // Kiểm tra key JWT
            if (string.IsNullOrEmpty(_jwtkey))
            {
                Console.WriteLine(" JWT Key is missing!");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            Console.WriteLine($" JWT Expiry Time (Minutes): {_jwtExpiry}");
            if (_jwtExpiry <= 0)
            {
                Console.WriteLine(" JWT Expiry time is invalid!");
            }

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddDays(_jwtExpiry),
                audience: _jwtAudience,
                issuer: _jwtIssuer,
                claims: claims,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine($" Generated Token: {tokenString}");

            return tokenString;
        }


        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            string token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { success = false, message = "No token found" });
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "Invalid token" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { success = false, message = "User not found" });
            }
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new
            {
                success = true,
                user = new
                {
                    user.Id,
                    user.Email,
                    user.UserName,
                    roles,
                }
            });
        }
    }

}

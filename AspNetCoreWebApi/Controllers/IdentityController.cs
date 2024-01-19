using AspNetCoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace AspNetCoreWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            SignInManager< ApplicationUser > signInManager,
            IConfiguration configuration,
            ILogger<IdentityController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    NormalizedUserName = model.UserName.ToUpper(),
                    NormalizedEmail = model.Email.ToUpper(),
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // User successfully registered
                    // Handle success, e.g., generate a token, send a confirmation email, or log the user in
                    return Ok(new { Message = "User registered successfully" });
                }
                else
                {
                    // Registration failed, handle errors
                    return BadRequest(new { Errors = result.Errors });
                }
            }
            else
            {
                // Model validation failed, return validation errors
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    var token = GenerateJwtToken(user);
                    return Ok(new { Token = token, Message = "User logged in successfully" });
                }
                else
                {
                    // Login failed, handle errors
                    return BadRequest(new { Message = "Invalid login attempt" });
                }
            }
            else
            {
                // Model validation failed, return validation errors
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "User logged out successfully" });
        }


        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Decode the Base64-encoded key
            var key = Convert.FromBase64String("iiEK+1ZQl4JzeS8U2LtXJ+uEDUvqIaO3OtwXW11kGlI=");
            //var key = Convert.FromBase64String(Configuration["JWT_SECRET"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.UserName)
            // Add other claims as needed
        }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [HttpPost("ReactLogin")]
        public async Task<IActionResult> ReactLogin([FromBody] LoginModel model)
        {
            _logger.LogInformation("ReactLogin called with username: {Username}", model.UserName);

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    var token = GenerateJwtToken(user);

                    _logger.LogInformation("Setting ReactAuthToken cookie for user: {Username}", model.UserName);
                    _logger.LogInformation("Token: {Token}", token);

                    // Set the JWT token in an HttpOnly cookie
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Set to true if you're using HTTPS
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(7)
                    };
                    Response.Cookies.Append("ReactAuthToken", token, cookieOptions);

                    return Ok(new { Message = "User logged in successfully" });
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt for user: {Username}", model.UserName);
                    // Login failed, handle errors
                    return BadRequest(new { Message = "Invalid login attempt" });
                }
            }
            else
            {
                _logger.LogWarning("Model validation failed for ReactLogin");
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }
        }


    }
}

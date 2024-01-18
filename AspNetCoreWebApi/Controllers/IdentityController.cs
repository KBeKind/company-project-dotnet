using AspNetCoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AspNetCoreWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            SignInManager< ApplicationUser > signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                    // User successfully logged in
                    // Handle success, e.g., generate a token, set authentication cookies, or return user data
                    return Ok(new { Message = "User logged in successfully" });
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
    }
}

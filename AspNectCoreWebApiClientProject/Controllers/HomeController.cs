using AspNectCoreWebApiClientProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;


namespace AspNectCoreWebApiClientProject.Controllers
{
    public class HomeController : Controller
    {

        private bool IsJwtTokenPresent()
        {
            var token = Request.Cookies["JWTToken"];
            return !string.IsNullOrEmpty(token);
            // Additional checks can be implemented to verify the validity of the token
        }

		private string GetUsernameFromJwtToken()
		{
			var token = Request.Cookies["JWTToken"];
			if (string.IsNullOrEmpty(token))
			{
				return null;
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
			var username = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "unique_name")?.Value;

			return username;
		}



		private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var isAuthenticated = IsJwtTokenPresent();
            ViewData["IsAuthenticated"] = isAuthenticated;

			if (isAuthenticated)
			{
				var username = GetUsernameFromJwtToken();
				ViewData["Username"] = username;
			}

			return View();
        }

        public IActionResult Privacy()
        {
            var isAuthenticated = IsJwtTokenPresent();
            ViewData["IsAuthenticated"] = isAuthenticated;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var isAuthenticated = IsJwtTokenPresent();
            ViewData["IsAuthenticated"] = isAuthenticated;

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

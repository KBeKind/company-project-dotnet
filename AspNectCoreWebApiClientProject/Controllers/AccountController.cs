

using AspNectCoreWebApiClientProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace AspNectCoreWebApiClientProject.Controllers
{

	
	public class AccountController : Controller
	{
		private readonly string apiBaseUrl = "http://localhost:5013/api/Identity";


		[HttpGet]
		public IActionResult Register()
		{
			// Return the registration view
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Register(FrontendRegistrationModel model)
		{

			if (!ModelState.IsValid)
			{
				return View(model); // Return the view with validation errors
			}


			try
			{
				using (var client = new HttpClient())
				{
					var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
					var response = await client.PostAsync($"{apiBaseUrl}/Register", content);

					if (response.IsSuccessStatusCode)
					{
						return RedirectToAction("Login"); // Redirect to login page after successful registration
					}
					else
					{
						// Read the response content for more details about the error
						var errorContent = await response.Content.ReadAsStringAsync();

						// Log the error or add it to the ModelState
						ModelState.AddModelError("", "Registration failed: " + errorContent);
					}

				}
			}
			catch (Exception ex)
			{
				// Handle exceptions
				ModelState.AddModelError("", "An error occurred: " + ex.Message);
			}

			return View(model); // Return the registration view with validation errors
		}


		[HttpGet]
		public IActionResult Login()
		{
			// Return the login view
			return View();
		}



		[HttpPost]
		public async Task<IActionResult> Login(FrontendLoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model); // Return the view with validation errors
			}

			try
			{
				using (var client = new HttpClient())
				{
					var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
					var response = await client.PostAsync($"{apiBaseUrl}/Login", content);

					if (response.IsSuccessStatusCode)
					{
						var jsonResponse = await response.Content.ReadAsStringAsync();

						if (string.IsNullOrEmpty(jsonResponse))
						{
							throw new InvalidOperationException("API response is empty.");
						}

						dynamic tokenObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

						if (tokenObject == null)
						{
							throw new InvalidOperationException("Failed to deserialize API response.");
						}

						// Assuming the token is valid and you've authenticated the user
						string token = tokenObject.token;
						if (string.IsNullOrEmpty(token))
						{
							throw new InvalidOperationException("Token is null or empty.");
						}

						// Setup claims for the authenticated user
						var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, model.UserName), // Ideally, use user's ID or email from the token payload
                    // Add more claims as needed
                };

						var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
						var authProperties = new AuthenticationProperties
						{
							// Configure additional properties as needed
						};

						// Sign in the user with the claims principal
						await HttpContext.SignInAsync(
							CookieAuthenticationDefaults.AuthenticationScheme,
							new ClaimsPrincipal(claimsIdentity),
							authProperties);

						// Optionally store the JWT token in a cookie or session if needed for API calls
						var cookieOptions = new CookieOptions
						{
							HttpOnly = true, // Make it accessible only to the server
							Secure = false, // Set to true in production when using HTTPS
							SameSite = SameSiteMode.Strict
						};
						Response.Cookies.Append("JWTToken", token, cookieOptions);

						return RedirectToAction("Index", "Home");
					}
					else
					{
						var errorContent = await response.Content.ReadAsStringAsync();
						ModelState.AddModelError("", "Login failed: " + errorContent);
					}
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "An error occurred during login: " + ex.Message);
			}

			return View(model); // Return the login view with validation errors
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout(string redirectTo = null)
		{
			try
			{
				// CLEAR AUTHENTICATION COOKIE
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

				// REMOVE JWT TOKEN
				Response.Cookies.Delete("JWTToken", new CookieOptions { Secure = false, HttpOnly = true });


				if (!string.IsNullOrEmpty(redirectTo) && redirectTo == "ReactRedirect")
				{
					return Redirect("http://localhost:3000");
				}
				else
				{
					return RedirectToAction("Index", "Home");
				}


			}
			catch (Exception ex)
			{
				// Log the exception and return an error view
				return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
		}


	}
}
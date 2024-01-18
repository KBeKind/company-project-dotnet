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
                        // Handle successful login

                        var token = await response.Content.ReadAsStringAsync();

                        var claims = new List<Claim>
                            {
                        new Claim(ClaimTypes.Name, model.UserName),
                        // Add other claims as needed
                            };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            // Configure additional properties as needed
                        };

                        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                        return RedirectToAction("Index", "Home"); // Redirect to home page after successful registration


                    }
                    else
                    {
                        // Read the response content for more details about the error
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
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Specify the scheme
                return RedirectToAction("Index", "Home"); // Redirect to the home page after logout
            }
            catch (Exception ex)
            {
                // Log the exception and return an error view
                // Consider logging the exception for debugging purposes
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }



    }
}
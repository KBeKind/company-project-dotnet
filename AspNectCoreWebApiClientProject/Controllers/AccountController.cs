using AspNectCoreWebApiClientProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
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
        public async Task<IActionResult> Login( FrontendLoginModel model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"{apiBaseUrl}/Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Handle successful login
                     
                            return RedirectToAction("Index", "Home"); // Redirect to home page after successful registration
                     

                        // You can set authentication cookies, redirect to a dashboard, or perform other actions based on your application's needs.

                    }
                    else
                    {
                        // Handle login errors
                        // You can return a view with error messages or handle it based on your application's needs.
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // You can log the exception or return an error view.
            }

            return View(model); // Return the login view with validation errors
        }


        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Perform logout logic (e.g., sign out the user)
                await HttpContext.SignOutAsync(); // Sign out the user

                // Redirect to a specific page or action after successful logout
                return RedirectToAction("Index", "Home"); // Redirect to the home page, for example
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // You can log the exception or return an error view.
                return View("Error"); // Return an error view, or handle it based on your application's needs.
            }
        }



    }
}
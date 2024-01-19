using AspNectCoreWebApiClientProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AspNectCoreWebApiClientProject.Controllers
{
	//[Authorize]
	public class EmployeeController : Controller
    {
        private readonly string apiBaseUrl = "http://localhost:5013/api/Employee";



        private HttpClient CreateHttpClientWithToken()
        {
            var client = new HttpClient();
            var token = Request.Cookies["JWTToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

	
		private bool IsJwtTokenPresent()
        {
            var token = Request.Cookies["JWTToken"];
            return !string.IsNullOrEmpty(token);
            // Additional checks can be implemented to verify the validity of the token
        }


        // GET: Employees
        public async Task<ActionResult> Index()
        {
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			List<Employee> employees = new List<Employee>();
            using (var client = CreateHttpClientWithToken())

            {
                using (var response = await client.GetAsync(apiBaseUrl)) 
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    employees = JsonConvert.DeserializeObject<List<Employee>>(apiResponse);
                }
            }
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<ActionResult> Details(int id)
        {
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			var employee = new Employee();
            using (var client = CreateHttpClientWithToken())

            {
                using (var response = await client.GetAsync($"{apiBaseUrl}/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }

            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee)
        {
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			try
            {
                using (var client = CreateHttpClientWithToken())

                {
                    var content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(apiBaseUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        var errorData = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(errorResponse);

                        if (errorData != null)
                        {
                            foreach (var key in errorData.Keys)
                            {
                                foreach (var errorMessage in errorData[key])
                                {
                                    
                                    ModelState.AddModelError(key, errorMessage);
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message); 
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			var employee = new Employee();
            using (var client = CreateHttpClientWithToken())
            {
                using (var response = await client.GetAsync($"{apiBaseUrl}/{id}")) 
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }

            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Employee employee)
        {
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			try
            {
                using (var client = CreateHttpClientWithToken())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                    var response = await client.PutAsync($"{apiBaseUrl}/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        var errorData = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(errorResponse);

                        if (errorData != null)
                        {
                            foreach (var key in errorData.Keys)
                            {
                                foreach (var errorMessage in errorData[key])
                                {
                                   
                                    ModelState.AddModelError(key, errorMessage);
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			var employee = new Employee();
            using (var client = CreateHttpClientWithToken())
            {
                using (var response = await client.GetAsync($"{apiBaseUrl}/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(apiResponse);
                }
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			try
            {
                using (var client = CreateHttpClientWithToken())
                {
                    var response = await client.DeleteAsync($"{apiBaseUrl}/{id}"); 

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
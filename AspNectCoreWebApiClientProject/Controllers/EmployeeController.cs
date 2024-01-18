using AspNectCoreWebApiClientProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AspNectCoreWebApiClientProject.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        private readonly string apiBaseUrl = "http://localhost:5013/api/Employee"; 

        // GET: Employees
        public async Task<ActionResult> Index()
        {
            List<Employee> employees = new List<Employee>();
            using (var client = new HttpClient())
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
            var employee = new Employee();
            using (var client = new HttpClient())
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee)
        {
            try
            {
                using (var client = new HttpClient())
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
            var employee = new Employee();
            using (var client = new HttpClient())
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
            try
            {
                using (var client = new HttpClient())
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
            var employee = new Employee();
            using (var client = new HttpClient())
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
            try
            {
                using (var client = new HttpClient())
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
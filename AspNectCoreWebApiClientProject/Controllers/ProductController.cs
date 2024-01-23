
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
	[Authorize]
	public class ProductController : Controller
	{
		private readonly string apiBaseUrl = "http://localhost:5013/api/Product";



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



		// GET: Product
		public async Task<ActionResult> Index()
		{
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			List<Product> products = new List<Product>();
			using (var client = CreateHttpClientWithToken())
			{
				using (var response = await client.GetAsync(apiBaseUrl))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
				}
			}
			return View(products);
		}

		// GET: Product/Details/5
		public async Task<ActionResult> Details(int id)
		{
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			var product = new Product();
			using (var client = CreateHttpClientWithToken())
			{
				using (var response = await client.GetAsync($"{apiBaseUrl}/{id}")) // Use the updated API base URL
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					product = JsonConvert.DeserializeObject<Product>(apiResponse);
				}
			}

			return View(product);
		}

		// GET: Product/Create
		public ActionResult Create()
		{
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(Product product)
		{
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			try
			{
				using (var client = CreateHttpClientWithToken())
				{
					var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
					var response = await client.PostAsync(apiBaseUrl, content); // Use the updated API base URL

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
									// Assuming the key is "" for model level errors
									// Adjust if your API returns a different key for model level errors
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
				ModelState.AddModelError(string.Empty, ex.Message); // Log the exception (optional)
			}
			return View(product);
		}

		// GET: Product/Edit/5
		public async Task<ActionResult> Edit(int id)
		{
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			var product = new Product();
			using (var client = CreateHttpClientWithToken())
			{
				using (var response = await client.GetAsync($"{apiBaseUrl}/{id}")) // Use the updated API base URL
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					product = JsonConvert.DeserializeObject<Product>(apiResponse);
				}
			}

			return View(product);
		}

		// POST: Product/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int id, Product product)
		{
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			try
			{
				using (var client = CreateHttpClientWithToken())
				{
					var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
					var response = await client.PutAsync($"{apiBaseUrl}/update/{id}", content); // Use the updated API base URL

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
									// Assuming the key is "" for model level errors
									// Adjust if your API returns a different key for model level errors
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
				ModelState.AddModelError(string.Empty, ex.Message); // Log the exception (optional)
			}
			return View(product);
		}

		// GET: Product/Delete/5
		public async Task<ActionResult> Delete(int id)
		{
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			var product = new Product();
			using (var client = CreateHttpClientWithToken())
			{
				using (var response = await client.GetAsync($"{apiBaseUrl}/{id}")) // Use the updated API base URL
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					product = JsonConvert.DeserializeObject<Product>(apiResponse);
				}
			}

			return View(product);
		}

		// POST: Product/Delete/5
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
					var response = await client.DeleteAsync($"{apiBaseUrl}/delete/{id}"); // Use the updated API base URL

					if (response.IsSuccessStatusCode)
					{
						return RedirectToAction(nameof(Index));
					}
				}
			}
			catch
			{
				// Log the exception (optional)
			}
			return RedirectToAction(nameof(Index));
		}


		[HttpGet]
		public async Task<ActionResult> Search(float? price)
		{
			var isAuthenticated = IsJwtTokenPresent();
			ViewData["IsAuthenticated"] = isAuthenticated;

			try
			{
				List<Product> products = new List<Product>();
				using (var client = CreateHttpClientWithToken())
				{
					using (var response = await client.GetAsync($"{apiBaseUrl}/price/search?price={price}"))
					{
						string apiResponse = await response.Content.ReadAsStringAsync();
						products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
					}
				}
				return View("Search", products);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View("Search", new List<Product>()); // Return an empty list in case of errors
		}



	}
}

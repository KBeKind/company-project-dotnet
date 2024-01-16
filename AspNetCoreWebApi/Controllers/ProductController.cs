using AspNetCoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public Product GetProduct(int id)
        {

            return _context.Products.Find(id);

        }


        [HttpPut("update/{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors or ID mismatch error
            }

            try
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("price/search")] // Custom route for searching products by price
        public IEnumerable<Product> SearchProductsByPrice([FromQuery] float price)
        {
            // Custom route logic to search for products by price
            // You can use the 'price' parameter to filter products
            // Replace with your actual logic

            var lowerBound = price - 10;
            var upperBound = price + 10;

            var productsInRange = _context.Products.Where(p => p.Price >= lowerBound && p.Price <= upperBound).ToList();

            return productsInRange;
        }


    }
}

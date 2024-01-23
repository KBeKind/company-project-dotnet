using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

		// PUT
		[HttpPut("update/{id}")]

		public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
			if (id != employee.Id || !ModelState.IsValid)
			{
				return BadRequest(ModelState); 
			}

			try
			{
				_context.Employees.Update(employee);
				_context.SaveChanges();
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			};
        }

        // POST
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>>AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

		// DELETE: api/Employees/5
		[HttpDelete("delete/{id}")]
		public IActionResult DeleteEmployee(int id)
		{
			var employee = _context.Employees.Find(id);
			if (employee == null)
			{
				return NotFound();
			}

			try
			{
				_context.Employees.Remove(employee);
				_context.SaveChanges();
				return Ok();
			}
			catch (Exception ex)
			{
				// Log the exception
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}




	}
}

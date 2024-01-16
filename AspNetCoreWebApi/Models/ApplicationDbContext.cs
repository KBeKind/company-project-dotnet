using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApi.Models
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
    }

   }


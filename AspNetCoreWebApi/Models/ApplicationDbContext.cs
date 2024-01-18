using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApi.Models
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<ApplicationUser> Users { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{ }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Remove unwanted properties from the IdentityUser entity
			builder.Entity<ApplicationUser>().Ignore(u => u.PhoneNumber);
			builder.Entity<ApplicationUser>().Ignore(u => u.PhoneNumberConfirmed);
			builder.Entity<ApplicationUser>().Ignore(u => u.TwoFactorEnabled);
			builder.Entity<ApplicationUser>().Ignore(u => u.EmailConfirmed);
			builder.Entity<ApplicationUser>().Ignore(u => u.AccessFailedCount);
		}


	}

}


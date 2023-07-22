using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data.Identity
{
	public class AppIdentityDbContext : IdentityDbContext<AppUser>
	{
		public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
		{

		}

		public DbSet<Gender> Genders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Gender>().ToTable(nameof(Gender));

			modelBuilder.Entity<Gender>().HasData(
				new Gender { Id = 1, Name = "Female" },
				new Gender { Id = 2, Name = "Male" });

			base.OnModelCreating(modelBuilder);
		}
	}
}

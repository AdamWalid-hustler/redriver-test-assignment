using RedRiverTest.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace RedRiverTest.Api.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
		public DbSet<Book> Books => Set<Book>();
		public DbSet<Citation> Citations => Set<Citation>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Book>().Property(b => b.Id).ValueGeneratedOnAdd();
           modelBuilder.Entity<Citation>().Property(c => c.Id).ValueGeneratedOnAdd();
		}
	}
}

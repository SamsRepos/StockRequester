using Microsoft.EntityFrameworkCore;
using StockRequester.Models;

namespace StockRequester.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : 
            base(options)
        {
            
        }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Bulky Books" },
                new Company { Id = 2, Name = "Water Rocks" },
                new Company { Id = 3, Name = "Nice Books" }
            );
        }
    }
}

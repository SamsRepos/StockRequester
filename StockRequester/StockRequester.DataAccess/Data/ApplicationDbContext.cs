using Microsoft.EntityFrameworkCore;
using StockRequester.Models;
using System.ComponentModel.Design;

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
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Bulky Books" },
                new Company { Id = 2, Name = "Water Rocks" },
                new Company { Id = 3, Name = "Nice Books" }
            );

            modelBuilder.Entity<Location>().HasData(
                new Location 
                { 
                    Id        = 1, 
                    Name      = "Edinburgh",
                    CompanyId = 1
                }
            );
        }
    }
}

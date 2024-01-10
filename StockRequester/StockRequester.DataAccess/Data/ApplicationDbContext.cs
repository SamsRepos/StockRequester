using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using StockRequester.Models;
using StockRequester.Utility;

namespace StockRequester.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : 
            base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TransferRequest> TransferRequests { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<InvitedEmail> InvitedEmails {  get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Bulky Books" },
                new Company { Id = 2, Name = "Water Rocks" },
                new Company { Id = 3, Name = "Nice Books" }
            );

            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, Name = "Edinburgh", CompanyId = 1 },
                new Location { Id = 2, Name = "Glasgow",   CompanyId = 1}
            );

            modelBuilder.Entity<TransferRequest>().HasData(
                new TransferRequest
                {
                    Id                    = 1,
                    ItemBlob              = Item.ItemToBlob(new Item
                                            {
                                                Name = "Harry Potter",
                                                Description = "Wizarding World Book!"
                                            }),
                    Quantity              = 10,
                    CompanyId             = 1,
                    OriginLocationId      = 1,
                    DestinationLocationId = 2,
                    StatusId              = 1
                }
            );

            modelBuilder.Entity<RequestStatus>().HasData(
                new RequestStatus
                {
                    Id     = 1,
                    Status = SD.RequestStatus_Pending
                }
            );

            // dealing with multiple foreign keys delete cascade issue 
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<TransferRequest>()
                .HasOne(b => b.CreatedByUser)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            //
            base.OnModelCreating(modelBuilder);
        }
    }
}

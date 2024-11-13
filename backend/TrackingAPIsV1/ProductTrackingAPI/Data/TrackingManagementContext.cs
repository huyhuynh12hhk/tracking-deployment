using Microsoft.EntityFrameworkCore;
using ProductTrackingAPI.Constants;
using ProductTrackingAPI.Models;
using ProductTrackingAPI.Utils;

namespace ProductTrackingAPI.Data
{
    public class TrackingManagementContext : DbContext
    {
        public TrackingManagementContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserDetail> DetailUsers { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<ClaimDetail> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserClaim>().HasKey(table => new {
                table.UserId,
                table.ClaimId
            });

            var claims = new ClaimDetail[]
            {
                new()
                {
                    Key = UserClaimTypes.role.ToString(),
                    Value = "Admin",

                },
                new()
                {
                    Key = UserClaimTypes.role.ToString(),
                    Value = "Supporter",

                },

                new()
                {
                    Key = UserClaimTypes.role.ToString(),
                    Value = "Supplier",

                },
                new()
                {
                    
                    Key = UserClaimTypes.role.ToString(),
                    Value = "Member",

                }
            };

            modelBuilder.Entity<ClaimDetail>().HasData(claims);


            var user = new UserDetail
            {
                Email = "admin01@gmail.com",
                FullName = "Admin System 01",
                Description = "This is root admin account of this system",

            };
            modelBuilder.Entity<UserDetail>().HasData(user);


            modelBuilder.Entity<UserAccount>().HasData(new UserAccount[]
            {
                new()
                {
                    UserId = user.Id,
                    IsConfirmed = true,
                    Password = AppPasswordHasher.HashPassword("@123456"),
                    Provider = ProviderCatalogs.None.ToString(),
                    AccountType = AccountTypes.Admin.ToString(),
                    Key = user.Email,

                }
            });

            modelBuilder.Entity<UserClaim>().HasData(new UserClaim[]
            {
                new()
                {
                   ClaimId = claims[0].Id,
                   UserId = user.Id
                }
            }) ;
        }
    }
}

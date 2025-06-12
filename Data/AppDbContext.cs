using EcommercePlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EcommercePlatform.Data
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<User>()
             .HasOne(p => p.AppUser)
             .WithOne(u => u.User)
             .HasForeignKey<User>(p => p.UserId);

            builder.Entity<Seller>()
            .HasOne(p => p.AppUser)
            .WithOne(u => u.Seller)
            .HasForeignKey<Seller>(p => p.UserId);


            builder.Entity<Reviews>()
             .HasOne(r => r.User)
             .WithMany(u => u.Reviews)
             .HasForeignKey(r => r.UserId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CartItem>()
                .HasOne(c => c.RecentCart)
                .WithMany(r => r.CartItems)
                .HasForeignKey(c => c.RecentCartId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.RecentCart)
                .WithMany(r => r.Orders)
                .HasForeignKey(o => o.RecentCartId)
                .OnDelete(DeleteBehavior.NoAction);




            var userRoleId = "7f1962a2-5e71-49c1-a6fd-b1c6c76bffa0";
            var sellerRoleId = "7f807728-f45c-4c45-a998-78f03013affb";
            var moderatorRoleId = "289ed434-6436-420b-8753-84addf50bc9a";

            var roles = new List<IdentityRole> {
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper(),
                },
                new IdentityRole {
                    Id = sellerRoleId,
                    ConcurrencyStamp = sellerRoleId,
                    Name = "Seller",
                    NormalizedName = "Seller".ToUpper(),
                },
                new IdentityRole
                {
                    Id = moderatorRoleId,
                    ConcurrencyStamp = moderatorRoleId,
                    Name = "Moderator",
                    NormalizedName = "Moderator"
                }

            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<RecentCart> RecentCarts { get; set; }

        public DbSet<Reviews> Reviews { get; set; }

        public DbSet<Seller> Sellers { get; set; }

        public DbSet<User> Users { get; set; }
    }
}

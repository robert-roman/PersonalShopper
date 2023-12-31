﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PersonalShopper.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace PersonalShopper.DAL
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrdersProducts { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(e => e.Cart)
                .WithOne(e => e.User)
                .HasForeignKey<Cart>(e => e.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(user => user.UserOrders)
                .WithOne(order => order.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartProduct>()
                .HasOne(cartProduct => cartProduct.Cart)
                .WithMany(cart => cart.CartProducts)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CartProduct>()
                .HasOne(cartProduct => cartProduct.Product)
                .WithMany(product => product.CartProducts)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CartProduct>()
                .HasKey(cp => new { cp.CartId, cp.ProductId });

            modelBuilder.Entity<Order>()
                .HasMany(order => order.OrderProducts);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });


            modelBuilder.Entity<UserRole>(ur =>
            {
                ur.HasKey(ur => new { ur.UserId, ur.RoleId });

                ur.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
                ur.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId);
            });

            modelBuilder.Entity<CartProduct>(cp =>
            {
                cp.HasKey(cp => new { cp.CartId, cp.ProductId });
            });
        }
    }
}

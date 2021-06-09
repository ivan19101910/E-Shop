﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PracticeWebApp.Models;

namespace PracticeWebApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            
        }

        public DbSet<PracticeWebApp.Models.Cart> Cart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasMany(c => c.Cart)
                .WithMany(s => s.Product)
                .UsingEntity <CartProduct> (
                   j => j
                    .HasOne(pt => pt.Cart)
                    .WithMany(t => t.CartProduct)
                    .HasForeignKey(pt => pt.CartId),
                j => j
                    .HasOne(pt => pt.Product)
                    .WithMany(p => p.CartProduct)
                    .HasForeignKey(pt => pt.ProductId),
                j =>
                {
                    j.HasKey(t => new { t.ProductId, t.CartId });
                    j.ToTable("CartProduct");
                }
            );
            modelBuilder.Entity<UserRole>().HasData(
            new UserRole[]
            {
                new UserRole { Id=1, Name="Адміністратор"},
                new UserRole { Id=2, Name="Покупець"}
            });
            modelBuilder.Entity<User>().HasData(
            new User[]
            {
                new User {Id=1, FirstName="Tom", LastName="Moriarty", Address = "Vul pushkina dom kolotushkina", DateOfBirth = DateTime.Now, 
                    PhoneNumber = "+38096758", Email="test@gmail.com", Password="1234", UserRoleId = 1},
                new User {Id=2, FirstName="Alex", LastName="Svytshch", Address = "Zubrivska 19", DateOfBirth = DateTime.Now,
                    PhoneNumber = "+38096758",Email="test2@gmail.com", Password="12345", UserRoleId = 2}
            });

            modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory[]
            {
                new ProductCategory{ Id=1, Name="Електроніка"},
                new ProductCategory { Id=2, Name="Підвіска та рульове"},
                new ProductCategory { Id=3, Name="Коробка передач та трансмісія"}
            });

            modelBuilder.Entity<ProductSubcategory>().HasData(
            new ProductSubcategory[]
            {
                new ProductSubcategory{ Id=1, CategoryId = 1, Name="Акустичні системи"},
                new ProductSubcategory{ Id=2, CategoryId = 1, Name="Парковочні системи"},
                new ProductSubcategory{ Id=3, CategoryId = 2, Name="Підвіска"},
                new ProductSubcategory{ Id=4, CategoryId = 2, Name="Рульове"},
                new ProductSubcategory{ Id=5, CategoryId = 3, Name="Зчеплення"},
                new ProductSubcategory{ Id=6, CategoryId = 3, Name="Запчастини трансмісії"},
            });


        }

        public DbSet<PracticeWebApp.Models.ProductSubcategory> ProductSubcategory { get; set; }
    }
    
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PracticeWebApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSubcategory> ProductSubcategories { get; set; }
        //public DbSet<ProductSubcategory> SubcategoryCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SubcategoryCategory> SubcategoryCategories { get; set; }

        public DbSet<PostService> PostServices { get; set; }

        public List<ProductCategory> GetAllCategories()
        {
            return new List<ProductCategory>
                (ProductCategories
                .Include(x => x.ProductSubcategories)
                .ThenInclude(x => x.SubcategoryCategories));
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasMany(c => c.Users)
                .WithMany(s => s.Products)
                .UsingEntity<CartProduct>(
                   j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.CartProduct)
                    .HasForeignKey(pt => pt.UserId),
                j => j
                    .HasOne(pt => pt.Product)
                    .WithMany(p => p.CartProducts)
                    .HasForeignKey(pt => pt.ProductId),
                j =>
                {
                    j.HasKey(t => new { t.ProductId, t.UserId });
                    j.ToTable("CartProduct");
                }
            );

            modelBuilder
                .Entity<Order>()
                .HasMany(c => c.Products)
                .WithMany(s => s.Orders)
                .UsingEntity <OrderProduct> (
                   j => j
                    .HasOne(pt => pt.Product)
                    .WithMany(t => t.OrderProducts)
                    .HasForeignKey(pt => pt.ProductId),
                j => j
                    .HasOne(pt => pt.Order)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(pt => pt.OrderId),
                j =>
                {
                    j.HasKey(t => new { t.ProductId, t.OrderId });
                    j.ToTable("OrderProduct");
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
                new User {Id=1, FirstName="Tom", LastName="Moriarty", Address = "Степана Бандери 28а", DateOfBirth = DateTime.Now,
                    PhoneNumber = "+38096758", Email="test@gmail.com", Password="1234", UserRoleId = 1},
                new User {Id=2, FirstName="Alex", LastName="Svytshch", Address = "Zubrivska 19", DateOfBirth = DateTime.Now,
                    PhoneNumber = "+38096758",Email="test2@gmail.com", Password="12345", UserRoleId = 2}
            });

            modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory[]
            {
                new ProductCategory{ Id=1, Name="Запчастини для ТО"},
                new ProductCategory { Id=2, Name="Тормозна система"},
                new ProductCategory { Id=3, Name="Коробка передач та трансмісія"}
            });

            modelBuilder.Entity<ProductSubcategory>().HasData(
            new ProductSubcategory[]
            {
                new ProductSubcategory{ Id=1, CategoryId = 1, Name="Фільтри"},
                new ProductSubcategory{ Id=2, CategoryId = 1, Name="Автомобільні масла"},
                new ProductSubcategory{ Id=3, CategoryId = 2, Name="Гальмівні елементи"},
                new ProductSubcategory{ Id=4, CategoryId = 2, Name="Гідравліка гальмівної системи"},
                new ProductSubcategory{ Id=5, CategoryId = 3, Name="Зчеплення"},
                new ProductSubcategory{ Id=6, CategoryId = 3, Name="Запчастини трансмісії"},
            });

            modelBuilder.Entity<SubcategoryCategory>().HasData(
            new SubcategoryCategory[]
            {
                new SubcategoryCategory{ Id=1, ProductSubcategoryId = 1, Name="Повітряний фільтр"},
                new SubcategoryCategory{ Id=2, ProductSubcategoryId = 1, Name="Масляний фільтр"},
                new SubcategoryCategory{ Id=3, ProductSubcategoryId = 2, Name="Моторне масло"},
                new SubcategoryCategory{ Id=4, ProductSubcategoryId = 2, Name="Трансмісійне масло"},
                new SubcategoryCategory{ Id=5, ProductSubcategoryId = 3, Name="Гальмівні колодки"},
                new SubcategoryCategory{ Id=6, ProductSubcategoryId = 3, Name="Гальмівні диски"},
                new SubcategoryCategory{ Id=7, ProductSubcategoryId = 4, Name="Головний гальмівний циліндр"},
                new SubcategoryCategory{ Id=8, ProductSubcategoryId = 4, Name="Підсилювач гальма"},
                new SubcategoryCategory{ Id=9, ProductSubcategoryId = 5, Name="Вижимний підшипник"},
                new SubcategoryCategory{ Id=10, ProductSubcategoryId = 5, Name="Диск зчеплення"},
                new SubcategoryCategory{ Id=11, ProductSubcategoryId = 6, Name="Підшипник напівосі"},
                new SubcategoryCategory{ Id=12, ProductSubcategoryId = 6, Name="ШРУС"},
            });

            byte[] bytes = { 0, 0, 0, 25 };
            //ImageConverter imageConverter = new ImageConverter();
            //byte[] xByte = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));

            modelBuilder.Entity<Product>().HasData(
            new Product[]
            {
                new Product{ Id=1,Image = bytes, Price= 1001,Name="Повітряний фільтр 1",Description="Description", SubcategoryCategoryId = 1},
                new Product{ Id=2,Image = bytes, Price= 1002,Name="Повітряний фільтр 2",Description="Description", SubcategoryCategoryId = 1},
                new Product{ Id=3,Image = bytes, Price= 1003,Name="Масляний фільтр 1",Description="Description", SubcategoryCategoryId = 2},
                new Product{ Id=4,Image = bytes, Price= 1004,Name="Масляний фільтр 2",Description="Description", SubcategoryCategoryId = 2},
                new Product{ Id=5,Image = bytes, Price= 1005,Name="Моторне масло 1",Description="Description", SubcategoryCategoryId = 3},
                new Product{ Id=6,Image = bytes, Price= 1006,Name="Моторне масло 2",Description="Description", SubcategoryCategoryId = 3},
            });

            modelBuilder.Entity<OrderStatus>().HasData(
            new OrderStatus[]
            {
                new OrderStatus{ Id=1,Name="Нове замовлення"},
                new OrderStatus{ Id=2,Name="Комплектується"},
                new OrderStatus{ Id=3,Name="Відправлено"},
                new OrderStatus{ Id=4,Name="Отримано"},
            });

            modelBuilder.Entity<PostService>().HasData(
            new PostService[]
            {
                new PostService{ Id=1,Name="Нова пошта"},
            });
        }
    }
}

using Microsoft.EntityFrameworkCore;
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
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //public DbSet<Order> OrderProducts { get; set; }
        public DbSet<ProductSubcategory> ProductSubcategory { get; set; }

        public DbSet<PostService> PostServices { get; set; }
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

            //modelBuilder
            //    .Entity<Comment>()
            //    .HasMany(c => c.Replies)
            //    .WithMany(s => s.Comments)
            //    .UsingEntity<CommentReply>(
            //       j => j
            //        .HasOne(pt => pt.Reply)
            //        .WithMany(t => t.CommentReplies)
            //        .HasForeignKey(pt => pt.ReplyId),
            //    j => j
            //        .HasOne(pt => pt.Comment)
            //        .WithMany(p => p.CommentReplies)
            //        .HasForeignKey(pt => pt.CommentId),
            //    j =>
            //    {
            //        j.HasKey(t => new { t.ReplyId, t.CommentId });
            //        j.ToTable("CommentReply");
            //    }
            //);
            //modelBuilder.Entity<Reply>(entity =>
            //{
            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.Replies)
            //        .HasForeignKey(d => d.UserId);

            //    entity.HasOne(d => d.Comment)
            //        .WithMany(p => p.Replies)
            //        .HasForeignKey(d => d.CommentId);
            //});
            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasOne(d => d.Status)
            //        .WithMany(p => p.Orders)
            //        .HasForeignKey(d => d.StatusId)
            //        .OnDelete(DeleteBehavior.ClientSetNull);
            //    //.HasConstraintName("Appointment_AppointmentStatus_FK");
            //});
            //modelBuilder.Entity<AppointmentService>(entity =>
            //{
            //    entity.HasOne(d => d.Appointment)
            //        .WithMany(p => p.AppointmentServices)
            //        .HasForeignKey(d => d.AppointmentId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("AppointmentService_Appointment_FK");

            //    entity.HasOne(d => d.Service)
            //        .WithMany(p => p.AppointmentServices)
            //        .HasForeignKey(d => d.ServiceId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("AppointmentService_Service_FK");
            //}

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

            byte[] bytes = { 0, 0, 0, 25 };

            modelBuilder.Entity<Product>().HasData(
            new Product[]
            {
                new Product{ Id=1,Image = bytes, Price= 1003,Name="test1",Description="Description", ProductSubcategoryId = 1},
                new Product{ Id=2,Image = bytes, Price= 1003,Name="test2",Description="Description", ProductSubcategoryId = 1},
                new Product{ Id=3,Image = bytes, Price= 1003,Name="test3",Description="Description", ProductSubcategoryId = 2},
                new Product{ Id=4,Image = bytes, Price= 1003,Name="test4",Description="Description", ProductSubcategoryId = 2},
                new Product{ Id=5,Image = bytes, Price= 1003,Name="test5",Description="Description", ProductSubcategoryId = 3},
                new Product{ Id=6,Image = bytes, Price= 1003,Name="test6",Description="Description", ProductSubcategoryId = 3},
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

            modelBuilder.Entity<Comment>().HasData(
            new Comment[]
            {
                new Comment{ Id=1,Text="Комент комент комент комент", ProductId = 1, UserId = 1},
                new Comment{ Id=2,Text="Комент2 комент2 комент2 комент2", ProductId = 1, UserId = 2},
            });
            //modelBuilder.Entity<Order>().HasData(
            //new Order[]
            //{
            //    new Order{ Id=1,UserId = 1, ProductId = 1, Amount = 1, Total = 123, Description = "", StatusId = 1},
            //    new Order{ Id=2,UserId = 1, ProductId = 1, Amount = 1, Total = 123, Description = "", StatusId = 1},
            //});

        }

        
    }
    
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        [DisplayName("Зображення")]
        public byte[] Image { get; set; }
        [DisplayName("Ціна")]
        public decimal Price { get; set; }
        [DisplayName("Товар")]
        public string Name { get; set; }
        [DisplayName("Опис")]
        public string Description { get; set; }
        [DisplayName("Підкатегорія товару")]
        public int SubcategoryCategoryId { get; set; } 
        [DisplayName("Підкатегорія товару")]
        public virtual SubcategoryCategory SubcategoryCategory { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public virtual ICollection<Comment> Comments { get; set; }
    }
}

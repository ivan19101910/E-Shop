﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class Product
    {
        public Product()
        {
            //AppointmentServices = new HashSet<AppointmentService>();
        }

        public int Id { get; set; }
        [DisplayName("Ціна")]
        public decimal Price { get; set; }
        [DisplayName("Товар")]
        public string Name { get; set; }
        [DisplayName("Опис")]
        public string Description { get; set; }
        [DisplayName("Категорія товару")]
        public int ProductCategoryId { get; set; }

        [DisplayName("Категорія товару")]
        public virtual ProductCategory ProductCategory { get; set; }

        public List<Cart> Cart { get; set; } = new List<Cart>();
        public List<CartProduct> CartProduct { get; set; } = new List<CartProduct>();
        //public virtual ICollection<AppointmentService> AppointmentServices { get; set; }
    }
}

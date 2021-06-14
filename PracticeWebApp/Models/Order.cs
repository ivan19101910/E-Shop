using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        //[HiddenInput]
        //[ReadOnly(true)]
        public int UserId { get; set; }
        [DisplayName("Сума замовлення")]
        public decimal Total { get; set; }

        public string Description { get; set; }
        [DisplayName("Місто")]
        public string City { get; set; }
        [DisplayName("Спосіб доставки")]
        public int PostServiceId { get; set; }
        [DisplayName("Номер та адреса відділення")]
        public string PostDepartmentAddress { get; set; }

        public int StatusId { get; set; } = 1;
        public virtual User User { get; set; }
        public virtual OrderStatus Status { get; set; }

        public virtual PostService PostService { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}

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
        [DisplayName("Номер замовлення")]
        public int Id { get; set; }
        [DisplayName("Користувач")]
        public int UserId { get; set; }
        [DisplayName("Сума замовлення")]
        public decimal Total { get; set; }
        [DisplayName("Опис")]
        public string Description { get; set; }
        [DisplayName("Місто")]
        public string City { get; set; }
        [DisplayName("Спосіб доставки")]
        public int PostServiceId { get; set; }
        [DisplayName("Номер та адреса відділення")]
        public string PostDepartmentAddress { get; set; }
        [DisplayName("Статус")]
        public int StatusId { get; set; } = 1;
        [DisplayName("Дата та час створення")]
        public DateTime CreatedDateTime { get; set; }
        [DisplayName("Користувач")]
        public virtual User User { get; set; }
        [DisplayName("Статус")]
        public virtual OrderStatus Status { get; set; }
        [DisplayName("Спосіб доставки")]
        public virtual PostService PostService { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();       
        
    }
}

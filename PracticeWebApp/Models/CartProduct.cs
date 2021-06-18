using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class CartProduct
    {
        [DisplayName("Користувач")]
        public int UserId { get; set; }
        [DisplayName("Товар")]
        public int ProductId { get; set; }
        [DisplayName("Кількість")]
        public int Amount { get; set; } = 0;
        [DisplayName("Користувач")]
        public virtual User User { get; set; }
        [DisplayName("Товар")]
        public virtual Product Product { get; set; }
    }
}

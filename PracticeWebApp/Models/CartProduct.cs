using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class CartProduct
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Amount { get; set; } = 0;

        public virtual User User { get; set; }

        public virtual Product Product { get; set; }
    }
}

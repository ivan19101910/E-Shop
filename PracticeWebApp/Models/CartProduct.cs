using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class CartProduct
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Amount { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual Product Product { get; set; }
    }
}

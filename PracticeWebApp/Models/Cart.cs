using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class Cart
    {
        public Cart()
        {
            //AppointmentServices = new HashSet<AppointmentService>();
        }

        public int Id { get; set; }

        public List<Product> Product { get; set; } = new List<Product>();
        public List<CartProduct> CartProduct { get; set; } = new List<CartProduct>();

        //public virtual User User { get; set; }
    }
}

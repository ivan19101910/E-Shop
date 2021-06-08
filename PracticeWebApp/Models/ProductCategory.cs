using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class ProductCategory
    {
        public ProductCategory()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        [DisplayName("Тип послуги")]
        public string Name { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}

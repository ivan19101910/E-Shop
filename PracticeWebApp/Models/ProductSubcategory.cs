using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class ProductSubcategory
    {
        public ProductSubcategory()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        [DisplayName("Підкатегорія")]
        public string Name { get; set; }

        [DisplayName("Категорія товару")]
        public int CategoryId { get; set; }
        [DisplayName("Категорія товару")]
        public virtual ProductCategory Category { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}

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
            ProductSubcategories = new HashSet<ProductSubcategory>();
        }
        public int Id { get; set; }
        [DisplayName("Категорія")]
        public string Name { get; set; }
        public virtual ICollection<ProductSubcategory> ProductSubcategories { get; set; }
    }
}

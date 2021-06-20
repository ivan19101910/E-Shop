using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class SubcategoryCategory
    {
        public int Id { get; set; }
        [DisplayName("Назва")]
        public string Name { get; set; }
        public int ProductSubcategoryId { get; set; }
        public virtual ProductSubcategory ProductSubcategory { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}

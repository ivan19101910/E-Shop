using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class PostService
    {
        public int Id { get; set; }
        [DisplayName("Назва")]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}

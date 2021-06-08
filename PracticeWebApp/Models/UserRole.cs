using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        [DisplayName("Тип користувача")]
        public string Name { get; set; }
    }
}

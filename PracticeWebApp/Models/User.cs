using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class User
    {

        public User()
        {
            //Appointments = new HashSet<Appointment>();
        }
        public int Id { get; set; }

        [DisplayName("Ім'я")]
        public string FirstName { get; set; }
        [DisplayName("Прізвище")]
        public string LastName { get; set; }
        [DisplayName("Адреса")]
        public string Address { get; set; }
        [DisplayName("Дата народження")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Номер телефону")]
        public string PhoneNumber { get; set; }
        [DisplayName("id Тип користувача")]
        public int UserRoleId { get; set; }

        [DisplayName("Тип користувача")]
        public virtual UserRole UserRole{ get; set; }

        //public virtual ICollection<Appointment> Appointments { get; set; }
    }
}

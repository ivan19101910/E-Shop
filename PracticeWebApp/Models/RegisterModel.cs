using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не вказаний Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Не вказаний пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введено невірно")]
        public string ConfirmPassword { get; set; }
    }
}

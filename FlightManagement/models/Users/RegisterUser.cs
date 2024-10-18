using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.models.Users
{
    public class RegisterUser
    {

        [Required(ErrorMessage = "Not specified surname")]
        [MaxLength(30, ErrorMessage = "Max length: 30 characters")]
        public string Surname { get; set; }


        [Required(ErrorMessage = "Not specified name")]
        [MaxLength(30, ErrorMessage = "Max length: 30 characters")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Not specified middle name")]
        [MaxLength(30, ErrorMessage = "Max length: 30 characters")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "Not specified user name")]
        [MaxLength(20, ErrorMessage = "Max length: 20 characters")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Not specified email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Not specified password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Not confirmed password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        //Поле для создания админа
        public bool IsAdmin { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.models.Users
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Not specified login")]
        [MaxLength(20, ErrorMessage = "Max length: 20 characters")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Not specified password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

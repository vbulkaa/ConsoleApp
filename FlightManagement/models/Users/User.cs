using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FlightManagement.DAL.models.Users
{
    public class User : IdentityUser
    {
        public string Surname { get; set; } // Добавлено
        public string Name { get; set; }     // Добавлено
        public string MiddleName { get; set; } // Добавлено
        public bool IsAdmin { get; set; }
    }
}

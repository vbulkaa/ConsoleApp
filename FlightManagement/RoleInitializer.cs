using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            string roleName = "Admin";
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}

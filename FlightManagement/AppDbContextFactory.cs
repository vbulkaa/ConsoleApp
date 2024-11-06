using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-CADBM5B\\MSSQLEXPRESS;Database=FlightManagement;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}


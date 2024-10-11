using FlightManagement.DAL.Configuration;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Airports> Airports { get; set; }
        public DbSet<Flights> Flights { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<Statuses> Statuses { get; set; }
        public DbSet<Stops> Stops { get; set; }

        //Применение конфигураций
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AirportConfig());
            modelBuilder.ApplyConfiguration(new FlightConfig());
            modelBuilder.ApplyConfiguration(new RouteConfig());
            modelBuilder.ApplyConfiguration(new StatusConfig());
            modelBuilder.ApplyConfiguration(new StopConfig());
        }
    }
}

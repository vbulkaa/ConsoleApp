using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.data
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airports>()
                .HasKey(a => a.AirportID);

            modelBuilder.Entity<Routes>()
                 .HasKey(r => r.RouteID);

            modelBuilder.Entity<Statuses>()
                .HasKey(r => r.StatusID);

            modelBuilder.Entity<Stops>()
                .HasKey(r => r.StopID);

            modelBuilder.Entity<Flights>()
                .HasKey(r => r.FlightID);


        }
    }
}

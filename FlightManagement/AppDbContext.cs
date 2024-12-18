﻿using FlightManagement.DAL.Configuration;
using FlightManagement.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using FlightManagement.DAL.models.Users;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;

namespace FlightManagement.DAL
{

    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {
        }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightManagement.models.Route> Routes { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<User> Users { get; set; }

        //Применение конфигураций
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);
             modelBuilder.ApplyConfiguration(new AirportConfig());
             modelBuilder.ApplyConfiguration(new FlightConfig());
             modelBuilder.ApplyConfiguration(new RouteConfig());
             modelBuilder.ApplyConfiguration(new StatusConfig());
             modelBuilder.ApplyConfiguration(new StopConfig());

            modelBuilder.Entity<FlightManagement.models.Route>()
            .HasMany(r => r.Stops)
            .WithOne(s => s.Route)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

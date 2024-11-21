using FlightManagement.models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Configuration
{
    public class StopConfig : IEntityTypeConfiguration<Stop>
    {
        public void Configure(EntityTypeBuilder<Stop> builder)
        {
            builder.HasKey(s => s.StopID); // Настройка первичного ключа
            builder.Property(s => s.ArrivalTime).IsRequired(); 
            builder.Property(s => s.DepartureTime).IsRequired(); 

            builder.HasOne(s => s.Route)
                .WithMany(r => r.Stops) // Настройка отношения
                .HasForeignKey(s => s.RouteID);
            builder.HasOne(s => s.Airport)
                .WithMany()
                .HasForeignKey(s => s.AirportID);
            builder.HasOne(s => s.Status)
                .WithMany()
                .HasForeignKey(s => s.StatusID);
        }
    }
}

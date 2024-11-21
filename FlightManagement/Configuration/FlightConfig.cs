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
    public class FlightConfig : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.HasKey(f => f.FlightID); // Настройка первичного ключа
            builder.Property(f => f.FlightNumber).IsRequired(); 
            builder.Property(f => f.AircraftType).IsRequired(); 
            builder.Property(f => f.TicketPrice)
                .HasColumnType("decimal(18,2)") // Указание типа данных
                .IsRequired() 
                .HasDefaultValue(0);

            builder.HasMany(f => f.Routes)
                .WithOne(r => r.Flight) // Настройка отношения один-ко-многим
                .HasForeignKey(r => r.FlightID);
        }
    }
}

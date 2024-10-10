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
    public class FlightConfig : IEntityTypeConfiguration<Flights>
    {
        public void Configure(EntityTypeBuilder<Flights> builder)
        {
            builder.HasKey(f => f.FlightID); // Настройка первичного ключа
            builder.Property(f => f.FlightNumber).IsRequired(); // Обязательное поле
            builder.Property(f => f.AircraftType).IsRequired(); // Обязательное поле
            builder.Property(f => f.TicketPrice)
                .HasColumnType("decimal(18,2)") // Указание типа данных
                .IsRequired() // Обязательное поле
                .HasDefaultValue(0); // Значение по умолчанию

            builder.HasMany(f => f.Routes)
                .WithOne(r => r.Flight) // Настройка отношения один-ко-многим
                .HasForeignKey(r => r.FlightID);
        }
    }
}

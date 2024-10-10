﻿using FlightManagement.models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Configuration
{
    public class StopConfig : IEntityTypeConfiguration<Stops>
    {
        public void Configure(EntityTypeBuilder<Stops> builder)
        {
            builder.HasKey(s => s.StopID); // Настройка первичного ключа
            builder.Property(s => s.ArrivalTime).IsRequired(); // Обязательное поле
            builder.Property(s => s.DepartureTime).IsRequired(); // Обязательное поле

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

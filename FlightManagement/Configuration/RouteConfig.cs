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
    public class RouteConfig : IEntityTypeConfiguration<Routes>
    {
        public void Configure(EntityTypeBuilder<Routes> builder)
        {
            builder.HasKey(r => r.RouteID); // Настройка первичного ключа
            builder.Property(r => r.DepartureTime).IsRequired(); // Обязательное поле
            builder.Property(r => r.Date).IsRequired(); // Обязательное поле

            builder.HasOne(r => r.Flight)
                .WithMany(f => f.Routes) // Настройка отношения
                .HasForeignKey(r => r.FlightID);
        }
    }
}

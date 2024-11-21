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
    public class AirportConfig : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            builder.HasKey(a => a.AirportID); 
            builder.Property(a => a.Name).IsRequired(); 
            builder.Property(a => a.Location).IsRequired(); 
        }
    }
}

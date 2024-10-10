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
    public class StatusConfig : IEntityTypeConfiguration<Statuses>
    {
        public void Configure(EntityTypeBuilder<Statuses> builder)
        {
            builder.HasKey(s => s.StatusID); // Настройка первичного ключа
            builder.Property(s => s.StatusName).IsRequired(); // Обязательное поле
        }
    }
}
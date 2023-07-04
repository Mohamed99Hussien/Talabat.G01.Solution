﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
  //   because warning during make Migration
        public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
        {
            public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
            {
                builder.Property(dMethod => dMethod.Cost)
                     .HasColumnType("int");
            }

        }
}
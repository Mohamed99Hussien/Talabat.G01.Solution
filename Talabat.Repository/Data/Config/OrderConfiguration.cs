using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // NP => Navgation Proprety
            builder.OwnsOne(O => O.ShippingAddress, NP => NP.WithOwner());// every ShippingAddress Convert to componentAddress during send to database

            builder.Property(O => O.Status)
                .HasConversion(
                
                OStatus => OStatus.ToString(),     // store in DataBase
                OStatus => (OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus) // return from DataBase

                );

          //  builder.HasOne(O => O.DeliveryMethod).WithOne();//every order has one DeliveryMethod

            builder.HasMany(O => O.Items).WithOne().OnDelete(DeleteBehavior.Cascade); // when Delete Order => Delete Item

        }
    }
}

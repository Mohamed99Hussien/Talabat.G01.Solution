﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class DeliveryMethod:BaseEntity
    {
        public DeliveryMethod(  )
        {
        }

        public DeliveryMethod(string shortName, string description, int cost, string deliveryTime)
        {
            ShortName = shortName;
            Description = description;
            Cost = cost;
            DeliveryTime = deliveryTime;
        }

        public string ShortName { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public string DeliveryTime { get; set; }
    }
}

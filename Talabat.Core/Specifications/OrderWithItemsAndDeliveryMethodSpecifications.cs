using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications
{
    public class OrderWithItemsAndDeliveryMethodSpecifications :BaseSpecification<Order>
    {
        // This Constructor is Used for get all the orders for a specific user 
        public OrderWithItemsAndDeliveryMethodSpecifications(string buyerEmail)  // take buyerEmail because I want filter order
            : base(O => O.BuyerEmail == buyerEmail)
        {
            // work => eager loading 
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            AddOrderByDecending(O => O.OrderDate);
        }

        // This Constructor is Used for get a specific order for a specific user 
        public OrderWithItemsAndDeliveryMethodSpecifications(int orderId,string buyerEmail)  // take buyerEmail because I want filter order
            : base(O => O.BuyerEmail == buyerEmail && O.Id == orderId)
        {
            // work => eager loading 
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            
        }
    }
}

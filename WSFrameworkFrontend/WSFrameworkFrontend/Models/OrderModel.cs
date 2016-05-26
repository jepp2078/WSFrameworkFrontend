using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class OrderModel
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public int Status { get; set; }
        public long ShopId { get; set; }
        public double Amount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class OrderModel
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        [Display(Name = "Shipping Addres")]
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        [Display(Name = "Order Created")]
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        [Display(Name = "Order Status")]
        public int Status { get; set; }
        public long ShopId { get; set; }
        [Display(Name = "Total Price")]
        public double Amount { get; set; }
    }
}
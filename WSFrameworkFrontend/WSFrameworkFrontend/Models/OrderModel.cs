using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class OrderModel
    {
        [Display(Name = "Order ID")]
        public long Id { get; set; }
        [Display(Name = "Customer ID")]
        public long CustomerId { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "Order Created")]
        public System.DateTime CreatedAt { get; set; }
        [Display(Name = "Order Updated")]
        public System.DateTime UpdatedAt { get; set; }
        [Display(Name = "Order Status")]
        public int Status { get; set; }
        public long ShopId { get; set; }
        [Display(Name = "Total Price")]
        public double Amount { get; set; }
    }
}
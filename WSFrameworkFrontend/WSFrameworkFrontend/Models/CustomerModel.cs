using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class CustomerModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public string Address { get; set; }
        [Display(Name = "Zip Code")]
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public long ShopId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class UserModel
    {
        [Display(Name = "User ID")]
        public string Id { get; set; }
        public string Email { get; set; }
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}
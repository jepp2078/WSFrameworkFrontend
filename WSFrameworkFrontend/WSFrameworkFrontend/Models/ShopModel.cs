using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class ShopModel
    {
        [Display(Name = "Shop ID")]
        public long Id { get; set; }
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Description")]
        public string DescriptionFull { get; set; }
        [Display(Name = "Webshop Views")]
        public Nullable<long> Views { get; set; }
        [Display(Name = "Webshop Visibility")]
        public Nullable<int> IsActive { get; set; }
        [Display(Name = "Created at")]
        public Nullable<System.DateTime> CreatedAt { get; set; }
        [Display(Name = "Updated at")]
        public Nullable<System.DateTime> UpdatedAt { get; set; }
    }
}
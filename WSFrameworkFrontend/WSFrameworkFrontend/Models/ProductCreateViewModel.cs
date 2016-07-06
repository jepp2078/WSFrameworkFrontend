using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class ProductCreateViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Product Name")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Description")]
        public string DescriptionFull { get; set; }
        [Display(Name = "Product Views")]
        public long Views { get; set; }
        [Display(Name = "Product Visibility")]
        public int IsActive { get; set; }
        [Display(Name = "Created at")]
        public System.DateTime CreatedAt { get; set; }
        [Display(Name = "Updated at")]
        public System.DateTime UpdatedAt { get; set; }
        [Display(Name = "Shop ID")]
        public long ShopId { get; set; }
        [Display(Name = "Product Stock")]
        public int Stock { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Product Price")]
        public Nullable<double> Price { get; set; }
        public ImageModel Image { get; set; }
        public List<long> Category { get; set; }
        [Display(Name = "Category")]
        public List<string> CategoryName { get; set; }
    }
}
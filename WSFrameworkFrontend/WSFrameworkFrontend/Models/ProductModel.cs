using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class ProductModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionFull { get; set; }
        public long Views { get; set; }
        public int IsActive { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public long ShopId { get; set; }
        public int Stock { get; set; }
        public Nullable<double> Price { get; set; }
    }
}
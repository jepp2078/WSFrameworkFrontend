using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class ImageModel
    {
        public long Id { get; set; }
        public Nullable<long> ProductId { get; set; }
        public string ImageUrl { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class ProductFullModel: ProductModel
    {
        public IList<ImageModel> Images { get; set; }
        public IList<CategoryModel> Categories { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class ShopViewModel : ShopModel
    {
        public IList<ProductViewModel> Products { get; set; }
        public ShopConfigurationModel Configuration { get; set; }
    }
}
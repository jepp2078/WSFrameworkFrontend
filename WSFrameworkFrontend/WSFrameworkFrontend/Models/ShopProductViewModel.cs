using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class ShopProductViewModel : ProductViewModel
    {
        public ShopConfigurationModel Configuration { get; set; }
    }
}
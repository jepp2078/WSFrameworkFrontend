using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class ShopConfigurationModel
    {
        public long ShopId { get; set; }
        [Display(Name = "Background Color")]
        public string BgColor { get; set; }
        [Display(Name = "Menu Color")]
        public string MenuColor { get; set; }
        [Display(Name = "Menu Text Color")]
        public string MenuTextColor { get; set; }
        [Display(Name = "Layout")]
        public Nullable<int> LayoutId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class OrderProductsFull : ProductModel
    {
        public long Quantity { get; set; }
        public IList<ImageModel> Images { get; set; }
        public IList<CategoryModel> Categories { get; set; }
    }

    public class OrderProducts : ProductModel
    {
        public long Quantity { get; set; }
        public ImageModel Image { get; set; }
        public List<long> Category { get; set; }
        public List<string> CategoryName { get; set; }
    }

    public class OrderProductViewModel : OrderModel
    {
        public IList<OrderProducts> Products { get; set; }
    }
}
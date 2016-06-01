using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Models
{
    public class HttpResponseModel
    {
        [Display(Name = "Reason")]
        public string ReasonMessage { get; set; }
    }
}
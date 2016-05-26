using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSFrameworkFrontend.Helpers
{
    public static class UriHelper
    {
        private static readonly string uri = "http://localhost:54454";

        public static string getUri()
        {
            return uri;
        }
    }
}
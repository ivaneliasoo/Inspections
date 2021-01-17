using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ReportsApp
{
    public static class Config
    {
        public static string ReportsApi = "http://inspectionsapi-dev.eba-r84ntzqp.us-east-2.elasticbeanstalk.com";

        public static string ApiHostName
        {
            get
            {
                var apiHostName = Regex.Replace(ReportsApi, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase)
                                   .Replace("/", string.Empty);
                return apiHostName;
            }
        }
    }
}

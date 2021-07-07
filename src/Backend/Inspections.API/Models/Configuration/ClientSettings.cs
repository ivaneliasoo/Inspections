using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inspections.API.Models.Configuration
{
    public class ClientSettings
    {
        public string ReportsImagesFolder { get; set; } = "MediaUpload/img/Reports";
        public string S3BucketName { get; set; } = default!;
    }
}

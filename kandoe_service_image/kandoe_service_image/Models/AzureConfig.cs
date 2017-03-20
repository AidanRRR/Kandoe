using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kandoe_service_image.Models
{
    public class AzureConfig
    {
        public string BlobConnectionString { get; set; }
        public string ContainerReference { get; set; }
    }
}

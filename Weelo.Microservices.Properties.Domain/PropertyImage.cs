using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weelo.Microservices.Properties.Domain
{
    public class PropertyImage
    {
        public Guid PropertyImageId { get; set; }

        public Guid PropertyId { get; set; }

        public string FilePath { get; set; }

        public bool Enabled { get; set; }

        public Property Property { get; set; }
    }
}

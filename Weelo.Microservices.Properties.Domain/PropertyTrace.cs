using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weelo.Microservices.Properties.Domain
{
    public class PropertyTrace
    {
        public Guid PropertyTraceId { get; set; }

        public Guid PropertyId { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal Tax { get; set; }

        public DateTime DateSale { get; set; }

        public Property Property { get; set; }
    }
}

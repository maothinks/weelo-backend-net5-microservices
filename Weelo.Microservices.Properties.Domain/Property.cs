using System;
using System.Collections.Generic;

namespace Weelo.Microservices.Properties.Domain
{
    public class Property
    {
        public Guid PropertyId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public decimal Price { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        public int OwnerId { get; set; }

        public int Views { get; set; }

        public string CoverPath { get; set; }

        public List<PropertyTrace> PropertyTraces { get; set; }

        public List<PropertyImage> PropertyImages { get; set; }

        //public object Owner { get; set; }
    }
}

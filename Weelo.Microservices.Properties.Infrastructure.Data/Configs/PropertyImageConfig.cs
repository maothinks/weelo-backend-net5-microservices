using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weelo.Microservices.Properties.Domain;

namespace Weelo.Microservices.Properties.Infrastructure.Data.Configs
{
    /// <summary>
    /// Define a new Property Images table in the Database
    /// </summary>
    class PropertyImageConfig : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.ToTable("PropertyImages");
            builder.HasKey(c => c.PropertyImageId);
        }
    }
}

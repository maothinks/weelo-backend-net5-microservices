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
    /// Define a new Property View table in the Database
    /// </summary>
    class PropertyViewConfig : IEntityTypeConfiguration<PropertyView>
    {
        public void Configure(EntityTypeBuilder<PropertyView> builder)
        {
            builder.ToTable("PropertyViews");
            builder.HasKey(c => c.PropertyViewId);
        }
    }
}

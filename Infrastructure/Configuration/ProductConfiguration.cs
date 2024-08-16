
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration
{
    // Class to configure entities (this class inherits the Interface from EF with type PRODUCT, so class used to config entities of PRODUCT class)
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            // Specify number of decimals our price will have - 10 digits with 2 decimal points (removes warning from EF when creating migrations)
            builder.Property(pri => pri.Price).HasColumnType("decimal(10,2)");
            //builder.Property(x => x.Name).IsRequired(); <- Can do this to be more specific for EACH entity 
            // TODO: Specify max length to what is needed (save space in DB -> More efficient!)

        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingApp.Modules.Products.Core.DomainModels;

namespace ShoppingApp.Modules.Products.Infrastructure.Postgres.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products);
        }
    }
}

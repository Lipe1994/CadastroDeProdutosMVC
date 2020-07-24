using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Data.Mappings
{
    public class AddressMappings : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(a => a.PublicPlace)
                .IsRequired()
                .HasColumnType("varchar(150)");
            
            builder.Property(a => a.complement)
                .IsRequired()
                .HasColumnType("varchar(150)");
            
            builder.Property(a => a.ZipCode)
                .IsRequired()
                .HasColumnType("varchar(10)");
            
            builder.Property(a => a.Neighborhood)
                .IsRequired()
                .HasColumnType("varchar(100)");
            
            builder.Property(a => a.City)
                .IsRequired()
                .HasColumnType("varchar(150)");
            
            builder.Property(a => a.State)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.HasOne(a => a.Provider)
                .WithOne(f => f.Address);
        }
    }


    public class ProviderMappings : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(p => p.Document)
                .IsRequired()
                .HasColumnType("varchar(25)");

            builder.HasMany(p => p.Products)
                .WithOne(prod => prod.Provider)
                .HasForeignKey(prod => prod.ProviderId);

            builder.HasOne(p => p.Address)
                .WithOne(a => a.Provider);
        }
    }


    public class ProductMappings : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(p => p.Image)
                .IsRequired()
                .HasColumnType("varchar(250)");
            

            builder.HasOne(prod => prod.Provider)
                .WithMany(p => p.Products);
        }
    }
}

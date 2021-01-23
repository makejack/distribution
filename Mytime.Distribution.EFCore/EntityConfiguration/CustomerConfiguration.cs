using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasIndex(e => e.OpenId).IsUnique();
            builder.HasMany(e => e.Childrens).WithOne(e => e.Parent).HasForeignKey(e => e.ParentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
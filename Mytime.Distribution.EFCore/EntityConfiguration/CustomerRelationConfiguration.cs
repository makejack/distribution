using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class CustomerRelationConfiguration : IEntityTypeConfiguration<CustomerRelation>
    {
        public void Configure(EntityTypeBuilder<CustomerRelation> builder)
        {
            builder.HasOne(e => e.Children).WithMany(e => e.CustomerRelationChildrens).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
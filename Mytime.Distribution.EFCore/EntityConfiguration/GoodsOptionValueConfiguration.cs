using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class GoodsOptionValueConfiguration : IEntityTypeConfiguration<GoodsOptionValue>
    {
        public void Configure(EntityTypeBuilder<GoodsOptionValue> builder)
        {
            builder.HasOne(e => e.Option).WithMany(e => e.GoodsOptionValues).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
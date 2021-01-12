using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class GoodsOptionCombinationConfiguration : IEntityTypeConfiguration<GoodsOptionCombination>
    {
        public void Configure(EntityTypeBuilder<GoodsOptionCombination> builder)
        {
            builder.HasOne(e => e.Option).WithMany(e => e.GoodsOptionCombinations).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
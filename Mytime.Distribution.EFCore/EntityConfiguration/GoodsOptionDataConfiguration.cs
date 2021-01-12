using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class GoodsOptionDataConfiguration : IEntityTypeConfiguration<GoodsOptionData>
    {
        public void Configure(EntityTypeBuilder<GoodsOptionData> builder)
        {
            builder.HasOne(e => e.GoodsOption).WithMany(e => e.GoodsOptionDatas).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
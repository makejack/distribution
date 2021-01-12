using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class GoodsMediaConfiguration : IEntityTypeConfiguration<GoodsMedia>
    {
        public void Configure(EntityTypeBuilder<GoodsMedia> builder)
        {
            builder.HasKey(e => new { e.GoodsId, e.MediaId });
        }
    }
}
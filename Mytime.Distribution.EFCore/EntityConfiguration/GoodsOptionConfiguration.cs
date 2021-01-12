using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class GoodsOptionConfiguration : IEntityTypeConfiguration<GoodsOption>
    {
        public void Configure(EntityTypeBuilder<GoodsOption> builder)
        {
            
        }
    }
}
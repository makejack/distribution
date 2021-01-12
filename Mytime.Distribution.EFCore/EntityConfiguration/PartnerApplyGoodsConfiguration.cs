using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class PartnerApplyGoodsConfiguration : IEntityTypeConfiguration<PartnerApplyGoods>
    {
        public void Configure(EntityTypeBuilder<PartnerApplyGoods> builder)
        {
            
        }
    }
}
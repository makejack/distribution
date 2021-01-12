using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class PartnerApplyConfiguration : IEntityTypeConfiguration<PartnerApply>
    {
        public void Configure(EntityTypeBuilder<PartnerApply> builder)
        {
            
        }
    }
}
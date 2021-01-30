using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class ReturnApplyConfiguration : IEntityTypeConfiguration<ReturnApply>
    {
        public void Configure(EntityTypeBuilder<ReturnApply> builder)
        {
            
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class OrderBillingConfiguration : IEntityTypeConfiguration<OrderBilling>
    {
        public void Configure(EntityTypeBuilder<OrderBilling> builder)
        {
            
        }
    }
}
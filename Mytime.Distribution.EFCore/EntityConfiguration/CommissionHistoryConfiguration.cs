using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class CommissionHistoryConfiguration : IEntityTypeConfiguration<CommissionHistory>
    {
        public void Configure(EntityTypeBuilder<CommissionHistory> builder)
        {
            builder.HasOne(e => e.OrderItem).WithOne(e => e.CommissionHistory).HasForeignKey<CommissionHistory>(e => e.OrderItemId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
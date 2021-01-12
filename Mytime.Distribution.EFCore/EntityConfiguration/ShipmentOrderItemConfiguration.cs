using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class ShipmentOrderItemConfiguration : IEntityTypeConfiguration<ShipmentOrderItem>
    {
        public void Configure(EntityTypeBuilder<ShipmentOrderItem> builder)
        {
            builder.HasKey(e => new { e.OrderItemId, e.ShipmentId });
        }
    }
}
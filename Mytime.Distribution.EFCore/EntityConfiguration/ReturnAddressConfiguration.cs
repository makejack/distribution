using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class ReturnAddressConfiguration : IEntityTypeConfiguration<ReturnAddress>
    {
        public void Configure(EntityTypeBuilder<ReturnAddress> builder)
        {
            
        }
    }
}
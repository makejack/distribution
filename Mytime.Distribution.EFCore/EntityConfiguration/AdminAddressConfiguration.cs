using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    
    public class AdminAddressConfiguration : IEntityTypeConfiguration<AdminAddress>
    {
        public void Configure(EntityTypeBuilder<AdminAddress> builder)
        {
            
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class SmsLogConfiguration : IEntityTypeConfiguration<SmsLog>
    {
        public void Configure(EntityTypeBuilder<SmsLog> builder)
        {
            
        }
    }
}
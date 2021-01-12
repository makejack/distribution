using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class RequestResponseLogConfiguration : IEntityTypeConfiguration<RequestResponseLog>
    {
        public void Configure(EntityTypeBuilder<RequestResponseLog> builder)
        {
            
        }
    }
}
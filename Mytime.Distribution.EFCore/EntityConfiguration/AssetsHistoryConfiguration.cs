using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class AssetsHistoryConfiguration : IEntityTypeConfiguration<AssetsHistory>
    {
        public void Configure(EntityTypeBuilder<AssetsHistory> builder)
        {
            
        }
    }
}
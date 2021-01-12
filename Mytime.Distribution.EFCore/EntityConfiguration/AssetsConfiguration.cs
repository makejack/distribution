using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class AssetsConfiguration : IEntityTypeConfiguration<Assets>
    {
        public void Configure(EntityTypeBuilder<Assets> builder)
        {
            
        }
    }
}
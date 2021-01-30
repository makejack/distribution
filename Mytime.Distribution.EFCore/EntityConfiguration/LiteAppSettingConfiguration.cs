using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class LiteAppSettingConfiguration : IEntityTypeConfiguration<LiteAppSetting>
    {
        public void Configure(EntityTypeBuilder<LiteAppSetting> builder)
        {
            
        }
    }
}
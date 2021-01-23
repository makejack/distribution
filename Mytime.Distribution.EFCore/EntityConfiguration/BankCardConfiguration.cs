using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class BankCardConfiguration : IEntityTypeConfiguration<BankCard>
    {
        public void Configure(EntityTypeBuilder<BankCard> builder)
        {
            
        }
    }
}
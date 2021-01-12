using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.EFCore.EntityConfiguration
{
    public class WithdrawalHistoryConfiguration : IEntityTypeConfiguration<WithdrawalHistory>
    {
        public void Configure(EntityTypeBuilder<WithdrawalHistory> builder)
        {
            
        }
    }
}
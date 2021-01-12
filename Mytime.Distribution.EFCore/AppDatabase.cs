using System;
using Microsoft.EntityFrameworkCore;

namespace Mytime.Distribution.EFCore
{
    public class AppDatabase : DbContext
    {
        public AppDatabase(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDatabase).Assembly);
        }
    }
}

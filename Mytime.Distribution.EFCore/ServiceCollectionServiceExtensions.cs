using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mytime.Distribution.EFCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddEFCoreUseSqlServer(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<AppDatabase>(o =>
            {
                o.UseSqlServer(connectionString);
            });
        }

        public static IServiceCollection AddEFCoreUseMySql(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<AppDatabase>(o =>
            {
                o.UseMySql(connectionString);
            });
        }

        public static IServiceCollection AddEFCore(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            return services.AddDbContext<AppDatabase>(options);
        }
    }
}
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;

namespace Mytime.Distribution.EFCore.Repositories
{
    public class RepositoryByInt<TEntity> : Repository<TEntity, int>, IRepositoryByInt<TEntity> where TEntity : AggregateRoot
    {
        public RepositoryByInt(AppDatabase context) : base(context)
        {
        }
        
    }
}
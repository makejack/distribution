using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Mytime.Distribution.Domain.IRepositories
{
    public interface IRepository<TEntity, TKey> : IRepository<TEntity> where TEntity : class
    {        
        Task<TEntity> FirstOrDefaultAsync(TKey key);
    }

    public interface IRepository<TEntity> where TEntity : class
    {
        IDbContextTransaction BeginTransaction();
        int Insert(TEntity entity, bool isSave = true);
        Task<int> InsertAsync(TEntity entity, bool isSave = true);
        IQueryable<TEntity> Query();
        int Remove(TEntity entity, bool isSave = true);
        Task<int> RemoveAsync(TEntity entity);
        int Save();
        Task<int> SaveAsync();
        int Update(TEntity entity, bool isSave = true);
        Task<int> UpdateAsync(TEntity entity);
        int UpdatePropery(TEntity entity, params string[] propertys);
        Task<int> UpdateProperyAsync(TEntity entity, params string[] propertys);
        int UpdateRange(IEnumerable<TEntity> entities);
        Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities);
    }
}
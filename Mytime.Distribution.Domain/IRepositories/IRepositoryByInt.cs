using System.Threading.Tasks;
using Mytime.Distribution.Domain.Entities;

namespace Mytime.Distribution.Domain.IRepositories
{
    public interface IRepositoryByInt<TEntity> : IRepository<TEntity, int> where TEntity : AggregateRoot
    {

    }
}
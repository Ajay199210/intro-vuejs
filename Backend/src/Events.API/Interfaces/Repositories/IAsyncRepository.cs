using Events.API.Data.Models;
using System.Linq.Expressions;

namespace Events.API.Interfaces.Repositories
{
    public interface IAsyncRepository<TBaseEntity> where TBaseEntity : BaseEntity
    {
        Task<TBaseEntity> GetByIdAsync(int id);
        Task<IEnumerable<TBaseEntity>> ListAsync();
        Task<IEnumerable<TBaseEntity>> ListAsync(Expression<Func<TBaseEntity, bool>> predicate);
        Task AddAsync(TBaseEntity entity);
        Task DeleteAsync(TBaseEntity entity);
        Task EditAsync(TBaseEntity entity);
    }
}

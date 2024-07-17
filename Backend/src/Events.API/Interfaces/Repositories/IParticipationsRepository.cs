using Events.API.Data.Models;
using System.Linq.Expressions;

namespace Events.API.Interfaces.Repositories
{
    public interface IParticipationsRepository
    {
        Task<Participation> GetByIdAsync(int id);
        Task<IEnumerable<Participation>> ListAsync();
        Task<IEnumerable<Participation>> ListAsync(Expression<Func<Participation, bool>> predicate);
        Task AddAsync(Participation participation);
        Task DeleteAsync(Participation participation);
        Task EditAsync(Participation participation);
    }
}

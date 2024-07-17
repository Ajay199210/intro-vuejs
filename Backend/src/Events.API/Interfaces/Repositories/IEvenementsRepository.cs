using Events.API.Data.Models;
using System.Linq.Expressions;

namespace Events.API.Interfaces.Repositories
{
    public interface IEvenementsRepository
    {
        Task<Evenement> GetByIdAsync(int id);
        Task<IEnumerable<Evenement>> ListAsync();
        Task<IEnumerable<Evenement>> ListAsync(Expression<Func<Evenement, bool>> predicate);
        Task AddAsync(Evenement evenement);
        Task DeleteAsync(Evenement evenement);
        Task EditAsync(Evenement evenement);

        public Task<decimal> GetTotaleVentesEvenement(int id);
    }
}

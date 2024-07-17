using Events.API.Data.Models;
using Events.API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Events.API.Data.Repositories
{
    public class ParticipationsRepository : IParticipationsRepository
    {
        protected readonly EventsDbContext _dbContext;

        public ParticipationsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Participation entity)
        {
            await _dbContext.Participations.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Participation entity)
        {
            _dbContext.Participations.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(Participation entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Participation> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Participation>()
                .Include(p => p.Evenement)
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Participation>> ListAsync()
        {
            return await _dbContext.Set<Participation>()
                .Include(p => p.Evenement)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Participation>> ListAsync(Expression<Func<Participation, bool>> predicate)
        {
            return await _dbContext.Set<Participation>()
                .Where(predicate)
                .ToListAsync();
        }
    }
}

using Events.API.Data.Models;
using Events.API.Exceptions;
using Events.API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Events.API.Data.Repositories
{
    public class EvenementsRespository : IEvenementsRepository
    {
        protected readonly EventsDbContext _dbContext;

        public EvenementsRespository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Evenement entity)
        {
            await _dbContext.Evenements.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Evenement entity)
        {
            _dbContext.Evenements.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(Evenement entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Evenement> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Evenement>()
                .AsNoTracking()
                .Include(e => e.Categories)
                .Include(e => e.Participations)
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Evenement>> ListAsync()
        {
            return await _dbContext.Set<Evenement>()
                .Include(e => e.Ville)
                .Include(e => e.Categories)
                .Include(e => e.Participations)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Evenement>> ListAsync(Expression<Func<Evenement, bool>> predicate)
        {
            return await _dbContext.Set<Evenement>()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotaleVentesEvenement(int id)
        {
            var evenement = await GetByIdAsync(id) ?? throw new HttpException
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = new
                {
                    Errors = $"{DateTime.Now} [-] L'événement (id = {id}) n'existe pas"
                }
            };

            var participations = await _dbContext.Participations.Include(p => p.Evenement).AsNoTracking().ToListAsync();
            var participationsConcernees = participations.FindAll(p => p.Evenement.Id == id);

            var nbPlacesTotal = participationsConcernees.Sum(p => p.NbPlaces);

            return nbPlacesTotal * evenement.PrixBillet;
        }
    }
}

using Events.API.DTO;

namespace Events.API.Interfaces
{
    public interface IStatisticsBL
    {
        public Task<decimal> GetTotaleVentesEvenement(int id);
        public Task<IEnumerable<VilleStatsDTO>> GetVillesPopulaires();
        public Task<IEnumerable<EvenementDTO>> GetEvenementsRentables();
    }
}

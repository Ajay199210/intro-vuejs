using AutoMapper;
using Events.API.Data.Models;
using Events.API.DTO;

namespace Events.API.Profiles
{
    public class EventsMappingProfile : Profile
    {
        public EventsMappingProfile()
        {
            // Le mappage s'effectue dans les deux directions

            // Mappage Ville
            CreateMap<Ville, VilleDTO>();
            CreateMap<VilleDTO, Ville>();

            CreateMap<Ville, VillePostPutDTO>();
            CreateMap<VillePostPutDTO, Ville>();

            CreateMap<VilleDTO, VilleStatsDTO>();
            CreateMap<VilleStatsDTO, VilleDTO>();

            // Mappage Categorie
            CreateMap<Categorie, CategorieDTO>();
            CreateMap<CategorieDTO, Categorie>();

            CreateMap<Categorie, CategoriePostPutDTO>();
            CreateMap<CategoriePostPutDTO, Categorie>();

            CreateMap<CategorieDTO, CategoriePostPutDTO>();
            CreateMap<CategoriePostPutDTO, CategorieDTO>();

            // Mappage Evenement
            CreateMap<Evenement, EvenementDTO>()
                //.ForMember(dest => dest.CategoriesIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id).ToList()))
                .ForMember(dest => dest.ParticipationsIds, opt => opt.MapFrom(src => src.Participations.Select(p => p.Id).ToList()));
            CreateMap<EvenementDTO, Evenement>();

            CreateMap<Evenement, EvenementPostPutDTO>();
            CreateMap<EvenementPostPutDTO, Evenement>();

            // Mappage Participation
            CreateMap<Participation, ParticipationDTO>();
            CreateMap<ParticipationDTO, Participation>();

            CreateMap<Participation, ParticipationPostDTO>();
            CreateMap<ParticipationPostDTO, Participation>();

            CreateMap<ParticipationDTO, ParticipationPostDTO>();
            CreateMap<ParticipationPostDTO, ParticipationDTO>();
        }
    }
}

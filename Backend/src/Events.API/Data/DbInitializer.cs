using Events.API.Data.Enums;
using Events.API.Data.Models;

namespace Events.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(EventsDbContext context)
        {
            if (context.Villes.Any() || context.Evenements.Any() ||
                context.Participations.Any() || context.Categories.Any())
            {
                return; // DB has been seeded
            }

            // Villes
            var villes = new Ville[]
            {
                new Ville { Nom = "Amos", Region = Region.ABITIBI },
                new Ville { Nom = "La Sarre", Region = Region.ABITIBI, NbEvenements = 1 },
                new Ville { Nom = "Val-d'Or", Region = Region.ABITIBI },

                new Ville { Nom = "Baie-Johan-Beetz", Region = Region.COTE_NORD, NbEvenements = 2},
                new Ville { Nom = "Essipit", Region = Region.COTE_NORD, NbEvenements = 1},
                new Ville { Nom = "Sheldrake", Region = Region.COTE_NORD},

                new Ville { Nom = "Bedford", Region = Region.ESTRIE},
                new Ville { Nom = "Danville", Region = Region.ESTRIE},
                new Ville { Nom = "Granby", Region = Region.ESTRIE},

                new Ville { Nom = "La Tuque", Region = Region.MAURICIE, NbEvenements = 3},
                new Ville { Nom = "Louiseville", Region = Region.MAURICIE},
                new Ville { Nom = "Trois-Rivières", Region = Region.MAURICIE},

                new Ville { Nom = "Gatineau", Region = Region.OUTAOUAI},
                new Ville { Nom = "Maniwaki", Region = Region.OUTAOUAI},
                new Ville { Nom = "Thurso", Region = Region.OUTAOUAI}
            };

            context.Villes.AddRange(villes);

            // Catégories
            var categories = new Categorie[]
            {
                new Categorie { Nom = "Education" },
                new Categorie { Nom = "Technologie" },
                new Categorie { Nom = "Sport" },
                new Categorie { Nom = "Finance" },
                new Categorie { Nom = "Divertissement" },
            };

            context.Categories.AddRange(categories);

            // Événements
            var evenements = new Evenement[]
            {
                new Evenement {
                    Titre = "Événement 1", Ville = villes[1], Adresse = "Adresse XYZ",
                    NomOrganisateur = "Jhon Doe",
                    DateDebut = DateTime.Now.AddDays(3), DateFin = DateTime.Now.AddDays(5),
                    Description = "Description de l'événement 1", PrixBillet = 45.5m,
                    //Categories = new List<Categorie> { categories[1], categories[2] }
                    CategoriesIds = new List<int> { 1,2 }
                },
                new Evenement {
                    Titre = "Événement 2", Ville = villes[3], Adresse = "Adresse YZX",
                    NomOrganisateur = "Jhon Doe",
                    DateDebut = DateTime.Now.AddDays(7), DateFin = DateTime.Now.AddDays(12),
                    Description = "Description de l'événement 2", PrixBillet = 25,
                    //Categories = new List<Categorie> { categories[2], categories[4] },
                    CategoriesIds = new List<int> { 2,4 }
                },
                new Evenement {
                    Titre = "Événement 3", Ville = villes[4], Adresse = "Adresse ZXX",
                    NomOrganisateur = "Jhon Doe",
                    DateDebut = DateTime.Now.AddDays(5), DateFin = DateTime.Now.AddDays(10),
                    Description = "Description de l'événement 3", PrixBillet = 30.00m,
                    //Categories = new List<Categorie> { categories[4] }
                    CategoriesIds = new List<int> { 4 }
                },
                new Evenement {
                    Titre = "Événement 4", Ville = villes[10], Adresse = "Adresse BCA",
                    NomOrganisateur = "Jhon Doe",
                    DateDebut = DateTime.Now.AddDays(3), DateFin = DateTime.Now.AddDays(7),
                    Description = "Description de l'événement 4", PrixBillet = 30.00m,
                    //Categories = new List<Categorie> { categories[1] }
                    CategoriesIds = new List<int> { 1 }
                },
                new Evenement {
                    Titre = "Événement 5", Ville = villes[10], Adresse = "Adresse ABC",
                    NomOrganisateur = "Jhon Doe",
                    DateDebut = DateTime.Now.AddDays(1), DateFin = DateTime.Now.AddDays(5),
                    Description = "Description de l'événement 5", PrixBillet = 30.00m,
                    //Categories = new List<Categorie> { categories[0], categories[3] }
                    CategoriesIds = new List<int> { 1,3 }
                },
                new Evenement {
                    Titre = "Événement 6", Ville = villes[3], Adresse = "Adresse DFG",
                    NomOrganisateur = "Jhon Doe",
                    DateDebut = DateTime.Now.AddDays(15), DateFin = DateTime.Now.AddDays(28),
                    Description = "Description de l'événement 6", PrixBillet = 10.00m,
                    //Categories = new List<Categorie> { categories[0] }
                    CategoriesIds = new List<int> { 1 }
                },
                new Evenement {
                    Titre = "Événement 7", Ville = villes[10], Adresse = "Adresse BOF",
                    NomOrganisateur = "Jhon Doe",
                    DateDebut = DateTime.Now.AddDays(4), DateFin = DateTime.Now.AddDays(4),
                    Description = "Description de l'événement 7", PrixBillet = 14.25m,
                    //Categories = new List<Categorie> { categories[2] }
                    CategoriesIds = new List<int> { 2 }
                },
            };

            context.Evenements.AddRange(evenements);

            // Participations
            var participations = new Participation[]
            {
                new Participation
                {
                    NomParticipant = "NomP1", PrenomParticipant = "PrenomP1", AdresseCourriel = "p1@test.com",
                    NbPlaces = 2,  EstConfirmee = false, Evenement = evenements[1],
                },
                new Participation
                {
                    NomParticipant = "NomP2", PrenomParticipant = "PrenomP2", AdresseCourriel = "p2@test.com",
                    NbPlaces = 1,  EstConfirmee = false, Evenement = evenements[2],
                },
                new Participation
                {
                    NomParticipant = "NomP3", PrenomParticipant = "PrenomP3", AdresseCourriel = "p3@test.com",
                    NbPlaces = 4,  EstConfirmee = true, Evenement = evenements[0],
                },
                new Participation
                {
                    NomParticipant = "NomP4", PrenomParticipant = "PrenomP4", AdresseCourriel = "p4@test.com",
                    NbPlaces = 2,  EstConfirmee = true, Evenement = evenements[1],
                }
            };

            context.Participations.AddRange(participations);

            context.SaveChanges();
        }
    }
}

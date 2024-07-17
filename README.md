# Application de gestion des événements

Ce projet est une application full-stack qui permet aux utilisateurs de gérer des événements. Le frontend est développé en Vue.js, tandis que l'API backend est propulsée par ASP.NET Core.


## Objectifs

* Développement d’application Web UI (SPA) avec le Framework VueJS
* Installation des outils de développement et de débogage (VueCLI)
* Sécurisation d’une application web UI avec OIDC (OpenID Connect)
* Consommation d’une API web à partir d’une application Web UI
* Mise en pratique des bonnes pratiques d’accessibilités

![Home Page](https://github.com/Ajay199210/intro-vuejs/blob/main/Frontend/home.png)

## Détails

La plateforme va permettre la publication et la consultation d’événements, ainsi que la possibilité d’inscrire des participations.

Les évènements publiés doivent obligatoirement contenir une date de début et de fin, un titre, une description, au moins une catégorie, une adresse de rue, le nom de l’organisateur et la ville concerné. On peut aussi renseigner optionnellement un prix (en chiffre).

Les villes sont identifiées par leur nom et régions (prédéfinies). Les catégories sont identifiées par leur nom.

Pour participer à un évènement la personne doit renseigner son adresse courriel, son nom et prénom et le nombre de place.

Lors de l’ajout d’une participation, un processus de confirmation externe sera déclenché. La participation sera considérée comme valide que lorsque ce processus sera terminé avec succès. On doit donc pouvoir suivre l’état de la demande d’ajout à partir d’un point de terminaison dédié.

On doit pouvoir créer, modifier, lister et supprimer toutes les ressources, sauf si une contrainte s’applique.

On doit pouvoir lister facilement tous les évènements d’une ville donné, ainsi que toutes les participations pour un événement donné.

## Installation du frontend

1. Clonez le dépôt :
   ```
   git clone https://github.com/ajay199210/intro-vuejs.git
   cd intro-vuejs/Frontend/src/web2.ui
   ```
2. Installez les dépendances 
   ```
   npm install
   npm install primevue
   ```
3. Démarrez le serveur de développement 
   ```
   npm run serve
   ```

## Notes

Svp notez que pour s'authentifier avec **IdentityServer4**, les noms d'utilisateurs admin et manager peuvent être utilisés comme utilisateurs ayant différent rôles dans l'application.

| Nom d'utilisateur | Mot de passe |
|:---:|:---:|
| admin | Pass-123 |
| manager | Pass-123 |

![Login](https://github.com/Ajay199210/intro-vuejs/blob/main/Frontend/login.png)

## Contributions

Merci à [Wissem Sayari](https://github.com/WissemSayari) pour son contribution au projet !

<template>
  <div class="hello" style="width: 50%; margin: auto;">
    <h1>{{ title }}</h1>
    <img alt="Vue logo" src="../assets/logo.png">
    <hr />
    <section>
      <h2>Aperçu du projet</h2>
      <p>
        Le projet est une application de gestion des événements conçue pour faciliter l'organisation et la
        participation à divers événements. Que ce soit pour des conférences, des ateliers, des fêtes ou tout autre type
        de rassemblement, cette application offre une interface conviviale pour les utilisateurs et les organisateurs.
      </p>
      <p>
        Les participants peuvent facilement s'inscrire, consulter les détails des événements et recevoir des
        notifications importantes. Pour les organisateurs, l'application fournit des outils robustes pour créer et gérer
        des événements, suivre les inscriptions et interagir avec les participants.
      </p>

      <hr />

      <h2>Technologies Utilisées</h2>
      <ul>
        <li>
          <b>Vue.js :</b> Un framework JavaScript progressif utilisé pour construire l'interface utilisateur de manière
          réactive
          et dynamique.
        </li>
        <li>
          <b>PrimeVue :</b> Une collection de composants d'interface utilisateur pour Vue.js, utilisée pour créer des
          éléments
          interactifs et attrayants.
        </li>
        <li>
          <b>Node.js :</b> Utilisé pour le backend de l'application, permettant de gérer les requêtes des utilisateurs
          et de
          communiquer avec la base de données.
        </li>
        <li>
          <b>PostgreSQL :</b> Utilisé pour le système de gestion de bases de données relationnelles open-source, réputé
          pour sa robustesse, sa stabilité et sa richesse en fonctionnalités.
          Conçu pour gérer des charges de travail allant des petites applications à des systèmes de données
          complexes et volumineux, PostgreSQL offre une conformité stricte aux standards SQL et des capacités
          avancées.
        </li>
        <li>
          <b>IdentityServer :</b> Utilisé pour la gestion d'authentification en utilisant les deux protocoles
          OAuth 2.0 et OpenID Connect. Peut aussi gérer les informations des utilisateurs directement ou
          s'intégrer avec des fournisseurs d'identié externes.
        </li>
      </ul>
    </section>

    <hr />

    <section>
      <h2>Présentation de l'équipe</h2>

      <div class="team-section">
        <div class="team-member">
          <h2>Dev 1</h2>
          <img id="dev1" src="../assets/profile.png" alt="Image profile dev 1" />
          <p><b>Poste :</b> Développeur Front-end</p>
          <p><b>Bio :</b> Dev 1 est un développeur front-end passionné avec plus de cinq ans d'expérience
            dans le développement web. Expert en HTML, CSS et JavaScript, il a travaillé sur divers projets allant de
            sites web corporatifs à des applications web interactives. Sa capacité à traduire les maquettes de design en
            interfaces utilisateurs fonctionnelles et esthétiques fait de lui un atout précieux pour l'équipe. Lorsqu'il
            ne code pas, il aime explorer les dernières tendances technologiques et contribuer à des projets open
            source.</p>
        </div>

        <div class="team-member">
          <h2>Dev 2</h2>
          <img src="../assets/profile.png" alt="Image profile dev2">
          <p><b>Poste :</b> Développeur Back-end</p>
          <p><b>Bio :</b> Dev 2 est un développeur back-end chevronné avec une solide expertise en
            conception de bases de données et en développement d'API. Avec une expérience de plus de sept ans, il
            maîtrise des langages tels que Python, Java et Node.js. Son rôle principal consiste à garantir la
            performance, la sécurité et l'évolutivité des applications serveur. Passionné par la résolution de problèmes
            complexes, il consacre également du temps à l'enseignement et au mentorat de jeunes développeurs. En dehors
            du travail, il aime la randonnée et la photographie de paysages.</p>
        </div>
      </div>

    </section>
  </div>
</template>

<script>
export default {
  name: 'HomeComponent',
  props: {
    title: String
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 auto 25px auto;
}

a {
  color: #42b983;
}

p, li {
  font-size: 1.15em;
}

img {
  /* float: left; */
  width: 200px;
  height: 250px;
  border-radius: 5px;
  margin: 0 5px 0 0;
}
/* #dev1 {
  width: 200px;
  height: 200px;
} */
</style>

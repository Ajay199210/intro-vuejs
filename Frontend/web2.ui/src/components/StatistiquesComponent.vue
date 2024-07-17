<template>
    <h1>Statistiques</h1>
    <!-- Villes populaires -->
    <div>
        <h2>Villes les plus populaires</h2>
        <div class="card">
            <DataTable :value="villesPopulaires" tableStyle="min-width: 50rem;">
                <Column field="nom" sortable header="Nom"></Column>
                <Column field="region" sortable header="Région"></Column>
                <Column field="nbEvenements" sortable header="Nombre d'événements"></Column>
            </DataTable>
        </div>
    </div>

    <!-- Événements rentables -->
    <div>
        <h2>Événements les plus rentables</h2>
        <div class="card">
            <DataTable :value="evenementsRentables" tableStyle="min-width: 50rem;">
                <Column field="titre" header="Titre" sortable></Column>
                <Column field="" sortable header="Ville">
                    <template #body="{ data }">
                        {{
                            this.getVilleNameById(data.villeId)
                        }}
                    </template>
                </Column>
                <Column field="participationsIds.length" header="Nombre de participations" sortable></Column>
                <Column field="prixBillet" header="Prix billet" sortable></Column>
                <Column field="dateDebut" header="Date de début" sortable></Column>
                <Column field="" header="Action" sortable>
                    <template #body="{ data }">
                        <div class="flex flex-wrap gap-2">
                            <Button v-tooltip.left="{ value: 'Consulter l\'événement', showDelay: 700, hideDelay: 300 }"
                                type="button" @click="$router.push(`/evenements/${data.id}/details`)"
                                icon="pi pi-info-circle" rounded>
                            </Button>
                        </div>
                    </template>
                </Column>
            </DataTable>
        </div>
    </div>
</template>

<script>
import { mapActions, mapState } from 'vuex';
import store from '@/store';

export default {
    name: "StatistiquesComponent",
    props: {},
    data() {
        return {
            titre: ""
        };
    },
    methods: {
        ...mapActions(['getVillesPopulairesApi', 'getEvenementsRentables']),
        loadVillesPopulaires() {
            this.getVillesPopulairesApi()
            .catch(() => {
                    this.$toast.add({
                        severity: 'error',
                        summary: 'Error Message',
                        detail: `Erreur lors de la communication avec le serveur lors lors du téléchargement des villes`,
                        // life: 3000
                    });
                });
        },
        loadEvenementsRentables() {
            this.getEvenementsRentables()
            .catch(() => {
                    this.$toast.add({
                        severity: 'error',
                        summary: 'Error Message',
                        detail: `Erreur lors de la communication avec le serveur lors du téléchargement des événements`,
                        // life: 3000
                    });
                });
        },
        getVilleNameById(id) {
            var ville = store.state.villes.find(v => v.id === id);

            return ville ? ville.nom : "S.O.";
        },
    },
    computed: {
        ...mapState({
            villesPopulaires: 'villesPopulaires',
            evenementsRentables: 'evenementsRentables'
        })
    },
    created() {
        this.loadVillesPopulaires();
        this.loadEvenementsRentables();
    },
}
</script>
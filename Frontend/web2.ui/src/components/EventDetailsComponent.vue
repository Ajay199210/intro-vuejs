<template>
    <h1>Détails de l'événement</h1>

    <table>
        <thead></thead>
        <tbody>
            <tr>
                <td><label for="title">Titre</label></td>
                <td>
                    <InputText id="title" type="text" v-model="event.titre" disabled />
                </td>
            </tr>
            <tr>
                <td><label for="city">Ville</label></td>
                <td>
                    <InputText id="city" type="text" v-model="eventCity" disabled />
                </td>
            </tr>
            <tr>

            </tr>
            <td><label for="categories">Catégories</label></td>
            <td>
                <InputText id="categories" type="text" v-model="eventCategories" disabled />
            </td>
            <tr>
                <td><label for="startDate">Date début</label></td>
                <td>
                    <InputText id="startDate" type="text" v-model="event.dateDebut" disabled />
                </td>
            </tr>
            <tr>
                <td><label for="endDate">Date de fin</label></td>
                <td>
                    <InputText id="endDate" type="text" v-model="event.dateFin" disabled />
                </td>
            </tr>
            <tr>
                <td><label for="organizer">Date de fin</label></td>
                <td>
                    <InputText id="organizer" type="text" v-model="event.nomOrganisateur" disabled />
                </td>
            </tr>
            <tr>
                <td><label for="description">Description</label></td>
                <td><Textarea id="description" type="text" v-model="event.description" disabled></Textarea></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right;">
                    <Button @click="$router.push('/evenements')">Retour</Button>
                </td>
            </tr>
        </tbody>
    </table>
</template>

<script>
import { mapActions } from 'vuex';
import store from '@/store';

export default {
    name: "EventDetailsComponents",
    props: {},
    data() {
        return {
            event: {},
            eventCity: '',
            eventCategories: ''
        };
    },
    methods: {
        ...mapActions(['getCategoriesApi', 'getEventByIdApi', 'getVillesApi']),
        getEventApi() { 
            this.getEventByIdApi(this.$route.params.id)
            .then(response => {
                    this.event = response
                    this.eventCity = this.getVilleNameById(this.event.villeId);
                    this.eventCategories = this.getCategoriesNamesByIds(this.event.categoriesIds);
                })
                .catch(() => {
                    this.$toast.add({
                        severity: 'error',
                        summary: 'Error Message',
                        detail: `Erreur lors de la communication avec le serveur lor de l'affichage de l'événement`,
                        // life: 3000
                    });
                });

        },
        loadCategories() {
            this.getCategoriesApi('/Categories')
                // .then(data => {
                //     console.log(data);
                // })
                .catch(() => {
                    this.$toast.add({
                        severity: 'error',
                        summary: 'Error Message',
                        detail: `Erreur lors de la communication avec le serveur lor de l'ajout de la participation`,
                        // life: 3000
                    });
                })
        },
        loadVilles() {
            this.getVillesApi()
                // .then(data => {
                // this.evenements = data;
                // console.log(data);
                // })
                .catch(() => this.$toast.error(
                    `Erreur lors de la communication avec 
                    le serveur lor du chargement de la liste des villes :(`
                ));
        },
        getVilleNameById(id) {
            var ville = store.state.villes.find(v => v.id === id);

            return ville ? ville.nom : "S.O.";
        },
        getCategoriesNamesByIds(ids) {
            var categoriesNames = [];
            ids.forEach(id => {
                var categorie = store.state.categories.find(c => c.id == id);
                if(categorie) {
                   categoriesNames.push(categorie.nom); 
                }
            });

            return categoriesNames.join(", ");
        }
    },
    computed: {
        // ...mapState({ events: 'events', villes: 'villes', categories: 'categories' }),
        // ...mapGetters()
    },
    created() {
        this.getEventApi();
    }
}
</script>

<style scoped>
label {
    margin-right: 10px;
}

table {
    border: 1px solid black;
    border-radius: 5px;
    padding: 10px;
}
</style>
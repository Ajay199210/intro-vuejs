<template>
    <div>
        <div class="card">
            <DataTable :value="events" tableStyle="min-width: 50rem;">
                <template #header>
                    <div class="flex justify-content-end">
                        <IconField iconPosition="left">
                            <InputIcon>
                                <i class="pi pi-search"></i>
                            </InputIcon>
                            <InputText type="text" placeholder="Filtrer sur le titre ou la description"
                                v-model="searchTerm" />
                        </IconField>
                    </div>
                </template>
                <Column field="titre" sortable header="Titre"></Column>
                <Column field="" sortable header="Ville">
                    <template #body="{ data }">
                        {{
                this.getVilleNameById(data.id)
            }}
                    </template>
                </Column>
                <Column field="participationsIds.length" header="Nb Participations"></Column>
                <Column field="" header="Catégories">
                    <template #body="{ data }">
                        {{
                this.getCategoriesNamesByIds(data.categoriesIds)
            }}
                    </template>
                </Column>
                <Column field="prixBillet" sortable header="Prix"></Column>
                <Column field="dateDebut" sortable header="Date de début"></Column>
                <Column field="" header="Actions">
                    <template #body="{ data }">
                        <div class="flex flex-wrap gap-2">
                            <Button v-tooltip.left="{ value: 'Consulter l\'événement', showDelay: 700, hideDelay: 300 }"
                                type="button" @click="$router.push(`/evenements/${data.id}/details`)"
                                icon="pi pi-info-circle" rounded>
                            </Button>
                            <Button
                                v-tooltip.top="{ value: 'Participer à l\'événement', showDelay: 700, hideDelay: 300 }"
                                type="button" icon="pi pi-pencil"
                                @click="$router.push(`/evenements/${data.id}/participer`)" rounded
                                severity="success"></Button>
                            <Button v-if="this.$oidc.userProfile.name == 'manager'"
                                v-tooltip.top="{ value: 'Supprimer l\'événement', showDelay: 700, hideDelay: 300 }"
                                @click="deleteEventApi({ id: data.id, index: index })" type="button" icon="pi pi-trash"
                                rounded severity="danger"></Button>
                        </div>
                    </template>
                </Column>
            </DataTable>
        </div>

        <div style="margin-top: 25px;">
            <Button @click="pageIndex--" :disabled="pageIndex <= 1">precedant</Button>
            <Button severity="info" style="margin: auto 5px">page {{ pageIndex }}</Button>
            <Button @click="pageIndex++" :disabled="pageIndex == pageCount">suivant</Button>
        </div>
    </div>
</template>

<script>
import { mapState, mapActions } from 'vuex'
import store from '@/store';

export default {
    name: "EventsListComponent",
    props: {},
    data() {
        return {
            event: '',
            searchTerm: '',
            pageIndex: 1,
            pageCount: 10,
        };
    },
    methods: {
        ...mapActions(['getEventsApi', 'deleteEventApi', 'getVillesApi', 'getCategoriesApi']),
        loadEvents() {
            this.getEventsApi({ searchTerm: this.searchTerm, pageIndex: this.pageIndex, pageCount: this.pageCount })
                .then(data => {
                    this.pageCount = data.pageCount;
                })
                .catch(() => {
                    this.$toast.add({
                        severity: 'error',
                        summary: 'Error Message',
                        detail: `Erreur lors de la communication avec le serveur lors du téléchargement des événements`,
                        // life: 3000
                    });
                });
        },
        loadVilles() {
            this.getVillesApi()
                // .then(data => {
                // this.evenements = data;
                // console.log(data);
                // })
                .catch(() => {
                    this.$toast.add({
                        severity: 'error',
                        summary: 'Error Message',
                        detail: `Erreur lors de la communication avec le serveur lors du téléchargement de la liste des villes`,
                        // life: 3000
                    });
                });
        },
        loadCategories() {
            this.getCategoriesApi()
                // .then(data => {
                // this.evenements = data;
                // console.log(data);
                // })
                .catch(() => {
                    this.$toast.add({
                        severity: 'error',
                        summary: 'Error Message',
                        detail: `Erreur lors de la communication avec le serveur lors du téléchargement des catégories`,
                        // life: 3000
                    });
                });
        },
        getVilleNameById(id) {
            var ville = store.state.villes.find(v => v.id === id);

            return ville ? ville.nom : "S.O.";
        },
        getCategoriesNamesByIds(ids) {
            var categoriesNames = [];
            ids.forEach(id => {
                var categorie = store.state.categories.find(c => c.id == id);
                if (categorie) {
                    categoriesNames.push(categorie.nom);
                }
            });

            return categoriesNames.join(", ");
        }
    },
    computed: {
        ...mapState({ events: 'events', villes: 'villes', categories: 'categories' }),
        // ...mapGetters()
    },
    created() {
        this.loadEvents();
        this.loadVilles();
        this.loadCategories();
    },
    watch: {
        'pageIndex'() {
            this.loadEvents()
        },
        'searchTerm'() {
            this.pageIndex = 1
            this.loadEvents()
        }
    },

}
</script>
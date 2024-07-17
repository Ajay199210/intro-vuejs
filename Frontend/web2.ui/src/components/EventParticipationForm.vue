<template>
    <h1>Participation à l'événement</h1>

    <!-- Notification -->
    <Toast position="bottom-right" />

    <form @submit.prevent="submit">
        <table>
            <thead>
            </thead>
            <tbody>
                <tr>
                    <td><label for="nom">Nom</label></td>
                    <td>
                        <InputText id="nom" type="text" v-model="nom" />
                    </td>
                </tr>
                <tr>
                    <td><label for="prenom">Prénom</label></td>
                    <td>
                        <InputText id="prenom" type="text" v-model="prenom" />
                    </td>
                </tr>
                <tr>
                    <td><label for="email">Courriel</label></td>
                    <InputText id="email" type="email" v-model="courriel" />
                </tr>
                <tr>
                    <td><label for="nbPlaces">Nombre de places</label></td>
                    <td>
                        <InputText id="nbPlaces" type="number" v-model="nbPlaces" min="1" max="10" />
                    </td>
                </tr>
                <tr>
                    <td class="button" colspan="2">
                        <Button type="submit">Soumettre</Button>
                    </td>
                </tr>
                <tr>
                    <td class="button" colspan="2">
                        <Button type="button" @click="$router.push('/evenements')">Retour</Button>
                    </td>
                </tr>
            </tbody>
        </table>
    </form>

</template>

<script>
import { mapActions } from 'vuex'

export default {
    name: "EventParticipationForm",
    props: {},
    data() {
        return {
            nom: '',
            prenom: '',
            courriel: '',
            nbPlaces: 0,
        };
    },
    methods: {
        ...mapActions(['postParticipationApi']),
        addParticipation() {
            this.postParticipationApi({
                nomParticipant: this.nom,
                prenomParticipant: this.prenom,
                adresseCourriel: this.courriel,
                nbPlaces: this.nbPlaces,
                evenementId: this.$route.params.id
            })
                .then(() => {
                    this.$toast.add({
                        severity: 'success',
                        summary: 'Success Message',
                        detail: 'La participation a été ajoutée avec succès !',
                        life: 3000
                    });
                })
                .catch(() => {
                    this.$toast.add({
                        severity: 'error',
                        summary: 'Error Message',
                        detail: `Erreur lors de la communication avec le serveur lors de l'ajout de la participation`,
                        // life: 3000
                    });
                })
        },
        async submit() {
            this.addParticipation();
        },
    },
}
</script>

<style scoped>
.button {
    text-align: right;
}

/* label {
    margin-right: 10px;
} */

table {
    border: 1px solid black;
    border-radius: 5px;
    padding: 25px;
}
</style>
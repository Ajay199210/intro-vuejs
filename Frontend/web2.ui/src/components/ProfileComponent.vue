<template>
    <h1>Profile utilisateur</h1>
    <div v-if="isUserAuthenticated">

        <table>
            <thead></thead>
            <tbody>
                <tr>
                    <td><label for="username">Username</label></td>
                    <td><InputText id="username" type="text" v-model="username" disabled /></td>
                </tr>
                <tr>
                    <td><label for="roles">Roles</label></td>
                    <td><InputText id="roles" type="text" v-model="roles" disabled /></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right;">
                        <Button @click="this.$oidc.signOut">
                            <span class="pi pi-sign-out"> Sign out</span>
                        </Button>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
</template>

<script>
import VueJwtDecode from 'vue-jwt-decode';

export default {
    name: "ProfileComponent",
    data() {
        return {
            isUserAuthenticated: this.$oidc.isAuthenticated,
            username: this.$oidc.userProfile.name,
            roles: ''
        }
    },
    created() {
        if (this.isUserAuthenticated) {
            var decodedToken = VueJwtDecode.decode(this.$oidc.accessToken);
            this.roles = decodedToken.role;
        }
    }
}
</script>
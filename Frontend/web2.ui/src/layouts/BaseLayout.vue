<template>
    <div class="container">
        <header>
            <slot name="header">
                <!-- If user is authenticated -->
                <Menubar v-if="this.$oidc.isAuthenticated" :model="items">
                    <template #start>
                        <img src="../assets/site_logo.png" alt="Logo">
                    </template>
                    <template>
                        <div class="card">
                            <Menubar :model="items">
                                <template #item="{ item }">
                                    <router-link v-if="item.route" :to="item.route" custom>
                                        <span class="ml-2">{{ item.label }}</span>
                                    </router-link>
                                </template>
                            </Menubar>
                        </div>
                    </template>
                    <template #end>
                        <router-link to="/profil">Profile ({{ this.$oidc.userProfile.name }})</router-link>
                    </template>
                </Menubar>
                <!-- If user is not logged in -->
                <Menubar v-else :model="items.slice(0, -1)">
                    <template #start>
                        <img src="../assets/site_logo.png" alt="Logo">
                    </template>
                    <template>
                        <div class="card">
                            <Menubar :model="items">
                                <template #item="{ item }">
                                    <router-link v-if="item.route" :to="item.route" custom>
                                        <span class="ml-2">{{ item.label }}</span>
                                    </router-link>
                                </template>
                            </Menubar>
                        </div>
                    </template>
                    <template #end>
                        <router-link v-if="!this.$oidc.isAuthenticated" to="/login">
                            <Button>
                                <span class="pi pi-sign-in"> Login</span>
                            </Button>
                        </router-link>
                    </template>
                </Menubar>
            </slot>
        </header>
        <main>
            <slot></slot>
        </main>
        <footer>
            <slot name="footer">
                Copyright &copy; 2024 - Programmation Web IV
            </slot>
        </footer>
    </div>
</template>

<script>
import VueJwtDecode from 'vue-jwt-decode';

export default {
    data() {
        return {
            userRole: '',
            items: [
                {
                    label: 'Acceuil',
                    command: () => {
                        this.$router.push('/');
                    }
                },
                {
                    label: 'Événements',
                    command: () => {
                        this.$router.push('/evenements');
                    }
                },
                {
                    label: 'Statistiques',
                    command: () => {
                        this.$router.push('/statistiques');
                    },
                }
            ]
        }
    },
    created() {
        if (this.isUserAuthenticated) {
            var decodedToken = VueJwtDecode.decode(this.$oidc.accessToken);
            this.userRole = decodedToken.role;
        }
        console.log(this.userRole);
    }
}
</script>

<style scoped>
img {
    width: 75px;
    height: 75px;
}
nav {
    padding: 15px;
    display: flex;
    background: #bbc5c9;
    font-size: x-large;
    justify-content: space-between;
}

nav a {
    font-weight: bold;
    color: #2c3e50;
    margin: 10px;
    text-decoration: none;
}

nav a.router-link-exact-active {
    color: #099455;
}

main {
    min-height: 300px;
}

footer {
    background: #d2e7e7;
    margin-top: 25px;
    height: 50px;
    padding: 15px;
}
</style>
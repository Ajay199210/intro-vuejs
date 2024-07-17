import axios from 'axios';
import mainOidc from './authClient.js';

const httpClient = axios.create({
    baseURL: 'https://localhost:7152',
    timeout: 3000,
    // auth: {
    //     username: 'identifiant',
    //     password: 'identifiant'
    // }
});

httpClient.defaults.headers.post['Content-Type'] = 'application/json';

// Configuration de l'ajout du jeton d'authentification à chaque requête
httpClient.interceptors.request.use(request => {
    const account = mainOidc.user;
    const isLoggedIn = mainOidc.isAuthenticated;
    const isApiUrl = request.url.startsWith('/api/')//prefix de votre api
     console.log(request.url)
    if (isLoggedIn && isApiUrl) {
        
        request.headers.Authorization = `Bearer ${account.access_token}`;
    }
    return request;
});

export default httpClient;
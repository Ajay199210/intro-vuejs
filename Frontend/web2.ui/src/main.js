import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import BaseLayout from './layouts/BaseLayout.vue'
import mainOidc from './api/authClient.js';

import PrimeVue from 'primevue/config';
import Menubar from 'primevue/menubar';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Textarea from 'primevue/textarea';
import Tooltip from 'primevue/tooltip';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';
import Image from 'primevue/image';


import 'primevue/resources/themes/lara-light-indigo/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';

mainOidc.startup().then(ok => {
    if (ok) {
        const app = createApp(App);
        
        app.component('InputText', InputText);
        app.component('Button', Button);
        app.component('Menubar', Menubar);
        app.component('DataTable', DataTable);
        app.component('Column', Column);
        app.component('Textarea', Textarea);
        app.component('Toast', Toast);
        app.component('Image', Image);
        app.directive('tooltip', Tooltip);
        
        app.config.globalProperties.$oidc = mainOidc;
        app.use(store)
            .use(router)
            .use(PrimeVue)
            .use(ToastService)
            .component('BaseLayout', BaseLayout)
            .mount('#app');
    }
});

mainOidc.useRouter(router);
export default router;

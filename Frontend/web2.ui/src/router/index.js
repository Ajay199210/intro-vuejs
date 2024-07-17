import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import mainOidc from '@/api/authClient.js'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/evenements',
    name: 'evenements',
    component: () => import(/* webpackChunkName: "evenements" */ '../views/EventsView.vue'),
    children: [
      {
        // EventsList will be rendered inside EventsView's <router-view>
        // when /evenements/ is matched
        path: '',
        name: 'eventslist',
        component: () => import(/* webpackChunkName: "evenements" */ '../components/EventsListComponent.vue'),
      },
      {
        // EventDetailsComponent will be rendered inside EventsView's <router-view>
        // when /evenements/1/details is matched
        path: ':id/details',
        name: 'evenement',
        component: () => import(/* webpackChunkName: "evenements" */ '../components/EventDetailsComponent.vue'),
      },
      {
        // EventParticipationForm will be rendered inside EventsView's <router-view>
        // when /evenements/1/participer is matched
        path: ':id/participer',
        name: 'participation',
        component: () => import(/* webpackChunkName: "evenements" */ '../components/EventParticipationForm.vue'),
      },
    ]
  },
  {
    path: '/statistiques',
    meta: { authName: mainOidc.authName },
    name: 'statistiques',
    component: () => import(/* webpackChunkName: "acceuil" */ '../components/StatistiquesComponent.vue')
  },
  {
    path: '/login',
    meta: { authName: mainOidc.authName },
    name: 'login',
    component: () => import(/* webpackChunkName: "acceuil" */ '../components/EventsListComponent.vue')
  },
  {
    path: '/profil',
    meta: { authName: mainOidc.authName },
    name: 'profil',
    component: () => import(/* webpackChunkName: "acceuil" */ '../components/ProfileComponent.vue')
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

export default router

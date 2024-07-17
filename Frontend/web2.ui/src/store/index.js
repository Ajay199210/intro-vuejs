import { createStore } from 'vuex';
import httpClient from '@/api/httpClient';

// Toutes les communications avec le serveur se feront à travers les actions du "store"
export default createStore({
  strict: true,
  state: {
    events: [],
    villes: [],
    categories: [],
    villesPopulaires: [],
    evenementsRentables: [],
    pageTotale: 0
  },
  getters: {
  },
  mutations: {
    setEvents(state, data) {
      state.events = data;
      state.pageTotale = data.pageCount;
    },
    deleteEvent(state, index) {
      state.events.splice(index, 1);
    },
    setVilles(state, data) {
      state.villes = data;
    },
    setCategories(state, data) {
      state.categories = data;
    },
    setVillesPopulaires(state, data) {
      state.villesPopulaires = data;
    },
    setEvenementsRentables(state, data) {
      state.evenementsRentables = data;
    }
  },
  actions: {
    // Events API
    async getEventsApi(context, requestParams) {
      return await httpClient.get('/api/Evenements', {
        params:
        {
          pageIndex: requestParams.pageIndex,
          pageCount: requestParams.pageCount,
          searchTerm: requestParams.searchTerm
        }
      })
        .then(response => {
          context.commit('setEvents', response.data)
          return response.data;
        })
        .catch(error => {
          console.log(error)
          return Promise.reject(error)
        });
    },
    getEventByIdApi(context, id) {
      return httpClient.get(`/api/Evenements/${id}`, {
      })
        .then(response => {
          return response.data;
        })
        .catch(error => {
          // console.log(error)
          return Promise.reject(error)
        });
    },
    async postParticipationApi(context, participation) {
      console.log(participation);
      return await httpClient.post('/api/Participations', participation)
        .then(response => {
          console.log(response.data);
          return response.data
        })
        .catch(error => {
          console.log(error)
          return Promise.reject(error)
        });
    },
    deleteEventApi(context, params) {
      // console.log(params)
      httpClient.delete(`/api/Evenements/${params.id}`)
        .then(() => context.commit('deleteEvent', params.index))
        .catch(error => {
          console.log(error)
          return Promise.reject(error)
        });
    },
    // Statistiques - villes populaires
    async getVillesPopulairesApi(context) {
      return await httpClient.get(`/api/Statistics/Villes/GetVillesPopulaires`)
        .then(response => context.commit('setVillesPopulaires', response.data))
        .catch(error => {
          console.log(error);
          return Promise.reject(error);
        });
    },
    // Statistiques - événements rentables
    async getEvenementsRentables(context) {
      return await httpClient.get(`/api/Statistics/Evenements/GetEvenementsRentables`)
        .then(response => context.commit('setEvenementsRentables', response.data))
        .catch(error => {
          console.log(error);
          return Promise.reject(error);
        });
    },
    // Villes - Chercher les villes
    async getVillesApi(context) {
      return await httpClient.get('/api/Villes')
        .then(response => {
          context.commit('setVilles', response.data)
          return response.data;
        })
        .catch(error => {
          console.log(error)
          return Promise.reject(error)
        });
    },
    // Categories - Chercher les villes
    async getCategoriesApi(context) {
      return await httpClient.get('/api/Categories')
        .then(response => {
          context.commit('setCategories', response.data)
          return response.data;
        })
        .catch(error => {
          console.log(error)
          return Promise.reject(error)
        });
    },
  },
  modules: {}
});

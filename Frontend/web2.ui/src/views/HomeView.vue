<template>
  <div class="home">
    <HomeComponent title="Programmation Web IV - TP3" />
  </div>
</template>

<script>
// @ is an alias to /src
import HomeComponent from '@/components/HomeComponent.vue'

export default {
  name: 'HomeView',
  components: {
    HomeComponent
  }
}
</script>

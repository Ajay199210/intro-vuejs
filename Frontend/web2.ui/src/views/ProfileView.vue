<template>
  <div class="">
    <ProfileComponent />
  </div>
</template>

<script>
// @ is an alias to /src
import ProfileComponent from '@/components/ProfileComponent.vue'

export default {
  name: 'ProfileView',
  components: {
    ProfileComponent
  }
}
</script>
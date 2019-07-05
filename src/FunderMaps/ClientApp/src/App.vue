<template>
  <Layout id="app">
    <router-view />
  </Layout>
</template>

<script>
import Layout from './views/layouts/layouts.vue'
export default {
  name: 'App',
  components: {
    Layout
  }
}
</script>


<style lang="scss">

html, body {
  height: 100%;
  font-family: 'Gibson' !important;
  font-size: 14px;
}
html {
  overflow-y: scroll;
}

#app {
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  min-height: 100%;
}

</style>

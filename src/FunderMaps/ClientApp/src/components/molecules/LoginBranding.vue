<template>
  <div class="LoginBranding">
    <img 
      class="d-none d-lg-block" 
      :src="image({ name: 'login/focus.jpg' })" 
      width="480" 
      height="480" />
    <Logo 
      :company="company"
      class="LoginBranding__logo" />
    <Copyright 
      :company="company"
      class="LoginBranding__copyright test" />
  </div>
</template>

<script>
import Copyright from 'atom/branding/Copyright'
import Logo from 'atom/branding/Logo'
import { image } from 'helper/assets'

export default {
  components: {
    Copyright, Logo
  },
  data() {
    return {
      company: 'FunderMaps'
    }
  },
  methods: {
    image
  }
}
</script>

<style lang="scss">
.LoginBranding {
  position: relative;
  user-select: none;
  
  &__logo {
    position: absolute;
    top: 20px;
    left: 20px;
  }
  &__copyright {
    position: absolute;
    bottom: 20px;
    left: 20px;
  }
}
</style>

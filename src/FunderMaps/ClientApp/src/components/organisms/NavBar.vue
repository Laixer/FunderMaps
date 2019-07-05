<template>
  <nav class="NavBar d-flex flex-row">
    <div class="NavBar__logo">
      <router-link :to="{ name: 'dashboard' }">
        <Logo :company="company" />
      </router-link>
    </div>
    <p class="NavBar__description align-self-center my-0 ml-3 pl-3">
      <span class="ml-2">
        Uploadtool voor uw rapportages
      </span>
    </p>
    <b-nav class="d-flex flex-row-reverse flex-grow-1">
      <b-nav-item-dropdown class="align-self-center mr-3" right>
        <template slot="button-content">
          <img 
            :src="user.getAvatar()" 
            class="m1 rounded-circle"
            height="36"
            width="36" 
            alt="Profile Menu">
        </template>
        <b-dropdown-item 
          v-for="(item, index) in menuItems" 
          :key="index"
          :to="item.to">
          {{ item.label }}
        </b-dropdown-item>
      </b-nav-item-dropdown>
    </b-nav>
  </nav>
</template>

<script>
import { image } from 'helper/assets'
import Logo from 'atom/branding/Logo';
import MenuItem from 'model/MenuItem';

import { mapGetters } from 'vuex'

export default {
  name: 'NavBar',
  components: {
    Logo
  },
  data() {
    return {
      company: 'FunderMaps',
      menuItems: [
        new MenuItem({
          label: 'Profile',
          to: { name: 'user' }
        }),
        new MenuItem({
          label: 'Sign out',
          to: { name: 'logout' }
        })
      ]
    }
  },
  computed: {
    ...mapGetters('user', [
      'user'
    ])
  },
  methods: {
    image
  }
}
</script>

<style lang="scss">
.NavBar {
  height: 70px;
  background: white;
  box-shadow: 0px -1px 0px 0px rgba(223,226,229,1) inset;
  user-select: none;

  &__logo {
    width: 250px;
    height: 100%;
    padding: 20px 0 0 30px;
  }
  &__description {
    color: #7F8FA4;
  }
}
</style>

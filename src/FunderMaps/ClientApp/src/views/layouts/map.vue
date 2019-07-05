<template>
  <div 
    v-if="hasRequiredData" 
    class="d-flex flex-column">
    <NavBar />
    <div class="d-flex align-items-stretch flex-grow-1">
      <SideBar :menu-items="menuItems" :slim="true" />
      <div class="flex-grow-1 d-flex flex-column">
        <HeaderBar />
        <div class="flex-grow h-100 position-relative">
          <slot />
        </div>
      </div>
    </div>
  </div>
  <div 
    v-else 
    class="d-flex flex-column justify-content-center align-items-center">
    <div 
      v-if="hasLoadingDataFailed" 
      class="text-center">
      Refreshing data failed <br/> 
      <router-link 
        :to="{ name: 'login'}">
        Please renew your authentication
      </router-link>
    </div>
    <div v-else>
      Refreshing data, please wait...
    </div>
  </div>
</template>

<script>
import SideBar from 'organism/SideBar';
import NavBar from 'organism/NavBar';
import HeaderBar from 'organism/HeaderBar';

import MenuItem from 'model/MenuItem';

import { isSuperUser } from 'service/auth'
import { mapGetters, mapActions } from 'vuex'

/**
 * The layout for the general user pages.
 * 
 * This component also has a key task in ensuring that all critical data is 
 * available that is required for the default layout. This is no longer a 
 * task performed by the login component, because it also needs to trigger 
 * upon a page refresh.
 * 
 */
export default {
  components: {
    SideBar, NavBar, HeaderBar
  },
  data() {
    return {
      loadingDataFailed: false,
      menuItems: [
        new MenuItem({
          label: 'Dashboard',
          icon: 'Home-icon.svg',
          to: { name: 'dashboard' }
        }),
        new MenuItem({
          label: 'Rapportages',
          icon: 'Report-icon.svg',
          to: { name: 'reports' }
        }),
        new MenuItem({
          label: 'Kaart',
          icon: 'Map-icon.svg',
          to: { name: 'demo-map' }
        }),
      ]
    }
  },
  computed: {
    ...mapGetters('user', [
      'isUserAvailable'
    ]),
    ...mapGetters('org', [
      'isOrganizationAvailable',
      'getOrgId'
    ]),
    ...mapGetters('attestation', [
      'arePrincipalUsersAvailable',
      'areContractorsAvailable'
    ]),
    hasRequiredData() {
      return this.isUserAvailable 
        && this.isOrganizationAvailable
        && this.arePrincipalUsersAvailable
        && this.areContractorsAvailable
    },
    hasLoadingDataFailed() {
      return this.loadingDataFailed
    }
  },
  async created() {
    try {
      await Promise.all([
        this.getUser(),
        this.getOrganization(),
        this.getPrincipalUsers(),
        this.getContractors(),
        this.getVersion()
      ])

      if (isSuperUser()) {
        this.menuItems.push(
          new MenuItem({
            label: 'Organisatie',
            icon: 'Tools-icon.svg',
            to: { name: 'organization' }
          })
        )
      }
    } catch(err) {
      if (err.response && err.response.status === 401) {
        this.$router.push({ name: 'login' })
      } else {
        this.loadingDataFailed = true;
      }
    }
  },
  methods: {
    ...mapActions('user', [
      'getUser'
    ]),
    ...mapActions('org', [
      'getOrganization'
    ]),
    ...mapActions('attestation', [
      'getPrincipalUsers',
      'getContractors'
    ]),
    ...mapActions('version', [
      'getVersion'
    ])
  }
}
</script>

<template>
  <div v-if="component" class="HeaderBar d-flex align-items-center">
    <component :is="component" />
  </div>
</template>

<script>

import ViewHeader from 'molecule/headerbars/View'
import EditHeader from 'molecule/headerbars/Edit'

import { mapGetters } from 'vuex'

export default {
  name: 'HeaderBar',
  components: {
    ViewHeader, EditHeader
  },
  computed: {
    ...mapGetters('report', [
      'activeReport'
    ]),
    component() {
      if (this.$route.name === 'view-report') {
        return 'ViewHeader'
      }
      if (
        [
          'new-report',
          'edit-report-1', 'edit-report-2', 'edit-report-3'
        ].includes(this.$route.name)
      ) {
        return 'EditHeader'
      }
      return false;
    }
  }
}
</script>

<style>
.HeaderBar {
  width: 100%;
}
</style>

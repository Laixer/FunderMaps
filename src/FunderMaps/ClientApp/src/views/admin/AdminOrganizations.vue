<template>
  <div>
    <OrganizationTable 
      title="Alle organisaties"
      :organizations="organisations"
      class="mt-4 pt-2 mb-5" />
  </div>
</template>

<script>
import OrganizationTable from 'organism/OrganizationTable'

import { mapGetters } from 'vuex'

export default {
  components: {
    OrganizationTable
  },
  computed: {
    ...mapGetters('org', [
      'organisations'
    ])
  }
}
</script>


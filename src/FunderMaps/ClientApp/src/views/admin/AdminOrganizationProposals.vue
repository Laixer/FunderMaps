<template>
  <div class="d-flex flex-column justify-content-center">
    <OrganizationProposalTable 
      v-if="proposals.length > 0"
      title="Alle organisatie voorstellen"
      :proposals="proposals"
      class="mt-4 pt-2 mb-5" />
    <div v-else>
      <Feedback class="mb-5" :feedback="feedback" />
    </div>
    <PrimaryArrowButton
      class="mx-auto"
      label="Nieuw voorstel"
      @click="handleClick"
      :hasIcon="false" />

    <CreateProposalModal />
  </div>
</template>

<script>
import Feedback from 'atom/Feedback'
import PrimaryArrowButton from 'atom/navigation/PrimaryArrowButton'
import OrganizationProposalTable from 'organism/OrganizationProposalTable'
import CreateProposalModal from 'organism/CreateProposalModal'

import { mapGetters } from 'vuex'

export default {
  components: {
    Feedback,
    OrganizationProposalTable,
    PrimaryArrowButton,
    CreateProposalModal
  },
  data() {
    return {
      feedback: {
        show: true, 
        variant: 'info',
        message: "Er zijn geen uitstaande voorstellen waarmee organisaties zich kunnen aanmelden"
      }
    }
  },
  computed: {
    ...mapGetters('org', [
      'proposals'
    ])
  },
  methods: {
    handleClick() {
      this.$bvModal.show('modal-new-proposal')
    }
  }
}
</script>


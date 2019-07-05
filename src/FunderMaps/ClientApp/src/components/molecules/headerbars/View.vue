<template>
  <div class="ViewHeader d-flex align-items-center pr-3">
    <div 
      v-if="isAvailableForReview"
      class="d-flex align-items-center">
      Status: 
      <div 
        :class="approvedClasslist"
        class="ViewHeader__choice ml-4 align-items-center"
        @click="handleApprove">
        <img :src='approveIcon' width="30" height="30" />
        <div class="ml-3">{{ approveLabel }}</div>
      </div>
      <span 
        :class="disapprovedClasslist"
        class="ViewHeader__choice ml-4 align-items-center"
        @click="handleDisapproveModal">
        <img :src='disapproveIcon' width="30" height="30" />
        <span class="ml-3">{{ disapproveLabel }}</span>
      </span>
    </div>
    <div class="flex-grow-1">
      <Feedback class="mb-0 mr-3" :feedback="feedback" />
    </div>

    <b-button 
      v-if="editable"
      :to="editRoute"
      variant="light"
      class="font-weight-bold">
      Bewerk
    </b-button>
    <b-button 
      :to="{ name: 'dashboard' }"
      variant="primary" 
      class="ml-2 mr-3 font-weight-bold d-flex align-items-center">
      <img :src='icon({ name: "Close-icon.svg" })' width="11" height="11" /> 
      <span class="ml-1">Sluiten</span>
    </b-button>

    <DisapproveModal 
      @disapprove="handleDisapprove"     
      @hidden="onHidden" />
  </div>
</template>

<script>

import { icon } from 'helper/assets'
import { mapGetters, mapActions } from 'vuex'
import DisapproveModal from 'organism/DisapproveModal'
import Feedback from 'atom/Feedback'

import { isSuperUser, canWrite } from 'service/auth'

export default {
  name: 'ViewHeader',
  components: {
    DisapproveModal, Feedback
  },
  data() {
    return {
      processing: false,
      // approved: null,
      feedback: {}
    }
  },
  computed: {
    ...mapGetters('report', [
      'activeReport'
    ]),
    editRoute() {
      let report = this.activeReport 
        ? this.activeReport 
        : { id: 'id', document_id: 'document' }
      return { 
        name: 'edit-report-2', 
        params: { id: report.id, document: report.document_id }
      }
    },
    editable() {
      if ( ! canWrite()) {
        return false
      }
      if (this.activeReport) {
        return (
          ! this.activeReport.isPendingReview() && 
          ! this.activeReport.isApproved()
        ) || isSuperUser()
      } 
      return false
    },
    isAvailableForReview() {
      // TODO: Can user review...? 
      if (this.activeReport) {
        return this.activeReport.isAvailableForReview()
      }
      return false;
    },
    isPendingReview() {
      if (this.activeReport) {
        return this.activeReport.isPendingReview()
      }
      return true
    },
    disapprovedClasslist() {
      return { 
        'ViewHeader__choice--active' : this.approved === false, 
        'd-none': this.approved === true, 
        'd-flex': this.approved !== true 
      }
    },
    approvedClasslist() {
      return { 
        'ViewHeader__choice--active' : this.approved === true, 
        'd-none': this.approved === false, 
        'd-flex': this.approved !== false 
      }
    },
    approved() {
      if (this.activeReport) {
        return this.activeReport.getApprovalState()
      }
      return null
    },
    approveIcon() {
      let name = this.approved === true ? 'ActiveApprove-icon.svg' : 'NeutralApprove-icon.svg';
      return icon({ name })
    },
    disapproveIcon() {
      let name = this.approved === false ? 'ActiveDisapprove-icon.svg' : 'NeutralDisapprove-icon.svg';
      return icon({ name })
    },
    approveLabel() {
      return this.approved === true ? 'Goedgekeurd' : 'Goedkeuren'
    },
    disapproveLabel() {
      return this.approved === false ? 'Afgekeurd' : 'Afkeuren'
    }
  },
  methods: {
    icon,
    ...mapActions('report', [
      'approveReport'
    ]),
    // TODO: Update with call - Done ?
    async handleApprove() {
      if ( ! this.isPendingReview || this.processing) {
        return;
      }
      this.processing = true;
      this.feedback = {
        variant: 'info',
        message: 'Bezig met verwerken...'
      }
      await this.approveReport()
      // this.approved = true
      this.processing = false
    },
    handleDisapproveModal() {
      if ( ! this.isPendingReview || this.processing) {
        return;
      }
      this.processing = true;
      this.$bvModal.show('report-disapprove')
    },
    handleDisapprove() {
      // this.approved = false
      this.processing = false;
    },
    onHidden() {
      this.processing = false;
    }
  }
}
</script>

<style lang="scss">
.ViewHeader {
  background: white;
  box-shadow: 0 1px 0 0 #CED0DA;
  height: 60px;
  width: 100%;
  padding-left: 39px;
  color: #7F8FA4;

  .btn {
    line-height: 19px;

    img {
      position: relative;
      top: -1px
    }
  }

  &__choice {
    width: 125px;
    cursor: pointer;

    &:hover, &--active {
      color: #354052;
      font-weight: 600;
    }
  }
}
</style>

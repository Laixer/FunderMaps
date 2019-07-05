<template>
  <div class="ReportForm">
    <ProgressSteps :steps="steps" />
    <div 
      v-if="activeReport" 
      class="Report mt-5 mr-0">

      <ReportStepHeader 
        :step="3"
        label="Controle overzicht" />

      <Feedback :feedback="feedback" />

      <ReportDetails 
        :activeReport="activeReport"
        :showLastEdited="false"
        :showUsers="true" />
        
      <div 
        v-if="samples.length !== 0" 
        class="Report__samples">
        <Sample  
          v-for="(sample, index) in samples" 
          :key="index" 
          :sample="sample" />
      </div>
      <div 
        v-else-if="nosamples"
        class="text-center mt-4">
        Deze rapportage bevat nog geen samples
      </div>
      <div 
        class="text-center mt-4"
        v-else>
        De addres gegevens worden geladen...
      </div>
    </div>

    <div 
      v-if="!activeReport" 
      class="d-flex w-100 h-100 align-items-center justify-content-center mt-5">
      <span v-if="!feedback.message">
        Het rapport wordt geladen. We halen het rapport hier opnieuw op 
        om te voorkomen dat de controle uitgevoerd wordt op data die niet opgeslagen is.
      </span>
      <Feedback :feedback="feedback" />
    </div>

    <div class="d-flex align-items-center justify-content-center mt-4">
      <BackButton 
        :disabled="isDisabled"
        :to="previousStep"
        class="mr-3"
        label="Vorige" />
      <PrimaryArrowButton 
        :disabled="isDisabled"
        label="Aanbieden ter review"
        @click="handleToPendingReview" />
    </div>
  </div>
</template>

<script>

import { mapGetters, mapActions } from 'vuex'

import ProgressStep from 'model/ProgressStep'
import ProgressSteps from 'molecule/ProgressSteps'
import ReportStepHeader from 'atom/ReportStepHeader'
import ReportDetails from 'organism/ReportDetails'
import Feedback from 'atom/Feedback'
import Sample from 'organism/Sample'

import PrimaryArrowButton from 'atom/navigation/PrimaryArrowButton'
import BackButton from 'atom/navigation/BackButton'

import { canWrite, isSuperUser } from 'service/auth'

export default {
  components: {
    ReportDetails, Sample, Feedback, 
    ProgressSteps, ReportStepHeader,
    PrimaryArrowButton, BackButton
  },
  data() {
    return {
      feedback: {},
      nosamples: false,
      isDisabled: false,
      steps: [
        new ProgressStep({
          status: 'passed',  
          step: 1,
          icon: 'Step-create-icon.svg'
        }),
        new ProgressStep({
          status: 'passed',
          step: 2,
          icon: 'Step-samples-icon.svg'
        }),
        new ProgressStep({
          status: 'active',
          step: 3,
          icon: 'Step-verify-icon.svg'
        })
      ]
    }
  },
  computed: {
    ...mapGetters('report', [
      'activeReport'
    ]),
    ...mapGetters('samples', [
      'samples'
    ]),
    previousStep() {
      let report = this.activeReport 
        ? this.activeReport 
        : { id: 'id', document_id: 'document' }
      return { name: 'edit-report-2', params: { 
        id: report.id, 
        document: report.document_id 
      } }
    },
  },
  async created() {
    if ( ! canWrite()) {
      this.$router.push({
        name: 'view-report',
        params: this.$route.params
      })
      return;
    }

    try {
      await this.getReportByIds({
        id: this.$route.params.id,
        document: this.$route.params.document
      })

      if (
        (this.activeReport.isPendingReview() ||
        this.activeReport.isApproved()) && 
        ! isSuperUser()
      ) {
        this.$router.push({
          name: 'view-report',
          params: this.$route.params
        })
        return;
      }
      
      await this.getSamples({ reportId: this.activeReport.id })
      if (this.samples.length === 0) {
        this.nosamples = true
      }
    } catch(err) {
      this.feedback = {
        variant: 'danger',
        message: 'Het opgevraagde rapport kan niet gevonden worden'
      }
    }
  },
  beforeDestroy() {
    this.clearActiveReport()
    this.clearSamples()
  },
  methods: {
    ...mapActions('report', [
      'getReportByIds',
      'clearActiveReport',
      'submitForReview'
    ]),
    ...mapActions('samples', [
      'getSamples',
      'clearSamples'
    ]),
    async handleToPendingReview() {
      this.isDisabled = true
      await this.submitForReview()
        .catch(() => {
          this.feedback = {
            variant: 'danger',
            message: 'De actie kon niet uitgevoerd worden. Probeer het later nog eens.'
          }
        })

      this.$router.push({
        name: 'dashboard'
      })
    }
  }
}
</script>

<style scoped lang="scss">
.Report {
  min-width: 600px;
  max-width: 870px;
  margin-right: 30px;
}
h3 {
  font-size: 16px;
  color: #354052;
  font-weight: 600;
}
</style>

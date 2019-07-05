<template>
  <div class="ReportForm">
    <ProgressSteps :steps="steps" />
    <div 
      v-if="activeReport"
      class="ReportForm__form mt-5">

      <ReportStepHeader 
        :step="2"
        :label="activeReport.document_id">
        <b-button 
          variant="light" 
          class="font-weight-bold d-flex align-items-center"
          @click="handleAddSample">
          <img :src='icon({ name: "Plus-icon.svg" })' width="11" height="11" /> 
          <span class="ml-1">Adres toevoegen</span>
        </b-button>
      </ReportStepHeader>

      <div>  
        <Feedback :feedback="feedback" />

        <div 
          v-if="samples.length !== 0" 
          class="Report__samples">  

          <Sample  
            v-for="(sample, index) in samples" 
            :key="index + '-' + Date.now()" 
            :sample="sample"
            :editMode="true" />

        </div>
        <div 
          v-else-if="nosamples"
          class="text-center mt-4">
          Deze rapportage bevat nog geen adressen
        </div>
        <div 
          class="text-center mt-4"
          v-else>
          De addres gegevens worden geladen...
        </div>
      </div>
    </div>

    <div 
      v-if="!activeReport" 
      class="d-flex w-100 h-100 align-items-center justify-content-center mt-5">
      <span v-if="!feedback.message">
        Het rapport wordt geladen. We halen het rapport hier opnieuw op 
        om te voorkomen dat er gewerkt wordt met verouderde data.
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
        :to="nextStep"
        label="Volgende" />
    </div>

  </div>
</template>

<script>

import ProgressSteps from 'molecule/ProgressSteps'
import ProgressStep from 'model/ProgressStep'
import Feedback from 'atom/Feedback'
import ReportStepHeader from 'atom/ReportStepHeader'
import PrimaryArrowButton from 'atom/navigation/PrimaryArrowButton'
import BackButton from 'atom/navigation/BackButton'
import Sample from 'organism/Sample'

import { mapActions, mapGetters } from 'vuex'
import { icon } from 'helper/assets'
import { isSuperUser, canWrite } from 'service/auth'

export default {
  name: 'Step2',
  components: {
    Feedback, ProgressSteps, 
    ReportStepHeader, PrimaryArrowButton,
    Sample, BackButton
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
          status: 'active',
          step: 2,
          icon: 'Step-samples-icon.svg'
        }),
        new ProgressStep({
          status: 'disabled',
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
      let report = this.activeReport ? this.activeReport : { id: 'id', document_id: 'document' }
      return { name: 'edit-report-1', params: { 
        id: report.id, 
        document: report.document_id 
      } }
    },
    nextStep() {
      let report = this.activeReport ? this.activeReport : { id: 'id', document_id: 'document' }
      return { name: 'edit-report-3', params: { 
        id: report.id, 
        document: report.document_id 
      } }
    }
  },
  async created() {
    try {
      if ( ! canWrite()) {
        this.$router.push({
          name: 'view-report',
          params: this.$route.params
        })
        return;
      }

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
    icon,
    ...mapActions('report', [
      'getReportByIds',
      'clearActiveReport'
    ]),
    ...mapActions('samples', [
      'getSamples',
      'clearSamples',
      'addUnsavedSample'
    ]),
    handleAddSample() {
      this.addUnsavedSample()
    }
  }
}
</script>
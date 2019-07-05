<template>
  <div 
    class="d-flex align-items-start">
    <div 
      v-if="activeReport" 
      class="Report flex-grow-1">
      
      <ReportDetails 
        :activeReport="activeReport"
        :showLastEdited="true"
        :showUsers="false" />

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
      v-if="activeReport" 
      class="d-flex flex-column">
      <div class="side p-3">
        <h3 class="">Organisaties</h3>
        <ReportOrgRole :org="activeReport.contractor" />
      </div>
      <div class="side p-3 mt-3">
        <h3>Betrokken personen</h3>
        <ReportUserRole :user="activeReport.reviewer" />
        <ReportUserRole :user="activeReport.creator" />
      </div>
    </div>

    <div 
      v-if="!activeReport" 
      class="d-flex w-100 h-100 align-items-center justify-content-center">
      <span v-if="!feedback.message">
        Het rapport wordt geladen...
      </span>
      <Feedback :feedback="feedback" />
    </div>
  </div>
</template>

<script>

import { mapGetters, mapActions } from 'vuex'

import ReportDetails from 'organism/ReportDetails'
import ReportUserRole from 'atom/review/ReportUserRole'
import ReportOrgRole from 'atom/review/ReportOrgRole'
import Feedback from 'atom/Feedback'
import Sample from 'organism/Sample'

export default {
  components: {
    ReportUserRole, ReportDetails,
    ReportOrgRole, Sample, Feedback
  },
  data() {
    return {
      feedback: {},
      nosamples: false
    }
  },
  computed: {
    ...mapGetters('report', [
      'activeReport'
    ]),
    ...mapGetters('samples', [
      'samples'
    ])
  },
  async created() {
    try {
      await this.getReportByIds({
        id: this.$route.params.id,
        document: this.$route.params.document
      })
      // console.log(this.activeReport)
      
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
      'clearActiveReport'
    ]),
    ...mapActions('samples', [
      'getSamples',
      'clearSamples'
    ])
  }
}
</script>

<style scoped lang="scss">
.Report {
  min-width: 600px;
  max-width: 870px;
  margin-right: 30px;
}
.side {
  width: 360px;
  background: white;
  border: 1px solid #CED0DA;
  border-radius: 5px;
}
h3 {
  font-size: 16px;
  color: #354052;
  font-weight: 600;
}
</style>

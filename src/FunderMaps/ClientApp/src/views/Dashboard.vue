<template>
  <div class="d-flex flex-column">
    <UploadArea />

    <ReportTable 
      title="Recente rapporten"
      :reports="latestReports({ count: 5 })"
      :synchronizing="loading"
      class="mt-4 pt-2 mb-5" />
    <PrimaryArrowButton
      class="mx-auto"
      label="Alle rapporten"
      :to="{ name: 'reports' }" />
  </div>
</template>

<script>
import PrimaryArrowButton from 'atom/navigation/PrimaryArrowButton'
import ReportTable from 'organism/ReportTable';
import UploadArea from 'molecule/UploadArea';

import { mapGetters, mapActions } from 'vuex'

let timer = null;

export default {
  name: 'Dashboard',
  components: {
    ReportTable, UploadArea, PrimaryArrowButton
  },
  data() {
    return {
      loading: true
    }
  },
  computed: {
    ...mapGetters('reports', [
      "latestReports"
    ])
  },
  created() {
    this.syncReports();
  },
  destroyed() {
    clearTimeout(timer);
  },
  methods: {
    ...mapActions('reports', [
      'getLatestReports'
    ]),

    // Update the report details on the dashboard every minute
    async syncReports() {
      this.loading = true;
      if (timer !== null) {
        clearTimeout(timer);
      }
      await this.getLatestReports({ count: 5 })
      this.loading = false;

      // TODO: Enable when not in dev
      timer = setTimeout(this.syncReports, (60 * 1000 * 9999999999999999)); // every minute
    }
  }
}
</script>

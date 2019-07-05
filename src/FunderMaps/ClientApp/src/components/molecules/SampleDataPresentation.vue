<template>
  <div>
    <DataRow 
      v-for="(row, index) in sampleData" 
      :key="index" 
      :items="row"
      :divider="index !== (sampleData.length - 1)" />
  </div>
</template>

<script>
import DataRow from 'molecule/DataRow'

export default {
  name: 'SampleDataPresentation',
  components: {
    DataRow
  },
  props: {
    sample: {
      type: Object,
      required: true,
    }
  },
  computed: {
    sampleData() {
      // console.log(this.sample)

      return [
        [
          {
            label: 'Funderingstype',
            value: this.sample.getFoundationType()
          },
          {
            label: 'Onderbouw',
            value: this.sample.getSubstructure()
          },
          {
            label: 'Bouwjaar',
            value: this.sample.built_year
          }
        ],
        [
          {
            label: 'Monitoringspunt',
            value: this.sample.monitoring_well
          },
          {
            label: 'Sondering',
            value: this.sample.cpt
          }
        ],
        [
          {
            label: 'Funderingskwaliteit',
            value: this.sample.getFoundationQuality()
          },
          {
            label: 'Funderingsherstel advies',
            value: this.sample.foundation_recovery_adviced
          }
        ],
        [
          {
            label: 'Oorzaak funderingsschade',
            value: this.sample.getFoundationDamageCause()
          },
          {
            label: 'Handhavingstermijn',
            value: this.sample.getEnforcementTerm()
          }
        ],
        [
          {
            label: 'Referentiestelsel',
            value: this.sample.getBaseMeasurementLevel()
          },
          {
            label: 'Hoogte Langshout',
            value: this.sample.wood_level
          }
        ],
        [
          {
            label: 'Grondwaterstand',
            value: this.sample.groundwater_level
          },
          {
            label: 'Maaiveldhoogte',
            value: this.sample.ground_level
          }
        ]
      ]
    }
  }
}
</script>

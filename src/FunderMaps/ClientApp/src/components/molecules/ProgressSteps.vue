<template>
  <div class="ProgressSteps d-flex justify-content-between">
    <ProgressLine 
      :steps="steps.length - 1" 
      :step="step" />
    <ProgressIndicator 
      v-for="(step, index) in steps"
      :key="index"
      :status="step.status"
      :iconName="step.iconName"
      :clickable="step.clickable"
      :step="step.step" />
  </div>
</template>

<script>
import ProgressIndicator from 'atom/ProgressIndicator'
import ProgressLine from 'atom/ProgressLine';

export default {
  name: "ProgressSteps",
  components: {
    ProgressIndicator, ProgressLine
  },
  props: {
    steps: {
      type: Array,
      default: function() {
        return []
      }
    }
  },
  computed: {
    step() {
      return this.steps.findIndex((step) => {
        return step.status === 'active';
      })
    }
  }
}
</script>

<style lang="scss">
.ProgressSteps {
  position: relative;
  margin: 0 30px;

  .ProgressLine {
    position: absolute;
  }
}
</style>

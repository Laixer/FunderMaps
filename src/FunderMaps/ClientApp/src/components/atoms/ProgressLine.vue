<template>
  <div class="ProgressLine">
    <div 
      :style="progress"
      class="ProgressLine__progress"></div>
  </div>
</template>

<script>
export default {
  name: 'ProgressLine',
  props: {
    steps: {
      type: Number,
      default: 2
    },
    step: {
      type: Number,
      default: 0
    }
  },
  computed: {
    progress() {
      return {
        'width': ((this.step / this.steps) * 100) + '%'
      }
    }
  }
}
</script>

<style lang="scss">
.ProgressLine {
  position: absolute;
  height: 6px;
  background: #CED0DA;
  width: calc(100% - 100px);
  left: 50px;
  top: 47px;

  &__progress {
    position: absolute;
    background: #17A4EA;
    height: 100%;
    top: 0;
    left: 0;
  }
}
</style>

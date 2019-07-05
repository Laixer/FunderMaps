<template>
  <div 
    :class="statusClass"
    class="ProgressIndicator">
    <div class="ProgressIndicator__sphere d-flex align-items-center justify-content-center">
      <img 
        v-if="iconName"
        :src="icon({ name: iconName })" 
        width="45" 
        height="45" />
    </div>
    <div class="ProgressIndicator__button"></div>
  </div>
</template>

<script>
import { icon } from 'helper/assets'

export default {
  name: 'ProgressIndicator',
  props: {
    status: {
      type: String,
      default: 'disabled',
      validator(value) {
        return ['disabled', 'active', 'passed'].includes(value)
      }
    },
    clickable: {
      type: Boolean,
      default: false
    },
    step: {
      type: Number,
      default: 1
    },
    iconName: {
      type: [String,Boolean],
      default: false
    }
  },
  computed: {
    statusClass() {
      let cls = {}
      cls['ProgressIndicator--'+this.status] = true
      return cls
    }
  },
  methods: {
    icon
  }
}
</script>

<style lang="scss">
.ProgressIndicator {

  &__sphere {
    position: relative;
    border-radius: 50%;
    width: 100px;
    height: 100px;
    background: #CED0DA;
  }
  &--active, &--passed {
    .ProgressIndicator {
      &__sphere {
        background-color: #17A4EA;
      }
    }
  }
  &--active {
    .ProgressIndicator {
      &__sphere {
        &:after {
          content: '';
          position: absolute;
          width: 110px;
          height: 110px;
          top: -5px;
          left: -5px;
          border-radius: 50%;
          border: 2px solid #17A4EA;
        }
      }
    }
  }
}
</style>

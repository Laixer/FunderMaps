<template>
  <div class="BasicDataItem d-flex flex-column">
    <strong>{{ label }}</strong>
    <span 
      :class="valueClass"
      class="mt-2">
      {{ boundValue }}
    </span>
  </div>
</template>

<script>

import { isObject } from 'helper/is.js'

export default {
  name: 'BasicDataItem',
  props: {
    label: {
      type: String,
      required: true
    },
    value: {
      type: [String, Number, Object],
      default: ''
    }
  },
  computed: {
    valueClass() {
      return {
        'text-muted': this.boundValue === 'Geen data'
      }
    },
    boundValue() {
      let value = (isObject(this.value) && this.value.hasOwnProperty('text')) 
        ? this.value.text
        : this.value;
      return value 
        ? value
        : 'Geen data';
    }
  }
}
</script>

<style lang="scss">
.BasicDataItem {
  strong {
    font-size: 16px;
    line-height: 1;
    font-weight: 400;
    color: #7F8FA4;
  }
  span {
    line-height: 19px;
    font-size: 16px;
    color: #354052;
  }
}
</style>

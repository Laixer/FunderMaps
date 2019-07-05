<template>
  <div class="CheckboxIndicator d-flex align-items-center">
    <span>
      <img :src="icon({ name: iconName })" width="16" height="16" /> 
    </span>
    <span>{{ label }}</span>
  </div>
</template>

<script>
import { icon } from 'helper/assets'

export default {
  name: 'CheckboxIndicator',
  props: {
    label: {
      type: String,
      required: true
    },
    value: {
      type: Boolean,
      required: true
    }
  },
  computed: {
    iconName() {
      return this.value ? 'Check-icon.svg' : 'None-icon.svg'
    }
  },
  methods: {
    icon
  }
}
</script>

<style lang="scss">
.CheckboxIndicator {
  font-size: 16px;
  color: #354052;

  span:first-child {
    width: 30px;
  }
}
</style>

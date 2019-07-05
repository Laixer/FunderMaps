<template>
  <b-button 
    variant="primary" 
    :to="to"
    :disabled="disabled" 
    class="PrimaryArrowButton font-weight-bold d-inline-flex align-items-center" 
    size="lg"
    @click="$emit('click')" 
    pill>
    <span class="mx-4 my-2">
      {{ label }}
    </span>
    <img 
      v-if="hasIcon"
      alt="arrow" 
      :src="icon({ name: 'ArrowRight-icon.svg' })" />
  </b-button>
</template>

<script>

import { icon } from "helper/assets";
export default {
  props: {
    label: {
      type: String,
      default: 'Alle rapporten'
    },
    to: {
      type: [Object, String],
      default: function() {
        return ''
      }
    },
    disabled: {
      type: Boolean,
      default: false
    },
    hasIcon: {
      type: Boolean,
      default: true
    }
  },
  methods: {
    icon
  }
}
</script>

<style>
.PrimaryArrowButton {
  font-size: 18px;
  line-height: 1;
}
</style>

<template>
  <b-button 
    variant="outline-primary" 
    :to="to"
    :disabled="disabled" 
    class="BackButton font-weight-bold d-inline-flex align-items-center" 
    size="lg" 
    pill>
    <img 
      alt="arrow" 
      :src="icon({ name: 'ArrowLeft-icon.svg' })" />
      
    <span class="mx-4 my-2">
      {{ label }}
    </span>
    
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
      type: Object,
      default: function() {
        return { name: 'reports' }
      }
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    icon
  }
}
</script>

<style lang="scss">
.BackButton.btn {
  font-size: 18px;
  line-height: 1;
  

  &:hover, &:active {
    background: transparent !important;  
    color: darken(#17A4EA, 20%);
    border-color: darken(#17A4EA, 20%);
  }
}
</style>

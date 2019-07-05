<template>
  <div 
    :class="classList"
    class="SampleLine d-flex align-items-center"
    @click="togglePanel">

    <span class="SampleLine__address flex-grow-1">
      {{ address || 'Nieuw adres' }}
    </span>

    <div 
      v-if="editMode"
      class="SampleLine__edit d-flex align-items-center mr-3 pr-3">
      <b-button 
        v-if="open"
        size="sm"
        variant="outline-primary"
        class="mr-3"
        @click.stop="$emit('save')">
        opslaan
      </b-button>
      <img 
        :src="delIcon" 
        width="16" 
        height="16"
        @click.prevent.stop="$emit('delete')" />
    </div>

    <img :src="arrow" width="10" height="10" />
    
  </div>
</template>

<script>

import { icon } from 'helper/assets'

export default {
  props: {
    address: {
      type: String,
      required: true
    },
    open: {
      type: Boolean,
      required: true
    },
    editMode: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    classList() {
      return {
        'SampleLine--open': !! this.open,
        'SampleLine--edit': !! this.editMode
      }
    },
    arrow() {
      let name = this.open 
        ? 'ArrowUp-icon.svg' 
        : 'ArrowDown-icon.svg'
      return icon({name});
    },
    delIcon() {
      return icon({ name: 'Delete-icon.svg' });
    }
  },
  methods: {
    icon,
    togglePanel() {
      this.$emit('toggle');
    }
  }
}
</script>

<style lang="scss">
.SampleLine {
  height: 60px;
  padding: 0 30px;
  cursor: pointer;

  &--open &__address {
    font-weight: 600;
  }
  &__edit {
    border-right: 1px solid #E8EAF1;
    height: 30px;
  }
}
</style>

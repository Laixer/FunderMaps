<template>
  <div 
    :style="typeStyles"
    class="TypeTag">
    {{ type.text }}
  </div>
</template>

<script>
export default {
  name: 'TypeTag',
  props: {
    type: {
      type: Object,
      required: true
    }
  },
  computed: {
    typeStyles() {
      return {
        color: this.type.color || 'white',
        backgroundColor: this.type.bgColor
      }
    }
  }
}
</script>

<style>
.TypeTag {
  display: inline-block;
  border-radius: 3px;
  font-size: 14px;
  font-weight: 600;
  text-transform: uppercase;
  line-height: 1; 
  padding: 7px 8px 4px;
  user-select: none;
}
</style>

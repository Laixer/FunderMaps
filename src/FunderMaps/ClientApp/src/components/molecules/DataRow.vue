<template>
  <div class="DataRow">
    <div class="DataRow__entries d-flex align-items-center ">
      <template v-for="(item, index) in items">
        <component 
          class="DataRow__item"
          :is="componentType({ item })"
          :key="index" 
          :label="item.label" 
          :value="item.value" />
      </template>
    </div>
    <Divider v-if="divider" />
  </div>
</template>

<script>
import BasicDataItem from 'atom/dataitems/BasicDataItem'
import CheckboxIndicator from 'atom/review/CheckboxIndicator'
import Divider from 'atom/Divider'

export default {
  name: 'DataRow',
  components: {
    Divider, BasicDataItem, CheckboxIndicator
  },
  props: {
    items: {
      type: Array,
      default: function() {
        return []
      }
    },
    divider: {
      type: Boolean,
      default: true
    }
  },
  methods: {
    componentType({ item }) {
      return item.value === true || item.value === false
        ? 'CheckboxIndicator'
        : 'BasicDataItem'
    }
  }
}
</script>

<style lang="scss">
.DataRow__entries {
  position: relative;
  margin-top: 20px;
  padding: 0 30px;
  margin-bottom: 20px;
}
.DataRow__item {
  width: 50%;

  // 3rd item is only on first row, as an exceptional datapoint
  &:nth-child(n+3) {
    position: absolute;
    top: 0;
    right: 30px;
    width: 15%;
  }
}
</style>

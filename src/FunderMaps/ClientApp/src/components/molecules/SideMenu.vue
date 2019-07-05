<template>
  <div class="list-group list-group-flush">
    <router-link 
      v-for="(item, index) in menuItems"
      :to="item.to"
      :key="index"
      :class="{ 'active' : item.isActive() }"
      class="list-group-item list-group-item-action d-flex 
             justify-content-between align-items-center pl-3 py-3">
      <span 
        :class="{ 'px-3': !slim }">
        <img 
          v-if="item.hasIcon()"
          :src="item.icon()" 
          width="17" 
          height="17" />
      </span>
      <span 
        v-if="!slim"
        class="flex-grow-1">
        {{ item.label }}
      </span>
      <span 
        v-if="item.notifications && !slim" 
        class="badge badge-danger badge-pill ml-1">
        {{ item.notifications }}
      </span>
    </router-link>
  </div>
</template>

<script>
// TODO: Break down into Atoms later

export default {
  props: {
    items: {
      type: Array,
      default: function() {
        return [];
      }
    },
    slim: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    menuItems() {
      let currentRouteName = this.$route.name;
      return this.items
        .map(item => {
          item.active = (item.to.name && item.to.name === currentRouteName);
          return item;
        })
    }
  }
}
</script>

<style lang="scss">
// Double class to override default style
.list-group {
  &.list-group {
    margin-right: 1px;
    border-bottom: 1px solid rgba(0, 0, 0, 0.125);
    line-height: 1;
  }
  .list-group-item {
    cursor: pointer
  }
  .list-group-item.active {
    position: relative;
    color: #17A4EA;
    background: white;
    border-color: rgba(0, 0, 0, 0.125);
    font-weight: 600;

    &:hover, &:focus {
      background-color: #f8f9fa
    }
    &:active {
      background-color: #e9ecef;
    }
    &:after {
      content: '';
      position: absolute;
      background-color: #17A4EA;
      height: 100%;
      width: 5px;
      top: 0;
      right: 0;
    }
  }
}
</style>

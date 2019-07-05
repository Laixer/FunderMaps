<template>
  <div class="ReportOrgRole d-flex align-items-center mt-4">
    <img :src="org.getAvatar()" width="32" height="32" class="rounded-circle" />
    <div class="ml-3">
      <div class="ReportOrgRole__name">{{ org.getName() }}</div>
      <div class="ReportOrgRole__role">{{ org.getRole() }}</div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    org: {
      type: Object,
      required: true
    }
  }
}
</script>

<style lang="scss">
.ReportOrgRole {
  &__name {
    color: #354052;
    font-size: 16px;
    line-height: 1
  }
  &__role {
    color: #7F8FA4;
    line-height: 1
  }
}
</style>

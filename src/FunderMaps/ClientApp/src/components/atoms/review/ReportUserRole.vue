<template>
  <div class="ReportUserRole d-flex align-items-center mt-4">
    <img :src="user.getAvatar()" width="32" height="32" class="rounded-circle" />
    <div class="ml-3">
      <div class="ReportUserRole__name">{{ user.getUserName() }}</div>
      <div class="ReportUserRole__role">{{ user.getRole() }}</div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    user: {
      type: Object,
      required: true
    }
  }
}
</script>

<style lang="scss">
.ReportUserRole {
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

<template>
  <tr 
    class="OrganizationTableLine d-flex align-items-center p-2 mb-2"
    @click="openOrganization">
    <td class="py-1 flex-grow-1">
      <strong>{{ organization.name }}</strong>
    </td>
    <td>
      {{ organization.email }}
    </td>
    <td class="d-flex justify-content-end">
      <b-button variant="light">
        Bewerk
      </b-button>
    </td>
  </tr>
</template>

<script>
export default {
  name: 'OrganizationTableLine',
  props: {
    organization: {
      type: Object,
      default: function() {
        return {}
      }
    }
  },
  methods: {
    openOrganization() {
      this.$router.push({ 
        name: 'admin-organization', 
        params: { 
          id: this.organization.id
        } 
      })
    }
  }
}
</script>

<style lang="scss">
.OrganizationTableLine {
  width: 100%;
  background: white;
  border: 1px solid #DFE2E5;
  border-radius: 4px;
  color: #7F8FA4;
  line-height: 1;
  transition: all 0.15s;
  user-select: none;
  cursor: pointer;

  &:hover {
    box-shadow: 0 0 15px 0 rgba(158, 169, 184, 0.7);
    transform: scale(1);
  }

  strong {
    font-weight: 600;
    color: #354052;
  }

  .btn {
    color: #7F8FA4;

    &:hover, &:active {
      color: darken(#7F8FA4, 10%)
    }
  }
}
</style>

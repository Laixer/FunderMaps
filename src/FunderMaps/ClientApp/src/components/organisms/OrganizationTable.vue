<template>
  <div class="OrganizationTable">
    <div class="d-flex align-items-end">
      <h2 v-if="hasTitle" class="ml-2 mb-3 d-inline-block">
        {{ title }}
      </h2>
      <div v-if="synchronizing" class="flex-grow-1 d-flex justify-content-end">
        <span class="badge badge-info badge-pill text-uppercase font-weight-bold px-2 py-1">
          Verversen van informatie ...
        </span>
      </div>
    </div>
    <table>
      <thead>
        <tr class="d-flex p-2">
          <th scope="col">
            Naam
          </th>
          <th scope="col">
            E-mail
          </th>
          <th scope="col"></th>
        </tr>
      </thead>
      <tbody>
        <OrganizationTableLine 
          v-for="(organization, index) in organizations" 
          :key="index" 
          :organization="organization" />
      </tbody>
    </table>
  </div>
</template>

<script>
import OrganizationTableLine from 'molecule/OrganizationTableLine';

export default {
  name: 'OrganizationTable',
  components: {
    OrganizationTableLine
  },
  props: {
    title: {
      type: String,
      default: ''
    },
    synchronizing: {
      type: Boolean,
      default: false
    },
    organizations: {
      type: Array,
      default: function() {
        return []
      }
    }
  },
  computed: {
    hasTitle() {
      return this.title !== ''
    }
  }
}
</script>

<style lang="scss">
.OrganizationTable {
  width: 100%;
  user-select: none;

  h2 {
    font-size: 18px;
    color: #354052;
    font-weight: 600;
  }
  table {
    width: 100%;

    th {
      color: #7F8FA4;
      font-size: 14px;
      font-weight: normal;
    }
    th, td {
      &:nth-child(1) {
        min-width: 200px;
        flex-grow: 1;
      }
      &:nth-child(2) {
        min-width: 300px;
        flex-grow: 2;
      }
      &:nth-child(3) {
        min-width: 100px;
      }
    }
  }
}

</style>

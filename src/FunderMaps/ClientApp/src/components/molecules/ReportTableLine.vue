<template>
  <tr 
    class="ReportTableLine d-flex align-items-center p-2 mb-2"
    @click="openReport">
    <td scope="row" class="py-1">
      <div 
        :style="{ 'backgroundColor': report.statusColor() }"
        class="ReportTableLine__status mx-auto"></div>
    </td>
    <td class="py-1 flex-grow-1">
      <strong>{{ report.label() }}</strong>
    </td>
    <td>{{ report.reviewerName() }}</td>
    <td>{{ report.date() }}</td>
    <td>
      <TypeTag 
        v-if="report.hasType()" 
        :type="report.getType()" />
    </td>
    <td class="d-flex justify-content-end">
      <b-button
        v-if="editable"
        variant="light"
        @click.stop="handleEdit">
        Bewerk
      </b-button>
      <b-button
        v-else
        variant="light">
        Bekijk
      </b-button>
    </td>
  </tr>
</template>

<script>
import TypeTag from 'atom/TypeTag';

import { isSuperUser, canWrite } from 'service/auth'

export default {
  name: 'ReportTableLine',
  components: {
    TypeTag
  },
  props: {
    report: {
      type: Object,
      default: function() {
        return {}
      }
    }
  },
  computed: {
    editable() {
      if ( ! canWrite()) {
        return false
      }
      return (
        ! this.report.isPendingReview() && 
        ! this.report.isApproved()
      ) || isSuperUser()
    }
  },
  methods: {
    openReport() {
      this.$router.push({ 
        name: 'view-report', 
        params: { 
          id: this.report.id, 
          document: this.report.document_id 
        } 
      })
    },
    handleEdit() {
      this.$router.push({ 
        name: 'edit-report-2', 
        params: { 
          id: this.report.id, 
          document: this.report.document_id 
        } 
      })
    }
  }
}
</script>

<style lang="scss">
.ReportTableLine {
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

  &__status {
    // background: #F5A623;
    width: 20px;
    height: 20px;
    border-radius: 50%;
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

<template>
  <div class="Report__details">
    <header class="d-flex align-items-center justify-content-between">
      <div>
        <h3>
          {{ activeReport.document_id }}
        </h3>
        <span v-if="showLastEdited && hasEditedDate">
          Laatst bewerkt: {{ lastEdited }}
        </span>
      </div>
      <TypeTag :type="activeReport.getType()" />
    </header>
    <ReportDate :date="activeReport.document_date" />
    <Divider />
    <div 
      v-if="showUsers"
      class="Report__users d-flex">
      <ReportUserRole :user="activeReport.creator" />
      <ReportUserRole :user="activeReport.reviewer" />
    </div>
    <Divider v-if="showUsers" />
    <div class="Report__indicators d-flex flex-wrap">
      <CheckboxIndicator 
        v-if="activeReport.norm"
        :value="activeReport.norm.conform_f3o"
        class="mb-1" 
        label="Conform F3O" />
      <CheckboxIndicator 
        :value="activeReport.joint_measurement"
        class="mb-1" 
        label="Lintvoegmeting"  />
      <CheckboxIndicator 
        :value="activeReport.inspection" 
        class="mb-1" 
        label="Onderzoeksput" />
      <CheckboxIndicator 
        :value="activeReport.floor_measurement"
        class="mb-1" 
        label="Vloer waterpas" />
    </div>
    <Divider v-if="activeReport.note" />
    <Note 
      v-if="activeReport.note"
      :note="activeReport.note" />
  </div>
</template>

<script>

import ReportDate from 'atom/review/ReportDate'
import ReportUserRole from 'atom/review/ReportUserRole'
import Note from 'atom/review/Note'
import CheckboxIndicator from 'atom/review/CheckboxIndicator'
import TypeTag from 'atom/TypeTag'
import Divider from 'atom/Divider'

import { 
  convertDateStringToDate, 
  weekDayFromDate, 
  monthYearStringFromDate 
} from 'helper/date'

export default {
  components: {
    TypeTag, Divider, ReportUserRole,
    ReportDate, CheckboxIndicator, Note
  },
  props: {
    activeReport: {
      type: Object,
      required: true
    },
    showLastEdited: {
      type: Boolean,
      default: true
    },
    showUsers: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    // IN: "2019-04-28T21:55:02.09066+00:00"
    // OUT: zondag 21:55 uur
    lastEdited() {
      let date = convertDateStringToDate(
        this.activeReport.update_date.substr(0, 10)
      )
      return weekDayFromDate({ date }) + ' ' 
        + date.getDate() + ' ' 
        + monthYearStringFromDate({ date }) + ' - '
        + this.activeReport.update_date.substr(11, 5) 
        + ' uur'
    },
    hasEditedDate() {
      return this.activeReport.update_date !== null
    }
  },
  created() {
    // console.log(this.activeReport)
  }
}
</script>

<style lang="scss">
.Report__details {
  background: #FAFBFC;
  border-radius: 5px;
  border: 1px solid #CED0DA;
  overflow: hidden;

  header {
    padding: 25px 30px;
    background: white;
    border-bottom: 1px solid #CED0DA;
    width: 100%;
    line-height: 1;

    h3 {
      margin: 0
    }
    span {
      color: #7F8FA4;
      font-size: 12px;
    }
  }
  
  .Report__users {
    padding: 0 30px;

    .ReportUserRole {
      width: 50%;
      margin-top: 0 !important;
    }
  }
  .Report__indicators {
    padding: 0 30px;

    .CheckboxIndicator {
      width: 50%
    }
    &:last-child {
      margin-bottom: 20px;
    }
  }

  .ReportDate {
    margin-top: 20px;
    padding: 0 30px;
  }
  .Note {
    padding: 0 30px;
    margin-bottom: 20px;
  }
}
</style>

<template>
  <div class="ReportDate d-flex align-items-center">
    <span>
      <img :src="icon({ name: 'Date-icon.svg' })" width="16" height="16" />
    </span>
    <span class="mr-3">Datum onderzoek</span>
    <span>{{ boundDate }}</span>
  </div>
</template>

<script>
import { icon } from 'helper/assets'
import { isDate } from 'helper/is'
import { monthYearStringFromDate } from 'helper/date'

export default {
  props: {
    date: {
      type: [String, Date],
      required: true
    }
  },
  computed: {
    boundDate() {
      if (isDate(this.date)) {
        // return this.date.getDate() + ' - ' 
        //   + (this.date.getMonth() + 1) + ' - '
        //   + this.date.getFullYear();
        return this.date.getDate() + ' ' 
        + monthYearStringFromDate({ date: this.date });
      }
      return this.date;
    }
  },
  methods: {
    icon
  }
}
</script>

<style lang="scss">
.ReportDate {
  font-size: 16px;

  span:first-child {
    width: 30px
  }
  span:nth-child(2) {
    color: #8292A6;
  }
  span:nth-child(3) {
    color: #354052;
  }
}
</style>

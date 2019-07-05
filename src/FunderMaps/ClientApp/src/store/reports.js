/**
 * Import Dependency
 */
import ReportModel from 'model/Report'

/**
 * Import API
 */
import reportAPI from 'api/reports';

/**
 * Declare Variable
 */
const state = {
  reports : [],
  reportCount: false
}


const getters = {
  latestReports: state => ({ count }) => {
    return state.reports.slice(0, (count || 5));
  },
  reports: state => {
    return state.reports
  },
  reportCount: state => {
    return state.reportCount
  }
}
const actions = {
  async getLatestReports({ commit }, { count }) {
    let response = await reportAPI.getLatestReports({ limit: count || 5 });
    if (response.status === 200 && response.data.length > 0) {
      commit('set_reports', {
        reports: response.data
      })
    } 
  },
  async getReportCount({ commit }) {
    let response = await reportAPI.getReportCount()
    if (response.status === 200 && response.data) {
      commit('set_report_count', {
        count: response.data.count
      })
    } 
  },
  async getReports({ commit }, { limit, page }) {
    let offset = limit * (page - 1);
    let response = await reportAPI.getReports({ limit, offset });
    if (response.status === 200 && response.data.length > 0) {
      commit('set_reports', {
        reports: response.data
      })
    }
  },
  clearReports({ commit }) {
    commit('clear_reports')
  }
}
const mutations = {
  set_reports(state, { reports }) {
    state.reports = reports.map( report => {
      return new ReportModel(report)
    })
  },
  set_report_count(state, { count }) {
    state.reportCount = count;
  },
  clear_reports(state) {
    state.reports = []
    state.reportCount = false
  }
}

/**
 * Export
 */
export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
}

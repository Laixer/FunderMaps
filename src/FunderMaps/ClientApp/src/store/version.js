/**
 * Import Dependency
 */

/**
 * Import API
 */
import versionAPI from 'api/version';

/**
 * Declare Variable
 */
const state = {
  version : null
}

const getters = {
  version: state => {
    return state.version;
  }
}
const actions = {
  async getVersion({ commit }) {
    let response = await versionAPI.getVersion();
    
    if (response.status === 200 && response.data) {
      commit('set_version', { version: response.data.version_string })
    }
  },
  clearVersion({ commit }) {
    commit('clear_version')
  }
}
const mutations = {
  set_version(state, { version }) {
    state.version = version
  },
  clear_version(state) {
    state.version = null;
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

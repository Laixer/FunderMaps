/**
 * Import Dependency
 */

/**
 * Import API
 */
import mapAPI from 'api/map';

/**
 * Declare Variable
 */
const state = {
  json : null
}

const getters = {
  mapData: state => {
    return state.json;
  },
  hasMapData: state => {
    return state.json !== null
  }
}
const actions = {
  async getMapData({ commit }) {
    let response = await mapAPI.getJson();
    
    if (response.status === 200 && response.data) {
      commit('set_json', { json: response.data })
    }
  },
  clearMapData({ commit }) {
    commit('clear_json')
  }
}
const mutations = {
  set_json(state, { json }) {
    state.json = json
  },
  clear_json(state) {
    state.json = null;
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

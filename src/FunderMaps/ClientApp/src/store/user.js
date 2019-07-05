/**
 * Import Dependency
 */
import UserModel from 'model/User'

/**
 * Import API
 */
import userAPI from 'api/user';

/**
 * Declare Variable
 */
const state = {
  user : null
}

const getters = {
  user: state => {
    return state.user;
  },
  isUserAvailable: state => {
    return state.user !== null
  }
}
const actions = {
  async getUser({ commit }) {
    let response = await userAPI.getUser();
    if (response.status === 200 && response.data) {
      commit('set_user', {
        user: response.data
      })
    }
  },
  async updateUser({ state }) {
    return await userAPI.updateUser(state.user)
  },
  clearUser({ commit }) {
    commit('clear_user')
  }
}
const mutations = {
  set_user(state, { user }) {
    state.user = new UserModel({
      given_name: user.given_name, 
      last_name: user.last_name, 
      avatar: user.avatar, 
      job_title: user.job_title, 
      phone_number: user.phone_number
    });
  },
  clear_user(state) {
    state.user = null
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

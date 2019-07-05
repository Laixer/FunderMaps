/**
 * Import Dependency
 */
import OrgUserModel from 'model/OrgUser'
import Vue from 'vue'

/**
 * Import API
 */
import orgUserAPI from 'api/orgUser'

/**
 * Declare Variable
 */
const state = {
  users : null
}

const getters = {
  orgUsers: state => {
    return state.users;
  },
  areOrganisationUsersAvailable: state => {
    return state.users !== null
  },
  getReviewers: state => {
    return state.users 
      ? state.users.filter(user => {
        return user.canReview()
      })
      : null;
  },
  getCreators: state => {
    return state.users 
    ? state.users.filter(user => {
      return user.canCreate()
    })
    : null;
  },
  getUserById: (state) => ({id}) => {
    return state.users.find(user => {
      return user.user.id === id
    })
  },
  getUserByEmail: (state) => ({email}) => {
    return state.users.find(user => {
      return user.user.email === email
    })
  }
}
const actions = {
  async getUsers({ commit }, { orgId }) {
    let response = await orgUserAPI.getOrganizationUsers({ orgId });
    if (response.status === 200 && response.data.length > 0) {
      commit('set_users', {
        users: response.data
      })
    } 
  },
  async updateUser({ commit }, { orgId, userData, role }) {
    let response = await orgUserAPI.updateOrganizationUser({ orgId, user: userData, role })
    commit('update_user', {
      userData, role
    })
    return response;
  },
  async createUser({ dispatch }, { orgId, userData, role }) {
    let response = await orgUserAPI.createOrganizationUser({ orgId, user: userData, role })
    if (response.status === 204) {
      dispatch('getUsers', {
        orgId
      })
    }
    return response;
  },
  async removeUser({ commit }, { orgId, id }) {
    let response = await orgUserAPI.removeOrganizationUser({ orgId, id })
    if (response.status === 204) {
      commit('remove_user', { id })
    }
  },
  clearUsers({ commit }) {
    commit('clear_users')
  }
}
const mutations = {
  set_users(state, { users }) {
    state.users = users.map(user => {
      return new OrgUserModel(user)
    })
  },
  update_user(state, { userData, role }) {
    let index = state.users.findIndex(user => {
      return user.user.id === userData.id
    })
    state.users[index].user = userData;
    state.users[index].role.name = role
  },
  remove_user(state, { id }) {
    let index = state.users.findIndex(user => {
      return user.user.id === id
    })
    Vue.delete(state.users, index);
  },
  clear_users(state) {
    state.users = null
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

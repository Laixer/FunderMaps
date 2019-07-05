/**
 * Import Dependency
 */
import OrganizationModel from 'model/Organization'
import OrganizationProposalModal from 'model/OrganizationProposalModal'
import Vue from 'vue'

/**
 * Import API
 */
import orgAPI from 'api/org';

/**
 * Declare Variable
 */
const state = {
  organization : null,
  // Admin
  organizations: null,
  proposals: null
}

const getters = {
  organization: state => {
    return state.organization;
  },
  isOrganizationAvailable: state => {
    return state.organization !== null;
  },
  getOrgId: state => {
    return (state.organization) 
      ? state.organization.getId()
      : null;
  },
  // Admin
  organisations: state => {
    return state.organizations
  },
  getOrgById: state => ({ id }) => {
    return state.organizations.find(org => {
      return org.id === id
    })
  },
  proposals: state => {
    return state.proposals
  },
  areProposalsAvailable: state => {
    return state.proposals !== null
  }
}
const actions = {
  async getOrganization({ commit }) {
    let response = await orgAPI.getOrganization();
    if (response.status === 200 && response.data.length > 0) {
      commit('set_organizations', {
        organizations: response.data
      })
    } 
  },
  clearOrg({ commit }) {
    commit('clear_org')
  },
  // Admin
  async updateOrg({ commit }, { orgId, data }) {
    let response = await orgAPI.updateOrganization({ orgId, data })
    if (response.status === 204) {
      commit('update_organization', {
        orgId, data
      })
    }
    return response;
  },
  async getProposals({ commit }) {
    let response = await orgAPI.getProposals();
    if (response.status === 200 && response.data) {
      commit('set_proposals', {
        proposals: response.data
      })
    } 
  },
  async removeProposal({ commit }, { token }) {
    let response = await orgAPI.removeProposal({ token });
    if (response.status === 204) {
      commit('remove_proposal', {
        token
      })
    } 
  },
  async createProposal({ commit }, { name, email }) {
    let response = await orgAPI.createProposal({ name, email });
    if (response.status === 200 && response.data) {
      commit('create_proposal', {
        proposal: response.data
      })
    } 
  },
  // New Organization
  async registerOrganization(context, { email, password, token }) {
    return await orgAPI.createOrganization({ email, password, token });
  }
}
const mutations = {
  set_organizations(state, { organizations }) {
    state.organization = new OrganizationModel(organizations[0]);
    state.organizations = organizations.map(org => new OrganizationModel(org))
  },
  clear_org(state) {
    state.organization = null
    state.organizations = null
  },
  // Admin
  update_organization(state, { orgId, data }) {
    let index = state.organizations.findIndex(org => {
      return org.id === orgId
    })
    state.organizations[index] = data
  },
  set_proposals(state, { proposals }) {
    state.proposals = proposals.map(
      proposal => new OrganizationProposalModal(proposal)
    )
  },
  remove_proposal(state, { token }) {
    let index = state.proposals.findIndex(proposal => {
      return proposal.token === token
    })
    Vue.delete(state.proposals, index);
  },
  create_proposal(state, { proposal }) {
    state.proposals.unshift(
      new OrganizationProposalModal(proposal)
    )
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

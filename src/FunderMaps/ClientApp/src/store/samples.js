/**
 * Import Dependency
 */
import SampleModel from 'model/Sample'
import Vue from 'vue'

/**
 * Import API
 */
import samplesAPI from 'api/samples';

/**
 * Declare Variable
 */
const state = {
  samples : []
}

const getters = {
  samples: state => {
    return state.samples
  },
  sampleCount: state => {
    return state.samples.length
  }
}
const actions = {
  async getSamples({ commit }, { reportId }) {
    let response = await samplesAPI.getSamples({ reportId });
    if (response.status === 200 && response.data.length > 0) {
      commit('set_samples', {
        samples: response.data
      })
    }
  },
  async clearSamples({ commit }) {
    commit('clear_samples')
  },
  async addUnsavedSample({ commit }) {
    commit('add_unsaved_sample')
  },
  async updateSample({ commit }, { id, data }) {
    let response = await samplesAPI.updateSample({ id, data });
    if (response.status === 204) {
      commit('update_sample', {
        id,
        data
      })
    }
  },
  async createSample({ commit }, { data }) {
    let response = await samplesAPI.createSample({ data });
    if (response.status === 200 && response.data) {
      commit('update_sample', {
        id: response.data.id,
        data: Object.assign(response.data, {
          creationstamp: data.creationstamp
        })
      })
    }
  },
  async deleteSample({ commit }, { id, creationstamp }) {
    if (id === '') {
      // not stored in API yet
      commit('delete_sample', { id, creationstamp })
    } else {
      let response = await samplesAPI.deleteSample({ id });
      if (response.status === 204) {
        commit('delete_sample', { id, creationstamp })
      }
    }
  }
}
const mutations = {
  set_samples(state, { samples }) {
    state.samples = samples.map( sample => {
      return new SampleModel({ sample, stored: true })
    })
  },
  clear_samples(state) {
    state.samples = [];
  },
  /**
   * Copy new sample from top most sample
   */
  add_unsaved_sample(state) {
    if (state.samples.length === 0) {
      state.samples = [
        new SampleModel({ sample: {}, stored: false, editorState: 'open' })
      ]
    } else {
      let address = Object.assign({}, state.samples[0].address);
      let sample = Object.assign({}, state.samples[0]);
      sample.id = ''
      sample.address = address;
      sample.address.building_number = ''
      sample.address.building_number_suffix = ''

      // used as alternative to 'id' reference for newly created items
      sample.creationstamp = Date.now() 
      
      state.samples.unshift(
        new SampleModel({
          sample,
          stored: false,
          editorState: 'open'
        })
      )
    }
  },
  /**
   * Update sample data in store (after positive API response)
   */
  update_sample(state, { id, data }) {
    let index = state.samples.findIndex(
      (sample) => sample.id === id
    )
    // For new samples we depend on an internal timestamp
    if (index === -1) {
      index = state.samples.findIndex(
        (sample) => sample.creationstamp === data.creationstamp
      )
    }
    if (index !== -1) {
      state.samples[index].updateValues({ data })
      state.samples[index].stored = true
    }
  },
  // Delete sample.
  delete_sample(state, { id, creationstamp }) {
    let index = state.samples.findIndex(
      (sample) => sample.id === id
    )
    // For new samples we depend on an internal timestamp
    if (index === -1) {
      index = state.samples.findIndex(
        (sample) => sample.creationstamp === creationstamp
      )
    }
    if (index !== -1) {
      Vue.delete(state.samples, index)
    }
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


import axios from '@/utils/axios'

// Collection
const getSamples = ({ reportId }) => {
  return axios.get(`/api/sample/report/${reportId}`)
}

// Single
const updateSample = ({ id, data }) => {
  return axios.put(`/api/sample/${id}`, data)
}
const createSample = ({ data }) => {
  return axios.post(`/api/sample/`, data)
}
const deleteSample = ({ id }) => {
  return axios.delete(`/api/sample/${id}`)
}

export default { 
  getSamples,

  updateSample,
  createSample,
  deleteSample
}

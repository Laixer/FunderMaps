
import axios from '@/utils/axios'

export default { 
  getOrganization: () => {
    return axios.get('/api/organization')
  },
  updateOrganization: ({ orgId, data }) => {
    return axios.put(`/api/organization/${orgId}`, data)
  },
  getProposals: () => {
    return axios.get('/api/organization_proposal')
  },
  removeProposal: ({ token }) => {
    return axios.delete(`/api/organization_proposal/${token}`)
  },
  createProposal: ({ name, email }) => {
    return axios.post(`/api/organization_proposal/`, {
      name, email
    })
  },
  createOrganization: ({ email, password, token }) => {
    return axios.post(`/api/organization_registration/proposal/${token}`, {
      email, password, role: 'superuser'
    })
  }
}

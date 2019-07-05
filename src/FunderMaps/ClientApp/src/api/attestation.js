
import axios from '@/utils/axios'

export default { 
  getPrincipalUsers: () => {
    return axios.post('/api/attestation/principal')
  },
  getOrganizations: () => {
    return axios.post('/api/attestation/organization')
  }
}


import axios from '@/utils/axios'

export default { 
  getOrganizationUsers: ({ orgId }) => {
    return axios.get(`/api/organization/${orgId}/user`)
  },
  updateOrganizationUser: ({ orgId, user, role }) => {
    return axios.put(`/api/organization/${orgId}/user/${user.id}`, 
      { user, role: { name: role } }
    )
  },
  createOrganizationUser: ({ orgId, user, role }) => {
    return axios.post(`/api/organization/${orgId}/user/`, 
      { email: user.email, password: user.password, role: role }
    )
  },
  removeOrganizationUser: ({ orgId, id }) => {
    return axios.delete(`/api/organization/${orgId}/user/${id}`)
  }
}

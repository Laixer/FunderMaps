
import axios from '@/utils/axios'

export default { 
  getUser: () => {
    return axios.get('/api/user')
  },
  updateUser: ({ 
    given_name, last_name, avatar, job_title, phone_number
  }) => {
    return axios.put('/api/user', {
      given_name, last_name, avatar, job_title, phone_number
    })
  }
}

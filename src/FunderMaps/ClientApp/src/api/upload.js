
import axios from '@/utils/axios'

export default { 
  postUpload: (file) => {
    return axios.post('/api/upload', { file })
  }
}

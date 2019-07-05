
import axios from '@/utils/axios'

export default {
  getVersion: async () => {
    return await axios.get('/api/version');
  }
}
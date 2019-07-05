
import axios from '@/utils/axios'

export default {
  getJson: async () => {
    return await axios.get('/api/map');
  }
}
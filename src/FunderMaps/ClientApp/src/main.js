import Vue from 'vue'
import App from '@/App.vue'
import router from '@/router'
import store from '@/store'

Vue.config.productionTip = false

// Services
import { refreshLogin } from '@/services/auth'

// Fonts (Gibson)
import "@/assets/sass/fonts.scss";

// Bootstrap config
import "@/assets/sass/bootstrap.scss";

// Bootstrap
import BootstrapVue from 'bootstrap-vue'
Vue.use(BootstrapVue)

// V-calender
import VCalender from 'v-calendar'
Vue.use(VCalender)

// Named Avatar Generator
import { config as configNamedAvatars } from 'utils/namedavatar'
configNamedAvatars()


new Vue({
  router,
  store,
  data() {
    return {
      timer: null
    }
  },
  created () {
    this.timer = setInterval(() => {
      refreshLogin()
    }, 60000 * 10)
  },
  render: h => h(App)
}).$mount('#app')

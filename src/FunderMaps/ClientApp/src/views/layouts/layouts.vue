<script>
import defaultLayout from './default'
import adminLayout from './admin'
import loginLayout from './login'
import mapLayout from './map'

/**
 * Determines which layout to use based on the route meta data
 */
export default {
  name: "Layouts",
  components: {
    defaultLayout, adminLayout, loginLayout, mapLayout
  },
  render(h) {
    let layout = (this.$route.meta && this.$route.meta.layout) 
      ? this.$route.meta.layout
      : 'default';

    if (layout === 'map') {
      return h('mapLayout', this.$slots.default)
    }
    if (layout === 'login') {
      return h('loginLayout', this.$slots.default)
    }
    if (layout === 'admin') {
      return h('adminLayout', this.$slots.default)
    }

    return h('defaultLayout', this.$slots.default)
  }
}
</script>

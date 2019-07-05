
<script>
// TODO: Unused for now
import { image } from 'helper/assets'

export default {
  name: 'FadedBackgroundCover',
  functional: true,
  props: {
    image: {
      type: String,
      default: 'login/background.png'
    }
  },
  render(h, context) {
    return h('div', {
      class: {
        'FadedBackgroundCover' : true
      },
      style: {
        backgroundImage: 'url(' + image({ name: context.props.image } ) + ')'
      },
    })
  }
}
</script>

<style lang="scss">
@import '@/assets/sass/variables.scss';

.FadedBackgroundCover {
  position: absolute;
  top: 0;
  left: 0;
  height: 100%;
  width: 100%;
  background-color: theme-color('primary');
  background-repeat: no-repeat;
  background-size: cover;
}
</style>


<script>
import { image } from 'helper/assets'
import { combineClassLists } from 'helper/render'

export default {
  name: 'Logo',
  functional: true,
  props: {
    company: {
      type: String,
      default: 'FunderMaps'
    }
  },
  render(h, context) {
    return h('img', {
      attrs: {
        src: image({ name: 'logo.svg' })
      },
      class: combineClassLists({ 
        classList: { Logo: true },
        context
      }),
      domProps: {
        alt: (context.props.company + ' logo').trim()
      }
    })
  }
}
</script>


<script>

import { combineClassLists } from 'helper/render'

export default {
  name: 'Copyright',
  functional: true,
  props: {
    company: {
      type: String,
      default: ''
    }
  },
  render(h, context) {
    
    return h('span', {
      class: combineClassLists({
        classList: { copyright: true },
        context
      }),
      domProps: {
        innerHTML: 
          ('&copy; Copyright ' 
          + new Date().getFullYear() 
          + ' ' + context.props.company)
          .trim()
      }
    })
  }
}
</script>
<style>
.copyright {
  font-size: 12px;
  color: white;
}
</style>

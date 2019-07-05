<script>
export default {
  name: 'PasswordResetLink',
  functional: true,
  props: {
    mailto: {
      type: String,
      default: 'info@zanhos.com'
    }
  },
  render(h, context) {
    let classList = {}
    classList['text-muted'] = true;

    return h('a', {
      attrs: {
        href: 'mailto:' + context.props.mailto
      },
      class: classList,
      domProps: {
        innerHTML: 'Wachtwoord vergeten?'
      }
    })
  }
}
</script>
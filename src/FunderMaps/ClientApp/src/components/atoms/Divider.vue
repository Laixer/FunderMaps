<script>
export default {
  name: 'Divider',
  functional: true,
  render(h) {
    return h('hr', {
      class: {
        'Divider': true
      }
    })
  }
}
</script>

<style lang="scss">
.Divider {
  background-color: #E8EAF1;
  height: 1px;
  width: 100%;
  border: none;
  padding: 0;
  margin: 20px 0
}
</style>

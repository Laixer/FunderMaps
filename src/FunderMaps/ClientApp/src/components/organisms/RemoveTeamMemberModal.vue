<template>
  <b-modal 
    id="modal-remove-teammember"
    ref="modal" 
    centered 
    okTitle='Verdwijderen'
    cancelTitle='Annuleren'
    @ok="onOk"
    @show="onShow"
    title="Teamlid verwijderen">
    <template slot="default">
      <Feedback :feedback="feedback" />
      Bevestig dat het account van <strong>{{ name }}</strong> verwijderd moet worden. 
      <br />
      <br />
      Let op, dit kan niet ongedaan gemaakt worden.
    </template>
  </b-modal>
</template>

<script>

import Feedback from 'atom/Feedback'

import { mapGetters, mapActions } from 'vuex'
import timeout from 'mixin/timeout'

export default {
  components: {
    Feedback
  },
  mixins: [ timeout ],
  props: {
    id: {
      type: String,
      default: null
    },
    orgId: {
      type: [ String, Number ],
      default: ''
    }
  },
  data() {
    return {
      name: '',
      isDisabled: false,
      feedback: {}
    }
  },
  computed: {
    ...mapGetters('orgUsers', [
      'getUserById'
    ]),
    orgUser() {
      return this.getUserById({ id: this.id })
    }
  },
  watch: {
    orgUser(orgUser) {
      if (orgUser) {
        this.name = orgUser.getUserName()
      }
    }
  },
  methods: {
    ...mapActions('orgUsers', [
      'removeUser'
    ]),
    onShow() {
      this.feedback = { show: false }
    },
    async onOk(e) {
      e.preventDefault()
      if (this.isDisabled) {
        this.feedback = {
          variant: 'info', 
          message: 'Reeds bezig met verwerken...'
        }
        return;
      }
      this.isDisabled = true;
      this.feedback = {
        variant: 'info', 
        message: 'Bezig met verwerken...'
      }
      
      try {
        await this.removeUser({
          orgId: this.orgId,
          id: this.id
        })
        this.feedback = {
          variant: 'success',
          message: 'Het teamlid is verwijderd'
        }
        this.setTimeout(() => {
          this.$refs.modal.hide()
          this.isDisabled = false;
        }, 500)
        
      } catch (err) {
        this.isDisabled = false;
        this.feedback = {
          variant: 'danger', 
          message: 'Het teamlid kon niet worden verwijderd'
        }
      }
    }
  }
}
</script>

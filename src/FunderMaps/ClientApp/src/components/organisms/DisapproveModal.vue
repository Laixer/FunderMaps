<template>
  <b-modal 
    ref="modal"
    id="report-disapprove" 
    title="Reden voor afkeuren"
    @ok="onOk">
    <Feedback :feedback="feedback" />
    <Form 
      ref="form" 
      autocomplete="off"
      @error="handleError"
      @submit="handleSubmit">
      
      <FormField 
        v-model="fields.reason.value"
        v-bind="fields.reason" />
    </Form>

  </b-modal>
</template>

<script>
import { required } from 'vuelidate/lib/validators';

import Feedback from 'atom/Feedback'
import Form from 'molecule/form/Form'
import FormField from 'molecule/form/FormField'

import { mapActions } from 'vuex'
import fields from 'mixin/fields'
import timeout from 'mixin/timeout'

export default {
  name: 'DisapproveModal',
  components: {
    Feedback, Form, FormField
  },
  mixins: [ fields, timeout ],
  data() {
    return {
      isDisabled: false,
      feedback: {},
      fields: {
        reason: {
          value: '',
          type: 'textarea',
          label: 'Omschrijving',
          validationRules: {
            required
          },
          disabled: false
        }
      }
    }
  },
  methods: {
    ...mapActions('report', [
      'rejectReport'
    ]),
    onOk(e) {
      e.preventDefault()
      if ( ! this.isDisabled) {
        this.$refs.form.submit()
      }
    },
    handleError() {
      this.feedback = {
        variant: 'danger', 
        message: 'Controleer de validatie feedback a.u.b.'
      }
    },
    async handleSubmit() {
      this.isDisabled = true;
      this.disableAllFields();
      this.feedback = {
        variant: 'info',
        message: 'Het bericht wordt verstuurd...'
      }

      try {
        await this.rejectReport({ 
          message: this.fieldValue('reason')
        })

        this.feedback = {
          variant: 'success',
          message: 'Het bericht is verzonden'
        }
        this.$emit('disapprove')
        
        this.setTimeout(() => {
          this.$refs.modal.hide()
          this.isDisabled = false
          this.enableAllFields()
        }, 600);
        
      } catch (err) {
        this.feedback = {
          variant: 'danger', 
          message: 'Het bericht kon niet worden verzonden'
        }
        this.isDisabled = false;
        this.enableAllFields()
      }
    }
  }
}
</script>

<style>

</style>

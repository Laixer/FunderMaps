<template>
  <b-modal 
    id="modal-new-proposal"
    ref="modal" 
    centered 
    okTitle='Aanmaken'
    cancelTitle='Annuleren'
    @ok="onOk"
    @show="onShow"
    title="Nieuw voorstel">
    <template slot="default">
      <Feedback :feedback="feedback" />
      <Form 
        ref="form" 
        autocomplete="off"
        @error="handleError"
        @submit="handleSubmit">

        <FormField 
          label="Naam"
          v-model="fields.name.value"
          v-bind="fields.name" />
        <FormField 
          label="Email"
          type="text"
          v-model="fields.email.value"
          v-bind="fields.email" />

      </Form>
    </template>
  </b-modal>
</template>

<script>

import { required, minLength, email } from 'vuelidate/lib/validators';

import Feedback from 'atom/Feedback'
import Form from 'molecule/form/Form'
import FormField from 'molecule/form/FormField'

import { mapActions } from 'vuex'
import fields from 'mixin/fields'
import timeout from 'mixin/timeout'

export default {
  components: {
    FormField, Form, Feedback
  },
  mixins: [ fields, timeout ],
  data() {
    return {
      isDisabled: false,
      feedback: {},
      fields: {
        email: {
          value: "",
          placeholder: 'naam@bedrijf.nl',
          validationRules: {
            required, email
          },
          disabled: false
        },
        name: {
          value: "",
          validationRules: {
            required,
            minLength: minLength(2)
          },
          disabled: false
        }
      }
    }
  },
  methods: {
    ...mapActions('org', [
      'createProposal'
    ]),
    onShow() {
      this.feedback = { show: false }
    },
    onOk(e) {
      e.preventDefault()
      if ( ! this.isDisabled) {
        this.$refs.form.submit()
      }
    },
    async handleSubmit() {
      this.disableAllFields()
      this.isDisabled = true;
      this.feedback = {
        variant: 'info', 
        message: 'Bezig met opslaan...'
      }
      
      try {
        // Make a copy, and add form field data
        await this.createProposal({
          name: this.fieldValue('name'), 
          email: this.fieldValue('email')
        })
        this.feedback = {
          variant: 'success',
          message: 'Het nieuwe voorstel is aangemaakt'
        }

        this.setTimeout(() => {
          this.$refs.modal.hide()
          this.isDisabled = false
          this.enableAllFields()
          this.clearAllFieldValues()
        }, 500)
        
      } catch (err) {
        this.feedback = {
          variant: 'danger', 
          message: 'Het voorstel kon niet aangemaakt worden.'
        }
        this.isDisabled = false;
        this.enableAllFields()
        this.clearAllFieldValues()
      }
      
    },
    handleError() {
      this.feedback = {
        variant: 'danger', 
        message: 'Controleer de validatie berichten a.u.b.'
      }
    }
  }
}
</script>

<template>
  <b-modal 
    id="modal-new-teammember"
    ref="modal" 
    centered 
    okTitle='Aanmaken'
    cancelTitle='Annuleren'
    @ok="onOk"
    @show="onShow"
    title="Nieuw teamlid">
    <template slot="default">
      <Feedback :feedback="feedback" />
      <Form 
        ref="form" 
        autocomplete="off"
        @error="handleError"
        @submit="handleSubmit">

        <FormField 
          label="Email"
          type="text"
          v-model="fields.email.value"
          v-bind="fields.email" />

        <FormField 
          label="Wachtwoord"
          v-model="fields.password.value"
          v-bind="fields.password" />

        <Divider />

        <FormField 
          label="Rol"
          type="select"
          v-model="fields.role.value"
          v-bind="fields.role" />

      </Form>
    </template>
  </b-modal>
</template>

<script>

import { required, minLength, email } from 'vuelidate/lib/validators';

import Divider from 'atom/Divider'

import Feedback from 'atom/Feedback'
import Form from 'molecule/form/Form'
import FormField from 'molecule/form/FormField'

import { mapActions } from 'vuex'
import fields from 'mixin/fields'
import timeout from 'mixin/timeout'

import { userRoles } from 'config/roles'

export default {
  components: {
    Divider, FormField, Form, Feedback
  },
  mixins: [ fields, timeout ],
  props: {
    orgId: {
      type: [ String, Number ],
      default: ''
    }
  },
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
        role: {
          value: null,
          validationRules: {
            required
          },
          disabled: false,
          options: [
            { value: null, text: 'Selecteer een optie' }
          ].concat(userRoles),
        },
        password: {
          value: '',
          type: 'password',
          validationRules: {
            required,
            minLength: minLength(7)
          },
          autocomplete: "new-password",
          disabled: false
        }
      }
    }
  },
  methods: {
    ...mapActions('orgUsers', [
      'createUser'
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
        await this.createUser({
          orgId: this.orgId,
          userData: this.fieldValues([
            'email', 'password'
          ]),
          role: this.fieldValue('role')
        })
        this.feedback = {
          variant: 'success',
          message: 'Het nieuwe teamlid is geregistreerd'
        }

        this.setTimeout(() => {
          this.$refs.modal.hide()
          this.isDisabled = false
          this.enableAllFields()
        }, 500)
        
      } catch (err) {
        this.feedback = {
          variant: 'danger', 
          message: 'Het teamlid kon niet worden geregistreerd. Mogelijk is het e-mail adres reeds in gebruik.'
        }
        this.isDisabled = false;
        this.enableAllFields()
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

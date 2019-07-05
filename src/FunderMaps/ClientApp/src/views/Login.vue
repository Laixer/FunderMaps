<template>
  <div class="LoginForm">
    <h1 class="mb-5">Inloggen</h1>

    <Form @submit="handleSubmit">
      
      <Feedback :feedback="feedback" />

      <FormField 
        v-model="fields.email.value"
        v-bind="fields.email" />
      <FormField 
        v-model="fields.password.value"
        v-bind="fields.password"
        class="mt-2" />
      
      <button 
        type="submit"
        :disabled='isDisabled'
        class="btn btn-success btn-lg btn-block rounded-pill font-weight-bold border-0 mt-3 p-3">
        Aanmelden
      </button>
    </Form>

    <div class="d-flex justify-content-center mt-5">
      <PasswordResetLink mailto="info@zanhos.com" />
    </div>
  </div>
</template>

<script>
import { login, getLastUserEmail } from 'service/auth'
import PasswordResetLink from 'atom/branding/PasswordResetLink';

import { required, email } from 'vuelidate/lib/validators';

import { mapActions } from 'vuex'

import Feedback from 'atom/Feedback'
import Form from 'molecule/form/Form'
import FormField from 'molecule/form/FormField'

import fields from 'mixin/fields'

export default {
  name: 'LoginForm',
  components: {
    PasswordResetLink,
    Feedback,
    Form,
    FormField
  },
  mixins: [ fields ],
  data() {
    return {
      feedback: {},
      isDisabled: false,
      fields: {
        email: {
          label: "E-mail",
          autocomplete: "username",
          type: "email",
          placeholder: "naam@bedrijf.nl",
          value: getLastUserEmail(),
          validationRules: {
            required, email
          },
          disabled: false
        },
        password: {
          label: "Wachtwoord",
          autocomplete: "current-password",
          type: "password",
          value: '',
          validationRules: {
            required
          },
          disabled: false
        }
      }
    }
  },
  methods: {
    ...mapActions('org', [
      'getOrganization'
    ]),
    ...mapActions('user', [
      'getUser'
    ]),
    handleSubmit(e) {
      e.preventDefault();
      
      this.disableAllFields()
      this.isDisabled = true
      this.feedback = {
        variant: 'info', 
        message: 'Bezig met inloggen...'
      }

      login({
        email:    this.fieldValue('email'), 
        password: this.fieldValue('password')
      })
        .then(() => {
          this.$router.push({ name: 'dashboard' })
        })
        .catch((err) => {
          this.enableAllFields()
          this.isDisabled = false

          if (err.response && err.response.status === 401) {
            this.feedback = {
              variant: 'danger', 
              message: 'Uw inlog gegevens zijn ongeldig'
            }
          } else {
            this.feedback = {
              variant: 'danger', 
              message: 'Onbekende fout. Probeer het later nog eens.'
            }
          }
        });
    },
  }
}
</script>

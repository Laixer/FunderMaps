<template>
  <div class="LoginForm">
    <h1 class="mb-5">Welkom!</h1>
    <p>Maak een beheerdersaccount aan om te beginnen</p>

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
        Aanmaken
      </button>
    </Form>
  </div>
</template>

<script>
import { required, minLength, email } from 'vuelidate/lib/validators';

import Feedback from 'atom/Feedback'
import Form from 'molecule/form/Form'
import FormField from 'molecule/form/FormField'

import { mapActions } from 'vuex'

import fields from 'mixin/fields'

export default {
  name: 'LoginForm',
  components: {
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
          placeholder: "",
          value: '',
          validationRules: {
            required, email
          },
          disabled: false
        },
        password: {
          label: "Wachtwoord",
          autocomplete: "new-password",
          type: "password",
          value: '',
          validationRules: {
            required,
            minLength: minLength(7)
          },
          disabled: false
        }
      }
    }
  },
  methods: {
    ...mapActions('org', [
      'registerOrganization'
    ]),
    handleSubmit(e) {
      e.preventDefault();
      
      this.disableAllFields()
      this.isDisabled = true
      this.feedback = {
        variant: 'info', 
        message: 'Bezig met registeren...'
      }

      this.registerOrganization({
        email:    this.fieldValue('email'), 
        password: this.fieldValue('password'),
        token:    this.$route.params.token
      })
        .then(() => {
          this.$router.push({ name: 'login' })
        })
        .catch((err) => {
          this.enableAllFields()
          this.isDisabled = false

          if (err.response && err.response.status === 401) {
            this.feedback = {
              variant: 'danger', 
              message: 'Uw gegevens zijn ongeldig'
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

<style>
.LoginForm p {
  font-size: 1.2rem;
}
</style>
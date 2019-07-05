<template>
  <b-modal 
    id="modal-teammember"
    ref="modal" 
    centered 
    okTitle='Opslaan'
    cancelTitle='Annuleren'
    @ok="onOk"
    @show="onShow"
    :title="'Teamlid: ' + name">
    <template slot="default">
      <Feedback :feedback="feedback" />
      <Form 
        ref="form" 
        autocomplete="off"
        @error="handleError"
        @submit="handleSubmit">

        <FormField 
          label="Voornaam"
          type="text"
          v-model="fields.given_name.value"
          v-bind="fields.given_name" />

        <FormField 
          label="Achternaam"
          type="text"
          v-model="fields.last_name.value"
          v-bind="fields.last_name" />

        <FormField 
          label="Email"
          type="text"
          v-model="fields.email.value"
          v-bind="fields.email" />

        <FormField 
          label="Functie"
          type="text"
          v-model="fields.job_title.value"
          v-bind="fields.job_title" />

        <FormField 
          label="Telefoon"
          type="text"
          v-model="fields.phone_number.value"
          v-bind="fields.phone_number" />
        <Divider />

        <FormField 
          label="Rol"
          type="select"
          v-model="fields.role.value"
          v-bind="fields.role" />
        <!-- <Divider /> -->

        <!-- <FormField 
          label="Password"
          type="password"
          v-model="fields.password.value"
          v-bind="fields.password"
          autocomplete="new-password" /> -->

      </Form>
    </template>
  </b-modal>
</template>

<script>

import { required, minLength, email } from 'vuelidate/lib/validators';

import { image } from 'helper/assets'
import Divider from 'atom/Divider'

import Feedback from 'atom/Feedback'
import Form from 'molecule/form/Form'
import FormField from 'molecule/form/FormField'

import { mapGetters, mapActions } from 'vuex'
import fields from 'mixin/fields'
import timeout from 'mixin/timeout'

import { userRoles } from 'config/roles'

export default {
  components: {
    Divider, FormField, Form, Feedback
  },
  mixins: [ fields, timeout ],
  props: {
    id: {
      type: String,
      default: null
    },
    orgId: {
      type: [String, Number],
      default: ''
    }
  },
  data() {
    return {
      name: '',
      isDisabled: false,
      feedback: {},
      fields: {
        given_name: {
          value: "",
          validationRules: {
            minLength: minLength(2)
          },
          disabled: false
        },
        last_name: {
          value: "",
          validationRules: {
            minLength: minLength(2)
          },
          disabled: false
        },
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
        job_title: {
          value: "",
          validationRules: {
          },
          disabled: false
        },
        phone_number: {
          value: "",
          validationRules: {
          },
          disabled: false
        }
      }
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
      if (orgUser){
        this.setFieldValues([
          { name: 'email', value: orgUser.user.email },
          { name: 'role', value: orgUser.role.name },
          { name: 'given_name', value: orgUser.user.given_name },
          { name: 'last_name', value: orgUser.user.last_name },
          { name: 'job_title', value: orgUser.user.job_title },
          { name: 'phone_number', value: orgUser.user.phone_number }
        ])

        this.name = orgUser.getUserName()
      }
    }
  },
  methods: {
    ...mapActions('orgUsers', [
      'updateUser'
    ]),
    image,
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
        let userData = Object.assign({}, this.orgUser.user, this.fieldValues([
          'email', 'job_title', 'last_name', 'given_name', 'phone_number'
        ]));
        await this.updateUser({
          orgId: this.orgId,
          userData,
          role: this.fieldValue('role')
        })
        this.feedback = {
          variant: 'success',
          message: 'De wijzigingen zijn opgeslagen'
        }

        this.setTimeout(() => {
          this.$refs.modal.hide()
          this.isDisabled = false
          this.enableAllFields()
        }, 500)
        
      } catch (err) {
        this.feedback = {
          variant: 'danger', 
          message: 'Wijzigingen zijn niet opgeslagen'
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

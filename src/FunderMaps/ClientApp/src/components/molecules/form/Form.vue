<template>
  <form @submit.prevent="handleSubmit" >
    <slot />

    <!-- Required to allow the form to be submitted programmatically -->
    <button ref="btn" style="display:none" type="submit" />
  </form>
</template>

<script>
/**
 * This Form component helps automate form validation
 * 
 *  It is a simplification of a more customized solution developed for another 
 *  project and does not handle field values or the submit process.
 */
export default {
  // Provide a mechanism for field components to register themselves to the form
  provide: function() { 
    return {
      registerFormField: this.registerFormField
    }
  },
  data() {
    return {
      fields: []
    }
  },
  methods: {
    /**
     * All fields contained in this form should register themselves in order 
     * to be processed by the mechanism provided by this component.
     * 
     * Note: every field should have a `validate`, `isValid` & `resetValidation` method.
     */
    registerFormField({ validate, isValid, resetValidation }) {
      this.fields.push({ validate, isValid, resetValidation })
    },
    /**
     * Run the validation on every registered field
     */
    validate() {
      this.fields.forEach(field => {
        field.validate()
      })
    },
    /**
     * Are all registered fields valid?
     */
    isValid() {
      return this.fields.every(field => {
        return field.isValid()
      })
    },
    /**
     * Reset the validation mechanism
     */
    resetValidation() {
      this.fields.forEach(field => {
        field.resetValidation()
      })
    },
    /**
     * Allow the form to be submitted programmatically
     */
    submit() {
      // Note: 
      //  Why not ref the form itself and use submit() on the form DOM element?
      //  Because then the submit event handler bound by vue is not triggered...
      this.$refs.btn.click()
    },
    /**
     * Capture the submit event, handle validation, and then either pass on 
     * the event on success, or trigger an error event instead.
     */
    handleSubmit(e) {
      this.validate()
      if (this.isValid()) {
        this.$emit('submit', e)
      } else {
        this.$emit('error', e)
      }
    }
  }
}
</script>
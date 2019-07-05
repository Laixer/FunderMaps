<template>
  <b-form-group
    class="FormField"
    :label="label"
    :state="state">

    <b-form-select 
      v-if="type === 'select'"
      v-model="fieldValue" 
      :options="options"
      :state="state" 
      :disabled="disabled"
      @input="handleInput"
      @blur="handleBlur" />
    
    <div 
      class="FormField--choice d-flex align-items-center w-100"
      v-else-if="type === 'radio' && options.length === 2">
      <b-form-radio 
        class="check"
        v-model="fieldValue"
        :state="state"
        :disabled="disabled" 
        :value="options[0].value">
        {{ options[0].text }}
      </b-form-radio>
      <b-form-radio 
        class="ml-3 none"
        v-model="fieldValue"
        :state="state"
        :disabled="disabled" 
        :value="options[1].value">
        {{ options[1].text }}
      </b-form-radio>
    </div>

    <b-form-radio-group
      v-else-if="type === 'radio'"
      v-model="fieldValue"
      :options="options"
      :state="state" 
      :placeholder="placeholder"
      :autocomplete="autocomplete"
      :disabled="disabled"
      @input="handleInput"
      @blur="handleBlur"
    ></b-form-radio-group>
    
    <v-date-picker
      locale="nl"
      :popover="{ visibility: 'click' }"
      v-else-if="type === 'datepicker'"
      @input="handleDatepickerInput"
      v-model="datepickerValue">
      <b-form-input 
        v-model="fieldValue" 
        type="text"
        :style="datepickerStyle"
        :state="state" 
        :placeholder="placeholder"
        :autocomplete="autocomplete"
        :disabled="disabled"
        @blur="handleDatepickerBlur"
        trim />
    </v-date-picker>

    <b-form-textarea
      v-else-if="type === 'textarea'"
      v-model="fieldValue" 
      :type="type"
      :state="state" 
      :placeholder="placeholder"
      :autocomplete="autocomplete"
      :disabled="disabled"
      @input="handleInput"
      @blur="handleBlur"
      :rows="rows"
      trim 
    ></b-form-textarea>

    <b-form-input 
      v-else
      v-model="fieldValue" 
      :type="type"
      :state="state" 
      :placeholder="placeholder"
      :autocomplete="autocomplete"
      :disabled="disabled"
      @input="handleInput"
      @blur="handleBlur"
      trim />

    <template slot="invalid-feedback">
      {{ invalidFeedback }}
    </template>
  </b-form-group>
</template>

<script>
import { validationMixin } from 'vuelidate'

export default {
  name: 'FormField',
  inject: ['registerFormField'],
  props: {
    value: {
      type: [String, Boolean, Number, Date],
      default: ''
    },
    label: {
      type: String,
      default: ''
    },
    disabled: {
      type: Boolean,
      default: false
    },
    type: {
      type: String,
      default: 'text'
    },
    autocomplete: {
      type: String,
      default: 'off'
    },
    placeholder: {
      type: String,
      default: ''
    },
    validationRules: {
      type: Object,
      default: () => {
        return {}
      }
    },
    // Makes it possible to disable validation
    novalidate: {
      type: Boolean,
      default: false
    },
    // Used by `type === select & radio`
    options: {
      type: Array,
      default: () => {
        return []
      }
    },
    // Used by `type === textarea`
    rows: {
      type: Number,
      default: 5,
    }
  },
  mixins: [ validationMixin ],
  data() {
    return {
      fieldValue: '',
      datepickerValue: '',
      blurred: false
    }
  },
  computed: {
    state() {
      if (this.novalidate) {
        return null
      }
      return this.$v.fieldValue.$dirty ? !this.$v.fieldValue.$error : null
    },
    invalidFeedback() {
      let validator = this.$v.fieldValue

      // Go over the validation rules, and return the 
      // name of the first rule that is broken
      let match = Object.keys(this.validationRules)
        .find((rule) => {
          return ! validator[rule]
        })

      if (match === -1) {
        return ''; // apparently no rules are broken?
      }

      let params = validator.$params

      switch (match) {
        case 'email':
          return 'Voer a.u.b. een geldig e-mail adres in'
        case 'required':
          return 'Dit is een vereist veld'
        case 'minLength':
          return 'Uw invoer moet minimaal ' + params.minLength.min +' karakters zijn.'
        case 'maxLength': 
          return 'Uw invoer mag maximaal ' + params.maxLength.max + ' karakters zijn.'
        
      }

      return 'Controleer uw invoer a.u.b.'
    },
    /**
     * V-Calender overrides the validation border set by Bootstrap.
     *  These style enforce the Bootstrap styles
     */
    datepickerStyle() {
      // TODO: Use bootstrap variables
      let color = '#ced4da';
      switch(this.state) {
        case true: 
          color = '#29CC8B'
          break;
        case false: 
          color = '#FF4E4E'
          break;
      }
      return { 
        'borderWidth': '1px',
        'borderColor' : color
      }
    }
  },
  watch: {
    /**
     * Observe changes to the value prop
     */
    value(newValue) {
      this.fieldValue = newValue
    },
    /**
     * Update Date Picker if appropriate
     *  And inform the parent when a date is invalid
     */
    fieldValue(newValue) {
      if (this.type === 'datepicker') {
        let date = new Date(newValue)
        if (! isNaN(date.getTime())) {
          this.datepickerValue = date
        } else {
          // NL formatting
          date = new Date(this.formatDate(newValue));
          if (! isNaN(date.getTime())) {
            this.datepickerValue = date
          } else {
            // Handle an invalid formatted date
            this.$emit('input', '')
          }
        }
      }
    }
  },
  created() {
    // On creation, set the internal field value
    this.fieldValue = this.value;

    // If no value was passed, and this is a datepicker, default to today
    if (this.type === 'datepicker' && ! this.value) {
      this.fieldValue = this.formatDateToString(new Date())
    }

    // If contained within a Form component, register the form field
    if (this.registerFormField) {
      this.registerFormField(this);
    }
  },
  /**
   * Vuelidate validation rules. Set through the validationRules prop
   */
  validations() {
    return (this.validationRules) 
      ? { fieldValue: this.validationRules } 
      : {}
  },
  methods: {
    // Start validation after initial blur
    handleBlur() {
      if (this.blurred === false) {
        this.validate();
      }
      this.blurred = true;
    },
    handleInput(value) {
      if (this.blurred || this.type === 'select') {
        this.validate();
      }
      this.$emit('input', value)
    },
    validate() {
      if (!this.novalidate) {
        this.$v.$touch()
      }
    },
    isValid() {
      // Not ideal, but otherwise form processing would cancel
      return this.novalidate ? true : !! this.state;
    },
    resetValidation() {
      this.$v.$reset()
    },
    /*****
     * DATE PICKER
     */
    handleDatepickerBlur() {
      this.validate();
    },
    handleDatepickerInput(date) {
       this.fieldValue = this.formatDateToString(date) 
       this.$emit('input', this.fieldValue)
    },
    formatDateToString(date) {
      let months = [
        "Jan", "Feb", "Mar", "Apr", "Mei", "Jun", 
        "Jul", "Aug", "Sep", "Okt", "Nov", "Dec" ]
      return date.getDate() 
        + ' ' + months[date.getMonth()] 
        + ' ' + date.getFullYear();
    },
    // TODO: Not ideal, but works for now
    formatDate(date) { // e.g. 4 Jun 2019
      let months = [
        "Jan", "Feb", "Mar", "Apr", "Mei", "Jun", 
        "Jul", "Aug", "Sep", "Okt", "Nov", "Dec" ]
      date = date.split(' ');
      return date[2] + '-' 
        + ('0'+ (months.indexOf(date[1]) + 1)).slice(-2) 
        + '-' + ('0' + date[0]).slice(-2)
    }
  }
}
</script>

<style lang="scss">
.FormField {
  font-size: 16px;

  input {
    color: rgba(53, 64, 82, 0.5);
  }

  label, legend {
    color: #7F8FA4;
    text-transform: uppercase;
    padding-bottom: 0;
  }

  &--choice {
    height: 33px;

    .custom-control-input:checked ~ .custom-control-label::before {
      border-color: transparent;
      background-color: transparent;
    }
    .custom-radio.check .custom-control-input:checked ~ .custom-control-label::after {
      background-color: white;
      background-image: url('../../../assets/icons/Check-icon.svg');
      background-size: cover;
    }
    .custom-radio.none .custom-control-input:checked ~ .custom-control-label::after {
      background-color: white;
      background-image: url('../../../assets/icons/None-icon.svg');
      background-size: cover;
    }
  }
}
</style>
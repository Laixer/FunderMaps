
export default {
  // Note: this `mixin` assumes the component has `fields` set as data attribute!
  methods: {

    /**
     * Get Field Value
     */
    fieldValue(name) {
      return this.fields[name].value;
    },
    fieldValues(names) {
      return names.reduce((obj, name) => { 
        obj[name] = this.fields[name].value; 
        return obj
      }, {})
    },
    allFieldValues() {
      return this.fieldValues(Object.keys(this.fields))
    },

    /**
     * Set Field values
     */
    setFieldValues(fieldValues) {
      if (Array.isArray(fieldValues)) {
        fieldValues.forEach(this.setFieldValue);
      } else {
        Object.keys(fieldValues).forEach(key => {
          this.setFieldValue({ name: key, value: fieldValues[key] })
        })
      }
    },
    setFieldValue({name, value}) {
      this.fields[name].value = value;
    },

    /**
     * Clear fields
     */
    clearFieldValue({name}) {
      // TODO: Check type (select, radio, checkbox || text, etc.)
      this.setFieldValue({ name, value: '' })
    },
    clearFieldValues(names) {
      names.forEach(name => this.clearFieldValue({name}))
    },
    clearAllFieldValues() {
      this.clearFieldValues(Object.keys(this.fields))
    },

    /**
     * Field disabled state
     */
    disableField(name) {
      this.fields[name].disabled = true
    },
    enableField(name) {
      this.fields[name].disabled = false
    },
    disableFields(names) {
      names.forEach(name => this.disableField(name))
    },
    enableFields(names) {
      names.forEach(name => this.enableField(name))
    },
    disableAllFields() {
      this.disableFields(Object.keys(this.fields))
    },
    enableAllFields() {
      this.enableFields(Object.keys(this.fields))
    },
    toggleFieldState(name) {
      this.fields[name].disabled = !this.fields[name].disabled
    }
  }
}
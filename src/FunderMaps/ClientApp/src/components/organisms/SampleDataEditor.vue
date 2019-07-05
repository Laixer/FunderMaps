<template>
  <Form 
    ref="form"
    class="py-4 px-5"
    @submit="handleSubmit"
    @error="handleFormError">
    
    <Feedback :feedback="feedback" />

    <div class="form-row mb-3">
      <FormField 
        v-model="fields.street_name.value"
        v-bind="fields.street_name"
        class="col-md-8" />
      <FormField 
        v-model="fields.building_number.value"
        v-bind="fields.building_number"
        class="col-md-2" />
      <FormField 
        v-model="fields.building_number_suffix.value"
        v-bind="fields.building_number_suffix"
        class="col-md-2" />
    </div>

    <div class="form-row mb-3">
      <FormField 
        v-model="fields.foundation_type.value"
        v-bind="fields.foundation_type"
        class="col-md-5" />
      <FormField 
        v-model="fields.substructure.value"
        v-bind="fields.substructure"
        class="col-md-5" />
      <FormField 
        v-model="fields.built_year.value"
        v-bind="fields.built_year"
        class="col-md-2" />
    </div>

    <div class="form-row">
      <FormField 
        v-model="fields.monitoring_well.value"
        v-bind="fields.monitoring_well"
        class="col-md-6" />
      <FormField 
        v-model="fields.cpt.value"
        v-bind="fields.cpt"
        class="col-md-6" />
    </div>

    <Divider />

    <div class="form-row mb-3">
      <FormField 
        v-model="fields.foundation_quality.value"
        v-bind="fields.foundation_quality"
        class="col-md-6" />
      <FormField 
        v-model="fields.foundation_recovery_adviced.value"
        v-bind="fields.foundation_recovery_adviced"
        class="col-md-6" />
    </div>

    <div class="form-row mb-3">
      <FormField 
        v-model="fields.foundation_damage_cause.value"
        v-bind="fields.foundation_damage_cause"
        class="col-md-6" />
      <FormField 
        v-model="fields.enforcement_term.value"
        v-bind="fields.enforcement_term"
        class="col-md-6" />
    </div>

    <div class="form-row">
      <FormField 
        v-model="fields.base_measurement_level.value"
        v-bind="fields.base_measurement_level"
        class="col-md-3" />
      <FormField 
        v-model="fields.wood_level.value"
        v-bind="fields.wood_level"
        class="col-md-3" />
      <FormField 
        v-model="fields.groundwater_level.value"
        v-bind="fields.groundwater_level"
        class="col-md-3" />
      <FormField 
        v-model="fields.ground_level.value"
        v-bind="fields.ground_level"
        class="col-md-3" />
    </div>

  </Form>
</template>

<script>

import { required, numeric, decimal, maxLength, minValue, maxValue } from 'vuelidate/lib/validators';
import { 
  foundationTypeOptions,
  foundationQualityOptions, 
  enforcementTermOptions,
  substructureOptions,
  foundationDamageCauseOptions,
  BaseMeasurementLevelOptions
} from 'config/enums'

import Divider from 'atom/Divider'
import Form from 'molecule/form/Form'
import FormField from 'molecule/form/FormField'
import Feedback from 'atom/Feedback'

import fields from 'mixin/fields'

import { mapGetters, mapActions } from 'vuex'

export default {
  components: {
    Form, FormField, Divider, Feedback
  },
  mixins: [fields],
  props: {
    sample: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      isDisabled: false,
      feedback: {},
      fields: {
        // LINE 1
        street_name: {
          label: 'Straatnaam',
          type: 'text',
          value: '',
          validationRules: {
            required,
            maxLength: maxLength(128)
          }
        },
        building_number: {
          label: 'Nummer',
          type: 'text', 
          value: '',
          validationRules: {
            required,
            numeric,
            minValue: minValue(1),
            maxValue: maxValue(65536)
          }
        },
        building_number_suffix: {
          label: 'Toevoeging',
          type: 'text',
          value: '',
          validationRules: {
            maxLength: maxLength(8)
          }
        },
        // LINE 2
        foundation_type: {
          label: 'Funderingstype',
          type: 'select',
          value: null,
          options: [{
            value: null,
            text: 'Selecteer een optie'
          }].concat(foundationTypeOptions),
          validationRules: {}
        },
        substructure: {
          label: 'Onderbouw',
          type: 'select',
          value: null,
          options: [{
            value: null,
            text: 'Selecteer een optie'
          }].concat(substructureOptions),
          validationRules: {}
        },
        built_year: {
          label: 'Bouwjaar',
          type: 'text', 
          value: '',
          validationRules: {
            numeric,
            minValue: minValue(1000),
            maxValue: maxValue(2100)
          }
        },
        // LINE 3
        monitoring_well: {
          label: 'Monitoringsput',
          type: 'text',
          value: '',
          validationRules: {
            maxLength: maxLength(32)
          }
        },
        cpt: {
          label: 'Sondering',
          type: 'text',
          value: '',
          validationRules: {
            maxLength: maxLength(32)
          }
        },
        // DIVIDER 
        // LINE 4
        foundation_quality: {
          label: 'Funderingskwaliteit',
          type: 'select',
          value: null,
          options: [{
            value: null,
            text: 'Selecteer een optie'
          }].concat(foundationQualityOptions),
          validationRules: {}
        },
        foundation_recovery_adviced: {
          label: 'Funderingsherstel advies',
          type: 'radio',
          value: null,
          options: [{
            value: true,
            text: 'Ja'
          }, {
            value: false,
            text: 'Nee'
          }],
          validationRules: {}
        },
        // LINE 5
        foundation_damage_cause: {
          label: 'Oorzaak funderingsschade',
          type: 'select',
          value: null,
          options: [{
            value: null,
            text: 'Selecteer een optie'
          }].concat(foundationDamageCauseOptions),
          validationRules: {
            required
          }
        },
        enforcement_term: {
          label: 'Handhavingstermijn',
          type: 'select',
          value: null,
          options: [{
            value: null,
            text: 'Selecteer een optie'
          }].concat(enforcementTermOptions),
          validationRules: {}
        },
        // LINE 6
        base_measurement_level: {
          label: 'Referentiestelsel',
          type: 'select',
          value: 'NAP',
          options: [{
            value: null,
            text: 'Selecteer een optie'
          }].concat(BaseMeasurementLevelOptions),
          validationRules: {
            required
          }
        },
        wood_level: {
          label: 'Hoogte langshout',
          type: 'text',
          value: '',
          validationRules: {
            decimal
          }
        },
        groundwater_level: {
          label: 'Grondwaterstand',
          type: 'text',
          value: '',
          validationRules: {
            decimal
          }
        },
        ground_level: {
          label: 'Maaiveldhoogte',
          type: 'text',
          value: '',
          validationRules: {
            decimal
          }
        }
      }
    }
  },
  computed: {
    ...mapGetters('report', [
      'activeReport'
    ])
  },
  created() {
    if (this.sample.stored === false) {
      this.feedback = {
        variant: 'info',
        message: 'Let op: Dit adres is nog niet opgeslagen'
      }
    }
    // Required fields by API
    if ( ! this.sample.base_measurement_level) {
      this.sample.base_measurement_level = 0; // NAP
    }
    if (this.sample.foundation_damage_cause === null) {
      this.sample.foundation_damage_cause = 7 // Unknown
    }

    this.setFieldValues({
      street_name: this.sample.address.street_name,
      building_number: this.sample.address.building_number,
      building_number_suffix: this.sample.address.building_number_suffix,
      foundation_type: this.optionValue({
        options: foundationTypeOptions,
        name: 'foundation_type' 
      }),
      substructure: this.optionValue({
        options: substructureOptions,
        name: 'substructure' 
      }),
      built_year: this.sample.built_year,
      monitoring_well: this.sample.monitoring_well,
      cpt: this.sample.cpt,
      foundation_quality: this.optionValue({
        options: foundationQualityOptions,
        name: 'foundation_quality' 
      }),
      foundation_recovery_adviced: this.booleanValue({ 
        name: 'foundation_recovery_adviced'
      }),
      foundation_damage_cause: this.optionValue({
        options: foundationDamageCauseOptions,
        name: 'foundation_damage_cause' 
      }),
      enforcement_term: this.optionValue({
        options: enforcementTermOptions,
        name: 'enforcement_term' 
      }),
      base_measurement_level: this.optionValue({
        options: BaseMeasurementLevelOptions,
        name: 'base_measurement_level' 
      }),
      wood_level: this.sample.wood_level,
      groundwater_level: this.sample.groundwater_level,
      ground_level: this.sample.ground_level
    })
  },
  methods: {
    ...mapActions('samples', [
      'updateSample',
      'createSample',
      'deleteSample'
    ]),
    optionValue({ options, name }) {
      let key = this.sample[name]
      return options[key] ? options[key].value : null
    },
    booleanValue({ name }) {
      return this.sample[name] === true || this.sample[name] === false 
        ? this.sample[name]
        : null;
    },
    // Called by parent
    save() {
      this.$refs.form.submit()
    },
    // Called by parent
    delete() {
      if (this.isDisabled) {
        return
      }
      this.isDisabled = true;
      this.disableAllFields()
      this.feedback = {
        variant: 'info',
        message: 'Het adres wordt verwijderd...'
      }
      this.deleteSample({
        id: this.sample.id,
        creationstamp: this.sample.creationstamp
      })
    },
    async handleSubmit() {
      if (this.isDisabled) {
        return
      }
      this.isDisabled = true;
      this.disableAllFields()
      this.feedback = {
        variant: 'info',
        message: 'Het adres wordt opgeslagen...'
      }

      let data = this.allFieldValues()
      if (this.sample.id) {
        data.id = this.sample.id
      } else {
        // Used internally, not by the API
        data.creationstamp = this.sample.creationstamp
      }

      // required by API
      if (data.base_measurement_level === null) {
        data.base_measurement_level = 0 // NAP
      }
      if (data.foundation_damage_cause === null) {
        data.foundation_damage_cause = 7 // Unknown
      }

      data.address = {
        street_name: data.street_name,
        building_number: data.building_number,
        building_number_suffix: data.building_number_suffix
      }
      data.report = this.activeReport.id
      
      // console.log("save in editor 2", data)
      if (data.id) {
        await this.updateSample({
          id: data.id,
          data
        })
        .then(this.handleSuccess)
        .catch(this.handleError)
      } else {
        await this.createSample({
          data
        })
        .then(this.handleSuccess)
        .catch(this.handleError)
      }
    },
    handleSuccess() {
      try {
        this.feedback = {
          variant: 'success',
          message: 'De wijzigingen zijn opgeslagen'
        }
        this.enableAllFields()
        this.isDisabled = false
        this.$refs.form.resetValidation()
      } catch(err) {
        console.log(err)
      }
    },
    handleError(err) {
      console.log(err)
      this.feedback = {
        variant: 'danger',
        message: 'De wijzigingen zijn niet opgeslagen'
      }
      this.enableAllFields()
      this.isDisabled = false
    },
    handleFormError() {
      this.feedback = {
        variant: 'danger',
        message: 'Controleer a.u.b. de invoer'
      }
    }
  }
}
</script>

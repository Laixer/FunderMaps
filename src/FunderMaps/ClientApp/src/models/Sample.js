
import { 
  foundationTypeOptions,
  foundationQualityOptions,
  substructureOptions,
  foundationDamageCauseOptions,
  enforcementTermOptions,
  BaseMeasurementLevelOptions,
  accessOptions
} from 'config/enums'

/**
 * The SampleModel
 */

let structure = {
  id: '',
  report: '',
  foundation_type: null,
  foundation_quality: null,
  substructure: null,
  monitoring_well: '',
  cpt: '',
  note: '',
  wood_level: null,
  groundwater_level: null,
  ground_level: null,
  foundation_recovery_adviced: false,
  foundation_damage_cause: null,
  built_year: '',
  enforcement_term: null,
  base_measurement_level: null,
  address: {
    id: null,
    street_name: null,
    building_number: null,
    building_number_suffix: null
  },
  policy: 0,
  create_date: '',
  update_date: '',
  delete_date: ''
}


let SampleModel = function ({ sample, stored, editorState }) {
  Object.assign(this, structure, sample)
  this.stored = stored;
  this.editorState = editorState || 'close'
}

SampleModel.prototype.updateValues = function({ data }) {
  Object.assign(this, structure, data)
}

// ****************************************************************************
//  Editor states
// ****************************************************************************

SampleModel.prototype.openEditor = function() {
  this.editorState = 'open'
}
SampleModel.prototype.closeEditor = function() {
  this.editorState = 'close'
}

// ****************************************************************************
//  Dropdown values
// ****************************************************************************

SampleModel.prototype.getFoundationType = function() {
  return foundationTypeOptions[this.foundation_type] || null; 
}
SampleModel.prototype.getFoundationQuality = function() {
  return foundationQualityOptions[this.foundation_quality] || null; 
}
SampleModel.prototype.getSubstructure = function() {
  return substructureOptions[this.substructure] || null; 
}
SampleModel.prototype.getFoundationDamageCause = function() {
  return foundationDamageCauseOptions[this.foundation_damage_cause] || null
}
// Note: returns an object {text, value}
SampleModel.prototype.getEnforcementTerm = function() {
  return enforcementTermOptions[this.enforcement_term] || null; 
}
SampleModel.prototype.getBaseMeasurementLevel = function() {
  return BaseMeasurementLevelOptions[this.base_measurement_level] || null; 
}
SampleModel.prototype.getAccess = function() {
  return accessOptions[this.policy] || null; 
}

export default SampleModel;
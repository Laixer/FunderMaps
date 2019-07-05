<template>
  <div 
    :class="{ 'ProfileSetting__editMode': editMode }"
    class="ProfileSetting">
    <div class="ProfileSetting__text d-flex">
      <label class="d-inline-block font-weight-bold d-flex align-items-center">
        <span>{{ label }}</span>
      </label>
      <input 
        v-if="editMode"
        class="form-control d-inline-block flex-grow-1" 
        type="text" 
        :disabled="disabled"
        :value="value"
        @input="$emit('input', $event.target.value)" />
      <span 
        v-else
        class="d-inline-block flex-grow-1" >
        {{ value }}
      </span>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ProfileSettings',
  props: {
    value: {
      type: [String, Number],
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
    editMode: {
      type: Boolean,
      default: true
    }
  }
}
</script>

<style lang="scss">
.ProfileSetting {
  border-bottom: 1px solid #E6EAEE;
  padding-bottom: 16px;
  padding-top: 18px;

  &:last-child {
    border: none;
  }
  &__editMode {
    border: none;
    padding-bottom: 10px;
    padding-top: 12px;
  }

  &__text {
    label {
      color: #354052;
      width: 125px;
      margin: 0;
    }
    input {
      // border: none;
      // padding: 0;
      font-size: 14px;
      height: auto;
      width: auto;
    }
    .form-control:disabled {
      background-color: #fff;
      cursor: not-allowed
    }
  }
}
</style>

<template>
  <b-alert 
    v-model="show" 
    class="Feedback"
    :variant="variant" 
    dismissible 
    fade>
    {{ message }}
  </b-alert>
</template>

<script>
export default {
  props: {
    // This input is processed by `processFeedback`
    feedback: {
      type: Object,
      required: true
    }
  },
  data() {
    // The processed data, which is bound to the template
    return {
      show: false,
      variant: 'info',
      message: ''
    }
  },
  watch: {
    /**
     * When the feedback prop is updated, the input object is evaluated 
     * and the bound data updated accordingly.
     */
    feedback(feedback) {
      this.processFeedback(feedback)
    }
  },
  created() {
    this.processFeedback(this.feedback)
  },
  methods: {
    /**
     * Set the bound values based on the feedback input
     */
    processFeedback(feedback) {
      if (feedback.variant) {
        this.variant = feedback.variant
      }
      if (feedback.message) {
        this.message = feedback.message
      }
      // If the message or variant is changed, we assume it should be visible
      if (feedback.variant || feedback.message) {
        this.show = true;
      }
      // The above assumption can be overwritten by explicitly passing a value
      if (feedback.show) {
        this.show = feedback.show
      }
    },
    /**
     * These two methods allow programmatic access through a `ref` binding
     */
    hideFeedback() {
      this.show = false
    },
    showFeedback({ variant, message }) {
      this.variant = variant
      this.message = message
      this.show = true
    }
  }
}
</script>

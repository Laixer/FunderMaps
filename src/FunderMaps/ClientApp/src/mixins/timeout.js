

export default {
  data() {
    return {
      _timeout: null,
      _duration: 300
    }
  },
  beforeDestroy() {
    this.clearTimeout()
  },
  methods: {
    clearTimeout() {
      if(this._timeout !== null) {
        clearTimeout(this._timeout)
      }
    },
    setTimeout(fn, duration) {
      this.clearTimeout()
      this._timeout = setTimeout(
        fn.bind(this), 
        duration || this._duration
      );
    }
  }
}
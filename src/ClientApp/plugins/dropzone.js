import Vue from 'vue'

let component = {}
if (process.browser) {
  component = require('vue2-dropzone')
}
component.name = 'dropzone'
component.render = function(createElement) {
  const that = this._self
  return createElement('form', {
    attrs: {
      class: 'dropzone',
      id: that.id || '',
      action: that.url || '',
      dropzoneOptions: that.dropzoneOptions,
      useCustomDropzoneOptions: that.useCustomDropzoneOptions
    }
  }, this.$slots.default)
}
Vue.component('dropzone', component)
import { es } from '@braid/vue-formulate-i18n'

export default {
  plugin: [es],
  locale: 'es',
  library: {
    materialinput: {
      clasification: 'text',
      component: 'MaterialInput',
      slotProps: {
        component: ['cols']
      }
    }
  }
}

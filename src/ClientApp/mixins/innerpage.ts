import { Vue, Component } from 'nuxt-property-decorator'

@Component
class InnerPageMixin extends Vue {
  created() {
    this.$store.dispatch('allowToGoBack', true, { root: true })
  }
  destroyed() {
    this.$store.dispatch('allowToGoBack', false, { root: true })
  }
}

export default InnerPageMixin
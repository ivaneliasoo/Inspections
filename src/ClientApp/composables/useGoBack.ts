import { onMounted, onUnmounted, } from '@vue/composition-api'
import { useStore } from '@nuxtjs/composition-api'

const useGoBack = () => {
  const store = useStore()
  onMounted(() => {
    store.dispatch('allowToGoBack', true, { root: true })
  })

  onUnmounted(() => {
    store.dispatch('allowToGoBack', false, { root: true })
  })
}

export default useGoBack

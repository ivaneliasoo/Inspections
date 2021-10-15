import { ref, customRef } from '@nuxtjs/composition-api'

const debounce = (fn: any, delay = 0, immediate = false) => {
  let timeout: any

  return (...args: any) => {
    if (immediate && !timeout) { fn(...args) }
    clearInterval(timeout)

    timeout = setTimeout(() => {
      fn(...args)
    }, delay)
  }
}

const useDebouncedRef = (initialValue: any, delay: number | undefined, immediate: boolean | undefined) => {
  const state = ref(initialValue)
  const debouncedRef = customRef((track, trigger) => ({
    get () {
      track()
      return state.value
    },
    set: debounce(
      (value: any) => {
        state.value = value
        console.log({value})
        trigger()
      },
      delay,
      immediate
    )
  }))
  return debouncedRef
}

export { debounce, useDebouncedRef }

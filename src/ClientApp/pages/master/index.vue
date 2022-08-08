<template>
  <OptionsCards :options="options" />
</template>

<script lang="ts">
// eslint-disable-next-line import/named
import { computed, defineComponent, useContext } from '@nuxtjs/composition-api'
import { CardOption } from '~/types'

export default defineComponent({
  setup() {
    const cardOptions: CardOption[] = [
      {
        name: 'users',
        text: 'Users',
        helpText: 'Manage Users',
        icon: 'mdi-account-multiple',
        color: 'accent',
        path: '/users',
      },
      {
        name: 'settings',
        text: 'Reports Settings',
        helpText:
          'Allows to Configure reports check lists, signatures, titles and names',
        icon: 'mdi-cog-outline',
        color: 'accent',
        path: '/configurations?configurationonly=true',
      },
    ]

    const { $auth } = useContext()

    const options = computed(() => {
      if (!$auth.user.isAdmin) {
        return cardOptions.filter((co) => co.name !== 'settings')
      }

      return cardOptions
    })

    return {
      options,
    }
  },
})
</script>

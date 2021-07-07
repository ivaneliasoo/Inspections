<template>
  <OptionsCards :options="options" />
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { CardOption } from '~/types'

  @Component
export default class MasterMenu extends Vue {
    cardOptions: CardOption[] = [
      {
        name: 'users',
        text: 'Users',
        helpText: 'Manage Users',
        icon: 'mdi-account-multiple',
        color: 'accent',
        path: '/users'
      },
      {
        name: 'settings',
        text: 'Reports Settings',
        helpText: 'Allows to Configure reports check lists, signatures, titles and names',
        icon: 'mdi-cog-outline',
        color: 'accent',
        path: '/configurations?configurationonly=true'
      },
      {
        name: 'addresses',
        text: 'Address Management',
        helpText: 'Allows to create, update, delete address',
        icon: 'mdi-cog-outline',
        color: 'accent',
        path: '/addresses'
      },
      {
        name: 'licenses',
        text: 'Licenses Management',
        helpText: 'Allows to create, update, delete licenses',
        icon: 'mdi-license',
        color: 'accent',
        path: '/licenses'
      }
    ]

    get options () {
      if (!this.$auth.user.isAdmin) { return this.cardOptions.filter(co => co.name !== 'settings') }

      return this.cardOptions
    }
}
</script>

<style scoped>

</style>

<template>
  <OptionsCards :options="options" />
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { CardOption } from '~/types'

  @Component
export default class LewLicensing extends Vue {
  cardOptions: CardOption[] = [
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

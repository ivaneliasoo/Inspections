<template>
  <div>
    <OptionsCards :options="options" />
    <v-row v-if="$auth.user.isAdmin">
      <v-col cols="12" md="6" sm="12">
        <v-card>
          <v-card-title class="justify-center">
            <h3>Licenses Expiring Soon</h3>
          </v-card-title>
          <v-data-table
            :headers="licensesHeader"
            :item-class="() => 'expiring-row'"
            item-key="licenseId"
            dense
            :items="expiring"
            @dblclick:row="goToLicenses"
          >
            <template #[`item.validityStart`]="{ item }">
              {{ parseDate(item.validityStart) }}
            </template>
            <template #[`item.validityEnd`]="{ item }">
              {{ parseDate(item.validityEnd) }}
            </template>
          </v-data-table>
        </v-card>
      </v-col>
      <v-col cols="12" md="6" sm="12">
        <v-card>
          <v-card-title class="justify-center">
            <h3>
              Expired Licenses
            </h3>
          </v-card-title>
          <v-data-table
            :headers="licensesHeader"
            :item-class="() => 'expired-row'"
            item-key="licenseId"
            dense
            :items="expired"
            @dblclick:row="goToLicenses"
          >
            <template #[`item.validityStart`]="{ item }">
              {{ parseDate(item.validityStart) }}
            </template>
            <template #[`item.validityEnd`]="{ item }">
              {{ parseDate(item.validityEnd) }}
            </template>
          </v-data-table>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script lang="ts">
import { DateTime } from 'luxon'
import { Component, Vue } from 'vue-property-decorator'
import { CardOption } from '~/types'
import { LicensesState } from '~/store/licenses'

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

  licensesHeader = [
    {
      text: 'ID',
      value: 'licenseId',
      sortable: true,
      align: 'left'
    },
    {
      text: 'License',
      value: 'number',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Valid From',
      value: 'validityStart',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Valid To',
      value: 'validityEnd',
      sortable: true,
      align: 'left'
    }
  ]

  async asyncData ({ store }: any) {
    await store.dispatch('licenses/getLicensesDashboard', null, { root: true })
  }

  get expiring () {
    return (this.$store.state.licenses as LicensesState).dashboard.expiring
  }

  get expired () {
    return (this.$store.state.licenses as LicensesState).dashboard.expired
  }

  parseDate (date: string) {
    return DateTime.fromISO(date).toLocaleString()
  }

  goToLicenses (_:any, row: any) {
    this.$router.push(`/licenses?id=${row.item.licenseId}`)
  }

  get options () {
    if (!this.$auth.user.isAdmin) { return this.cardOptions.filter(co => co.name !== 'settings') }

    return this.cardOptions
  }
}
</script>

<style scoped>

</style>

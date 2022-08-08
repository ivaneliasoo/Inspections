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
            <h3>Expired Licenses</h3>
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
import {
  defineComponent,
  computed,
  useAsync,
  useRouter,
  useContext,
} from '@nuxtjs/composition-api'
import useDateTime from '~/composables/useDateTime'
import { CardOption } from '~/types'
import { useLicensesStore } from '~/composables/useLicensesStore'

export default defineComponent({
  setup() {
    const licensesStore = useLicensesStore()
    const router = useRouter()
    const { $auth } = useContext()
    const { parseDate } = useDateTime()

    const cardOptions: CardOption[] = [
      {
        name: 'addresses',
        text: 'Address Management',
        helpText: 'Allows to create, update, delete address',
        icon: 'mdi-cog-outline',
        color: 'accent',
        path: '/addresses',
      },
      {
        name: 'licenses',
        text: 'Licenses Management',
        helpText: 'Allows to create, update, delete licenses',
        icon: 'mdi-license',
        color: 'accent',
        path: '/licenses',
      },
    ]

    const licensesHeader = [
      {
        text: 'ID',
        value: 'licenseId',
        sortable: true,
        align: 'left',
      },
      {
        text: 'License',
        value: 'number',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Valid From',
        value: 'validityStart',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Valid To',
        value: 'validityEnd',
        sortable: true,
        align: 'left',
      },
    ]

    useAsync(() => licensesStore.getLicensesDashboard())

    const expiring = computed(() => {
      return licensesStore.dashboard.expiring
    })

    const expired = computed(() => {
      return licensesStore.dashboard.expired
    })

    const goToLicenses = (_: any, row: any) => {
      router.push(`/licenses?id=${row.item.licenseId}`)
    }

    const options = computed(() => {
      if (!$auth.user.isAdmin) {
        return cardOptions.filter((co) => co.name !== 'settings')
      }

      return cardOptions
    })

    return {
      options,
      licensesHeader,
      expiring,
      expired,
      parseDate,
      goToLicenses,
    }
  },
})
</script>

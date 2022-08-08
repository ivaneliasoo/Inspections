<template>
  <div>
    <alert-dialog
      v-model="componentState.dialogRemove"
      title="Remove Addresses"
      message="This operation will remove this Address. If Proceed, you no longer get it suggested again"
      :code="componentState.selectedItem.id"
      :description="componentState.selectedItem.title"
      @yes="deleteAddress()"
    />
    <v-data-table
      :items="addresses"
      item-key="id"
      :search="componentState.filter.filterText"
      dense
      :loading="componentState.loading"
      :headers="headers"
      :class="$device.isTablet ? 'tablet-text' : ''"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Addresses</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="componentState.filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="
              componentState.dialog = true
              componentState.isNew = true
              componentState.item = {
                id: 0,
                country: 'Singapore',
                addressLine2: '',
              }
            "
          >
            <v-icon dark> mdi-plus </v-icon>
          </v-btn>
          <v-dialog
            v-model="componentState.dialog"
            persistent
            scrollable
            :fullscreen="$vuetify.breakpoint.smAndDown"
            :max-width="!$vuetify.breakpoint.smAndDown ? '50%' : '100%'"
          >
            <ValidationObserver v-slot="{ valid, reset }" tag="form">
              <v-card>
                <v-card-title>
                  <span class="headline">Edit Address</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="6">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          immediate
                        >
                          <v-autocomplete
                            v-model="componentState.item.licenseId"
                            :items="licenses"
                            item-text="number"
                            item-value="licenseId"
                            :error-messages="errors"
                            autocomplete="nope"
                            label="Electrical License"
                            hint="Select a License From the List"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="6">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          immediate
                        >
                          <v-textarea
                            v-model="componentState.item.addressLine"
                            autocomplete="nope"
                            name="addressLine"
                            rows="1"
                            :error-messages="errors"
                            label="Address"
                          />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="4">
                        <v-text-field
                          v-model="componentState.item.unit"
                          autocomplete="nope"
                          name="unit"
                          label="Unit"
                        />
                      </v-col>
                      <v-col cols="12" md="4">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          immediate
                        >
                          <v-text-field
                            v-model="componentState.item.country"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="country"
                            label="Country"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="4">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          immediate
                        >
                          <v-text-field
                            v-model="componentState.item.postalCode"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="postalCode"
                            label="Postal Code"
                          />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col>
                        <v-textarea
                          v-model="componentState.item.addressLine2"
                          autocomplete="nope"
                          rows="2"
                          name="addressLine2"
                          label="Other Remark"
                        />
                      </v-col>
                    </v-row>
                  </v-container>
                </v-card-text>
                <v-card-actions>
                  <v-spacer />
                  <v-btn
                    color="success"
                    text
                    :loading="componentState.loading"
                    :disabled="!valid"
                    @click="upsertAddress()"
                  >
                    Save
                  </v-btn>
                  <v-btn
                    color="default"
                    text
                    @click="
                      reset()
                      componentState.item = {
                        id: 0,
                        country: 'Singapore',
                        addressLine2: '',
                      }
                      componentState.dialog = false
                    "
                  >
                    Cancel
                  </v-btn>
                </v-card-actions>
              </v-card>
            </ValidationObserver>
          </v-dialog>
        </v-toolbar>
      </template>
      <template #[`item.actions`]="{ item }">
        <v-tooltip v-if="isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              color="primary"
              class="mr-2"
              v-on="on"
              @click="
                selectItem(item)
                componentState.isNew = false
                componentState.dialog = true
              "
            >
              mdi-pencil
            </v-icon>
          </template>
          <span>Edit</span>
        </v-tooltip>
        <v-tooltip v-if="isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              color="error"
              v-on="on"
              @click="
                selectItem(item)
                componentState.dialogRemove = true
              "
            >
              mdi-delete
            </v-icon>
          </template>
          <span>Delete</span>
        </v-tooltip>
      </template>
    </v-data-table>
  </div>
</template>
<script lang="ts">
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import {
  computed,
  useFetch,
  reactive,
  useContext,
  defineComponent,
} from '@nuxtjs/composition-api'
import { AddressDto } from '@/types/Addresses'
import { useAddressStore } from '~/composables/useAddressStore'
import { useLicensesStore } from '~/composables/useLicensesStore'
import useGoBack from '~/composables/useGoBack'

export default defineComponent({
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  setup() {
    useGoBack()
    const addressStore = useAddressStore()
    const licensesStore = useLicensesStore()
    const { $auth, error } = useContext()

    const headers: any[] = [
      {
        text: 'ID',
        value: 'id',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Address',
        value: 'addressLine',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Unit',
        value: 'unit',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Country',
        value: 'country',
        sortable: true,
        align: 'left',
      },
      {
        text: 'Postal Code',
        value: 'postalCode',
        sortable: true,
        align: 'left',
      },
      {
        text: '',
        value: 'actions',
        sortable: false,
        align: 'left',
      },
    ]

    const componentState = reactive({
      dialog: false,
      dialogRemove: false,
      loading: false,
      filter: {
        filterText: '',
      },
      selectedItem: {} as AddressDto,
      item: { principal: false },
      isNew: false,
    })

    const isAdmin = computed(() => $auth.user.isAdmin)

    const addresses = computed(() => {
      return addressStore.$state.addressList
    })

    const licenses = computed(() => {
      return [
        {
          licenseId: 0,
          number: 'No Licensed',
          Name: 'No Licensed',
          PersonInCharge: '',
          Contact: '',
          Email: '',
          Amp: 0,
          Volt: 0,
          KVA: 0,
          validityStart: null,
          validityEnd: null,
        },
        ...licensesStore.licensesList,
      ]
    })

    useFetch(async () => {
      if (!$auth.user.isAdmin) {
        error({ statusCode: 403, message: 'Forbidden' })
      }

      await Promise.all([
        addressStore.getAddresses({}),
        licensesStore.getLicenses({}),
      ])
    })

    const selectItem = (item: AddressDto): void => {
      componentState.selectedItem = item
      addressStore
        .getAddressById(componentState.selectedItem.id)
        .then((resp) => (componentState.item = resp))
    }

    const deleteAddress = async () => {
      await addressStore.deleteAddress(componentState.selectedItem.id)
      componentState.dialog = false
    }
    const upsertAddress = async () => {
      try {
        componentState.loading = true
        if (!componentState.isNew) {
          await addressStore.updateAddress(componentState.item)
        } else {
          await addressStore.createAddress(componentState.item)
          await addressStore.getAddresses({})
        }
      } catch (error) {
        // eslint-disable-next-line no-console
        console.debug({ error })
      } finally {
        componentState.dialog = false
        componentState.isNew = true
        componentState.loading = false
      }
    }

    return {
      headers,
      componentState,
      upsertAddress,
      deleteAddress,
      selectItem,
      addresses,
      licenses,
      isAdmin,
    }
  },
})
</script>

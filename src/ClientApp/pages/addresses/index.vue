<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Addresses"
      message="This operation will remove this Address. If Proceed, you no longer get it suggested again"
      :code="selectedItem.id"
      :description="selectedItem.title"
      @yes="deleteAddress();"
    />
    <v-data-table
      :items="addresses"
      item-key="id"
      :search="filter.filterText"
      dense
      :loading="loading"
      :headers="headers"
      :class="$device.isTablet ? 'tablet-text':''"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Addresses</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="dialog = true; isNew = true; item = { id: 0, country: 'Singapore', addressLine2: '' }"
          >
            <v-icon dark>
              mdi-plus
            </v-icon>
          </v-btn>
          <v-dialog
            v-model="dialog"
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
                        <ValidationProvider v-slot="{ errors }" rules="required" immediate>
                          <v-autocomplete
                            v-model="item.licenseId"
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
                        <ValidationProvider v-slot="{ errors }" rules="required" immediate>
                          <v-textarea
                            v-model="item.addressLine"
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
                          v-model="item.unit"
                          autocomplete="nope"
                          name="unit"
                          label="Unit"
                        />
                      </v-col>
                      <v-col cols="12" md="4">
                        <ValidationProvider v-slot="{ errors }" rules="required" immediate>
                          <v-text-field
                            v-model="item.country"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="country"
                            label="Country"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="4">
                        <ValidationProvider v-slot="{ errors }" rules="required" immediate>
                          <v-text-field
                            v-model="item.postalCode"
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
                          v-model="item.addressLine2"
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
                    :loading="loading"
                    :disabled="!valid"
                    @click="upsertAddress()"
                  >
                    Save
                  </v-btn>
                  <v-btn
                    color="default"
                    text
                    @click="reset(); item = { id: 0, country: 'Singapore', addressLine2: '' }; dialog = false"
                  >
                    Cancel
                  </v-btn>
                </v-card-actions>
              </v-card>
            </ValidationObserver>
          </v-dialog>
        </v-toolbar>
      </template>
      <template #item[actions]="{ item }">
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              color="primary"
              class="mr-2"
              v-on="on"
              @click="selectItem(item); isNew = false; dialog = true"
            >
              mdi-pencil
            </v-icon>
          </template>
          <span>Edit</span>
        </v-tooltip>
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              color="error"
              v-on="on"
              @click="selectItem(item); dialogRemove = true"
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
import { Component, mixins } from 'nuxt-property-decorator'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { AddressesState } from 'store/addresses'
import { LicensesState } from 'store/licenses'
import InnerPageMixin from '@/mixins/innerpage'
import { AddressDto } from '@/types/Addresses'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class AddressesAdmin extends mixins(InnerPageMixin) {
  dialog: boolean = false
  dialogRemove: boolean = false
  loading: boolean = false

  filter: any = {
    filterText: ''
  }

  headers: any[] = [
    {
      text: 'ID',
      value: 'id',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Address',
      value: 'addressLine',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Unit',
      value: 'unit',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Country',
      value: 'country',
      sortable: true,
      align: 'left'
    },
    {
      text: 'Postal Code',
      value: 'postalCode',
      sortable: true,
      align: 'left'
    },
    {
      text: '',
      value: 'actions',
      sortable: false,
      align: 'left'
    }
  ]

  selectedItem: AddressDto = {} as AddressDto
  item: any = { principal: false }
  isNew: boolean = false

  get addresses (): AddressDto[] {
    return (this.$store.state.addresses as AddressesState).addressList
  }

  get licenses () {
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
        validityEnd: null
      },
      ...(this.$store.state.licenses as LicensesState).licensesList
    ]
  }

  selectItem (item: AddressDto): void {
    this.selectedItem = item
    this.$store
      .dispatch('addresses/getAddressById', this.selectedItem.id, {
        root: true
      })
      .then(resp => (this.item = resp))
  }

  async fetch () {
    if (!this.$auth.user.isAdmin) { this.$nuxt.error({ statusCode: 403, message: 'Forbidden' }) }

    Promise.all([await this.$store.dispatch('addresses/getAddresses', {}, { root: true }),
      await this.$store.dispatch('licenses/getLicenses', {}, { root: true })])
  }

  deleteAddress () {
    this.$store
      .dispatch('addresses/deleteAddress', this.selectedItem.id, { root: true })
      .then(() => {
        this.dialog = false
      })
  }

  async upsertAddress () {
    try {
      this.loading = true
      if (!this.isNew) {
        await this.$store.dispatch('addresses/updateAddress', this.item, { root: true })
      } else {
        await this.$store.dispatch('addresses/createAddress', this.item, { root: true })
        await this.$store.dispatch('addresses/getAddresses', {}, { root: true })
      }
    } catch (error) {
      // eslint-disable-next-line no-console
      console.debug({ error })
    } finally {
      this.dialog = false
      this.isNew = true
      this.loading = false
    }
  }
}
</script>

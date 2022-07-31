<template>
  <div>
    <alert-dialog
      v-if="selectedLicense"
      v-model="dialogRemove"
      title="Remove Licenses"
      message="This operation will remove this License. If Proceed, you no longer get it Available again"
      :code="selectedLicense.id"
      :description="selectedLicense.title"
      @yes="deleteLicense()"
    />
    <v-data-table
      :items="Licenses"
      item-key="id"
      :search="filter.filterText"
      dense
      :loading="loading"
      :headers="headers"
      :class="$device.isTablet ? 'tablet-text' : ''"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Licenses</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter.filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="
              dialog = true;
              isNew = true;
              license = { licenseId: 0, validity: { start: '', end: '' } };
            "
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
                  <span class="headline">Edit License</span>
                </v-card-title>
                <v-card-text>
                  <v-container>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="4">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          immediate
                        >
                          <v-text-field
                            v-model.number="license.licenseId"
                            readonly
                            autocomplete="nope"
                            name="LicenseId"
                            :error-messages="errors"
                            label="Id"
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
                            v-model="license.name"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="name"
                            label="License Name (Company's/Owner's name)"
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
                            v-model="license.number"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="licenseNumber"
                            label="Number"
                          />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col>
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          immediate
                        >
                          <v-text-field
                            v-model="license.personInCharge"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="personInCharge"
                            label="Name of Person in Charge"
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col>
                        <v-text-field
                          v-model="license.contact"
                          autocomplete="nope"
                          name="contact"
                          label="Contact"
                        />
                      </v-col>
                      <v-col>
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="email"
                          immediate
                        >
                          <v-text-field
                            v-model="license.email"
                            autocomplete="nope"
                            :error-messages="errors"
                            name="email"
                            label="E-Mail"
                          />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                    <v-row align="center" justify="space-between">
                      <v-col>
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          immediate
                          vid="validFrom"
                        >
                          <DatePickerBase
                            v-model="license.validityStart"
                            type="number"
                            label="License"
                            titulo="Valid From"
                            :error-messages="errors"
                            max=""
                          />
                        </ValidationProvider>
                      </v-col>
                      <v-col cols="12" md="6">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required|precedesDate:validFrom"
                          immediate
                        >
                          <DatePickerBase
                            v-model="license.validityEnd"
                            type="number"
                            label="License"
                            titulo="Valid To"
                            :error-messages="errors"
                            max=""
                          />
                        </ValidationProvider>
                      </v-col>
                    </v-row>
                    <span class="subtitle-text">Approval Load: </span>
                    <v-row align="center" justify="space-between">
                      <v-col cols="12" md="4">
                        <ValidationProvider
                          v-slot="{ errors }"
                          rules="required"
                          immediate
                        >
                          <v-text-field
                            v-model.number="license.amp"
                            autocomplete="nope"
                            :error-messages="errors"
                            type="number"
                            name="amp"
                            label="Amp"
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
                            v-model.number="license.volt"
                            autocomplete="nope"
                            :error-messages="errors"
                            type="number"
                            name="volt"
                            label="Volt"
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
                            v-model.number="license.kva"
                            autocomplete="nope"
                            :error-messages="errors"
                            type="number"
                            name="kva"
                            label="kVA"
                          />
                        </ValidationProvider>
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
                    @click="upsertLicense()"
                  >
                    Save
                  </v-btn>
                  <v-btn
                    color="default"
                    text
                    @click="
                      reset();
                      license = { id: 0 };
                      dialog = false;
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
        <v-tooltip v-if="$auth.user.isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              color="primary"
              class="mr-2"
              v-on="on"
              @click="
                selectLicense(item);
                isNew = false;
                dialog = true;
              "
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
              @click="
                selectLicense(item);
                dialogRemove = true;
              "
            >
              mdi-delete
            </v-icon>
          </template>
          <span>Delete</span>
        </v-tooltip>
      </template>
      <template #[`item.validityStart`]="{ item }">
        {{ parseDate(item.validityStart) }}
      </template>
      <template #[`item.validityEnd`]="{ item }">
        {{ parseDate(item.validityEnd) }}
      </template>
    </v-data-table>
  </div>
</template>
<script lang="ts">
import { ref, computed, defineComponent, useFetch, useContext, useRoute } from '@nuxtjs/composition-api'

import { email } from 'vee-validate/dist/rules'
import { ValidationObserver, ValidationProvider, extend } from 'vee-validate'
import useDateTime from '../../composables/useDateTime'
import { useLicensesStore } from '~/composables/useLicensesStore'
import { LicenseDTO } from '@/types/Licenses'
import useGoBack from '~/composables/useGoBack'

extend('precedesDate', {
  validate: (dateTo, { dateFrom }: any) => {
    return dateTo >= dateFrom
  },
  message: 'end validity date cant be a date before than start validity date',
  params: [{ name: 'dateFrom', isTarget: true }]
})

extend('email', { ...email })

export default defineComponent({
  name: 'LicensesIndex',
  components: {
    ValidationObserver,
    ValidationProvider
  },
  setup () {
    useGoBack()
    const { parseDate } = useDateTime()

    const licensesStore = useLicensesStore()

    const route = useRoute()
    const { $auth, error } = useContext()
    const headers = ref([
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
      },
      {
        text: '',
        value: 'actions',
        sortable: false,
        align: 'left'
      }
    ])
    const isNew = ref<boolean>(false)
    const dialog = ref<boolean>(false)
    const dialogRemove = ref<boolean>(false)
    const license = ref({ licenseId: 0, validityStart: '', validityEnd: '' } as LicenseDTO)
    const selectedLicense = ref({} as LicenseDTO)
    const loading = ref<boolean>(false)
    const startVisible = ref(false)
    const endVisible = ref(false)
    const filter = ref({
      filterText: ''
    })

    const Licenses = computed((): LicenseDTO[] => {
      return licensesStore.licensesList
    })

    const selectLicense = (item: LicenseDTO): void => {
      selectedLicense.value = item
      licensesStore.getLicenseById(selectedLicense.value.licenseId)
        .then(resp => (license.value = resp))
    }

    useFetch(async () => {
      if (!$auth.user.isAdmin) { error({ statusCode: 403, message: 'Forbidden' }) }
      await licensesStore.getLicenses({})

      if (route.value.query.id) {
        licensesStore.getLicenseById(route.value.query.id)
          .then(resp => (license.value = resp))
        isNew.value = false
        dialog.value = true
      }
    })

    const deleteLicense = () => {
      licensesStore.deleteLicense(selectedLicense.value.licenseId)
        .then(() => {
          dialog.value = false
        })
    }

    const upsertLicense = async () => {
      loading.value = true
      if (!isNew.value) { await licensesStore.updateLicense(license.value) } else {
        await licensesStore.createLicense(license.value)
        await licensesStore.getLicenses({})
      }
      dialog.value = false
      isNew.value = true
      loading.value = false
    }

    return {
      headers,
      dialog,
      dialogRemove,
      isNew,
      loading,
      startVisible,
      endVisible,
      filter,
      license,
      Licenses,
      selectedLicense,
      parseDate,
      upsertLicense,
      deleteLicense,
      selectLicense
    }
  }
})
</script>

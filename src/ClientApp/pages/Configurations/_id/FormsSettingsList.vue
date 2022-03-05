<template>
  <div v-if="forms ">
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Forms Settings"
      message="This operation will remove this Form Setting. If Proceed, you no longer get it suggested again"
      :code="selectedForm.id"
      :description="selectedForm.title"
      @yes="deleteForm()"
    />
    <v-data-table
      :items="forms"
      item-key="id"
      :search="filterText"
      dense
      :loading="loading"
      :headers="headers"
      :class="$device.isTablet ? 'tablet-text' : ''"
    >
      <template #top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Forms Settings</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filterText" />
          <v-spacer />
          <v-btn
            class="mx-2"
            x-small
            fab
            dark
            color="primary"
            @click="router.push(`FormsSettings?id=-1`)"
          >
            <v-icon dark>
              mdi-plus
            </v-icon>
          </v-btn>
        </v-toolbar>
      </template>
      <template #item.actions="{ item }">
        <v-tooltip v-if="isAdmin" top>
          <template #activator="{ on }">
            <v-icon
              color="primary"
              class="mr-2"
              v-on="on"
              @click="
                router.push(`FormsSettings?id=${item.id}`);
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
                selectedForm = item;
                dialogRemove = true;
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
import { defineComponent, useContext, ref, useAsync, useRoute, useRouter } from '@nuxtjs/composition-api'
import useGoBack from '~/composables/useGoBack'
import { useNotifications } from '~/composables/use-notifications'
import { FormDefinitionResponse } from '~/services/api'

export default defineComponent({
  setup () {
    useGoBack()

    const { notify } = useNotifications()
    const { $formsApi, $reportsConfigApi, $auth } = useContext()
    const route = useRoute()
    const router = useRouter()
    const { id } = route.value.params
    const forms = useAsync(async () => {
      const response = await $reportsConfigApi.apiReportConfigurationIdGet(id)
      return response.data.forms
    })

    const headers = [
      {
        text: 'Id',
        value: 'id',
        sortable: true,
        align: 'center'
      },
      {
        text: 'Name',
        value: 'name',
        sortable: true,
        align: 'center'
      },
      {
        text: 'Title',
        value: 'title',
        sortable: true,
        align: 'center'
      },
      {
        text: 'Icon',
        value: 'icon',
        sortable: true,
        align: 'center'
      },
      {
        text: '',
        value: 'actions',
        sortable: true,
        align: 'center'
      }
    ]

    const filterText = ref('')
    const selectedForm = ref<FormDefinitionResponse>({})
    const dialogRemove = ref(false)
    const loading = ref(false)

    const isAdmin = $auth.user.isAdmin

    const deleteForm = async (id: number) => {
      try {
        await $formsApi.deleteFormDefinition(id)
        notify({
          type: 'success',
          message: 'Form Deleted'
        })
      } catch (error) {
        notify({
          type: 'error',
          message: error.message
        })
      }
    }

    return { headers, forms, selectedForm, filterText, dialogRemove, loading, isAdmin, router, deleteForm }
  }
})
</script>

<style>
</style>

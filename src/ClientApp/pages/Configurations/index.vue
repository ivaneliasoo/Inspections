<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Configuration"
      message="This operation will remove this Configuration. You'll not be able to proceed if this configration has been used in any reports"
      :code="selectedItem.id"
      :description="selectedItem.title"
      @yes="deleteConfig();"
      @no="dialogRemove=false"
    />
    <v-data-table
      :items="configs"
      item-key="id"
      :headers="headers"
      dense
    >
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Manage Reports Configurations</v-toolbar-title>
          <v-divider
            class="mx-4"
            inset
            vertical
          />
          <grid-filter :filter.sync="filter" />
          <v-spacer />
          <v-btn class="mx-2" x-small 
            fab dark color="primary"
            @click="$router.push({ name: 'Configurations-id', params: { id: -1 } })">
              <v-icon dark>mdi-plus</v-icon>
          </v-btn>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="$router.push({ name: 'Configurations-id', params: { id: item.id } })"
        >
          mdi-pencil
        </v-icon>
        <v-icon
          small
          color="error"
          @click="selectedItem= item; dialogRemove = true"
        >
          mdi-delete
        </v-icon>
      </template>
      <template v-slot:item.type="{ item }">
        {{ item.type === 0 ? 'Inspection':'Unkown' }}
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import { ReportConfigurationState } from 'store/configurations'
import { ReportConfiguration } from '~/types'
import GridFilter from '@/components/GridFilter.vue'
import AlertDialog from '@/components/AlertDialog.vue'

@Component({
  components: {
    GridFilter
  }
})
export default class ReportsConfigurationPage extends Vue {
  dialogRemove: boolean =false
  selectedItem: ReportConfiguration = {} as ReportConfiguration
  filter: string = ''
  headers: any[] = [
    {
      text: 'Id',
      value: 'id',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Type',
      value: 'type',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Title',
      value: 'title',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Form Name',
      value: 'formName',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Remarks Label Text',
      value: 'remarksLabelText',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Defined CheckLists Qty',
      value: 'definedCheckLists',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Defined Signatures Qty',
      value: 'definedSignatures',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Used by (Reports)',
      value: 'usedByReports',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: '',
      value: 'actions',
      sortable: true,
      align: 'center',
      class: 'secundary'
    }
  ]

  get configs (): ReportConfiguration[] {
    return (this.$store.state.configurations as ReportConfigurationState).configurations
  }

  deleteConfig() {
    this.$store.dispatch('configurations/deleteConfiguration', this.selectedItem.id, { root: false })
  }

  fetch ({ store }: any) {
    store.dispatch('configurations/getConfigurations', '', { root: true })
  }
}
</script>

<template>
  <div>
    <v-data-table :items="signatures" item-key="id" :search="filter" dense :headers="headers">
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Signatures</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter" />
          <v-spacer />
          <v-dialog v-model="dialog" max-width="500px">
            <template v-slot:activator="{ on, attrs }">
              <v-btn color="primary" dark class="mb-2" v-bind="attrs" v-on="on">
                New Signature
              </v-btn>
            </template>
            <v-card>
              <v-card-title>
                <span class="headline">New Signature</span>
              </v-card-title>
              <v-card-text>
                <v-container>
                  <v-row>
                  </v-row>
                </v-container>
              </v-card-text>
              <v-card-actions>
                <v-spacer />
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{}">
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click=""
        >
          mdi-pencil
        </v-icon>
        <v-icon
          small
          color="error"
          @click=""
        >
          mdi-delete
        </v-icon>
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import { SignatureState } from 'store/signatures'
import { SignatureDTO } from '../types/Signatures/ViewModels/SignatureDTO'
import GridFilter from '@/components/GridFilter.vue'

@Component({
  components: {
    GridFilter
  }
})
export default class SignaturesPage extends Vue {
  dialog: boolean = false;
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
      text: 'Title',
      value: 'title',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Annotation',
      value: 'annotation',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: 'Principal',
      value: 'responsableName',
      sortable: true,
      align: 'center',
      class: 'secundary'
    },
    {
      text: '',
      value: 'actions',
      sortable: false,
      align: 'center',
      class: 'secundary'
    }
  ];

  get signatures (): SignatureDTO[] {
    return (this.$store.state.signatures as SignatureState)
      .signaturesList
  }

  fetch ({ store }: any) {
    store.dispatch('signatures/getSignatures', '', { root: true })
  }
}
</script>

<style>

</style>
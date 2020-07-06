<template>
  <div>
    <alert-dialog
      v-model="dialogRemove"
      title="Remove Reports"
      message="This operation will remove this report and all data related"
      :code="selectedItem.id"
      :description="selectedItem.name"
      @yes="deleteReport();"
      @no="dialogRemove=false"
    />
    <message-dialog v-model="dialog" :actions="['yes','cancel']" @yes="createReport" @cancel="dialog=false">
      <template v-slot:title="{}">
        New Report
      </template>
      <ValidationObserver tag="form" ref="obs">
        <v-row>
          <v-col cols="12" xs="12" md="6">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-select
                id="selReportType"
                v-model="newReport.reportType"
                :error-messages="errors"
                item-value="id"
                item-text="text"
                label="Report Type"
                :items="[{ id:0, text: 'Inspection' }]"
              />
            </ValidationProvider>
          </v-col>
          <v-col cols="12" xs="12" md="6">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-select
                id="selConfigurations"
                v-model="newReport.configurationId"
                :error-messages="errors"
                label="Select a Saved Configuration"
                item-value="id"
                item-text="title"
                :items="configurations"
              />
            </ValidationProvider>
          </v-col>
          <v-col cols="12">
            <ValidationProvider rules="required" v-slot="{ errors }">
              <v-text-field
                id="txtName"
                v-model="newReport.name"
                :error-messages="errors"
                label="Report Name"
              />
            </ValidationProvider>
          </v-col>
        </v-row>
      </ValidationObserver>
    </message-dialog>
    <v-data-table :items="reportList" item-key="id" :search="filter" dense :headers="headers">
      <template v-slot:top="{}">
        <v-toolbar flat color="white">
          <v-toolbar-title>Inspection Reports</v-toolbar-title>
          <v-divider class="mx-4" inset vertical />
          <grid-filter :filter.sync="filter" />
          <v-spacer />
          <v-btn class="mx-2" x-small 
            fab dark color="primary"
            @click="dialog = true">
              <v-icon dark>mdi-plus</v-icon>
          </v-btn>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon
          :disabled="!item.isClosed || !item.photoRecords.length>0"
          small
          color="primary"
          class="mr-2"
          @click="printHelper.printCompoundedPhotoRecord(item.id)"
        >
          mdi-camera
        </v-icon>
        <v-icon
          :disabled="!item.isClosed"
          small
          color="primary"
          class="mr-2"
          @click="printHelper.print(item.id)"
        >
          mdi-printer
        </v-icon>
        <v-icon
          small
          color="primary"
          class="mr-2"
          @click="$router.push({ name: 'Reports-id', params: { id: item.id} })"
        >
          mdi-file-chart
        </v-icon>
        <v-icon
          small
          :disabled="item.isClosed"
          color="error"
          @click="selectItem(item); dialogRemove = true"
        >
          mdi-delete
        </v-icon>
      </template>
      <template v-slot:item.date="{ item }">
        {{ formatDate(item.date) }}
      </template>
      <template v-slot:item.photoRecords="{ item }">
        {{ item.photoRecords.length }}
      </template>
      <template v-slot:item.notes="{ item }">
        {{ item.notes.length }}
      </template>
      <template v-slot:item.checkList="{ item }">
        {{ item.checkList.length }}
      </template>
      <template v-slot:item.signatures="{ item }">
        {{ item.signatures.length }}
      </template>
      <template v-slot:item.completed="{ item }">
        <v-simple-checkbox v-model="item.completed" disabled />
      </template>
      <template v-slot:item.isClosed="{ item }">
        <v-simple-checkbox v-model="item.isClosed" disabled />
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import moment from 'moment'
import { Vue, Component } from 'nuxt-property-decorator'
import { ReportConfigurationState } from 'store/configurations'
import { ReportsState } from 'store/reportstrore'
import { ValidationObserver, ValidationProvider } from 'vee-validate'
import { Report, CreateReport, ReportConfiguration, EMALicenseType, CheckList, Signature, ResponsableType, CheckValue } from '~/types'
import { PrintHelper } from '@/Helpers'

@Component({
  components: {
    ValidationObserver,
    ValidationProvider
  }
})
export default class ReportsPage extends Vue {
  printHelper!: PrintHelper
  $refs!: {
    obs: InstanceType<typeof ValidationObserver>
  }
  dialog: Boolean = false
  dialogRemove: Boolean = false
  selectedItem: Report = {} as Report
  newReport:CreateReport = {} as CreateReport
  filter: String = ''
  hostName: string= this.$axios!.defaults!.baseURL!.replace('/api','')
    headers: any[] = [
      {
        text: 'Id',
        value: 'id',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Report Name',
        value: 'name',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Date',
        value: 'date',
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
        text: 'Notes',
        value: 'notes',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Checks',
        value: 'checkList',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Signatures',
        value: 'signatures',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Photos',
        value: 'photoRecords',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Completed',
        value: 'completed',
        sortable: true,
        align: 'center',
        class: 'secundary'
      },
      {
        text: 'Closed',
        value: 'isClosed',
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

    get reportList (): Report[] {
      return (this.$store.state.reportstrore as ReportsState)
        .reportList
    }

    fetch ({ store }: any) {
      store.dispatch('configurations/getConfigurations', '', { root: true })
      store.dispatch('reportstrore/getReports', '', { root: true })
    }

    mounted() {
      this.printHelper = new PrintHelper(this.$store)
    }

    get configurations (): ReportConfiguration[] {
      return (this.$store.state.configurations as ReportConfigurationState).configurations
    }

    selectItem (item: Report): void{
      this.selectedItem = item
    }

    formatDate (date:string): string {
      return moment(date).format('YYYY-MM-DD HH:mm')
    }

    // only if i've more time then expected
    get idValid () {
      return this.$refs.obs.flags.valid
    }

    async createReport () {
      if(await this.$refs.obs.validate() === true)
      this.$store.dispatch('reportstrore/createReport', this.newReport, { root: true })
        .then(() => {
          this.$store.dispatch('reportstrore/getReports', '', { root: true })
          this.dialog = false
        })
    }

    deleteReport () {
      this.$store.dispatch('reportstrore/deleteReport', this.selectedItem.id, { root: true })
        .then(() => {
          this.dialog = false
        })
    }

    async print(item: Report) {
      
    const report: Report = await this.$store.dispatch(
      'reportstrore/getReportById',
      item.id,
      { root: true }
    );
    (window as any).pdfMake.vfs = pdfFonts.pdfMake.vfs;

    

    const docDef: TDocumentDefinitions  = {
       header: [{ 
          text: 'Inspection Report',
          margin: [0,15,30,0],
          style: 'name'
        },
        { 
          text: 'CSE EI(R1) FORM (Rev #4)',
          margin: [0,0,30,0],
          style: 'subtitle'
        }],
      content: [
        { 
          text: 'PARTICULARS OF INSTALLATION',
          style: 'title'
        },
        { 
          text: `Name of Installation: ${report.name.toUpperCase()}`,
          style: 'fieldName'
        },
        { 
          text: `Address of Installation: ${report.address ? report.address.toUpperCase() : ''}`,
          style: 'fieldName'
        },
        { 
          text: `Electrical Installation License No: ${EMALicenseType[report.license.licenseType]}/ ${report.license.number} Date & Time of Inspection: ${moment(report.date).format('DD/MM/YYYY')}`,
          style: 'fieldName'
        }
      ],
      styles: {
        name: {
          fontSize: 16,
          bold: true,
          alignment: "right"

        },
        subtitle: {
          fontSize: 12,
          decoration: 'underline',
          alignment: "right"
        },
        title: {
          fontSize: 12,
          decoration: 'underline',
          margin: [ 0, 0, 0, 10 ],
          bold: true
        },
        titleNoBold: {
          fontSize: 14,
          margin: [ 0, 0, 0, 10 ],
        },
        fieldName: {
          fontSize: 11,
          lineHeight: 1.5,
        },
        text_right: {
          fontSize: 10
        },
        checklistName: {
          fontSize: 12,
          bold: true
        },
        checklistItem: {
          fontSize: 11,
          leadingIndent: 15,
          lineHeight: 1.5
        }
      },
      pageSize: 'A4',
      pageOrientation: 'portrait',
      pageMargins: [40,80,40,40]
    }

    const checksdata:any = []
    report.checkList.forEach((checklist: CheckList, index: number) => {
      checksdata.push(this.mapChackLists(checklist, index))
    });
    (docDef.content as any).push(checksdata)


    const principalSignature: Signature | undefined = report.signatures.find((sign:Signature) => sign.principal);

    (docDef.content as any).push([
      { 
        text: `Name of ${ResponsableType[principalSignature!.responsable.type]}: ${principalSignature!.responsable.name}   Designation: ${principalSignature!.designation}` ,
        style: 'filedName' ,
        margin: [0,10,0,0]
      },
      { 
        text: `Signature: ______________________________________________________________ Date: ${moment(principalSignature!.date).format('DD/MM/YYYY')}` ,
        style: 'filedName' 
      },
      { text: 'Notes: ', margin: [0,10,0,0] }
      ])
    
    const notes = report.notes.forEach(note => {
      if (note.needsCheck)
        (docDef.content as any).push([ { columns: [{ text: `${note.text}`, margin: [0,0,0,10], width: '90%' },  { stack: [
  {canvas: [ { type: 'rect', x: 15, y: 0, w: 30, h: 15, color: 'white', lineColor: 'black' } ], margin: [0,0,0,10], width: '10%'},
  {columns: [
   {
    width: 30,
     noWrap: false, //clip at with by wrapping to next line and...
     maxHeight: 15, //don't show the wrapped text
     svg: `${note.checked ? '<svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"	 width="16px" height="16px" viewBox="0 0 229.153 229.153" style="enable-background:new 0 0 229.153 229.153;"	 xml:space="preserve"><g>	<path d="M92.356,223.549c7.41,7.5,23.914,5.014,25.691-6.779c11.056-73.217,66.378-134.985,108.243-193.189		C237.898,7.452,211.207-7.87,199.75,8.067C161.493,61.249,113.274,117.21,94.41,181.744		c-21.557-22.031-43.201-43.853-67.379-63.212c-15.312-12.265-37.215,9.343-21.738,21.737    C36.794,165.501,64.017,194.924,92.356,223.549z"/></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>':'<svg fill="#000000" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 26 26" width="26px" height="26px"><path d="M 21.734375 19.640625 L 19.636719 21.734375 C 19.253906 22.121094 18.628906 22.121094 18.242188 21.734375 L 13 16.496094 L 7.761719 21.734375 C 7.375 22.121094 6.746094 22.121094 6.363281 21.734375 L 4.265625 19.640625 C 3.878906 19.253906 3.878906 18.628906 4.265625 18.242188 L 9.503906 13 L 4.265625 7.761719 C 3.882813 7.371094 3.882813 6.742188 4.265625 6.363281 L 6.363281 4.265625 C 6.746094 3.878906 7.375 3.878906 7.761719 4.265625 L 13 9.507813 L 18.242188 4.265625 C 18.628906 3.878906 19.257813 3.878906 19.636719 4.265625 L 21.734375 6.359375 C 22.121094 6.746094 22.121094 7.375 21.738281 7.761719 L 16.496094 13 L 21.734375 18.242188 C 22.121094 18.628906 22.121094 19.253906 21.734375 19.640625 Z"/></svg>'}`, 
    color: 'black'
    }],
    relativePosition: {
      x: 25,
      y: -30
    }
   }
 ]} ]}])
      else  
        (docDef.content as any).push([ { columns: [{ text: `${note.text}`, margin: [0,0,0,10], width: '90%' } ]}])
    });
    
    const otherSigns = report.signatures.filter(s=> !s.principal)
    otherSigns!.forEach(sign => {
      (docDef.content as any).push([
      {
        text: `${sign.title}`, style: 'titleNoBold'
      },
      {
        text: `${sign.annotation}`, style: 'fieldName'
      },
      { 
        text: `Name of ${ResponsableType[sign!.responsable.type]}: ${sign!.responsable.name}   Designation: ${sign!.designation}` ,
        style: 'filedName' ,
        margin: [0,5,0,0]
      },
      { 
        text: `Signature: ______________________________________________________________ Date: ${moment(sign!.date).format('DD/MM/YYYY')}` ,
        style: 'filedName',
        margin: [0,5,0,0] 
      },
      { 
        text: `Remarks (if any): ${sign.remarks ?? ''}______________________________________________________________________________________________________________________________________________________________________________________________`,
        style: 'filedName',
        margin: [0,5,0,0]
      },
      ])
    });
    pdfMake.createPdf(docDef).open()
  }

  mapChackLists(checklist: CheckList, index: number) {
    const mappedChecks = checklist.checks.map((check, index) => { return { columns: [{ text: `    ${index+1}.${index+1} ${check.text}`, style: 'checklistItem', width:'65%' },
    // { canvas: [ { type: 'rect', x: 15, y: 0, w: 30, h: 15, color: 'white',lineColor: 'black' } ], width: '10%' } 
    { stack: [
  {canvas: [ { type: 'rect', x: 15, y: 0, w: 30, h: 15, color: 'white', lineColor: 'black' } ], margin: [0,0,0,10], width: '10%'},
  {columns: [
   {
    width: 30,
     noWrap: false, //clip at with by wrapping to next line and...
     maxHeight: 15, //don't show the wrapped text
     svg: `${(check.checked as any) === CheckValue.True ? 
                                                        '<svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"	 width="16px" height="16px" viewBox="0 0 229.153 229.153" style="enable-background:new 0 0 229.153 229.153;"	 xml:space="preserve"><g>	<path d="M92.356,223.549c7.41,7.5,23.914,5.014,25.691-6.779c11.056-73.217,66.378-134.985,108.243-193.189		C237.898,7.452,211.207-7.87,199.75,8.067C161.493,61.249,113.274,117.21,94.41,181.744		c-21.557-22.031-43.201-43.853-67.379-63.212c-15.312-12.265-37.215,9.343-21.738,21.737    C36.794,165.501,64.017,194.924,92.356,223.549z"/></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>'
                                                        :(check.checked as any) === CheckValue.False ? '<svg fill="#000000" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 26 26" width="26px" height="26px"><path d="M 21.734375 19.640625 L 19.636719 21.734375 C 19.253906 22.121094 18.628906 22.121094 18.242188 21.734375 L 13 16.496094 L 7.761719 21.734375 C 7.375 22.121094 6.746094 22.121094 6.363281 21.734375 L 4.265625 19.640625 C 3.878906 19.253906 3.878906 18.628906 4.265625 18.242188 L 9.503906 13 L 4.265625 7.761719 C 3.882813 7.371094 3.882813 6.742188 4.265625 6.363281 L 6.363281 4.265625 C 6.746094 3.878906 7.375 3.878906 7.761719 4.265625 L 13 9.507813 L 18.242188 4.265625 C 18.628906 3.878906 19.257813 3.878906 19.636719 4.265625 L 21.734375 6.359375 C 22.121094 6.746094 22.121094 7.375 21.738281 7.761719 L 16.496094 13 L 21.734375 18.242188 C 22.121094 18.628906 22.121094 19.253906 21.734375 19.640625 Z"/></svg>'
                                                        :'<svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"	 viewBox="0 0 409.6 409.6" style="enable-background:new 0 0 409.6 409.6;" xml:space="preserve"><g>	<g>		<path d="M392.533,187.733H17.067C7.641,187.733,0,195.374,0,204.8s7.641,17.067,17.067,17.067h375.467			c9.426,0,17.067-7.641,17.067-17.067S401.959,187.733,392.533,187.733z"/>	</g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>'}`, 
    color: 'black'
    }],
    relativePosition: {
      x: 22.5,
      y: -25
    }
   }
 ]},
    ,{ text: '__________________', width: '25%' }] } })
    const checksContent =  [{ text: `${index+1}    ${checklist.text} ${checklist.annotation}        ${index===0 ? 'Remarks': ''}`, style: 'checklistName', margin: [0, 10, 0, 10] }, mappedChecks]
    return checksContent
  }
}
</script>

<style>

</style>
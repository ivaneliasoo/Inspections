<template>
  <div>
    <v-row class="small">
      <v-col cols="6">
        <ReportStatus style="height: 150px" />
      </v-col>
      <v-col cols="6">
        <ReportsByInspector id="prueba" />
      </v-col>
    </v-row>
    <v-row>
      <v-col><v-btn @click="print">Print</v-btn></v-col>
      <v-col></v-col>
    </v-row>
  </div>
</template>
<script lang="ts">
import { Vue, Component } from 'nuxt-property-decorator'
import pdfMake from 'pdfmake/build/pdfmake'
import pdfFonts from 'pdfmake/build/vfs_fonts'
import { TDocumentDefinitions } from 'pdfmake/interfaces';
@Component
export default class IndexPage extends Vue {
  print() {
    (window as any).pdfMake.vfs = pdfFonts.pdfMake.vfs;
    pdfMake.fonts = {
      
    }

    const docDef: TDocumentDefinitions  = {
      content: [
        { 
          text: 'Inspection Report',
          style: 'name'
        },
        { 
          text: 'CSE EI(R1) FORM (Rev #4)',
          style: 'subtitle'
        },
        { 
          text: 'PARTICULARS OF INSTALLATION',
          style: 'title'
        },
        { 
          text: 'Name of Installation: ',
          style: 'fieldName'
        },
        { 
          text: 'Address of Installation: √',
          style: 'fieldName'
        },
        { 
          text: 'Electrical Installation License No: ',
          style: 'fieldName'
        },
        { 
          text: 'Date & Time of Inspection: ',
          style: 'fieldName'
        }
      ],
      styles: {
        name: {
          fontSize: 12,
          bold: true,
          alignment: "right"

        },
        subtitle: {
          fontSize: 10,
          decoration: 'underline',
          alignment: "right"
        },
        title: {
          fontSize: 12,
          decoration: 'underline',
          margin: [ 0, 0, 0, 10 ],
          bold: true
        },
        fieldName: {
          fontSize: 10,
          lineHeight: 1.2,
          bold: true
        },
        text_right: {
          fontSize: 10
        }
      }
    }

    pdfMake.createPdf(docDef).open()
  }
}
</script>

<style>
.small {
    max-width: 600px;
    margin:  150px auto;
  }
</style>

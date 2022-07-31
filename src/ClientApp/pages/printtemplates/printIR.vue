<template>
  <div style="display: block">
    <header>
      <PrintingLogoir />
      <div class="content">
        <!-- <h1 class="title">Inspection Report</h1> -->
        <h4 class="subtitle">
          INCIDENT REPORT
        </h4>
        <div class="direccion">
          HSBC Institutional<br>
          Trust Services (Singapore) Limited As Trustee of Sabana Reit<br>
          151 Lorong Chuan #02-03<br>
          New Tech Park<br>
          Singapore 556741<br>
        </div>
      </div>
    </header>
    <div class="wrapper">
      <div class="row">
        <v-col justify="center" cols="12">
          <client-only>
            <VueEditor
              v-model="htmlForm.IncidentDetails"
              :editor-options="{
                modules: {
                  toolbar: false
                }
              }"
            />
          </client-only>
          <div v-if="reportData.signatures" class="row">
            <div class="particulars">
              Yours truly:
              <div>
                <img :src="reportData.signatures[0].drawnSign" width="120" alt="">
                ____________________________________
              </div>
              <div>
                <label>
                  {{ reportData.signatures[0].responsibleName }}
                </label>
              </div>
              <div>
                <label>
                  {{ reportData.signatures[0].designation }}
                </label>
              </div>
            </div>
          </div>
        </v-col>
      </div>
    </div>
    <PrintingReportsPhotoRecord v-if="printPhotos" :items="photoRecordsPages" />
  </div>
</template>

<script>
import moment from 'moment'
export default {
  name: 'PrintingReports',
  layout: 'printlayout',
  async asyncData ({ route, $axios }) {
    if (route && route.query && route.query.id) {
      const id = parseInt(route.query.id)
      const token = route.query.token

      const printPhotos =
        route.query && route.query.printPhotos
          ? route.query.printPhotos === 'true'
          : false
      const isCompoundedPhotoRecord =
        route.query && route.query.compoundedPhotoRecord
          ? route.query.compoundedPhotoRecord === 'true'
          : false
      const result = await $axios.$get(`reports/${id}`, {
        headers: {
          Authorization: `bearer ${token}`
        }
      })
      const photoRecords = await $axios.$get(`reports/${id}/photorecord`, {
        headers: {
          Authorization: `bearer ${token}`
        }
      })

      const configuration = await $axios.$get(
        `reportconfiguration/${result.reportConfigurationId}`,
        {
          headers: {
            Authorization: `bearer ${token}`
          }
        }
      )

      const pagesLength = Math.ceil(photoRecords.length / 8)

      const photoRecordsPages = [[]]
      if (photoRecords && photoRecords.length) {
        let currentPage = 0
        photoRecords.forEach((photo, index) => {
          if (index > 0 && index % 8 === 0) {
            photoRecordsPages.push([])
            currentPage++
          }
          if (photoRecordsPages[currentPage]) {
            photoRecordsPages[currentPage].push(photo)
          }
        })

        if (pagesLength % 2 !== 0) {
          photoRecordsPages[currentPage].push({
            thumbnailBase64: '',
            label: ''
          })
        }
      }

      let htmlForm = {}
      const orform = result.forms.filter(
        f => f.name === 'IncidentDetails'
      )

      if (result && orform && orform.length > 0) {
        htmlForm = orform[0].values
      }

      return {
        reportId: id,
        reportData: result,
        photoRecordsPages,
        printPhotos,
        isCompoundedPhotoRecord,
        isPrintable: false,
        htmlForm,
        configuration
      }
    }
  },
  data () {
    return {
      reportId: -1,
      reportData: {},
      photoRecords: [],
      photoRecordsPages: []
    }
  },
  computed: {
    formatedDate () {
      if (this.reportData && this.reportData.date) {
        return moment(this.reportData.date).format('DD-MM-YYYY HH:mm')
      }
      return ''
    },
    lastChecksCount () {
      if (this.reportData && this.reportData.checkLists) {
        return this.reportData.checkLists.length
      }
      return 0
    }
  },
  auth: false,
  mounted () {
    window.isPrintable = true
  }
}
</script>

<style scoped>
header .container {
  display: flex;
  align-items: center;
}
.direccion {
  font-weight: bold;
}
.subtitle {
  text-align: right;
  padding-right: 10px;
}

.container {
  padding: 20px;
  justify-content: end;
}
.wrapper {
  display: flex;
  /* align-items: center; */
  flex-direction: column;
  padding: 20px;
  gap: 10px;
  min-height: 680px;
}
.row {
  display: flex;
  flex-wrap: wrap;
}
.cell {
  overflow: hidden;
  padding: 20px;
  background: #ddd;
  width: 50vw;
}
.description {
  padding: 20px;
  border: 1px solid black;
}
.note {
  display: flex;
  flex-direction: column;
}
.note span {
  text-decoration: underline;
}
.instructions {
  font-weight: bold;
}
.instructions span {
  text-decoration: underline;
}
.particulars {
  width: 100%;
}
.particulars span {
  font-weight: bold;
  text-decoration: underline;
}
.particular-inputs {
  display: flex;
  padding: 20px 0px;
}
.particular-inputs label {
  width: 150px;
}
.particular-inputs input {
  width: 100%;
}
.special-input {
  width: 50%;
}
input {
  background-color: transparent;
  border: none;
  border-bottom: 1px solid black;
  color: #555;
  box-sizing: border-box;
}
input:focus {
  outline: none;
}
.ordered-cell {
  overflow: hidden;
  padding: 20px;
  /* margin: 2px 1em; */
  background: #ddd;
  /* flex: 1; */
  width: 100%;
  align-items: center;
}
ol {
  counter-reset: section;
  list-style-type: none;
}
/* li::before {
    counter-increment: section;
    content: counters(section,".") ". ";
    } */
.ordered-cell ol {
  padding: 0px;
}
.ordered-cell ol li {
  display: flex;
  /* justify-content: space-between;
        align-items: center; */
}
.ordered-cell ol li span {
  text-decoration: underline;
  font-weight: bold;
}
.list-box {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}
.my_table {
  width: 100%;
}
:deep(.ql-container.ql-snow) {
    border: none !important;
}
</style>

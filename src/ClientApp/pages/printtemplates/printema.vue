<template>
  <div style="display: block">
    <header>
      <PrintingLogo />
      <div class="content">
        <h2 class="subtitle">
          EI(R1) REPORT
        </h2>
      </div>
    </header>
    <div class="wrapper">
      <div class="row">
        <div class="cell">
          REPORT BY LICENSED ELECTRICAL WORKER (LEW) FOR CONDOMINIUMS AND
          MULTI-TENANTED BUILDINGS
        </div>
      </div>
      <div class="row">
        <div class="description">
          This form has 7 sections and shall be filled up during site inspection
          in the presence of the consumer’s representative. It may take at least
          40 minutes to fill up the form, depending on the scale of the
          consumer’s electrical installation. You will require your licence
          number and the particulars of the electrical installation licence to
          complete this report.
        </div>
      </div>
      <div class="row">
        <div class="note">
          <span> Note: </span>
          The original copy of this report shall be submitted to the Energy
          Market Authority of Singapore, in accordance with the conditions of
          the installation licence issued to the premises
        </div>
      </div>
      <div class="row">
        <div class="instructions">
          <span>All</span>
          boxes must be filled. Tick (), (X) or (NA) as appropriate to denote
          acceptable, unacceptable and not applicable conditions respectively.
          The “Remarks” column is provided for additional details including
          rectification action for unacceptable conditions.
        </div>
      </div>
      <div class="row">
        <div class="particulars">
          <span> PARTICULARS OF INSTALLATION </span>
          <div class="particular-inputs">
            <label for=""> Name of Installation: </label>
            <input v-model="reportData.name" type="text">
          </div>
          <div class="particular-inputs">
            <label for=""> Address of Installation: </label>
            <input v-model="reportData.address" type="text">
          </div>
          <div class="particular-inputs">
            <label for=""> Licence No: </label>
            <input
              v-model="reportData.licenseNumber"
              class="special-input"
              type="text"
            >
            <label for=""> Date of Inspection: </label>
            <input v-model="formatedDate" class="special-input" type="text">
          </div>
        </div>
      </div>
      <PrintingReportsCheckLists
        v-if="
          reportData.checkLists &&
            configuration.checkListMetadata &&
            configuration.checkListMetadata.display === 'Numbered'
        "
        :check-lists="reportData.checkLists"
      />
      <div
        v-if="
          reportData.checkLists &&
            configuration.checkListMetadata &&
            configuration.checkListMetadata.display === 'Inline'
        "
      >
        {{ reportData.checkLists.map((c) => c.text) }}
      </div>
      <div class="row">
        <table class="my_table">
          <tr>
            <th style="text-align: left">
              DECLARATION BY LEW
            </th>
            <th style="text-align: right" />
          </tr>
          <tr>
            <td>
              I have carried out inspection on
              <input v-model="formatedDate" type="text"> (date of inspection),
              witnessed by the representative of the licensee and hereby I
              declare that the electrical installation is fit and safe for
              operation.
            </td>
          </tr>
        </table>
      </div>
      <div v-if="reportData.signatures" class="row">
        <div class="particulars">
          <div class="particular-inputs">
            <label for=""> Name of LEW: </label>
            <input
              v-model="reportData.signatures[0].responsibleName"
              class="special-input"
              type="text"
            >
            <label for=""> Licence No: </label>
            <input
              v-model="reportData.licenseNumber"
              class="special-input"
              type="text"
            >
          </div>
          <div class="particular-inputs">
            <label for=""> Signature: </label>
            <img :src="reportData.signatures[0].drawnSign" width="120" alt="">
          </div>
        </div>
      </div>
      <div v-if="reportData.signatures" class="row">
        <div class="particulars">
          <span>
            THE INSPECTION WAS WITNESSED BY REPRESENTATIVE OF THE LICENSEE
          </span>
          <div class="particular-inputs">
            <label for=""> Name of representative: </label>
            <input
              v-model="reportData.signatures[1].responsibleName"
              type="text"
            >
          </div>
          <div class="particular-inputs">
            <label for=""> Designation: </label>
            <input v-model="reportData.signatures[1].designation" type="text">
          </div>
          <div class="particular-inputs">
            <label for=""> Signature: </label>
            <img :src="reportData.signatures[1].drawnSign" width="120" alt="">
          </div>
        </div>
      </div>
    </div>
    <!-- TODO-IVAN Hacer Encabezado del Compounded Photo Record -->
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

      let operationalReadings = {}
      const orform = result.forms.filter(
        f => f.name === 'OperationalReadings'
      )
      if (result && orform && orform.length > 0) {
        operationalReadings = orform[0].values
      }

      return {
        reportId: id,
        reportData: result,
        photoRecordsPages,
        printPhotos,
        isCompoundedPhotoRecord,
        isPrintable: false,
        operationalReadings,
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

<style>
header .container {
  display: flex;
  align-items: center;
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
</style>

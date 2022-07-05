import moment from 'moment';
<template>
  <div style="width: 900px">
    This form may take you 5-8 minutes to complete.
    <div class="vbox border-ltr">
      <div class="row-center bold space">
        THE BUILDING CONTROL ACT (CAP 29)
      </div>
      <div class="row-center bold space space-bottom">
        CERTIFICATE OF DESIGN &amp; SUPERVISION OF LIGHTNING PROTECTION SYSTEM
      </div>
    </div>
    <div class="hbox border-ltr">
      <div class="vbox col50p border-r">
        <div class="row space-left">
          Commissioner of Building Control
        </div>
        <div class="row space-left">
          Building &amp; Construction Authority
        </div>
        <div class="row space-left">
          52 Jurong Gateway Road #11-01
        </div>
        <div class="row space-left">
          Singapore 608550
        </div>
        <div class="row space-left space-bottom">
          Website:
          <a
            href="http://www.bca.gov.sg/"
            class="text_"
          >http://www.bca.gov.sg/</a>
        </div>
      </div>
      <div class="vbox col50p space-left">
        <div class="row">
          <span class="underline">INSTRUCTIONS</span>:
        </div>
        <div class="row">
          (1)<span>&nbsp;&nbsp;&nbsp;</span>One copy is to be submitted
        </div>
        <div class="row">
          (2)<span>&nbsp;&nbsp;&nbsp;</span>Please tick in the appropriate box.
        </div>
        <div class="row space-bottom">
          (3)<span>&nbsp;&nbsp;&nbsp;</span>This form is to be filled in BLACK
          INK only.
        </div>
      </div>
    </div>

    <div class="vbox border-ltr">
      <div class="row space">
        <label>
          This Certificate relates to the lightning protection system installed
          in the building(s) approved under: -
        </label>
      </div>

      <div class="row space">
        <label class="col1"> Project Ref. No.: </label>
        &lsqb;<input
          v-model="approvedInfo.ProjectRefNo"
          class="bg-yellow"
          type="text"
        >&rsqb;
      </div>

      <div class="row space">
        <label class="col1"> Address: </label>
        &lsqb;<input
          v-model="reportData.address"
          class="bg-yellow"
          type="text"
        >&rsqb;
      </div>

      <div class="row space">
        <label class="col1"> TS/MK &amp; Lot/Plot: </label>
        &lsqb;<input
          v-model="approvedInfo.TSMKLotPlot"
          class="bg-yellow"
          type="text"
        >&rsqb;
      </div>

      <div class="row space space-bottom">
        <label class="col1"> Remarks: </label>
        &lsqb;<input
          v-model="approvedInfo.MainRemarks"
          class="bg-yellow"
          type="text"
        >&rsqb;
      </div>
    </div>

    <div class="vbox border-ltr">
      <div class="row space bold">
        Part 1 : Lightning Protection System Design Certification
      </div>
    </div>

    <div class="vbox border-ltr">
      <div class="row col space-left">
        <p class="left-align">
          I certify that the design of the above-mentioned lightning protection
          system complies with the requirements of the Building Control
          Regulations 2003, SS:555 and the relevant Singapore Standard Code of
          Practice.
        </p>
      </div>
      <div class="row col space">
        <label class="col10p"> (a) </label>
        <label class="col90p">
          The design of the lightning protection system is in accordance with -
        </label>
      </div>
      <div class="row col">
        <label class="col20p" />
        <label class="col80p">
          the Code of Practice for Protection Against Lightning - SS 555:2010;
          or <br>
          the Code of Practice for Protection Against Lightning - SS 555:2018.
        </label>
      </div>
      <div class="row col space">
        <label class="col10p"> (b) </label>
        <label class="col90p">
          The design and installation of the lightning protection system are
          based on alternative solution
        </label>
      </div>
      <div class="row col space">
        <label class="col10p" />
        <label class="col90p">
          Remarks and Alternative Solution provided:
        </label>
      </div>
      <div class="row col space">
        <label class="col10p" />
        <textarea
          v-model="approvedInfo.DesignSolutionProvided"
          rows="4"
          class="col80p"
        />
      </div>
    </div>

    <div class="hbox border-ltr">
      <div class="vbox col50p border-r space-bottom">
        <div class="row space-left">
          Address of Professional Engineer
        </div>
        <div class="row space-left">
          <textarea
            v-model="approvedInfo.DesignAddressOfProfessionalEngineer"
            rows="5"
            class="col95p"
          />
        </div>
      </div>
      <div class="vbox col50p space-left">
        <div class="row">
          Name, PE Registration Number &amp; Signature of Professional Engineer
        </div>
        <div class="row">
          <div>
            <img :src="reportData.signatures[0].drawnSign" style="margin-left: 15px;object-fit: cover;width: 200px; height: 100px" alt="">
            <input
              v-model="approvedInfo.DesignPhoneNumberOfProfessionalEngineer"
              class="col95p"
            >
          </div>
        </div>
      </div>
    </div>
    <div class="hbox border-ltr border-b">
      <div class="vbox col50p border-r">
        <div class="row space">
          <label class="col2"> Tel No.: </label>
          &lsqb;<input
            v-model="approvedInfo.DesignPhoneNumberOfProfessionalEngineer"
            class="bg-yellow"
            type="text"
          >&rsqb;
        </div>
      </div>
      <div class="vbox col50p space-left space-bottom">
        <div class="row space">
          <label class="col2"> Date: </label>
          &lsqb;<input
            v-model="formatedDate"
            class="bg-yellow"
            type="text"
          >&rsqb;
        </div>
      </div>
    </div>
    <br>

    <div class="vbox border-ltr">
      <div class="row space bold">
        Part 2 : Lightning Protection System Supervision Certification
      </div>
    </div>

    <div class="vbox border-ltr">
      <div class="row col space-left">
        <p class="left-align">
          I have supervised the above-mentioned lightning protection
          installation and hereby certify the installation works complies with
          the requirements of the Building Control Regulations 2003, SS:555 and
          the relevant Singapore Standard Code of Practice. I further certify
          that the installation works has been inspected and tested by me in
          accordance to relevant Singapore Standard Code of Practice.
        </p>
      </div>
      <div class="row col space">
        <label class="col10p" />
        <label class="col90p"> Remarks: </label>
      </div>
      <div class="row col space">
        <label class="col10p" />
        <textarea
          v-model="approvedInfo.SupervisionSolutionProvided"
          rows="6"
          class="col80p"
        />
      </div>
    </div>

    <div class="hbox border-ltr">
      <div class="vbox col50p border-r space-bottom">
        <div class="row space-left">
          Address of Professional Engineer
        </div>
        <div class="row space-left">
          <textarea
            v-model="approvedInfo.SupervisionAddressOfProfessionalEngineer"
            rows="5"
            class="col95p"
          />
        </div>
      </div>
      <div class="vbox col50p space-left">
        <div class="row">
          Name, PE Registration Number &amp; Signature of Professional Engineer
        </div>
        <div class="row">
          <div>
            <img :src="reportData.signatures[0].drawnSign" style="margin-left: 15px;object-fit: cover;width: 200px; height: 100px" alt="">
            <input
              v-model="approvedInfo.SupervisionPhoneNumberOfProfessionalEngineer"
              class="col95p"
            >
          </div>
        </div>
      </div>
    </div>
    <div class="hbox border-ltr border-b">
      <div class="vbox col50p border-r">
        <div class="row space">
          <label class="col2"> Tel No.: </label>
          &lsqb;<input
            v-model="approvedInfo.SupervisionPhoneNumberOfProfessionalEngineer"
            class="bg-yellow"
            type="text"
          >&rsqb;
        </div>
      </div>
      <div class="vbox col50p space-left space-bottom">
        <div class="row space">
          <label class="col2"> Date: </label>
          &lsqb;<input
            v-model="formatedDate"
            class="bg-yellow"
            type="text"
          >&rsqb;
        </div>
      </div>
    </div>

    <br>
    <div class="vbox border-ltr">
      <div class="row-center bold space">
        THE BUILDING CONTROL ACT (CAP 29)
      </div>
      <div class="row-center bold space space-bottom">
        CERTIFICATE OF DESIGN &amp; SUPERVISION OF LIGHTNING PROTECTION
        SYSTEM<br>
        (Earth Resistance &amp; Electrical Continuity Test Form)
      </div>
    </div>
    <div class="hbox border-ltr">
      <div class="vbox col50 border-r">
        <div class="row space-left">
          Commissioner of Building Control
        </div>
        <div class="row space-left">
          Building &amp; Construction Authority
        </div>
        <div class="row space-left">
          52 Jurong Gateway Road #11-01
        </div>
        <div class="row space-left">
          Singapore 608550
        </div>
        <div class="row space-left space-bottom">
          Website:
          <a
            href="http://www.bca.gov.sg/"
            class="text_"
          >http://www.bca.gov.sg/</a>
        </div>
      </div>
      <div class="vbox col50 space-left">
        <div class="row">
          <span class="underline">INSTRUCTIONS</span>:
        </div>
        <div class="row">
          (1)<span>&nbsp;&nbsp;&nbsp;</span>One copy is to be submitted
        </div>
        <div class="row space-bottom">
          (2)<span>&nbsp;&nbsp;&nbsp;</span>This form is to be filled in BLACK
          INK only.
        </div>
      </div>
    </div>

    <div class="vbox border-ltr">
      <div class="row space">
        <label class="col1"> Project Ref. No.: </label>
        &lsqb;<input
          v-model="approvedInfo.ProjectRefNo"
          class="bg-yellow"
          type="text"
        >&rsqb;
      </div>
    </div>

    <div class="vbox border-ltr">
      <div class="row space bold">
        Lightning Protection System Earthing &amp; Electrical Continuity Test
      </div>
    </div>

    <div class="hbox border-ltr">
      <div class="vbox col40p border-r">
        <div class="row space">
          <label class="col1"> Address: </label>
          &lsqb;<input
            v-model="reportData.address"
            class="bg-yellow"
            type="text"
          >&rsqb;
        </div>
      </div>
      <div class="vbox col60p space-left">
        <div class="row space">
          <label class="col1"> Test Date: </label>
          &lsqb;<input
            v-model="electricalTest.TestDate"
            class="bg-yellow"
            type="text"
          >&rsqb;
        </div>
      </div>
    </div>

    <div class="vbox border-ltr">
      <div class="row space bold">
        Test Instrument Details
      </div>
    </div>

    <div class="hbox border-ltr">
      <div class="vbox col40p border-r">
        <div class="row space">
          <label class="col2"> Brand &amp; Model 1: </label>
          &lsqb;
          <input
            v-model="electricalTest.Brand1"
            class="bg-yellow"
            type="text"
            style="width: 150px"
          >
          &rsqb;
        </div>
      </div>
      <div class="vbox col30p border-r space-left">
        <div class="row space">
          <label class="col1"> Serial No.: </label>
          &lsqb;
          <input
            v-model="electricalTest.Serial1"
            class="bg-yellow"
            type="text"
          >
          &rsqb;
        </div>
      </div>
      <div class="vbox col30p space-left">
        <div class="row space">
          <label class="col1"> Calibrated: </label>
          &lsqb;
          <input
            v-model="electricalTest.Calibrated1"
            class="bg-yellow"
            type="text"
          >
          &rsqb;
        </div>
      </div>
    </div>

    <div class="hbox border-ltr">
      <div class="vbox col40p border-r">
        <div class="row space">
          <label class="col2"> Brand &amp; Model 2: </label>
          &lsqb;<input
            v-model="electricalTest.Brand2"
            class="bg-yellow"
            type="text"
            style="width: 150px"
          >&rsqb;
        </div>
      </div>
      <div class="vbox col30p border-r space-left">
        <div class="row space">
          <label class="col1"> Serial No.: </label>
          &lsqb;<input
            v-model="electricalTest.Serial2"
            class="bg-yellow"
            type="text"
          >&rsqb;
        </div>
      </div>
      <div class="vbox col30p space-left">
        <div class="row space">
          <label class="col1"> Calibrated: </label>
          &lsqb;<input
            v-model="electricalTest.Calibrated2"
            class="bg-yellow"
            type="text"
          >&rsqb;
        </div>
      </div>
    </div>

    <div class="vbox space border-ltr border-b">
      <div class="row space bold">
        Table 1 : Earth Resistance System Test
      </div>

      <table class="table">
        <thead>
          <tr>
            <th style="width: 15%">
              Type of Test
            </th>
            <th colspan="12">
              Resistance of Earth Termination System [Ohm]
            </th>
            <th rowspan="2">
              Remarks
            </th>
          </tr>
        </thead>

        <tbody>
          <tr>
            <td>Earth Electrode Point</td>
            <td v-for="index in 12" :key="index">
              {{ index }}
            </td>
            <td>
              <input
                v-model="certificate2.table1.tableRemarks[0]"
                type="text"
              >
            </td>
          </tr>
          <tr>
            <td>Point Resistance [R &lt; N x 10Ω]</td>
            <td v-for="index in 12" :key="index">
              <input
                v-model="certificate2.table1.pointResistance[index]"
                type="text"
              >
            </td>
            <td>
              <input
                v-model="certificate2.table1.tableRemarks[1]"
                type="text"
              >
            </td>
          </tr>
          <tr>
            <td>Electrical Continuity Test Between N &amp; N+1</td>
            <td v-for="index in 12" :key="index">
              <input
                v-model="certificate2.table1.electricalContinuity[index]"
                type="text"
              >
            </td>
            <td>
              <input
                v-model="certificate2.table1.tableRemarks[2]"
                type="text"
              >
            </td>
          </tr>
        </tbody>

        <tbody>
          <tr>
            <td>Earth Electrode Point</td>
            <td
              v-for="index in Array(12)
                .fill(1)
                .map((_, i) => i + 13)"
              :key="index"
            >
              {{ index }}
            </td>
            <td>
              <input
                v-model="certificate2.table1.tableRemarks[3]"
                type="text"
              >
            </td>
          </tr>
          <tr>
            <td>Point Resistance [R &lt; N x 10Ω]</td>
            <td
              v-for="index in Array(12)
                .fill(1)
                .map((_, i) => i + 13)"
              :key="index"
            >
              <input
                v-model="certificate2.table1.pointResistance[index]"
                type="text"
              >
            </td>
            <td>
              <input
                v-model="certificate2.table1.tableRemarks[4]"
                type="text"
              >
            </td>
          </tr>
          <tr>
            <td>Electrical Continuity Test Between N &amp; N+1</td>
            <td
              v-for="index in Array(12)
                .fill(1)
                .map((_, i) => i + 13)"
              :key="index"
            >
              <input
                v-model="certificate2.table1.electricalContinuity[index]"
                type="text"
              >
            </td>
            <td>
              <input
                v-model="certificate2.table1.tableRemarks[5]"
                type="text"
              >
            </td>
          </tr>
        </tbody>
      </table>

      <div class="row space">
        Overall resistance of the Earth Termination System
      </div>

      <div class="row space">
        <label class="col1"> [Roverall &le; 10Ω]: </label>
        &lsqb;<input
          v-model="electricalTest.ETSOverallResistance"
          class="bg-yellow"
          type="text"
        >&rsqb;
      </div>

      <div class="row col space">
        <label class="col90p"> Remarks: </label>
      </div>
      <div class="row col space">
        <textarea
          v-model="electricalTest.ETSRemarks"
          rows="6"
          class="col90p"
        />
      </div>
    </div>

    <br>
    <div class="vbox space border-ltr border-b">
      <div class="row space bold">
        Table 2 : Natural Down Conductor Electrical Resistance Test
      </div>

      <table class="table">
        <thead>
          <tr>
            <th style="width: 15%">
              Type of Test
            </th>
            <th colspan="12">
              Continuity Test for Down Conductor System [Ohm]
            </th>
            <th rowspan="2">
              Remarks
            </th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>Electrical Continuity Test</td>
            <td v-for="index in 12" :key="index">
              {{ index }}
            </td>
            <td>
              <input
                v-model="electricalTest.ContinuityTestRemarks"
                type="text"
              >
            </td>
          </tr>

          <tr>
            <td>Overall value in Ohm [R &lt; 0.2Ω]</td>
            <td v-for="index in 12" :key="index">
              <input
                v-model="certificate2.table1.electricalContinuity[index]"
                type="text"
              >
            </td>
            <td>
              <input
                v-model="certificate2.table2.tableRemarks[1]"
                type="text"
              >
            </td>
          </tr>
        </tbody>
        <tbody>
          <tr>
            <td>Electrical Continuity Test</td>
            <td
              v-for="index in Array(12)
                .fill(1)
                .map((_, i) => i + 13)"
              :key="index"
            >
              {{ index }}
            </td>
            <td>
              <input
                v-model="certificate2.table2.tableRemarks[2]"
                type="text"
              >
            </td>
          </tr>

          <tr>
            <td>Overall value in Ohm [R &lt; 0.2Ω]</td>
            <td v-for="index in 12" :key="index">
              <input
                v-model="certificate2.table2.electricalContinuity[index]"
                type="text"
              >
            </td>
            <td>
              <input
                v-model="certificate2.table2.tableRemarks[3]"
                type="text"
              >
            </td>
          </tr>
        </tbody>
      </table>
      <div class="row space">
        * Table 2 applicable for down conductor using natural component such as
        rebar of concrete column.
      </div>

      <div class="row col space">
        <label class="col90p"> Remarks: </label>
      </div>
      <div class="row col space">
        <textarea
          v-model="electricalTest.ContinuityTestRemarks"
          rows="7"
          class="col90p"
        />
      </div>

      <div class="row col space">
        <label class="col5p"> 1. </label>
        <label class="col90p">
          Test Report shall be provided for each structure/building.
        </label>
      </div>
      <div class="row col space">
        <label class="col5p"> 2. </label>
        <label class="col90p">
          LPS as-built plans should include air-termination system, down
          conductor system, earth termination system, details of equipotential
          bonding with metallic fixtures/structural steel works, zones of
          lightning protection provided by rolling sphere and/or protection
          angle, photos of concealed equipotential bonding points between metal
          fixtures, steel rebar of concrete with LPS, etc.
        </label>
      </div>
      <div class="row col space">
        <label class="col5p"> 3. </label>
        <label class="col90p">
          Any other details as required by BCA but not mentioned above.
        </label>
      </div>
    </div>
    <div style="height: 200px" />
    <div class="row space">
      <div>
        <img :src="reportData.signatures[0].drawnSign" style="margin-left: 15px;object-fit: cover;width: 300px; height: 200px" alt="">
        &lsqb;<input
          v-model="reportData.signatures[0].responsibleName"
          class="bg-yellow"
          type="text"
        >&rsqb;
      </div>
    </div>
    <div class="row space">
      Signature/Name/PE Registration Number of Professional Engineer
    </div>
  </div>
</template>

<script>
import moment from 'moment'
export default {
  name: 'LightningProtection',
  async asyncData ({ route, $axios }) {
    if (route && route.query && route.query.id) {
      const id = parseInt(route.query.id)
      const token = route.query.token

      const printPhotos = route.query && route.query.printPhotos ? route.query.printPhotos === 'true' : false
      const isCompoundedPhotoRecord = route.query && route.query.compoundedPhotoRecord ? route.query.compoundedPhotoRecord === 'true' : false
      const result = await $axios.$get(`reports/${id}`, {
        headers: {
          Authorization: `bearer ${token}`
        }
      })

      const configuration = await $axios.$get(`reportconfiguration/${result.reportConfigurationId}`, {
        headers: {
          Authorization: `bearer ${token}`
        }
      })

      let approvedInfo = {}
      let electricalTest = {}
      if (result) {
        approvedInfo = result.forms.filter(f => f.name === 'ApprovedInfo')[0].values
        electricalTest = result.forms.filter(f => f.name === 'ElectricalTest')[0].values
      }

      return {
        reportId: id,
        reportData: result,
        printPhotos,
        isCompoundedPhotoRecord,
        isPrintable: false,
        approvedInfo,
        electricalTest,
        configuration,
      }
    }
  },
  data () {
    return {
      reportId: -1,
      reportData: {},
      certificate1: {
      // CERTIFICATE OF DESIGN & SUPERVISION OF LIGHTNING PROTECTION SYSTEM
        projectRefNo: '',
        address: '',
        TS_MK_Lot_Plot: '',
        remarks: '',
        part1: {
          remarks: '',
          engineerAddress: '',
          engineerNameRegNumber: '',
          telfNo: '',
          date: ''
        },
        part2: {
          remarks: '',
          engineerAddress: '',
          engineerNameRegNumber: '',
          telfNo: '',
          date: ''
        }
      },
      certificate2: {
      // Earth Resistance & Electrical Continuity Test Form
        projectRefNo: '',
        address: '',
        testDate: '',
        brandModel1: '',
        serialNo1: '',
        calibrated1: '',
        brandModel2: '',
        serialNo2: '',
        calibrated2: '',
        table1: {
          pointResistance: [],
          electricalContinuity: [],
          tableRemarks: [],
          overallResistance: '',
          generalRemarks: ''

        },
        table2: {
          electricalContinuity: [],
          tableRemarks: [],
          generalRemarks: ''
        },
        engineerNameRegNumber: ''
      }
    }
  },
  computed: {
    formatedDate () {
      if (this.reportData && this.reportData.date) {
        return moment(this.reportData.date).format('DD-MM-YYYY HH:mm')
      }
      return ''
    }
  },
  auth: false,
  mounted () {
    window.isPrintable = true
  }
}
</script>

<style scoped>
.table {
  width: 100%;
  border: 1px solid black;
  border-collapse: collapse;
}

td {
  border: 1px solid black;
}

input {
  background-color: transparent;
  border: none;
  border-bottom: 1px solid black;
  color: #555;
  width: 80%;
}

input:focus {
  outline: none;
}

.bordered {
  border: 1px solid black;
}

.border-ltr {
  border-left: 1px solid black;
  border-top: 1px solid black;
  border-right: 1px solid black;
}

.border-r {
  border-right: 1px solid black;
}

.border-b {
  border-bottom: 1px solid black;
}

.vbox {
  display: flex;
  justify-content: flex-start;
  flex-direction: column;
}

.hbox {
  display: flex;
  justify-content: flex-start;
}

.row {
  display: flex;
  justify-content: flex-start;
}

.row-center {
  display: flex;
  justify-content: center;
}

.space {
  padding: 10px;
}

.space-bottom {
  padding-bottom: 12px;
}

.space-left {
  padding-left: 10px;
}

.col {
  width: 100%;
}

.col1 {
  width: 150px;
  text-align: left;
}

.col2 {
  width: 140px;
  text-align: left;
}

.col5p {
  width: 5%;
  text-align: left;
}

.col10p {
  width: 10%;
  text-align: left;
}

.col20p {
  width: 20%;
  text-align: left;
}

.col30p {
  width: 30%;
  text-align: left;
}

.col40p {
  width: 40%;
  text-align: left;
}

.col50p {
  width: 50%;
  text-align: left;
}

.col60p {
  width: 60%;
  text-align: left;
}

.col80p {
  width: 80%;
  text-align: left;
}

.col90p {
  width: 90%;
  text-align: left;
}

.col95p {
  width: 95%;
  text-align: left;
}

.left-align {
  text-align: left;
  text-align-last: left;
}

.bg-yellow {
  background-color: lightyellow;
}

.bold {
  font-weight: bold;
}

.underline {
  text-decoration: underline;
}
</style>

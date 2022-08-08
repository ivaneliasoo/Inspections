<template>
  <div style="display: block">
    <header>
      <div class="container">
        <PrintingLogo />
        <div class="title">
          <h1 class="">
            {{ reportData.title }}
          </h1>
          <h5 class="subtitle">
            {{ reportData.formName }}
          </h5>
        </div>
      </div>
    </header>
    <div class="wrapper">
      <div class="row">
        <div class="particulars">
          <span> PARTICULARS OF INSTALLATION </span>
          <div class="particular-inputs">
            <label for=""> Location: </label>
            <input v-model="reportData.address" type="text" />
          </div>
          <div class="particular-inputs" style="align-items: center">
            <div class="box">
              <label for=""> Client: </label>
              <input v-model="reportData.licenseName" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 290px !important">
                Approved Load:
              </label>
              <input v-model="reportData.licenseAmp" type="text" />
              <label for="" style="width: 35px !important"> A/ </label>
              <input v-model="reportData.licenseVolt" type="text" />
              <label for=""> V </label>
            </div>
          </div>
          <div class="particular-inputs">
            <div class="box">
              <label for=""> EI Licence: <b>E/</b> </label>
              <input
                v-model="reportData.licenseNumber"
                class="special-input"
                type="text"
              />
            </div>
            <div class="box">
              <label for=""> Drawing No: </label>
              <input
                v-model="additionalFields.DrawingNo"
                class="special-input"
                type="text"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <span style="text-decoration: underline; font-weight: bold">
          2. BEFORE TURN-ON
        </span>
        <div class="particulars">
          <div class="particular-inputs">
            <div class="box">
              <label for="">
                <b> a) Phase Out Test for Cable</b>
              </label>
            </div>
            <div class="box">
              <!-- <span>Acceptable / Not Acceptable</span> -->
              <input
                id="one"
                :checked="beforeTurnOn.PhaseOutTestCable === 'Acceptable'"
                type="radio"
                value="One"
              />
              <label for="one">Acceptable</label>
              /
              <input
                id="two"
                :checked="beforeTurnOn.PhaseOutTestCable === 'Not Acceptable'"
                type="radio"
                value="Two"
              />
              <label for="two">Not Acceptable</label>
            </div>
          </div>
        </div>
        <div
          v-for="checkList in reportData.checkLists"
          :key="checkList.checkListId"
          class="particulars"
        >
          <div class="particular-inputs" />
          <span style="text-decoration: none; width: 150px">
            <b>{{ checkList.text }}: </b>
          </span>
          <div
            v-for="checkListItem in checkList.checks"
            :key="checkListItem.id"
            class="box-checks"
          >
            <label for="">
              {{ checkListItem.text }}
            </label>
            <input v-model="checkListItem.checked" type="checkbox" />
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="align-items: center">
            <span style="text-decoration: none; width: 150px">
              <b>d) Earth Pit</b>
            </span>
            <div class="box">
              <label for=""> Ring circuit </label>
              <input
                v-model="beforeTurnOn.EarthPitRingCircuit"
                type="checkbox"
              />
            </div>
            <div class="box">
              <label for=""> No. of Earth Pit </label>
              <input
                v-model="beforeTurnOn.EarthPitNoOfEarthPit"
                class="special-input"
                type="text"
              />
            </div>
            <div class="box" style="align-items: center">
              <label for=""> Overall Earth pit Value </label>
              <input
                v-model="beforeTurnOn.EarthPitOverallEarthPitValue"
                class="special-input"
                type="text"
              />
              <label for="">Ω</label>
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs">
            <div class="box">
              <label for="">
                <b> e) Sub Main Cable Protection</b>
              </label>
              <input
                v-model="beforeTurnOn.SubMainCableProtection"
                type="checkbox"
              />
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="gap: 10px">
            <div class="box">
              <label for="" style="width: 100px !important"> L1-E: </label>
              <input v-model="beforeTurnOn.L1E" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L1-N: </label>
              <input v-model="beforeTurnOn.L1N" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L1-L2: </label>
              <input v-model="beforeTurnOn.L1L2" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L1-E: </label>
              <input v-model="beforeTurnOn.L1E" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L1-N: </label>
              <input v-model="beforeTurnOn.L1N" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L1-L2: </label>
              <input v-model="beforeTurnOn.L1L2" type="text" />
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="gap: 10px">
            <div class="box">
              <label for="" style="width: 100px !important"> L2-E: </label>
              <input v-model="beforeTurnOn.L2E" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L2-N: </label>
              <input v-model="beforeTurnOn.L2N" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L2-L2: </label>
              <input v-model="beforeTurnOn.L2L2" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L2-E: </label>
              <input v-model="beforeTurnOn.L2E" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L2-N: </label>
              <input v-model="beforeTurnOn.L2N" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L2-L2: </label>
              <input v-model="beforeTurnOn.L2L2" type="text" />
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="gap: 10px">
            <div class="box">
              <label for="" style="width: 100px !important"> L3-E: </label>
              <input v-model="beforeTurnOn.L3E" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L3-N: </label>
              <input v-model="beforeTurnOn.L3N" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L3-L2: </label>
              <input v-model="beforeTurnOn.L2L3" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L3-E: </label>
              <input v-model="beforeTurnOn.L3E" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L3-N: </label>
              <input v-model="beforeTurnOn.L3N" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L3-L2: </label>
              <input v-model="beforeTurnOn.L2L3" type="text" />
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="gap: 10px">
            <div class="box">
              <label for="" style="width: 100px !important"> N-E: </label>
              <input v-model="beforeTurnOn.NE" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> Remarks: </label>
              <input v-model="beforeTurnOn.IncomingCablesRemarks" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> N-E: </label>
              <input v-model="beforeTurnOn.NE" type="text" />
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> Remarks: </label>
              <input v-model="beforeTurnOn.Remarks" type="text" />
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <span style="text-decoration: underline; font-weight: bold">
          3. AFTER TURN-ON
        </span>
        <div class="particulars">
          <div class="particular-inputs">
            <div class="box">
              <label for="">
                <b> a) Phase Sequence Test</b>
              </label>
            </div>
            <div class="box">
              <!-- <span>Acceptable / Not Acceptable</span> -->
              <input
                id="one"
                :checked="
                  afterTurnOn.PhaseSequenceTest === 'Acceptable' ? true : false
                "
                type="radio"
                value="one"
              />
              <label for="one">Acceptable</label>
              /
              <input
                id="two"
                :checked="
                  afterTurnOn.PhaseSequenceTest === 'Not Acceptable'
                    ? true
                    : false
                "
                type="radio"
                value="two"
              />
              <label for="two">Not Acceptable</label>
            </div>
          </div>
        </div>
        <b>b) Voltage</b>
        <div class="particulars">
          <div class="particular-inputs">
            <div class="box">
              <label for="" style="width: 100px !important"> L1-N: </label>
              <input v-model="afterTurnOn.L1N" type="text" />
              V&nbsp;
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L2-N: </label>
              <input v-model="afterTurnOn.L2N" type="text" />
              V&nbsp;
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L3-N: </label>
              <input v-model="afterTurnOn.L3N" type="text" />
              V&nbsp;
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> N-E: </label>
              <input v-model="afterTurnOn.NE" type="text" />
              V&nbsp;
            </div>
          </div>
          <div class="particular-inputs">
            <div class="box">
              <label for="" style="width: 100px !important"> L1-L2: </label>
              <input v-model="afterTurnOn.L1L2" type="text" />
              V&nbsp;
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L1-L3: </label>
              <input v-model="afterTurnOn.L1L3" type="text" />
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L2-L3: </label>
              <input v-model="afterTurnOn.L2L3" type="text" />
              V&nbsp;
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="align-items: center">
            <span style="text-decoration: none; width: 250px">
              <b>c) Earth Loop Impedance</b>
            </span>
            <div class="box">
              <label for="" style="width: 100px !important"> L1-E: </label>
              <input v-model="afterTurnOn.L1E" type="text" />
              Ω
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L2-E: </label>
              <input v-model="afterTurnOn.L2E" type="text" />
              Ω
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> L3-E: </label>
              <input v-model="afterTurnOn.L3E" type="text" />
              Ω
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="align-items: center; gap: 10px">
            <span style="text-decoration: none; width: 200px">
              <b>d) Earth-fault Settings:</b>
            </span>
            <div class="box">
              <label for="" style="width: 100px !important"> RCCB: </label>
              <input v-model="afterTurnOn.RCCB" type="text" />
              mA
            </div>
            <span> OR</span>
            <div class="box">
              <label for="" style="width: 100px !important"> ELR: </label>
              <input v-model="afterTurnOn.ELRA" type="text" />
              A
              <input v-model="afterTurnOn.ELRS" type="text" />
              s
            </div>
            <div class="box">
              <label for="" style="width: 100px !important"> EF: </label>
              <input v-model="afterTurnOn.EFA" type="text" />
              A
              <input v-model="afterTurnOn.EFS" type="text" />
              s
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs">
            <div >
              <label for="">
                <b> e) Function Test</b>
              </label>
            </div>
            <div >
              <!-- <span>Acceptable / Not Acceptable</span> -->
              <input
                id="one"
                :checked="afterTurnOn.FunctionTest === 'Acceptable'"
                type="radio"
                value="One"
              />
              <label for="one">Acceptable</label>
              /
              <input
                id="two"
                :checked="afterTurnOn.FunctionTest === 'Not Acceptable'"
                type="radio"
                value="Two"
              />
              <label for="two">Not Acceptable</label>
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs">
            <div >
              <label for="">
                <b> f) Tariff Meter "Direction":</b>
              </label>
            </div>
            <div >
              <!-- <span>Acceptable / Not Acceptable / Not Applicable</span> -->
              <input
                id="one"
                :checked="afterTurnOn.TariffMeterDirection === 'Acceptable'"
                type="radio"
                value="One"
              />
              <label for="one">Acceptable</label>
              /
              <input
                id="two"
                :checked="afterTurnOn.TariffMeterDirection === 'Not Acceptable'"
                type="radio"
                value="Two"
              />
              <label for="two">Not Acceptable</label>
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs">
            <div >
              <label for="">
                <b> g) Polarity Test:</b>
              </label>
            </div>
            <div >
              <!-- <span>Acceptable / Not Acceptable</span> -->
              <input
                id="one"
                :checked="afterTurnOn.PolarityTest === 'Acceptable'"
                type="radio"
                value="One"
              />
              <label for="one">Acceptable</label>
              /
              <input
                id="two"
                :checked="afterTurnOn.PolarityTest === 'Not Acceptable'"
                type="radio"
                value="Two"
              />
              <label for="two">Not Acceptable</label>
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="particulars">
          <span style="text-decoration: underline; font-weight: bold">
            4. INSPECTIONS BY
          </span>
          <div class="particular-inputs">
            <label for="" style="width: 20% !important"> Recorded by: </label>
            <input
              v-model="reportData.recordedBy"
              class="special-input"
              type="text"
            />
            <label for="" style="width: 20% !important">
              Witness, if any:
            </label>
            <input
              v-model="reportData.wtiness"
              class="special-input"
              type="text"
            />
          </div>
          <div class="particular-inputs">
            <label for=""> Date: </label>
            <input v-model="formatedDate" class="special-input" type="text" />
            <label for=""> Signature: </label>
            <input
              v-model="reportData.signature"
              class="special-input"
              type="text"
            />
          </div>
          <div class="particular-inputs">
            <label for=""> Remarks: </label>
            <input v-model="reportData.remarks" type="text" />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import moment from 'moment'
export default {
  name: 'PrintingReports',
  layout: 'printlayout',
  async asyncData({ route, $axios }) {
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
          Authorization: `bearer ${token}`,
        },
      })

      const configuration = await $axios.$get(
        `reportconfiguration/${result.reportConfigurationId}`,
        {
          headers: {
            Authorization: `bearer ${token}`,
          },
        }
      )

      let beforeTurnOn = {}
      let afterTurnOn = {}
      let additionalFields = {}
      if (result) {
        beforeTurnOn = result.forms.filter((f) => f.name === 'BEFORETURNON')[0]
          .values
        afterTurnOn = result.forms.filter((f) => f.name === 'AFTERTURNON ')[0]
          .values
        additionalFields = result.forms.filter(
          (f) => f.name === 'AdditionalFieldsTT'
        )[0].values
      }

      return {
        reportId: id,
        reportData: result,
        printPhotos,
        isCompoundedPhotoRecord,
        isPrintable: false,
        beforeTurnOn,
        afterTurnOn,
        configuration,
        additionalFields,
      }
    }
  },
  data() {
    return {
      reportId: -1,
      reportData: {},
    }
  },
  computed: {
    formatedDate() {
      if (this.reportData && this.reportData.date) {
        return moment(this.reportData.date).format('DD-MM-YYYY HH:mm')
      }
      return ''
    },
  },
  auth: false,
  mounted() {
    window.isPrintable = true
  },
}
</script>

<style scoped>
.container {
  /* padding: 20px; */
  justify-content: space-between;
}

header .container {
  display: flex;
  align-items: center;
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

  /* padding: 20px; */
  background: #ddd;
  width: 50vw;
}

.description {
  /* padding: 20px; */
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

  /* padding: 20px 0px; */
  justify-content: space-between;
}

.box-checks {
  display: inline-flex;
  align-items: center;
  justify-content: space-between;
  margin: 0 10px;
}

.box {
  display: flex;
  align-items: center;
}

.box label {
  width: 210px !important;
}

.particular-inputs label {
  width: auto;
}

input {
  background-color: transparent;
  border: none;
  border-bottom: 1px solid black;
  color: #555;
  box-sizing: border-box;
}

.particular-inputs input {
  width: 100%;
}

.special-input {
  width: 50%;
}

.special-input-short {
  width: 20px;
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
  padding: 0;
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

.title-box {
  display: flex;
  flex-direction: column;
  text-align: right;
}

.title-box h5 {
  text-decoration: underline;
}
</style>

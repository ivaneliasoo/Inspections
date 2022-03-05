<template>
  <div style="display: block">
    <header>
      <div class="container">
        <h1 class="mh-logo">
          <!-- <PrintingLogo /> -->
        </h1>
        <div class="title-box">
          <h1 class="">Testing Turn-On Record (TT System)</h1>
          <h5 class="">CSE T&C-TT FORM</h5>
        </div>
      </div>
    </header>
    <div class="wrapper">
      <div class="row">
        <div class="particulars">
          <span>
            PARTICULARS OF INSTALLATION
          </span>
          <div class="particular-inputs">
            <label for="">
              Location:
            </label>
            <input v-model="reportData.location" type="text">
          </div>
          <div class="particular-inputs" style="align-items: center;">
            <div class="box">
              <label for="">
                Client:
              </label>
              <input v-model="reportData.client" type="text">
            </div>
            <div class="box" style="">
              <label for="" style="width: 290px !important;">
                Approved Load:
              </label>
              <input v-model="reportData.approvedLoad" type="text">
              <label for="" style="width:35px !important">
                A/
              </label>
              <input v-model="reportData.approvedAt" type="text">
              <label for="">
                V
              </label>
            </div>
          </div>
          <div class="particular-inputs">
            <div class="box">
              <label for="">
                EI Licence: <b>E/</b>
              </label>
              <input v-model="reportData.eiLicence" class="special-input" type="text">
            </div>
            <div class="box">
              <label for="">
                Drawing No:
              </label>
              <input v-model="reportData.drawnum" class="special-input" type="text">
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <span style="text-decoration: underline; font-weight: bold;">
          2. BEFORE TURN-ON
        </span>
        <div class="particulars">
          <div class="particular-inputs" style="gap: 10px;">
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-E:
              </label>
              <input v-model="reportData.beforeTurnOnLOneELeft" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-N:
              </label>
              <input v-model="reportData.beforeTurnOnLOneNLeft" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-L2:
              </label>
              <input v-model="reportData.beforeTurnOnLOneLTwoLeft" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-E:
              </label>
              <input v-model="reportData.beforeTurnOnLOneERight" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-N:
              </label>
              <input v-model="reportData.beforeTurnOnLOneNRight" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-L2:
              </label>
              <input v-model="reportData.beforeTurnOnLOneLTwoRight" type="text">
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="gap: 10px;">
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-E:
              </label>
              <input v-model="reportData.beforeTurnOnLOneELeftTwo" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-N:
              </label>
              <input v-model="reportData.beforeTurnOnLOneNLeftTwo" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-L2:
              </label>
              <input v-model="reportData.beforeTurnOnLOneLTwoLeftTwo" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-E:
              </label>
              <input v-model="reportData.beforeTurnOnLOneERightTwo" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-N:
              </label>
              <input v-model="reportData.beforeTurnOnLOneNRightTwo" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-L2:
              </label>
              <input v-model="reportData.beforeTurnOnLOneLTwoRightTwo" type="text">
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="gap: 10px;">
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-E:
              </label>
              <input v-model="reportData.beforeTurnOnLOneELeftThree" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-N:
              </label>
              <input v-model="reportData.beforeTurnOnLOneNLeftThree" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-L2:
              </label>
              <input v-model="reportData.beforeTurnOnLOneLTwoLeftThree" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-E:
              </label>
              <input v-model="reportData.beforeTurnOnLOneERightThree" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-N:
              </label>
              <input v-model="reportData.beforeTurnOnLOneNRightThree" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-L2:
              </label>
              <input v-model="reportData.beforeTurnOnLOneLTwoRightThree" type="text">
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="gap: 10px;">
            <div class="box">
              <label for="" style="width: 100px !important;">
                N-E:
              </label>
              <input v-model="reportData.beforeTurnOnNELeft" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                Remarks:
              </label>
              <input v-model="reportData.beforeTurnOnRemarksLeft" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-E:
              </label>
              <input v-model="reportData.beforeTurnOnNERight" type="text">
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-N:
              </label>
              <input v-model="reportData.beforeTurnOnRemarksRight" type="text">
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <span style="text-decoration: underline; font-weight: bold;">
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
              <input id="one" v-model="reportData.phaseSequenceTest" type="radio" value="One">
              <label for="one">Acceptable</label>
              /
              <input id="two" v-model="reportData.phaseSequenceTest" type="radio" value="Two">
              <label for="two">Not Acceptable</label>
            </div>
          </div>
        </div>
        <b>b) Voltage</b>
        <div class="particulars">
          <div class="particular-inputs">
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-N:
              </label>
              <input v-model="reportData.voltageLOneN" type="text">
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L2-N:
              </label>
              <input v-model="reportData.voltageLTwoN" type="text">
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L3-N:
              </label>
              <input v-model="reportData.voltageLThreeN" type="text">
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                N-E:
              </label>
              <input v-model="reportData.voltageNE" type="text">
              V
            </div>
          </div>
          <div class="particular-inputs">
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-L2:
              </label>
              <input v-model="reportData.voltageLOneLTwo" type="text">
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-L3:
              </label>
              <input v-model="reportData.voltageLOneLThree" type="text">
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L2-L3:
              </label>
              <input v-model="reportData.voltageLTwoLThree" type="text">
              V
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="align-items: center;">
            <span style="text-decoration:none; width: 250px;">
              <b>c) Earth Loop Impedance</b>
            </span>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-E:
              </label>
              <input v-model="reportData.earthImpedanceLOneE" type="text">
              Ω
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L2-E:
              </label>
              <input v-model="reportData.earthImpedanceLTwoE" type="text">
              Ω
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L3-E:
              </label>
              <input v-model="reportData.earthImpedanceLThreeE" type="text">
              Ω
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="align-items: center; gap:10px;">
            <span style="text-decoration:none; width: 200px;">
              <b>d) Earth-fault Settings:</b>
            </span>
            <div class="box">
              <label for="" style="width: 100px !important;">
                RCCB:
              </label>
              <input v-model="reportData.earthFaultRCCB" type="text">
              mA
            </div>
            <span> OR</span>
            <div class="box">
              <label for="" style="width: 100px !important;">
                ELR:
              </label>
              <input v-model="reportData.earthFaultELROne" type="text">
              A
              <input v-model="reportData.earthFaultELRTwo" type="text">
              s
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                EF:
              </label>
              <input v-model="reportData.earthFaultEFOne" type="text">
              A
              <input v-model="reportData.earthFaultEFTwo" type="text">
              s
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs">
            <div class="box">
              <label for="">
                <b> e) Function Test</b>
              </label>
            </div>
            <div class="box">
              <!-- <span>Acceptable / Not Acceptable</span> -->
              <input id="one" v-model="reportData.functionTest" type="radio" value="One">
              <label for="one">Acceptable</label>
              /
              <input id="two" v-model="reportData.functionTest" type="radio" value="Two">
              <label for="two">Not Acceptable</label>
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs">
            <div class="box">
              <label for="">
                <b> f) Tariff Meter "Direction":</b>
              </label>
            </div>
            <div class="box">
              <!-- <span>Acceptable / Not Acceptable / Not Applicable</span> -->
              <input id="one" v-model="reportData.tariffMeter" type="radio" value="One">
              <label for="one">Acceptable</label>
              /
              <input id="two" v-model="reportData.tariffMeter" type="radio" value="Two">
              <label for="two">Not Acceptable</label>
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs">
            <div class="box">
              <label for="">
                <b> g) Polarity Test:</b>
              </label>
            </div>
            <div class="box">
              <!-- <span>Acceptable / Not Acceptable</span> -->
              <input id="one" v-model="reportData.polarityTest" type="radio" value="One">
              <label for="one">Acceptable</label>
              /
              <input id="two" v-model="reportData.polarityTest" type="radio" value="Two">
              <label for="two">Not Acceptable</label>
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs">
            <span style="text-decoration:none; width: 150px;">
              <b>b) Earth Bonding</b>
            </span>
            <div class="box">
              <label for="">
                Electrical Bonding
              </label>
              <input v-model="reportData.earthBondingElectricalBonding" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Water-pipe
              </label>
              <input v-model="reportData.earthBondingWaterPipe" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Fire-pipe
              </label>
              <input v-model="reportData.earthBondingFirePipe" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Gas-pipe
              </label>
              <input v-model="reportData.earthBondingGasPipe" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Other Metallic parts
              </label>
              <input v-model="reportData.earthBondingOtherMetallicParts" type="checkbox">
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs">
            <span style="text-decoration:none; width: 150px;">
              <b>c) Checklist</b>
            </span>
            <div class="box">
              <label for="">
                Labeling
              </label>
              <input v-model="reportData.checklistLabeling" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Fire Extinguisher
              </label>
              <input v-model="reportData.checklistFireExtinguisher" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Danger Sign
              </label>
              <input v-model="reportData.checklistDangerSign" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Safety Chart
              </label>
              <input v-model="reportData.checklistSafetyChart" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Rubber Mat
              </label>
              <input v-model="reportData.checklistRuberMat" type="checkbox">
            </div>
          </div>
          <div class="particular-inputs">
            <span style="width: 150px;"></span>
            <div class="box">
              <label for="">
                Single Line Diagram
              </label>
              <input v-model="reportData.checklistSingleLineDiagram" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                EI License
              </label>
              <input v-model="reportData.checklistEiLicense" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                13A SSO
              </label>
              <input v-model="reportData.checklistThirteenASSO" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Lighting
              </label>
              <input v-model="reportData.checklistLighting" type="checkbox">
            </div>
          </div>
        </div>
        <div class="particulars">
          <div class="particular-inputs" style="align-items: center;">
            <span style="text-decoration:none; width: 150px;">
              <b>d) Earth Pit</b>
            </span>
            <div class="box">
              <label for="">
                Ring circuit
              </label>
              <input v-model="reportData.earthPitRingCircuit" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                No. of Earth Pit
              </label>
              <input v-model="reportData.earthPitNum" class="special-input" type="text">
            </div>
            <div class="box" style="align-items: center;">
              <label for="">
                Overall Earth pit Value
              </label>
              <input v-model="reportData.earthPitOverallValue" class="special-input" type="text">
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
            </div>
            <div class="box">
              <!-- <span>Acceptable / Not Acceptable</span> -->
              <input id="one" v-model="reportData.subMainCableProtection" type="radio" value="One">
              <label for="one">Acceptable</label>
              /
              <input id="two" v-model="reportData.subMainCableProtection" type="radio" value="Two">
              <label for="two">Not Acceptable</label>
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="particulars">
          <span style="text-decoration: underline; font-weight: bold;">
            4. INSPECTIONS BY
          </span>
          <div class="particular-inputs">
            <label for="" style="    width: 20%!important;">
              Recorded by:
            </label>
            <input v-model="reportData.recordedBy" class="special-input" type="text">
            <label for="" style="    width: 20%!important;">
              Witness, if any:
            </label>
            <input v-model="reportData.wtiness" class="special-input" type="text">
          </div>
          <div class="particular-inputs">
            <label for="">
              Date:
            </label>
            <input v-model="reportData.date" class="special-input" type="text">
            <label for="">
              Signature:
            </label>
            <input v-model="reportData.signature" class="special-input" type="text">
          </div>
          <div class="particular-inputs">
            <label for="">
              Remarks:
            </label>
            <input v-model="reportData.remarks" type="text">
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
  data () {
    return {
      reportId: -1,
      reportData: {}
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
    header .container{
        display: flex;
        align-items: center;
    }
    .container{
        /* padding: 20px; */
        justify-content: space-between;
    }
    .wrapper{
        display: flex;
        /* align-items: center; */
        flex-direction: column;
        padding: 20px;
        gap: 10px;
    }
    .row{
        display: flex;
        flex-wrap: wrap;
    }
    .cell{
        overflow: hidden;
        /* padding: 20px; */
        background: #ddd;
        width: 50vw;
    }
    .description{
        /* padding: 20px; */
        border: 1px solid black;
    }
    .note{
        display: flex;
        flex-direction: column;
    }
    .note span {
        text-decoration: underline;
    }
    .instructions{
        font-weight: bold;
    }
    .instructions span {
        text-decoration: underline;
    }
    .particulars{
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
    .box{
        display: flex;
        align-items: center;
    }
    .box label{
        width: 210px !important;
    }
    .particular-inputs label{
        width: auto;
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
    input{
        background-color: transparent;
        border: none;
        border-bottom: 1px solid black;
        color: #555;
        box-sizing: border-box;
    }
    input:focus{
        outline: none;
    }
    .ordered-cell{
        overflow: hidden;
        padding: 20px;
        /* margin: 2px 1em; */
        background: #ddd;
        /* flex: 1; */
        width: 100%;
        align-items: center;
    }
    ol{
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
    .ordered-cell ol li{
        display: flex;
        /* justify-content: space-between;
        align-items: center; */
    }
    .ordered-cell ol li span{
        text-decoration: underline;
        font-weight: bold;
    }
    .list-box{
        display: flex;
        justify-content: space-between;
        align-items: center;
        width: 100%;
    }
    .my_table{
        width: 100%;
    }
    .title-box{
        display: flex;
        flex-direction: column;
        text-align: right;
    }
    .title-box h5{
        text-decoration: underline;
    }
</style>

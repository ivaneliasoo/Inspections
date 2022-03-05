<template>
  <div style="display: block">
    <header>
      <div class="container">
        <h1 class="mh-logo">
          <!-- <PrintingLogo /> -->
        </h1>
        <div class="title-box">
          <h1 class="">
            Testing Turn-On Record (TNS System)
          </h1>
          <h5 class="">
            CSE T&C-TT FORM
          </h5>
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
          <div class="particular-inputs">
            <div class="box">
              <label for="">
                <b> a) Phase Out Test for Cable</b>
              </label>
            </div>
            <div class="box">
              <!-- <span>Acceptable / Not Acceptable</span> -->
              <input id="one" v-model="reportData.phaseOutTestCable" type="radio" value="One">
              <label for="one">Acceptable</label>
              /
              <input id="two" v-model="reportData.phaseOutTestCable" type="radio" value="Two">
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
              <input v-model="reportData.elBonding" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Water-pipe
              </label>
              <input v-model="reportData.waterPipe" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Fire-pipe
              </label>
              <input v-model="reportData.firePipe" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Gas-pipe
              </label>
              <input v-model="reportData.gasPipe" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Other Metallic parts
              </label>
              <input v-model="reportData.otherMetallicParts" type="checkbox">
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
              <input v-model="reportData.checkLabeling" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Fire Extinguisher
              </label>
              <input v-model="reportData.checkFireExtinguisher" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Danger Sign
              </label>
              <input v-model="reportData.checkDangerSign" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Safety Chart
              </label>
              <input v-model="reportData.checkSafetyChart" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Rubber Mat
              </label>
              <input v-model="reportData.checkReportData" type="checkbox">
            </div>
          </div>
          <div class="particular-inputs">
            <span style="width: 150px;" />
            <div class="box">
              <label for="">
                Single Line Diagram
              </label>
              <input v-model="reportData.checkSingleLineDiagram" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                EI License
              </label>
              <input v-model="reportData.checkEiLicence" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                13A SSo
              </label>
              <input v-model="reportData.checkThirtheenASSo" type="checkbox">
            </div>
            <div class="box">
              <label for="">
                Lighting
              </label>
              <input v-model="reportData.checkLighting" type="checkbox">
            </div>
          </div>
        </div>
        <!-- <div class="particulars">
                <div class="particular-inputs" style="align-items: center;">
                    <span style="text-decoration:none; width: 150px;">
                        <b>d) Earth Pit</b>
                    </span>
                    <div class="box">
                        <label for="">
                            Electrical Bonding
                        </label>
                        <input type="checkbox">
                    </div>
                    <div class="box">
                        <label for="">
                            No. of Earth Pit
                        </label>
                        <input class="special-input" type="text">
                    </div>
                    <div class="box" style="align-items: center;">
                        <label for="">
                            Overall Earth pit Value
                        </label>
                        <input class="special-input" type="text">
                        <label for="">Ω</label>
                    </div>

                </div>
            </div> -->
        <!-- <div class="particulars">
                <div class="particular-inputs">
                    <div class="box">
                        <label for="">
                            <b> e) Sub Main Cable Protection</b>
                        </label>
                    </div>
                    <div class="box">
                        <span>Acceptable / Not Acceptable</span>
                    </div>
                </div>
            </div> -->
        <div class="particulars">
          <span style="text-decoration: none;">f) Insulation test</span>
          <table class="my_table">
            <tr>
              <th style="text-align: left;">
                Incoming Cables (MΩ):
              </th>
              <th style="text-align: right;">
                Main Switchboard/Outgoing Circuits (MΩ):
              </th>
            </tr>
            <tr>
              <td style="text-align: center;border: 1px solid black;padding: 5px;">
                <div class="box">
                  <label for="" style="width:100px !important">
                    L1-E:
                  </label>
                  <input v-model="reportData.InLOneE" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L1-N:
                  </label>
                  <input v-model="reportData.InLOneN" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L1-L2:
                  </label>
                  <input v-model="reportData.InLOneLTwo" class="special-input" type="text">
                </div>
                <div class="box">
                  <label for="" style="width:100px !important">
                    L2-E:
                  </label>
                  <input v-model="reportData.InLTwoE" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L2-N:
                  </label>
                  <input v-model="reportData.InLTwoN" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L2-L3:
                  </label>
                  <input v-model="reportData.InLTwoLThree" class="special-input" type="text">
                </div>
                <div class="box">
                  <label for="" style="width:100px !important">
                    L3-E:
                  </label>
                  <input v-model="reportData.InLThreeE" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L3-N:
                  </label>
                  <input v-model="reportData.InLThreeN" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L1-L3:
                  </label>
                  <input v-model="reportData.InLOneThree" class="special-input" type="text">
                </div>
                <div class="box">
                  <label for="" style="width:100px !important">
                    N-E:
                  </label>
                  <input v-model="reportData.InNE" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    Remarks:
                  </label>
                  <input v-model="reportData.InRemarks" class="special-input" type="text">
                </div>
              </td>
              <td style="text-align: center;border: 1px solid black;padding: 5px;">
                <div class="box">
                  <label for="" style="width:100px !important">
                    L1-E:
                  </label>
                  <input v-model="reportData.OutLOneE" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L1-N:
                  </label>
                  <input v-model="reportData.OutLOneN" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L1-L2:
                  </label>
                  <input v-model="reportData.OutLOneLTwo" class="special-input" type="text">
                </div>
                <div class="box">
                  <label for="" style="width:100px !important">
                    L2-E:
                  </label>
                  <input v-model="reportData.OutLTwoE" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L2-N:
                  </label>
                  <input v-model="reportData.OutLTwoN" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L2-L3:
                  </label>
                  <input v-model="reportData.OutLTwoLThree" class="special-input" type="text">
                </div>
                <div class="box">
                  <label for="" style="width:100px !important">
                    L3-E:
                  </label>
                  <input v-model="reportData.OutLThreeE" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L3-N:
                  </label>
                  <input v-model="reportData.OutLThreeN" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    L1-L3:
                  </label>
                  <input v-model="reportData.OutLOneThree" class="special-input" type="text">
                </div>
                <div class="box">
                  <label for="" style="width:100px !important">
                    N-E:
                  </label>
                  <input v-model="reportData.OutNE" class="special-input" type="text">
                  <label for="" style="width:100px !important">
                    Remarks:
                  </label>
                  <input v-model="reportData.OutRemarks" class="special-input" type="text">
                </div>
              </td>
            </tr>
          </table>
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
        <b>b) Earth Bonding</b>
        <div class="particulars">
          <div class="particular-inputs">
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-N:
              </label>
              <input v-model="reportData.earthBondingLOneN" type="text">
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L2-N:
              </label>
              <input v-model="reportData.earthBondingLTwoN" type="text">
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L3-N:
              </label>
              <input v-model="reportData.earthBondingLThreeN" type="text">
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                N-E:
              </label>
              <input v-model="reportData.earthBondingNE" type="text">
              V
            </div>
          </div>
          <div class="particular-inputs">
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-L2:
              </label>
              <input v-model="reportData.earthBondingLOneLTwo" type="text">
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L1-L3:
              </label>
              <input v-model="reportData.earthBondingLOneLThree" type="text">
              V
            </div>
            <div class="box">
              <label for="" style="width: 100px !important;">
                L2-L3:
              </label>
              <input v-model="reportData.earthBondingLTwoLThree" type="text">
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
            <input v-model="reportData.recordedBy"  class="special-input" type="text">
            <label for="" style="    width: 20%!important;">
              Witness, if any:
            </label>
            <input v-model="reportData.wtiness"  class="special-input" type="text">
          </div>
          <div class="particular-inputs">
            <label for="">
              Date:
            </label>
            <input v-model="reportData.date"  class="special-input" type="text">
            <label for="">
              Signature:
            </label>
            <input v-model="reportData.signature"  class="special-input" type="text">
          </div>
          <div class="particular-inputs">
            <label for="">
              Remarks:
            </label>
            <input v-model="reportData.remarks"  type="text">
          </div>
        </div>
      </div>
    </div>
    <!-- TODO-IVAN Hacer Encabezado del Compounded Photo Record -->
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

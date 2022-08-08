<template>
  <div>
    <div v-if="showHeader" style="height: 40px">
      <table style="border: blank; height: 100%; width: 100%">
        <tbody>
          <tr>
            <td
              class="text-center font-weight-bold"
              style="font-size: 11pt; border-style: none"
            >
              <table
                style="
                  width: 100%;
                  border-collapse: collapse;
                  border: 1px solid black;
                "
              >
                <tr>
                  <td style="width: 84%">
                    {{ jobList.title }} ({{ jobList.jobs.length }})
                  </td>
                  <td style="width: 8%">
                    <v-btn icon small @click="delJob">
                      <v-icon>mdi-delete</v-icon>
                    </v-btn>
                  </td>
                  <td style="width: 8%">
                    <v-btn icon small @click="addJob">
                      <v-icon>mdi-plus</v-icon>
                    </v-btn>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div
      :style="mainDivStyle()"
      @drop="dropJob($event)"
      @dragover="dragoverHandler($event)"
    >
      <table
        style="
          width: 100%;
          border-collapse: separate;
          border: blank;
          border-spacing: 0 15px;
        "
      >
        <tbody>
          <tr v-for="(jobItem, index) in jobList.jobs" :key="index">
            <td
              name="table-cell"
              class="table-cell text-left"
              draggable="true"
              data-source="confirmed-job"
              :data-jobid="jobItem.id"
              @dragstart="jobDragStart"
              @click="onJobClick($event, index)"
            >
              <table width="100%">
                <colgroup span="10">
                  <col v-for="n in 10" :key="n" width="10" />
                </colgroup>
                <tr style="height: 40%; border-bottom-style: hidden">
                  <td colspan="2" style="border-right-style: hidden">
                    <v-btn class="mx-2" fab dark small outlined color="black">
                      <input
                        v-model="jobItem.tag"
                        class="text-center"
                        type="text"
                        size="2"
                        maxlength="2"
                        style="
                          font-size: 11pt;
                          text-transform: uppercase;
                          outline: none;
                        "
                      />
                    </v-btn>
                  </td>
                  <td colspan="9">
                    <textarea
                      v-model="jobItem.scope"
                      class="text-caption"
                      rows="2"
                      placeholder="Scope"
                      style="width: 100%; outline: none; resize: none"
                    />
                  </td>
                </tr>
                <tr
                  style="
                    height: 20%;
                    border-top-style: hidden;
                    border-bottom-style: hidden;
                  "
                >
                  <td colspan="5" style="border-right-style: hidden">
                    <input
                      v-model="jobItem.comments"
                      class="text-caption"
                      type="text"
                      placeholder="Comments"
                      :style="jobProjectionCommentsStyle(jobItem.comments)"
                      @change="$forceUpdate()"
                    />
                  </td>
                  <td colspan="5" rowspan="2">
                    <textarea
                      v-if="jobStatus === 'inProgress'"
                      v-model="jobItem.teams"
                      class="text-caption"
                      rows="2"
                      placeholder="Teams"
                      style="
                        outline: none;
                        resize: none;
                        color: blue;
                        width: 100%;
                      "
                    />
                  </td>
                </tr>
                <tr style="height: 20%; border-top-style: hidden">
                  <td colspan="5" style="border-right-style: hidden">
                    <label v-if="jobItem.value" class="text-caption">$</label>
                    <input
                      v-model="jobItem.value"
                      class="text-caption table-input"
                      type="text"
                      placeholder="Value"
                      style="outline: none; display: table-cell; width: 80%"
                      @change="$forceUpdate()"
                    />
                  </td>
                </tr>
                <tr style="height: 20%">
                  <td class="text-center" colspan="4">
                    <input
                      v-model="jobItem.teamCount"
                      class="text-caption"
                      type="text"
                      size="8"
                      placeholder="Team count"
                      style="text-align: center; outline: none"
                    />
                  </td>
                  <td class="text-center" colspan="2">
                    <input
                      v-model="jobItem.duration"
                      class="text-caption"
                      type="text"
                      size="6"
                      placeholder="Days"
                      style="text-align: center; outline: none"
                    />
                  </td>
                  <td
                    class="text-center"
                    colspan="4"
                    :style="jobProjectionShiftBackground(jobItem.shift)"
                  >
                    <select
                      v-model="jobItem.shift"
                      class="text-caption table-input"
                      :style="jobProjectionShiftColor(jobItem.shift)"
                      style="text-align: center"
                      @change="updateShift"
                    >
                      <option value="" disabled selected style="color: grey">
                        Select shift
                      </option>
                      <option value="Day" style="color: black">Day</option>
                      <option value="Night" style="color: black">Night</option>
                      <option value="Mixed" style="color: black">Mixed</option>
                    </select>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    jobStatus: String,
    jobList: Object,
    height: String,
    showHeader: Boolean,
  },
  data: () => ({
    selected: null,
  }),
  updated() {
    if (!this.selected && this.jobList.jobs) {
      this.selected = new Array(this.jobList.jobs.length).fill(false)
    }
  },
  // computed: {
  //   jobProjection() {
  //     return `
  //         width: 100%;
  //         height: 130px;
  //         border-collapse: collapse;
  //         border: 1px solid black;`;
  //   }
  // },
  methods: {
    addJob() {
      this.unselect()
      this.$emit('add-job', this.jobStatus)
    },
    delJob() {
      const jobs = []
      for (let i = 0; i < this.selected.length; i++) {
        if (this.selected[i]) {
          const job = this.jobList.jobs[i]
          jobs.push(job)
          this.selected[i] = false
        }
      }
      const cells = document.getElementsByName('table-cell')
      for (const cell of cells) {
        if (cell.classList.contains('selected')) {
          cell.classList.remove('selected')
        }
      }

      this.$emit('del-job', jobs)
    },
    jobDragStart(ev) {
      const source = ev.target.getAttribute('data-source')
      if (source == 'confirmed-job' || source == 'upcoming-job') {
        const data = {
          source,
          jobId: ev.target.getAttribute('data-jobid'),
          category: this.jobStatus,
        }
        ev.dataTransfer.setData('application/json', JSON.stringify(data))
      }
    },
    dropJob(event) {
      console.log('dropJob')
      const srcData = JSON.parse(event.dataTransfer.getData('application/json'))
      const jobId = srcData.jobId
      const category = srcData.category
      if (category !== this.jobStatus) {
        this.$emit('update-job-status', jobId, this.jobStatus)
      }
    },
    dragoverHandler(event) {
      event.preventDefault()
      event.dataTransfer.dropEffect = 'move'
    },
    findAncestor(el) {
      for (; el.getAttribute('name') !== 'table-cell'; el = el.parentElement) {}
      return el
    },
    onJobClick(event, jobIndex) {
      const td = this.findAncestor(event.target)
      if (td.classList.contains('selected')) {
        td.classList.remove('selected')
      } else {
        td.classList.add('selected')
      }
      this.selected[jobIndex] = !this.selected[jobIndex]
    },
    unselect() {
      for (let i = 0; i < this.selected.length; i++) {
        if (this.selected[i]) {
          this.selected[i] = false
        }
      }
      const cells = document.getElementsByName('table-cell')
      for (const cell of cells) {
        if (cell.classList.contains('selected')) {
          cell.classList.remove('selected')
        }
      }
    },
    mainDivStyle() {
      return `height: ${this.height}; overflow-y: scroll;`
    },
    jobProjectionCommentsStyle(comments) {
      if (!comments) {
        return 'outline: none; margin-top: 9px; color: black; display: table-cell; width:100%'
      }
      const color =
        comments.includes('urgent') ||
        comments.includes('Urgent') ||
        comments.includes('URGENT')
          ? 'magenta'
          : 'black'
      return `outline: none; margin-top: 9px; color: ${color}; display: table-cell; width:100%`
    },
    jobProjectionShiftColor(shift) {
      if (!shift) {
        return 'color: grey;'
      } else if (shift === 'Night') {
        return 'color: white;'
      } else {
        return 'color: black;'
      }
    },
    jobProjectionShiftBackground(shift) {
      if (shift === 'Day') {
        return 'background-color:rgb(198,224,180)'
      } else if (shift === 'Night') {
        return 'background: linear-gradient(rgb(20,50,0),rgb(100,140,90));'
      } else if (shift === 'Mixed') {
        return 'background: linear-gradient(to right, rgb(198,224,180) 0%, rgb(198,224,180) 40%, rgb(20,50,0) 80%, rgb(20,50,0) 100%);'
      }
      return 'background: white;'
    },
    updateShift() {
      this.$forceUpdate()
    },
  },
}
</script>

<style scoped>
.table-cell {
  /* border-style: solid; */
  border: 1px solid black;
}

.selected {
  /* border-style: solid;  */
  border: 3px solid red;
}

.job-projection {
  width: 100%;
  height: 130px;
  border-collapse: collapse;
  border: 1px solid black;
}
</style>

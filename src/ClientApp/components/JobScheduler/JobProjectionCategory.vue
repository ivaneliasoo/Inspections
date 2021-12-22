<template>
  <div>
    <div style="height: 40px" v-if="showHeader">
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
                  <td style="width 84%">
                    {{ jobList.title }}
                  </td>
                  <td style="width 8%">
                    <v-btn icon small @click="delJob">
                      <v-icon>mdi-delete</v-icon>
                    </v-btn>
                  </td>
                  <td style="width 8%">
                    <v-btn icon small @click="$emit('add-job', jobStatus)">
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

    <div :style="mainDivStyle()">
      <table
        style="
          width: 100%;
          border-collapse: separate;
          border: blank;
          border-spacing: 0 15px;
        "
      >
        <tbody>
          <tr v-for="(jobItem, index) in jobList.jobs" :key="index"
          >
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
                  <col v-for="n in 10" width="10" :key="n" />
                </colgroup>
                <tr style="height: 40%; border-botton-style: hidden">
                  <td colspan="2" style="border-right-style: hidden">
                    <v-btn class="mx-2" fab dark small outlined color="black">
                      <input
                        class="text-center"
                        type="text"
                        size="2"
                        maxlength="2"
                        style="
                          font-size: 11pt;
                          text-transform: uppercase;
                          outline: none;
                        "
                        v-model="jobItem.tag"
                      />
                    </v-btn>
                  </td>
                  <td colspan="9">
                    <textarea
                      class="text-caption"
                      rows="2"
                      placeholder="Scope"
                      v-model="jobItem.scope"
                      style="width: 100%; outline: none; resize: none"
                    >
                    </textarea>
                  </td>
                </tr>
                <tr
                  style="
                    height: 20%;
                    border-top-style: hidden;
                    border-bottom-style: hjobStatusden;
                  "
                >
                  <td colspan="5" style="border-right-style: hidden">
                    <input
                      class="text-caption"
                      type="text"
                      placeholder="Comments"
                      @change="$forceUpdate()"
                      v-model="jobItem.comments"
                      :style="jobProjectionCommentsStyle(jobItem.comments)"
                    />
                  </td>
                  <td colspan="5" rowspan="2">
                    <textarea
                      v-if="jobStatus === 'inProgress'"
                      class="text-caption"
                      rows="2"
                      placeholder="Teams"
                      v-model="jobItem.teams"
                      style="
                        outline: none;
                        resize: none;
                        color: blue;
                        width: 100%;
                      "
                    >
                    </textarea>
                  </td>
                </tr>
                <tr style="height: 20%; border-top-style: hidden">
                  <td colspan="5" style="border-right-style: hidden">
                    <input
                      class="text-caption table-input"
                      type="text"
                      placeholder="Value"
                      v-model="jobItem.value"
                      style="outline: none; display: table-cell; width: 100%"
                    />
                  </td>
                </tr>
                <tr style="height: 20%">
                  <td class="text-center" colspan="4">
                    <input
                      class="text-caption"
                      type="text"
                      size="8"
                      placeholder="Team count"
                      style="text-align: center; outline: none"
                      v-model="jobItem.teamCount"
                    />
                  </td>
                  <td class="text-center" colspan="2">
                    <input
                      class="text-caption"
                      type="text"
                      size="6"
                      placeholder="Days"
                      style="text-align: center; outline: none"
                      v-model="jobItem.duration"
                    />
                  </td>
                  <td
                    class="text-center"
                    colspan="4"
                    :style="jobProjectionShiftBackground(jobItem.shift)"
                  >
                    <select
                      class="text-caption table-input"
                      :style="jobProjectionShiftColor(jobItem.shift)"
                      style="text-align: center"
                      v-model="jobItem.shift"
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

<script>
export default {
  props: {
    jobStatus: String,
    jobList: Object,
    height: String,
    showHeader: Boolean,
  },
  data: () => ({
    selected: null
  }),
  updated() {
    if (!this.selected && this.jobList.jobs) {
      this.selected = new Array(this.jobList.jobs.length).fill(false);
    }
  },
  computed: {
    jobProjection() {
      return `
          width: 100%;
          height: 130px;
          border-collapse: collapse;
          border: 1px solid black;`;
    }
  },
  methods: {
    delJob() {
      console.log("this.selected", this.selected)
      const jobs = [];
      for (let i=0; i < this.selected.length; i++) {
        if (this.selected[i]) {
          const job = this.jobList.jobs[i];
          jobs.push(job);
          this.selected[i] = false;
        }
      }
      let cells = document.getElementsByName("table-cell");
      for (const cell of cells) {
        if (cell.classList.contains("selected")) {
            cell.classList.remove("selected");
        }        
      }

      this.$emit('del-job', jobs);
    },
    findAncestor(el) {
        for ( ; el.getAttribute('name') !== "table-cell"; el = el.parentElement);
        return el;
    },    
    onJobClick(event, jobIndex) {
      const td = this.findAncestor(event.target);
      if (td.classList.contains("selected")) {
          td.classList.remove("selected");
      } else {
        td.classList.add("selected");
      }
      this.selected[jobIndex] = !this.selected[jobIndex];
    },
    mainDivStyle() {
      return `height: ${this.height}; overflow-y: scroll;`;
    },
    jobProjectionCommentsStyle(comments) {
      if (!comments) {
        return "outline: none; margin-top: 9px; color: black; display: table-cell; width:100%";
      }
      const color =
        comments.includes("urgent") ||
        comments.includes("Urgent") ||
        comments.includes("URGENT")
          ? "magenta"
          : "black";
      return `outline: none; margin-top: 9px; color: ${color}; display: table-cell; width:100%`;
    },
    jobProjectionShiftColor(shift) {
      if (!shift) {
        return "color: grey;";
      } else if (shift === "Night") {
        return "color: white;";
      } else {
        return "color: black;";
      }
    },
    jobProjectionShiftBackground(shift) {
      if (shift === "Day") {
        return "background-color:rgb(198,224,180)";
      } else if (shift === "Night") {
        return "background: linear-gradient(rgb(20,50,0),rgb(100,140,90));";
      } else if (shift === "Mixed") {
        return "background: linear-gradient(to right, rgb(198,224,180) 0%, rgb(198,224,180) 40%, rgb(20,50,0) 80%, rgb(20,50,0) 100%);";
      }
      return "background: white;";
    },
    jobDragStart(ev) {
      const source = ev.target.getAttribute("data-source");
      if (source == "confirmed-job" || source == "upcoming-job") {
        const data = {
          source: source,
          jobId: ev.target.getAttribute("data-jobid"),
        };
        ev.dataTransfer.setData("application/json", JSON.stringify(data));
      }
    },
    updateShift() {
      this.$forceUpdate();
    }
  },
};
</script>

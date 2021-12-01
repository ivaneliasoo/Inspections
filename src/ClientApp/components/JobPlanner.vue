<template>
  <div>

    <div style="position: fixed; top: 80px; left: 80px;">
      <v-menu
        left
        bottom
        fixed
      >
        <template v-slot:activator="{ on, attrs }">
          <v-btn
            icon
            v-bind="attrs"
            v-on="on"
          >
            <v-icon>mdi-menu</v-icon>
          </v-btn>
        </template>
        <v-list class="menu">
          <v-list-item>
            <v-list-item-title @click="showJobSchedule">
              Job schedule projection
            </v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title @click="showJobProjectionOverview">
              Job projection overview
            </v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title @click="showManPower">
              Manpower planning
            </v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title @click="showManPowerForecast">
              Manpower forecast
            </v-list-item-title>
          </v-list-item>
          <v-divider></v-divider>
          <v-list-item>
            <v-list-item-title @click="save">
              Save
            </v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </div>

  <v-container v-show="jobScheduleVisible" fluid class="my-n2 py-n2 mx-4 px-0 fill-height">
    <v-row>
      <v-col cols="10">
        <v-row>
          <v-col class="mt-0 pt-0 mb-n4 pb-n4 " cols=7>
            <div class="text-h6" style="text-align: bottomz; display: inline">
              Job Schedule Projection
            </div>
          </v-col>
          <v-col class="text-right mt-0 pt-0 mb-n4 pb-n4" cols=2>
            <div id="flatpickr" class="rounded text-body-2"
              style="padding: 3px; box-shadow: 1px 3px lightgrey;">
              <flat-pickr
                  v-model="selectedDate"
                  @on-change="gotoDate"
                  :config="dateConfig"
                  class="calendar"
                  placeholder="Select date"
                  name="date"/>
            </div>
          </v-col>
          <v-col class="text-center mt-0 pt-0 mb-n4 pb-n4" cols=3>
            <v-btn small @click="previousWeek">
              Previous Week
            </v-btn>
            &nbsp; &nbsp;
            <v-btn small @click="nextWeek">
              Next Week
            </v-btn>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div style="height: 780px; overflow-y: scroll;">
              <table
                id="job-sched"
                class="my-0 py-0 job-sched">
                  <colgroup>
                    <col style="width:8%;">
                    <col v-bind:span="teams.length" v-bind:style="colWidth()">
                  </colgroup>
                  <thead>
                      <tr>
                          <th class="text-caption font-weight-bold">Date</th>
                          <th v-for="team in teams" :key="team.id" class="text-caption font-weight-bold">
                            {{team.foreman}}
                          </th>
                      </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(day, index) in jobSchedule" :key="index">
                      <td class="text-caption text-left font-weight-bold date" :style="dayColor(day.date)">
                        <div style="text-align: center;">
                          {{dayOfWeek(day.date)}}<br>
                          {{monthAndDate(day.date)}}
                        </div>
                      </td>
                      <td v-for="(team, teamIndex) in teams" :key="team.id"
                            class="text-left draggable"
                            draggable="true"
                            data-source="scheduled-job"
                            @click="onTableCellClick($event, day.jobs[team.id])"
                            @drop="dropHandler($event, index, teamIndex)"
                            @dragover="dragoverHandler($event)"
                            @dragstart="scheduledJobDragstart($event, index, teamIndex)"
                            :style = "jobScheduleStyle(day.jobs[team.id])">
                        <div v-if="splitShift(day.jobs[team.id])">
                          <label class="font-weight-bold">
                            Morning
                          </label><br>
                          {{ day.jobs[team.id].job1 }}<br>
                          <label class="font-weight-bold">
                            Afternoon
                          </label><br>
                          {{ day.jobs[team.id].job2 }}
                        </div>
                        <div v-else>
                          <label v-if="day.jobs[team.id].shift==='day'"
                            class="font-weight-bold">
                            Day
                          </label>
                          <label v-if="day.jobs[team.id].shift==='night'"
                            class="font-weight-bold">
                            Night
                          </label><br>
                          {{ day.jobs[team.id].job1 }}
                        </div>
                      </td>
                    </tr>
                  </tbody>
              </table>
            </div>
          </v-col>
        </v-row>
      </v-col>
      <v-col cols="2">
        <v-row>
          <v-col cols="12" class="mt-1 pt-0 mb-0 pb-0 d-flex text-center">
              <div>
                Confirmed Jobs
              </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div style="height: 330px; width: 90%; overflow-y: auto;">
              <table
                id="confirmed-jobs"
                class="my-0 py-0 job-table">
                  <thead v-on:click="openJobTable('confirmed')">
                    <tr>
                      <th class="header text-caption font-weight-bold">Job scope</th>
                      <th class="header text-caption font-weight-bold">Pri</th>
                    </tr>
                  </thead>
                  <tbody v-on:click="openJobTable('confirmed')">
                    <tr v-for="(job, index) in confirmedJobs" :key="index" :style="background(job.priority)">
                      <td class="text-caption text-left draggable"
                          style="width:80%;"
                          draggable="true"
                          data-source="confirmed-job"
                          :data-jobid="job.id"
                          @dragstart = "jobDragStart">
                        <div style="height:40px; overflow: hidden">
                          {{job.scope}}
                        </div>
                      </td>
                      <td class="text-caption text-left" style="width:20%;">
                        <div style="height:40px; overflow: hidden">
                          {{job.priority}}
                        </div>
                      </td>
                    </tr>
                  </tbody>
              </table>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="12" class="mt-1 pt-0 mb-0 pb-0 d-flex text-center">
              <div>
                Upcoming Jobs
              </div>
          </v-col>
        </v-row>
        <v-row class="my-0 py-0">
          <v-col>
            <div style="height: 330px; width: 90%; overflow-y: auto;">
              <table
                id="upcoming-jobs"
                class="my-0 py-0 job-table">
                  <thead v-on:click="openJobTable('upcoming')">
                    <tr>
                      <th class="text-caption font-weight-bold">Job scope</th>
                      <th class="text-caption font-weight-bold">Pri</th>
                    </tr>
                  </thead>
                  <tbody v-on:click="openJobTable('upcoming')">
                    <tr v-for="(job, index) in upcomingJobs" :key="index" :style="background(job.priority)">
                      <td class="text-caption text-left draggable"
                          style="width:80%"
                          draggable="true"
                          data-source="upcoming-job"
                          :data-jobid="job.id"
                          @dragstart = "jobDragStart">
                        <div style="height:40px; overflow: hidden">
                          {{job.scope}}
                        </div>
                      </td>
                      <td class="text-caption text-left" style="width:20%">
                        <div style="height:40px; overflow: hidden">
                          {{job.priority}}
                        </div>
                      </td>
                    </tr>
                  </tbody>
              </table>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <p justify="end" class="my-0 py-0 text-caption">Version {{ version }}</p>
          </v-col>
        </v-row>
      </v-col>
    </v-row>

    <v-dialog v-model="jobTableDialog.open" max-width="900px">
      <v-card>
        <v-card-title>
          <span class="ml-4 headline">{{jobTableDialog.title}}</span>
          <v-spacer></v-spacer>
          <v-btn class="mt-2 mr-5" icon @click="delJob"><v-icon>mdi-delete</v-icon></v-btn>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col>
                <v-simple-table
                  id="simple-job-table"
                  class="simple-job-table"
                  fixed-header
                  height="600px">
                    <thead>
                      <tr>
                        <th></th>
                        <th>Job scope</th>
                        <th>Project value</th>
                        <th>Sales person</th>
                        <th>Priority</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(job, index) in editedJobTable" :key="index">
                        <td>
                          <input type="checkbox" v-model="job.checked">
                        </td>
                        <td>
                          <textarea class="text-caption textarea" rows="2" cols="45"
                            v-model="job.scope" @change="addToJobs(job)">
                          </textarea>
                        </td>
                        <td>
                          <input class="text-caption text-right table-input" type="text" size="10"
                            v-model="job.value" @change="addToJobs(job)"
                          >
                        </td>
                        <td>
                          <input class="text-caption table-input" type="text" size="20"
                            v-model="job.salesPerson" @change="addToJobs(job)"
                          >
                        </td>
                        <td v-bind:style="background(job.priority)">
                          <input class="text-caption text-right table-input" type="text" size="4"
                            v-bind:style="background(job.priority)"
                            v-model="job.priority"
                            @change="markChange"
                            @blur="sortByPriority(index)"
                          >
                        </td>
                      </tr>
                    </tbody>
                </v-simple-table>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeJobTable">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="jobDialog.open" max-width="800px">
      <v-card>
        <v-card-title>
          <span class="mx-2 headline">Assign Job</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col cols=12>
                <v-row class="my-0 py-n0">
                  <v-col cols=6>
                    <div id="flatpickr" class="mt-2 pt-1 mb-0 pb-0 rounded text-body-2"
                      style="padding: 3px; box-shadow: 1px 3px lightgrey;">
                      <flat-pickr
                          v-model="jobDialog.startDate"
                          :config="jobDateConfig"
                          class="calendar"
                          placeholder="Select date"
                          name="startDate"/>
                    </div>
                  </v-col>
                  <v-col cols=6>
                    <div id="flatpickr" class="mt-2 pt-1 mb-0 pb-0 rounded text-body-2"
                      style="padding: 3px; box-shadow: 1px 3px lightgrey;">
                      <flat-pickr
                          v-model="jobDialog.endDate"
                          :config="jobDateConfig"
                          class="calendar"
                          placeholder="Select date"
                          name="endtDate"/>
                    </div>
                  </v-col>
                </v-row>
                <v-row class="my-0 py-0">
                  <v-col cols=10>
                    <label class="my-0 py-0 font-weight-bold">
                      Team: {{getForeman(jobInfo.team)}}
                    </label>
                  </v-col>
                  <v-col cols=2 class="mb-2 pb-2 text-right">
                    <v-btn icon small @click="flipJobs">
                      <v-icon>mdi-orbit-variant</v-icon>
                    </v-btn>
                    <v-btn icon small @click="addSchedJob">
                      <v-icon>mdi-plus</v-icon>
                    </v-btn>
                  </v-col>
                </v-row>
                <v-row class="my-0 py-0">
                  <v-col>
                    <div v-if="splitShift(jobInfo)">
                      <v-row class="my-n4 py-0">
                        <v-col cols=10>
                          <label class="font-weight-bold">
                            Morning
                          </label>
                        </v-col>
                      </v-row>
                      <v-row class="my-n4 py-0">
                        <v-col>
                          <v-textarea
                            rows=3
                            outlined
                            v-model="jobInfo.job1">
                          </v-textarea>
                        </v-col>
                      </v-row>
                      <v-row class="my-n4 py-0">
                        <v-col cols=10>
                          <label class="font-weight-bold">
                            Afternoon
                          </label>
                        </v-col>
                      </v-row>
                      <v-row class="my-0 py-0">
                        <v-col>
                          <v-textarea
                            rows=3
                            outlined
                            v-model="jobInfo.job2">
                          </v-textarea>
                        </v-col>
                      </v-row>
                    </div>
                    <div v-else>
                      <v-row class="my-n4 py-0">
                        <v-col cols=10>
                          <v-label class="my-n5 py-0 font-weight-bold">
                            {{ jobInfo.shiftLabel() }}
                          </v-label>
                        </v-col>
                      </v-row>
                      <v-row class="my-0 py-0">
                        <v-col>
                          <v-textarea class="my-0 py-0"
                            outlined
                            v-model="jobInfo.job1">
                          </v-textarea>
                        </v-col>
                      </v-row>
                    </div>
                  </v-col>
                </v-row>
                <v-row>
                  <v-radio-group class="ml-8"
                    row v-model="jobInfo.shift">
                    <v-radio
                      label="Day"
                      value="day"
                    ></v-radio>
                    <v-radio
                      label="Night"
                      value="night"
                    ></v-radio>
                    <v-radio
                      label="TBC"
                      value="tbc"
                    ></v-radio>
                  </v-radio-group>
                </v-row>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeSchedJob">
            Close
          </v-btn>
          <v-btn color="blue darken-1" text @click="saveSchedJob">
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <div id="content"></div>
  </v-container>

  <v-container v-show="jobProjectionVisible" fluid class="my-0 py-0 mx-4 px-0 fill-height">
    <v-row>
      <v-col>
        <v-row>
          <v-col class="mt-0 pt-0 mb-n4 pb-n4" cols=7>
            <div class="text-h6" style="text-align: bottomz; display: inline">
              Job Projection Overview
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col v-for="(jobList, id) in jobProjectionTable" :key="id">
            <div style="height: 55px; overflow-y: scroll;">
              <table style="border: blank; width: 100%;">
                <tbody>
                  <tr>
                    <td class="text-center font-weight-bold" style="font-size: 11pt; border-style: none">
                      <table style="width: 100%; border-collapse: collapse; border: 1px solid black;">
                        <tr>
                          <td style="width 80%">
                            {{jobList.title}}
                          </td>
                          <td style="width 20%">
                            <v-btn icon small @click="addJob(id)">
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

            <div style="height: 680px; overflow-y: scroll;">
              <table style="border: blank; width: 100%;">
                <tbody>
                  <tr v-for="(jobItem, jobId) in jobList.jobs" :key="jobId">
                    <td class="text-left" style="border-style: none;">
                      <table class="job-projection">
                        <colgroup span = "10">
                            <col v-for="n in 10" width="10" :key="n">
                        </colgroup>
                        <tr style="height: 40%; border-botton-style: hidden;">
                          <td colspan="2" style="border-right-style: hidden;">
                            <v-btn
                              class="mx-2"
                              fab
                              dark
                              small
                              outlined
                              color="black"
                            >
                            <input class="table-input text-center" type="text" size="2" maxlength="2"
                              style="font-size: 11pt; text-transform:uppercase"
                              v-model="jobItem.tag"
                            >
                            </v-btn>
                          </td>
                          <td colspan="9">
                            <textarea class="text-caption textarea" rows="2" style="width:100%" placeholder="Scope"
                              v-model="jobItem.scope">
                            </textarea>
                          </td>
                        </tr>
                        <tr style="height: 20%; border-top-style: hidden; border-bottom-style: hidden;">
                          <td colspan="5" style="border-right-style: hidden;">
                            <input class="text-caption table-input" type="text" placeholder="Comments"
                              @change="$forceUpdate()"
                              v-model="jobItem.comments" :style="jobProjectionCommentsStyle(jobItem.comments)"
                            >
                          </td>
                          <td colspan="5" rowspan="2">
                            <textarea v-if="id==='inProgress'"
                              class="text-caption textarea" rows="2" placeholder="Teams"
                              v-model="jobItem.teams" style="color: blue; width:100%;">
                            </textarea>
                          </td>
                        </tr>
                        <tr style="height: 20%; border-top-style: hidden;">
                          <td colspan="5" style="border-right-style: hidden;">
                            <input class="text-caption table-input" type="text" placeholder="Value"
                              v-model="jobItem.value" style="display:table-cell; width:100%"
                            >
                          </td>
                        </tr>
                        <tr style="height: 20%">
                          <td class="text-center" colspan="4">
                            <input class="text-caption table-input" type="text" size="8" placeholder="Team count"
                              style="text-align: center;"
                              v-model="jobItem.teamCount"
                            >
                          </td>
                          <td class="text-center" colspan="2">
                            <input class="text-caption table-input" type="text" size="6" placeholder="Days"
                              style="text-align: center;"
                              v-model="jobItem.duration"
                            >
                          </td>
                          <td class="text-center" colspan="4" :style="jobProjectionShiftBackground(jobItem.shift)">
                            <select class="text-caption table-input" :style="jobProjectionShiftColor(jobItem.shift)"
                              style="text-align: center;"
                              v-model="jobItem.shift" @change="updateShift">
                              <option value="" disabled selected style="color: grey">Select shift</option>
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
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>

  <v-container v-show="manPowerVisible" fluid class="my-0 py-0 mx-4 px-0 fill-height">
    <v-row>
      <v-col cols="12">
        <v-row>
          <v-col class="mt-0 pt-0 mb-n4 pb-n4 " cols=7>
            <div class="text-h6" style="text-align: bottomz; display: inline">
              Man Power Planning
            </div>
          </v-col>
          <v-col class="text-right mt-0 pt-0 mb-n4 pb-n4" cols=2>
            <div id="flatpickr" class="rounded text-body-2"
              style="padding: 3px; box-shadow: 1px 3px lightgrey;">
              <flat-pickr
                  v-model="selectedDate"
                  @on-change="gotoDate"
                  :config="dateConfig"
                  class="calendar"
                  placeholder="Select date"
                  name="date"/>
            </div>
          </v-col>
          <v-col class="text-center mt-0 pt-0 mb-n4 pb-n4" cols=3>
            <v-btn small @click="previousWeek">
              Previous week
            </v-btn>
            &nbsp; &nbsp;
            <v-btn small @click="nextWeek">
              Next week
            </v-btn>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div style="height: 780px; overflow-y: scroll;">
              <table
                id="manpower"
                class="my-0 py-0 job-sched">
                  <colgroup>
                    <col style="width:4%;">
                    <col style="width:7%;">
                    <col :span="teams.length" :style="colWidthMp()">
                  </colgroup>
                  <thead>
                      <tr>
                        <th colspan="3" class="font-weight-bold"
                                style="font-size: 1.2em; background-color: rgb(48,84,150); color:white">
                            Manpower Planning Guide
                        </th>
                        <th :colspan="teams.length-1" class="font-weight-bold"
                                style="font-size: 1.2em; background-color: rgb(48,84,150); color:white">
                        </th>
                      </tr>
                  </thead>
                  <tbody v-for="(day, index) in jobSchedule" :key="index">
                    <tr>
                        <td class="font-weight-bold date" align ="center" rowspan="4" :style="dayStyle(day.date)">
                            <div style="text-align: center; writing-mode: vertical-lr; transform: rotate(180deg);">
                                {{dayOfWeek(day.date)}} {{monthAndDate(day.date)}}
                            </div>
                        </td>
                        <td class="text-caption text-center font-weight-bold foreman">
                            Manpower: {{ day.manPowerTotals().manPower }}
                        </td>
                        <td v-for="team in teams" :key="team.id"
                            @click="editTeam(team, day.jobs[team.id])"
                            class="text-caption text-center font-weight-bold foreman"
                            style="background-color: rgb(180,198,231)">
                            {{team.foreman}}
                        </td>
                    </tr>
                    <tr>
                        <td class="text-caption text-center font-weight-bold foreman">
                            On leave: {{ day.manPowerTotals().onLeave }}
                        </td>
                        <td v-for="team in teams" :key="team.id"
                            @click="editTeam(team, day.jobs[team.id])"
                            class="text-caption text-center font-weight-bold">
                            {{team.vehicle}}
                        </td>
                    </tr>
                    <tr>
                        <td class="text-caption text-center font-weight-bold foreman">
                        </td>
                        <td v-for="team in teams" :key="team.id" class="text-caption font-weight-bold"
                            @drop="dropTeamMember($event, day.jobs[team.id])"
                            @click="editTeam(team, day.jobs[team.id])">
                              <div v-for="teamMember in day.jobs[team.id].teamMembers" :key="teamMember"
                                  style="text-align: center;"
                                  class="draggable"
                                  draggable="true"
                                  @dragover="dragoverHandler($event)"
                                  @dragstart="teamMemberDragstart"
                                  data-source="team-member"
                                  :data-day-index="index"
                                  :data-foreman="team.id"
                                  :data-team-member="teamMember">
                                  {{teamMember}}
                              </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-caption text-center font-weight-bold foreman">
                            Day notes
                        </td>
                        <td v-for="team in teams" :key="team.id"
                            class="text-left"
                            :style = "jobScheduleStyle(day.jobs[team.id])">
                        <div v-if="splitShift(day.jobs[team.id])">
                          <label class="font-weight-bold">
                            Morning
                          </label><br>
                          {{ day.jobs[team.id].job1 }}<br>
                          <label class="font-weight-bold">
                            Afternoon
                          </label><br>
                          {{ day.jobs[team.id].job2 }}
                        </div>
                        <div v-else>
                          <label v-if="day.jobs[team.id].shift==='day'"
                            class="font-weight-bold">
                            Day
                          </label>
                          <label v-if="day.jobs[team.id].shift==='night'"
                            class="font-weight-bold">
                            Night
                          </label><br>
                          {{ day.jobs[team.id].job1 }}
                        </div>
                      </td>
                    </tr>
                  </tbody>
              </table>
            </div>
          </v-col>
        </v-row>
      </v-col>
    </v-row>

    <v-dialog v-model="teamDialog.open" max-width="600px">
      <v-card>
        <v-card-title>
          <span class="mx-2 headline">{{teamDialog.title}}</span>
        </v-card-title>
        <v-card-text>
          <v-container>
              <v-row class="my-0 py-0">
                <v-col>
                  <v-text-field class="my-n2 py-0" label="Foreman" v-model="teamDialog.editedTeam.foreman">
                  </v-text-field>
                </v-col>
              </v-row>
              <v-row class="my-0 py-0">
                <v-col cols="8">
                  <v-text-field class="my-n2 py-0" label="Vehicle license" v-model="teamDialog.editedTeam.vehicle">
                  </v-text-field>
                </v-col>
                <v-col cols="4">
                  <v-text-field class="my-n2 py-0" label="Position" v-model="teamDialog.editedTeam.position">
                  </v-text-field>
                </v-col>
              </v-row>
              <v-row class="my-0 py-0">
                <v-col align="right">
                  <v-spacer></v-spacer>
                  <v-btn class="mt-0 pt-0 mb-n4 pb-n4" small @click="deleteTeam">
                    Delete Team
                  </v-btn>
                  &nbsp; &nbsp;
                  <v-btn class="mt-0 pt-0 mb-n4 pb-n4" small @click="newTeam">
                    New Team
                  </v-btn>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-simple-table
                    id="team-memmbers"
                    class="simple-job-table"
                    fixed-header
                    height="440px">
                      <colgroup>
                        <col style="width:8%;">
                        <col style="width:92%;">
                      </colgroup>
                      <thead>
                        <tr>
                          <th>
                            <v-btn icon @click="deleteTeamMember"><v-icon>mdi-delete</v-icon></v-btn>
                          </th>
                          <th>Team members</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="(teamMember, index) in teamDialog.teamMembers" :key="index">
                          <td class="text-center">
                            <input type="checkbox" v-model="teamDialog.checked[index]">
                          </td>
                          <td>
                            <input class="text-caption text-left table-input" type="text" size="54"
                              v-model="teamDialog.teamMembers[index]"
                            >
                          </td>
                        </tr>
                      </tbody>
                  </v-simple-table>
                </v-col>
              </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn class="mb-2 pt-2" small @click="closeTeam">
           Close
          </v-btn>
          &nbsp; &nbsp;
          <v-btn class="mb-2 pt-2" small @click="saveTeam">
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>

  <v-container v-show="manPowerForecastVisible" fluid class="my-0 py-0 mx-4 px-0 fill-height">
    <v-row>
      <v-col cols="12">
        <v-row>
          <v-col class="mt-0 pt-0 mb-n4 pb-n4" cols=7>
            <div class="text-h6" style="text-align: bottomz; display: inline">
              4 Day Schedule and Man Power Forecast
            </div>
          </v-col>
          <v-col class="text-right mt-0 pt-0 mb-n4 pb-n4 " cols=2>
            <div id="flatpickr" class="rounded text-body-2"
              style="padding: 3px; box-shadow: 1px 3px lightgrey;">
              <flat-pickr
                  v-model="selectedDate"
                  @on-change="gotoDayWindow"
                  :config="dateConfig"
                  class="calendar"
                  placeholder="Select date"
                  name="date"/>
            </div>
          </v-col>
          <v-col class="text-center mt-0 pt-0 mb-n4 pb-n4" cols=3>
            <v-btn small @click="prevDayWindow">
              Previous
            </v-btn>
            &nbsp; &nbsp;
            <v-btn small @click="nextDayWindow">
              Next
            </v-btn>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div style="height: 780px; overflow-y: scroll;">
              <table
                id="manpower-forecast"
                class="my-0 py-0 job-sched">
                  <colgroup>
                    <col style="width:4%;">
                    <col style="width:7%;">
                    <col v-bind:span="teams.length" v-bind:style="colWidthMp()">
                  </colgroup>
                  <thead>
                      <tr>
                        <th colspan="3" class="font-weight-bold"
                                style="font-size: 1.2em; background-color: rgb(48,84,150); color:white">
                            Manpower Planning Guide
                        </th>
                        <th :colspan="teams.length-1" class="font-weight-bold"
                                style="font-size: 1.2em; background-color: rgb(48,84,150); color:white">
                        </th>
                      </tr>
                  </thead>
                  <tbody v-for="(day, index) in dayWindow.days" :key="index">
                    <tr>
                        <td class="font-weight-bold date" align ="center" rowspan="4" :style="dayStyle(day.date)">
                            <div style="text-align: center; writing-mode: vertical-lr; transform: rotate(180deg);">
                                {{dayOfWeek(day.date)}} {{monthAndDate(day.date)}}
                            </div>
                        </td>
                        <td class="text-caption text-center font-weight-bold foreman">
                            Manpower: {{ day.manPowerTotals().manPower }}
                        </td>
                        <td v-for="team in teams" :key="team.id"
                            class="text-caption text-center font-weight-bold foreman">
                            {{team.foreman}}
                        </td>
                    </tr>
                    <tr>
                        <td class="text-caption text-center font-weight-bold foreman">
                            On leave: {{ day.manPowerTotals().onLeave }}
                        </td>
                        <td v-for="team in teams" :key="team.id"
                            class="text-caption text-center font-weight-bold">
                            {{team.vehicle}}
                        </td>
                    </tr>
                    <tr>
                        <td class="text-caption text-center font-weight-bold foreman">
                        </td>
                        <td v-for="team in teams" :key="team.id" class="text-caption font-weight-bold">
                              <div v-for="teamMember in getSchedJob(day, team.id).teamMembers" :key="teamMember"
                                  style="text-align: center;">
                                  {{teamMember}}
                              </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-caption text-center font-weight-bold foreman">
                            Day notes
                        </td>
                        <td v-for="team in teams" :key="team.id"
                            class="text-left"
                            :style = "jobScheduleStyle(getSchedJob(day,team.id))">
                        <div v-if="splitShift(getSchedJob(day,team.id))">
                          <label class="font-weight-bold">
                            Morning
                          </label><br>
                          {{ getSchedJob(day,team.id).job1 }}<br>
                          <label class="font-weight-bold">
                            Afternoon
                          </label><br>
                          {{ getSchedJob(day,team.id).job2 }}
                        </div>
                        <div v-else>
                          <label v-if="getSchedJob(day,team.id).shift==='day'"
                            class="font-weight-bold">
                            Day
                          </label>
                          <label v-if="getSchedJob(day,team.id).shift==='night'"
                            class="font-weight-bold">
                            Night
                          </label><br>
                          {{ getSchedJob(day,team.id).job1 }}
                        </div>
                      </td>
                    </tr>
                  </tbody>
              </table>
            </div>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>

  </div>
</template>

<style scoped>
html {
  overflow-y: auto;
}

.job-sched {
  width: 99%;
  height: 99%;
  overflow-x: hidden;
  overflow-y: auto;
  border-collapse: collapse;
}

.job-sched th {
  background-color: rgb(217,225,242);
  padding: 6px 4px 6px 4px;
  position: sticky;
  top: 0;
  z-index: 1;
  box-shadow: inset 1px 1px #000, 0 1px #000;
}

.job-sched .date {
  vertical-align: middle;
}

.job-sched td {
  vertical-align: top;
  border: 1px solid #000;
  padding: 5px 2px 0px 5px;
}

.job-sched .selected {
  border: 3px solid red;
}

.job-table {
  width:99%;
  height: 99%;
  overflow-x: hidden;
  overflow-y: auto;
  border-collapse: collapse;
}

.job-table th {
  background-color: rgb(217,225,242);
  position: sticky;
  top: 0;
  z-index: 1;
  box-shadow: inset 1px 1px #000, 0 1px #000;
  padding: 6px 4px 6px 4px;
}

.job-table td {
  border: 1px solid #000;
  border-left: 2px solid #000;
  padding: 4px 4px 4px 4px;
}

.job-projection {
  width: 100%;
  height: 130px;
  border-collapse: collapse;
  border: 1px solid black;
}

.job-projection td {
  font-size: 10pt;
}

.job-projection .narrow {
  width: 10%;
}

.job-projection .wide {
  width: 40%;
}

.job-projection .uniform {
  width: 33%;
}

.simple-job-table {
  border-collapse: collapse;
}

.simple-job-table th, .simple-job-table td {
  border: 2px solid #000;
  padding: 2px 2px 2px 2px;
}

.table-input {
  border-top-style: hidden;
  border-right-style: hidden;
  border-left-style: hidden;
  border-bottom-style: hidden;
}

.table-input:focus {
    outline: none;
}

.textarea {
  margin-top: 4px;
  border-style: none;
  border-color: Transparent;
  overflow: hidden;
  resize: none;
}

.textarea:focus {
  outline: none;
}

.shift {
  height: 100px;
  border: 1px solid black;
  border-radius: 10px;
  margin: 0px;
  padding: 8px;
}

.calendar {
  border: none;
  outline: none;
}
</style>

<script>
// import axios from "axios";

import flatPickr from 'vue-flatpickr-component';
import 'flatpickr/dist/flatpickr.css';
import 'flatpickr/dist/themes/material_blue.css';

import { Job, SchedJob, Team, Shift, Selection, Clipboard, JobStatus, Day, JobState } from '../composables/jp_entity.js';
import { datediff, date2string, string2date, addDays} from '../composables/jp_util.js';

  const priorityColors = [
    "tomato", "tomato", "orange", "orange", "yellow", "yellow", "lightgreen", "lightgreen", "limegreen", "limegreen"
  ];

  const dayColors = [
    "rgb(198,89,17)", "rgb(180,198,231)", "rgb(180,198,231)", "rgb(180,198,231)", "rgb(180,198,231)", "rgb(180,198,231)", "rgb(248,203,173)"
  ];

  export default {
    name: 'JobPlanner',
    components: {
        flatPickr
    },
    data: () => ({
      version: '1.0',
      jobScheduleVisible: true,
      jobProjectionVisible: false,
      manPowerVisible: false,
      manPowerForecastVisible: false,
      title: "Job Planner",
      timeoutId: null,
      timeStamp: null,
      jobs: [],
      dayWindow: { days: [], size: 4 },
      priorityColors: priorityColors,
      selection: null,
      copyData: null,
      jobScheduleCount: 0,
      firstUpdate: true,
      currentDate: "",
      selectedDate: "",
      dayIndex: {},
      deletedSchedJobs: [],
      schedJobGroups: {},
      teams: [],
      jobInfo: new SchedJob(),
      editedJob: {},
      //editedJobTable: [],
      changeJobDialog: false,
      changeJobDialog2: false,
      jobDialog: { open: false, startDate: "", endDate: "" },
      jobTableDialog: { open: false, option: "", title: "", change: false },
      teamDialog: { open: false, oper: "", title: "", maxTeamMembers: 8, team: {}, checked: [],
          teamMembers: [], editedTeam: {}, schedJob: {} },
      workerDialog: false,
      dailyPlanDialog: false,
      dateConfig: {
          altFormat: 'D, M j, Y',
          altInput: true,
          dateFormat: 'Y-m-d'
      },
      jobDateConfig: {
          altFormat: 'D, M j, Y',
          altInput: true,
          dateFormat: 'Y-m-d'
      },
      refreshJobProjectionTable: 0,
      refreshConfirmedJobs: 0,
      refreshUpcomingJobs: 0,
      autosaveInterval: 120,
      notify: () => {}
    }),
    computed: {
      jobSchedule() {
        this.currentDate;
        this.jobScheduleCount;

        if (this.currentDate === "") {
          this.currentDate = date2string(new Date());
        }

        const date = string2date(this.currentDate);
        const firstDay = new Date(date.setDate(date.getDate() - date.getDay() + 1));
        const days = [];
        for (var i=0; i<7; i++) {
            const d = addDays(firstDay, i)
            const sday = date2string(d);
            //const dd = this.dayIndex[sday];
            let day = this.getDay(sday);
            days.push(day);
        }
        return days;
      },
      confirmedJobs() {
        this.refreshConfirmedJobs;
        let confirmedJobs = this.jobs.filter( job => job.id > -1 && job.state === JobState.confirmed );
        this.sort(confirmedJobs);
        for (let i=confirmedJobs.length; i<25; i++) {
          const job = new Job( { id: 0, scope: "", value: "", status: JobStatus.blank, priority: ""} )
          confirmedJobs.push(job);
        }
        return confirmedJobs;
      },
      upcomingJobs() {
        this.refreshUpcomingJobs;
        let upcomingJobs = this.jobs.filter( job => job.id > -1 && job.state === JobState.upcoming );
        this.sort(upcomingJobs);
        for (let i=upcomingJobs.length; i<25; i++) {
          const job = new Job( { id: 0, scope: "", value: "", status: JobStatus.blank, priority: ""} )
          upcomingJobs.push(job);
        }
        return upcomingJobs;
      },
      editedJobTable() {
        return (this.jobTableDialog.option === JobState.confirmed) ? this.confirmedJobs : this.upcomingJobs;
      },
      jobProjectionTable() {
        this.refreshJobProjectionTable;
        const jpt = {
          inProgress: {
            title: "Confirmed jobs/in progress",
            jobs: this.jobs
              .filter( job => job.id > -1 && job.status === JobStatus.inProgress )
              .sort( (a, b) => a.priority - b.priority )
          },
          onHold: {
            title: "Confirmed but on hold jobs",
            jobs: this.jobs
              .filter( job => job.id > -1 && job.status === JobStatus.onHold )
              .sort( (a, b) => a.priority - b.priority )
          },
          highChance: {
            title: "High chance jobs",
            jobs: this.jobs
              .filter( job => job.id > -1 && job.status === JobStatus.highChance )
              .sort( (a, b) => a.priority - b.priority )
          },
          standBy: {
            title: "Stand-by jobs",
            jobs: this.jobs
              .filter( job => job.id > -1 && job.status === JobStatus.standBy )
              .sort( (a, b) => a.priority - b.priority )
          }
        };
        return jpt;
      }
    },
    created() {
      var self = this;
      document.body.addEventListener("keydown", function(e){
          e = e || window.event;
          // var key = e.which || e.keyCode; // keyCode detection
          var key = e.which || e.code; // keyCode detection
          var ctrl = e.ctrlKey ? e.ctrlKey : ((key === 17) ? true : false); // ctrl detection

          if ( key == 86 && ctrl ) {
              // console.log("Ctrl + V Pressed !");
              self.paste();
          } else if ( key == 67 && ctrl ) {
              // console.log("Ctrl + C Pressed !");
              self.copy();
          }
      },false);

      this.initialize();
    },
    updated() {
      if (this.firstUpdate) {
        this.firstUpdate = false;
        this.selectedDate = date2string(new Date());
        this.currentDate = this.selectedDate;
        this.initDayWindow();
      }
      this.notify();
    },
    methods: {
      updateTeamCount() {
        this.$forceUpdate();
      },
      updateShift() {
        this.$forceUpdate();
      },
      getDay(sday) {
        let day = this.dayIndex[sday];
        if (!day) {
          day = new Day({date: sday, jobs: {}});
          this.dayIndex[sday] = day;
        }
        this.teams.forEach( team => {
          if (!day.jobs[team.id]) {
            day.jobs[team.id] = new SchedJob({id: 0, team: team.id,
                date: sday, shift: Shift.unasigned,
                job1: "", job2: "", teamMembers : ["-"] });
          }
        });
        return day;
      },
      initialize() {
        this.getSchedData();
      },
      endpoint() {
        //return `http://localhost:5000/api/jobschedule`
        return '/jobschedule';
      },
      getSchedData() {
        this.$axios({
          url: this.endpoint(),
          method: "GET",
          crossDomain: true
        }).then(response => {
          this.initJobSchedule(response.data);
        })
      },
      initJobs(jobs) {
        jobs.forEach(ajob => {
          const job = new Job(ajob);
          this.jobs.push(job);
        });
      },
      updateJobs(jobs) {
        this.jobs = this.jobs.filter( job => job.id > 0);
        jobs.forEach( job => {
          const index = this.jobs.findIndex( aJob => job.id == aJob.id );
          if (index < 0) {
            // add new job
            this.jobs.push(new Job(job));
          } else {
            this.jobs[index] = new Job(job);
          }
        });
        this.refreshConfirmedJobs++;
        this.refreshUpcomingJobs++;
      },
      updateTeams(teams) {
        teams.forEach( team => {
          const index = this.teams.findIndex( aTeam => team.id == aTeam.id );
          if (index < 0) {
            this.teams.push(new Team(team));
          } else {
            this.teams[index] = new Team(team);
          }
        });
        this.teams.sort( (t1, t2) => t1.position - t2.position );
        this.$forceUpdate();
      },
      initJobSchedule(scheduleData) {
        SchedJob.groupIndex = this.schedJobGroups;
        this.timeStamp = scheduleData.timeStamp;
        this.updateTeams(scheduleData.teams);
        this.initJobs(scheduleData.jobs);

        const schedJobs = scheduleData.schedJobs;
        schedJobs.forEach( schedJob => this.updateSchedJob(schedJob) );

        this.currentDate = date2string(new Date());

        setInterval(this.save, this.autosaveInterval * 1000);
      },
      updateSchedJob(schedJob) {
        var sjGroup = this.schedJobGroups[schedJob.id];
        if (!sjGroup) {
          sjGroup = {};
          this.schedJobGroups[schedJob.id] = sjGroup;
        }

        const date = schedJob.date;
        let day = this.dayIndex[date];
        if (!day) {
          day = new Day({id: 0, date: date, jobs: {}});
          this.dayIndex[date] = day;
        }

        const sj = new SchedJob(schedJob);
        day.jobs[schedJob.team] = sj;
        sjGroup[date] = sj;
      },
      updateJobSchedule(updatedData) {
        const schedJobs = updatedData.schedJobs;
        schedJobs.forEach( schedJob => this.updateSchedJob(schedJob) );

        this.updateJobs(updatedData.jobs);
        this.updateTeams(updatedData.teams);
        this.jobScheduleCount++;
      },
      getSchedJob(day, teamId) {
        let sj = day.jobs[teamId];
        if (!sj) {
          sj = new SchedJob({id: 0, team: teamId,
              date: day.date, shift: Shift.unasigned,
              job1: "", job2: "", teamMembers : ["-"] });
        }
        return sj;
      },
      getDays() {
        const days = Object.values(this.dayIndex);
        return days.sort( (a, b) => a.date - b.date );
      },
      initDayWindow() {
        this.gotoDayWindow([], this.currentDate);
      },
      gotoDayWindow(selectedDates, dateStr) {
        const days = this.getDays();
        if (days.length == 0) {
          return;
        }
        const dayWindow = this.dayWindow
        this.currentDate = dateStr;
        let startDate = string2date(dateStr);
        for (let i=0; i<dayWindow.size; i++) {
          const date = addDays(startDate, i);
          dateStr = date2string(date);
          const day = this.getDay(dateStr);
          dayWindow.days[i] = day;
        }
        this.$forceUpdate();
      },
      nextDayWindow() {
        let date = addDays(string2date(this.currentDate), this.dayWindow.size);
        this.gotoDayWindow([], date2string(date));
      },
      prevDayWindow() {
        let date = addDays(string2date(this.currentDate), -(this.dayWindow.size));
        this.gotoDayWindow([], date2string(date));
      },
      showJobSchedule() {
        this.manPowerVisible = false;
        this.manPowerForecastVisible = false;
        this.jobProjectionVisible = false;
        this.jobScheduleVisible = true;
      },
      showJobProjectionOverview() {
        this.manPowerVisible = false;
        this.manPowerForecastVisible = false;
        this.jobScheduleVisible = false;
        this.jobProjectionVisible = true;
      },
      showManPower() {
        this.jobScheduleVisible = false;
        this.manPowerForecastVisible = false;
        this.jobProjectionVisible = false;
        this.manPowerVisible = true;
      },
      showManPowerForecast() {
        this.jobScheduleVisible = false;
        this.manPowerVisible = false;
        this.jobProjectionVisible = false;
        this.manPowerForecastVisible = true;
      },
      gotoDate(selectedDates, dateStr) {
        this.unselect();
        this.currentDate = dateStr;
      },
      previousWeek() {
        const date = string2date(this.currentDate);
        //const currDay = date.getDay();
        const nextMonday = addDays(date, -7);
        this.unselect();
        this.currentDate = date2string(nextMonday);
        this.selectedDate = this.currentDate;
      },
      nextWeek() {
        const date = string2date(this.currentDate);
        //const currDay = date.getDay();
        const nextMonday = addDays(date, 7);
        this.unselect();
        this.currentDate = date2string(nextMonday);
        this.selectedDate = this.currentDate;
      },
      dayOfWeek(date) {
        const str = new Intl.DateTimeFormat("en-US", { weekday: "long", month: "long", day: "numeric" }).format(string2date(date));
        return str.substring(0, str.indexOf(',')+1);
      },
      dayStyle(strDate) {
        const date = string2date(strDate)
        return `vertical-align: middle; background-color: ${dayColors[date.getDay()]};`
      },
      dayColor(date) {
        return `background-color: ${dayColors[string2date(date).getDay()]};`;
      },
      monthAndDate(strDate) {
        const date = string2date(strDate);
        const str = new Intl.DateTimeFormat("en-US", { weekday: "long", month: "long", day: "numeric" }).format(date);
        return str.substring(str.indexOf(',')+1).trim();
      },
      isBlank(job) {
        return (!job.id && !job.scope && !job.salesPerson && !job.priority && !job.value)
      },
      getSchedJobs(includeDeleted) {
        const schedJobs = {}
        for (const date of Object.keys(this.dayIndex)) {
          const day = this.dayIndex[date];
          for (const team of Object.keys(day.jobs)) {
            const sj = day.jobs[team];
            if (sj.id != 0) {
              schedJobs[sj.getPk()] = sj;
              if (sj.teamMembers.length == 1 && sj.teamMembers[0] == "-") {
                sj.teamMembers = [];
              }
            }
          }
        }
        if (includeDeleted) {
          for (const sj of this.deletedSchedJobs) {
            sj.id = -1;
            schedJobs[sj.getPk()] = sj;
          }
        }
        return Object.values(schedJobs);
      },
      save() {
        console.log("Saving data");
        const schedJobs = this.getSchedJobs(true);

        const isBlank = (job) => (!job.id && !job.scope && !job.salesPerson && !job.priority && !job.value)

        var jobs = this.jobs;
        jobs = jobs.filter( job => !isBlank(job) );
        jobs.forEach(job => job.priority === "" ? 0 : job.priority );

        const data = {jobs: jobs, schedJobs: schedJobs, teams: this.teams};
        const url = this.endpoint();

        var self = this;

        this.$axios({
          url: this.endpoint(),
          method: "PUT",
          data: data,
          crossDomain: true
        })
        .then(function (response) {
          self.updateJobSchedule(response.data);
        })
        .catch(function (error) {
          console.log(error);
        });
      },
      copy() {
        if (!this.selection) {
          return;
        }
        this.copyData = new Clipboard(this.teams[this.selection.col-1].id,
            this.jobSchedule[this.selection.startRow-1].date,
            this.selection.endRow-this.selection.startRow+1);
      },
      paste() {
        if (!this.copyData) {
          return;
        }

        const team = this.teams[this.selection.col-1].id;
        const foreman = this.teams[this.selection.col-1].foreman;

        if (foreman == "On leave") {
          return;
        }

        const source = {
          startDate: this.copyData.startDate,
          numRows: this.copyData.numRows,
          team: this.copyData.team
        }
        const target = {
          startDate: this.jobSchedule[this.selection.startRow-1].date,
          numRows: -1,
          team: team
        }

        this.copySchedJobs(source, target, false)

        this.unselect();
        this.$forceUpdate();
      },
      scheduledJobDragstart(event, dayIndex, teamIndex) {
        const source = event.target.getAttribute("data-source");
        const data = { source: source, dayIndex: dayIndex, teamIndex: teamIndex };
        event.dataTransfer.setData("application/json", JSON.stringify(data));
      },
      jobDragStart(ev) {
        const source = ev.target.getAttribute("data-source");
        if (source == "confirmed-job" || source == "upcoming-job") {
          const data = { source: source, jobId: ev.target.getAttribute("data-jobid") };
          ev.dataTransfer.setData("application/json", JSON.stringify(data));
        }
      },
      dropJob(srcData, targetIndex, teamIndex) {
        const teamId = this.teams[teamIndex].id;
        const foreman = this.teams[teamIndex].foreman;

        if (foreman === "On leave") {
          return;
        }

        const date = this.jobSchedule[targetIndex].date;
        const jobTable = srcData.source == "confirmed-job" ? this.confirmedJobs : this.upcomingJobs;

        const jobIndex = jobTable.findIndex(item => item.id == srcData.jobId);
        const job = jobTable[jobIndex];

        const currSchedJob = this.jobSchedule[targetIndex].jobs[teamId];

        if (currSchedJob.shift == Shift.unasigned) {
          const shift = foreman === "Store" ? Shift.unasigned : Shift.day;
          const schedJob = new SchedJob({id: 0, team: teamId,
              date: date, shift: shift, job1: job.scope, job2: job.scope });
          this.copyOneToOne(schedJob, -1, date, teamId, true)
        }

        this.$forceUpdate();
      },
      dropHandler(event, dayIndex, teamIndex) {
        event.preventDefault();
        const srcData = JSON.parse(event.dataTransfer.getData("application/json"));

        if (srcData.source === 'confirmed-job' || srcData.source === 'upcoming-job') {
          this.dropJob(srcData, dayIndex, teamIndex);
          return;
        }

        this.dropScheduledJob(srcData, dayIndex, teamIndex);
      },
      dropScheduledJob(srcData, targetIndex, teamIndex) {
        const teamId = this.teams[teamIndex].id;
        const foreman = this.teams[teamIndex].foreman;

        if (foreman === "Store" || foreman === "On leave") {
          return;
        }

        var sourceIndex = srcData.dayIndex;

        const srcDate = this.jobSchedule[sourceIndex].date;
        const targetDate = this.jobSchedule[targetIndex].date;


        if ((srcData.teamIndex === teamIndex) && (srcDate === targetDate)) {
          // source and target are the same cell
          return;
        }

        let sourceData, targetData;
        if (!this.selection) {
          sourceData = {
            startDate: srcDate,
            numRows: 1,
            team: this.teams[srcData.teamIndex].id
          }
          targetData = {
            startDate: targetDate,
            numRows: 1,
            team: teamId
          }
        } else {
          sourceData = {
            startDate: this.jobSchedule[this.selection.startRow-1].date,
            numRows: this.selection.endRow-this.selection.startRow+1,
            team: this.teams[srcData.teamIndex].id
          }
          targetData = {
            startDate: targetDate,
            numRows: -1,
            team: teamId
          }
        }

        this.copySchedJobs(sourceData, targetData, true)

        this.jobScheduleCount++;
        this.unselect();
      },
      sameJob(startDate, numRows, team) {
        if (numRows <= 1) {
          return true;
        }
        const d = string2date(startDate);
        for (var i=1; i<numRows; i++) {
          const d1 = date2string(addDays(d, i));
          const d2 = date2string(addDays(d, i-1));
          if (this.dayIndex[d1].jobs[team].id !== this.dayIndex[d2].jobs[team].id) {
            return false;
          }
        }
        return true;
      },
      nextGroupId() {
        const ids = Object.keys(this.schedJobGroups).sort( (id1, id2) => id1-id2 );
        return ids.length == 0 ? 1 : parseInt(ids[ids.length-1])+1;
      },
      nextGroupElement(dates, group, from) {
        for (let i=from; i<dates.length; i++) {
          const date = dates[i]
          const sj = group[date];
          if (sj) {
            return i;
          }
        }
        return -1
      },
      updateSchedJobGroup(groupId) {
        let id = groupId;
        const group = this.schedJobGroups[id];
        const len = Object.entries(group).length;
        const dates = Object.keys(group).sort( (d1, d2) => d1<d2 ? -1 : (d1===d2 ? 0 : 1) );
        let newGroup = {};
        let i=this.nextGroupElement(dates, group, 0);
        for (; i<len; i++) {
          const date = dates[i];
          const sj = group[date];
          if (!sj) {
            break;
          }
          sj.id = id;
          newGroup[date] = sj;
        }
        this.schedJobGroups[id] = newGroup;

        // check for a second group
        id = this.nextGroupId();
        newGroup = {};
        i=this.nextGroupElement(dates, group, i);

        if (i>-1) {
          for (; i<len; i++) {
            const date = dates[i];
            const sj = group[date];
            if (!sj) {
              break;
            }
            sj.id = id;
            newGroup[date] = sj;
          }
          this.schedJobGroups[id] = newGroup;
        }
      },
      copySchedJobs(source, target, deleteSource) {
        const sameJob = this.sameJob(source.startDate, source.numRows, source.team)

        if (sameJob) {
          const src  = {startDate: source.startDate, numRows: source.numRows, team: source.team};
          const numRows = target.numRows == -1 ? source.numRows : target.numRows;
          const trgt = {startDate: target.startDate, numRows: numRows, team: target.team};

          this.copyOneToMany(src, trgt)
        } else {
          const sourceStartDate = string2date(source.startDate);
          const targetStartDate = string2date(target.startDate);
          for (var i=0; i<source.numRows; i++) {
            const src  = {startDate: addDays(sourceStartDate, i), numRows: 1, team: source.team};
            const trgt = {startDate: addDays(targetStartDate, i), numRows: 1, team: target.team};
            this.copyOneToMany(src, trgt);
          }
        }

        if (deleteSource) {
          this.deleteSchedJobs(source);
        }
        // this.printGroups();
      },
      deleteSchedJobs(source) {
        let deleted = {};
        let startDate = string2date(source.startDate);
        for (let i=0; i<source.numRows; i++) {
          const date = date2string(addDays(startDate, i));
          const sj = this.dayIndex[date].jobs[source.team];
          const groupId = sj.id;
          delete this.dayIndex[date].jobs[source.team];
          this.schedJobGroups[groupId][date] = null;
          sj.id = -1;
          this.deletedSchedJobs.push(sj);
          deleted[groupId] = true;
        }
        for (const groupId of Object.keys(deleted)) {
          this.updateSchedJobGroup(groupId);
        }
      },
      copyOneToMany(source, target) {
        const targetStartDate = string2date(target.startDate);

        const newGroup = {};
        const newId = this.nextGroupId();
        this.schedJobGroups[newId] = newGroup;

        for (let i=0; i<target.numRows; i++) {
          const d = date2string(addDays(targetStartDate, i));
          let day = this.dayIndex[d];
          if (!day) {
            day = new Day({id: 0, date: d, jobs: {} })
            this.dayIndex[d] = day;
          }

          const schedJob = new SchedJob(this.dayIndex[source.startDate].jobs[source.team]);
          schedJob.id = newId;
          schedJob.date = d;
          schedJob.team = target.team
          day.jobs[target.team] = schedJob;

          newGroup[d] = schedJob;

          const idx = this.teams.findIndex( t => t.id == target.team );
          schedJob.teamMembers = this.teams[idx].teamMembers.slice();
        }
      },
      copyOneToOne(schedJob, groupId, date, team, assignTeamMembers) {
        let group;
        if (groupId == -1) {
          group = {};
          groupId = this.nextGroupId();
          this.schedJobGroups[groupId] = group;
        } else {
          group = this.schedJobGroups[groupId];
        }

        let day = this.dayIndex[date];
        if (!day) {
          day = new Day({id: 0, date: date, jobs: {} })
          this.dayIndex[date] = day;
        }

        day.jobs[team] = schedJob;

        schedJob.id = groupId;
        schedJob.date = date;
        schedJob.team = team;
        group[date] = schedJob;

        if (assignTeamMembers) {
          const idx = this.teams.findIndex( t => t.id == team );
          schedJob.teamMembers = this.teams[idx].teamMembers.slice();
        }
      },
      teamMemberDragstart(ev) {
        const data = { dayIndex: ev.target.getAttribute("data-day-index"),
            team: ev.target.getAttribute("data-team"), teamMember: ev.target.getAttribute("data-team-member")};
        ev.dataTransfer.setData("application/json", JSON.stringify(data));
      },
      dropTeamMember(event, targetJob) {
        event.preventDefault();
        const teamMembers = targetJob.teamMembers;
        const srcData = event.dataTransfer.getData("application/json");
        const src = JSON.parse(srcData);
        const dayIndex = Number(src.dayIndex);
        const srcJob = this.jobSchedule[dayIndex].jobs[src.team];
        const srcTeamMembers = srcJob.teamMembers;
        srcTeamMembers.splice(srcTeamMembers.indexOf(src.teamMember), 1);
        if (teamMembers.length == 1 && teamMembers[0] == "-") {
          teamMembers[0] = src.teamMember;
        } else {
          teamMembers.push(src.teamMember);
        }
        srcJob.setLastUpdate();
        targetJob.setLastUpdate();
        this.$forceUpdate();
      },
      dragoverHandler(event) {
        event.preventDefault();
        event.dataTransfer.dropEffect = "move"
      },
      colWidth() {
        const str = `width:${92/this.teams.length}%;`;
        return str;
      },
      colWidthMp() {
        const str = `width:${90/this.teams.length}%;`;
        return str;
      },
      findAncestor(el, tag) {
          for ( ; el.tagName != tag; el = el.parentElement);
          return el;
      },
      unselect() {
        if (!this.selection) {
          return;
        }
        const table = document.getElementById("job-sched");
        for (var i=this.selection.startRow; i<=this.selection.endRow; i++) {
          const cell = table.rows[i].cells[this.selection.col];
          while (cell.classList.contains("selected")) {
              cell.classList.remove("selected");
          }
        }
        this.selection = null;
      },
      onTableCellClick(event, job) {
        if (!this.timeoutId) {
            this.timeoutId = setTimeout(() => {
              this.onTableCellSingleClick(event, job);
              this.timeoutId = null;
            }, 250);//tolerance in ms
        } else {
            // double click
            // open job dialog
            clearTimeout(this.timeoutId);
            this.timeoutId = null;
            this.openSchedJobDialog(job);
        }
      },
      // printGroups() {
      //   console.log("printGroups")
      //   for (const groupId of Object.keys(this.schedJobGroups)) {
      //     const group = this.schedJobGroups[groupId];
      //     let psj;
      //     for (const date of Object.keys(group)) {
      //       const sj = group[date];
      //       console.log(`${sj.id} ${sj.team} ${sj.date}`)
      //       if (psj && psj == sj) {
      //         console.log(`${psj.date} and ${sj.date} are the same `)
      //       }
      //       if (psj && psj.getJob1() == sj.getJob1()) {
      //         console.log("jobs are the same object")
      //       }
      //     }
      //   }
      // },
      onTableCellSingleClick(event, job) {
        if (job.shift == Shift.unasigned) {
          this.unselect();
        }

        const td = this.findAncestor(event.target, "TD");
        const tr = this.findAncestor(td, "TR");

        // No previous selection
        if (!this.selection) {
          td.classList.add("selected");
          this.selection = new Selection(td.cellIndex, tr.rowIndex);
          return;
        }

        // Clicked on cell on another column
        if ((this.selection.col != td.cellIndex) || (tr.rowIndex < this.selection.startRow)) {
          // console.log("Clicked on cell on another column. Unselect all selections and select this cell");
          this.unselect();
          td.classList.add("selected");
          this.selection = new Selection(td.cellIndex, tr.rowIndex);
          return;
        }

        // Clicked on a selected cell. Unselect it
        if ((tr.rowIndex >= this.selection.startRow && tr.rowIndex <= this.selection.endRow)) {
            // console.log("Clicked on a selected cell. Unselect all selections");
            this.unselect();
            return;
        }

        // console.log("Clicked on cell on the same column, below current selection. Add this cell to selection");
        var table = document.getElementById("job-sched");
        for (var i=this.selection.endRow+1; i<=tr.rowIndex; i++) {
          const cell = table.rows[i].cells[td.cellIndex];
          cell.classList.add("selected");
        }
        this.selection.endRow = tr.rowIndex;
      },
      splitShift(jobInfo) {
        if (jobInfo) {
          return jobInfo.splitShift;
        }
        return this.jobInfo.splitShift;
      },
      schedJobInfo(date, team) {
        return date + ':' + team;
      },
      schedJobLabel() {
        if (this.jobInfo.shift == Shift.unasigned) {
          return "";
        } else if (this.jobInfo.shift === Shift.night) {
          return "Night";
        } else if (this.jobInfo.job1 === this.jobInfo.job2) {
          return "Day";
        } else if (this.jobInfo.job1) {
          return "Morning";
        } else if (this.jobInfo.job2) {
          return "Afternoon";
        }
      },
      getForeman(teamId) {
        const idx = this.teams.findIndex( team => team.id === teamId );
        return idx > -1 ? this.teams[idx].foreman : "";
      },
      openSchedJobDialog(job) {
        this.editedJob = job;
        this.jobInfo = new SchedJob(job);
        this.jobInfo.shift = Shift.day;
        this.jobDialog.startDate = job.startDate;
        this.jobDialog.endDate = job.endDate;
        this.jobDialog.open = true;
      },
      getDayIndex(date) {
        const jobSchedule = this.jobSchedule;
        return jobSchedule.findIndex( day => day.date === date );
      },
      saveSchedJob() {
        let idx1 = this.editedJob.startDate < this.jobDialog.startDate ? this.getDayIndex(this.editedJob.startDate) : -1;
        let idx2 = this.editedJob.endDate < this.jobDialog.startDate ? this.getDayIndex(this.editedJob.endDate) :
            this.getDayIndex(this.jobDialog.startDate)-1;
        let idx3 = this.editedJob.endDate > this.jobDialog.endDate ? this.getDayIndex(this.jobDialog.endDate)+1 : -1;
        let idx4 = this.getDayIndex(this.editedJob.endDate);

        const sourceData = {
          startDate: this.editedJob.date,
          numRows: 1,
          team: this.jobInfo.team
        }
        const targetData = {
          startDate: this.jobDialog.startDate,
          numRows: datediff(this.jobDialog.startDate, this.jobDialog.endDate)+1,
          team: this.jobInfo.team
        }

        // const sj = this.jobInfo;
        this.editedJob.update( this.jobInfo );
        if (this.editedJob.shift != Shift.unasigned && this.editedJob.job1) {
          let team = this.teams.find(team => team.id === this.jobInfo.team);
          this.editedJob.teamMembers = team ? team.teamMembers.slice() : [];
        }
        const schedJob = this.editedJob;

        if (schedJob.job1.toLowerCase().trim() === "on leave") {
          schedJob.job1 = "On Leave";
          schedJob.shift = Shift.onLeave;
        }

        this.copySchedJobs(sourceData, targetData, false);

        const team = this.editedJob.team;
        if (idx1 != -1) {
          var source1 = {
            startDate: this.jobSchedule[idx1].date,
            numRows: idx2-idx1+1,
            team: team
          }
        }
        if (idx3 != -1) {
          var source2 = {
            startDate: this.jobSchedule[idx3].date,
            numRows: idx4-idx3+1,
            team: team
          }
        }

        // this will be called after Vue has updated the page to delete the scheduled jobs
        // that need to be deleted
        this.notify = () => {
          this.notify = () => {};

          if (idx1 != -1) {
            this.deleteSchedJobs(source1);
          }

          if (idx3 != -1) {
            this.deleteSchedJobs(source2);
          }

          this.jobScheduleCount++;
        }

        this.jobScheduleCount++;
        this.jobDialog.open = false;
      },
      flipJobs() {
        const temp = this.jobInfo.job1;
        this.jobInfo.job1 = this.jobInfo.job2;
        this.jobInfo.job2 = temp;
        this.$forceUpdate();
      },
      closeSchedJob() {
        this.jobDialog.open = false;
      },
      openJobTable(option) {
        if (option === JobState.confirmed) {
          this.jobTableDialog.option = JobState.confirmed;
          this.jobTableDialog.title = "Confirmed Jobs"
        } else if (option === JobState.upcoming) {
          this.jobTableDialog.option = JobState.upcoming;
          this.jobTableDialog.title = "Upcoming Jobs"
        }
        this.jobTableDialog.open = true;
      },
      closeJobTable() {
        this.jobTableDialog.open = false;
      },
      markChange() {
        this.jobTableDialog.change = true;
      },
      addToJobs(job) {
        if (job.status === JobStatus.blank) {
          job.status = (this.jobTableDialog.option === JobState.confirmed) ? JobStatus.onHold : JobStatus.standBy;
          job.priority = !job.priority ? 10 : job.priority;
          this.jobs.push(job);
          this.refreshJobProjectionTable++;
        }
      },
      sortByPriority() {
        if (this.jobTableDialog.change) {
          this.sort(this.editedJobTable);
        }
      },
      sort(jobTable) {
          jobTable.sort( (a, b) => {
            const pa = a.priority ? a.priority : 1000000;
            const pb = b.priority ? b.priority : 1000000;
            return pa - pb;
          })
      },
      jobScheduleStyle(jobInfo) {
        const shift = jobInfo.shift;
        const fontSize = '0.7em'
        if (shift == Shift.day) {
          return `background-color:rgb(198,224,180);font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`;
        } else if (shift == Shift.night) {
          return `background-image:linear-gradient(rgb(20,50,0),rgb(100,140,90));color:white;font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`
        } else if (shift == Shift.rest) {
          return `background-color:lightyellow;font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`
        } else if (shift == Shift.onLeave) {
          return `background-color:rgb(68,114,196);color:white;font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`
        } else if (shift == Shift.unasigned) {
          if (jobInfo.team == "Store" && jobInfo.job1) {
            return `background-color:rgb(198,224,180);font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`;
          }
          return `background-color:white;font-size:${fontSize};`;
        }
      },
      jobProjectionCommentsStyle(comments) {
        if (!comments) {
          return "margin-top: 9px; color: black; display:table-cell; width:100%";
        }
        const color = (comments.includes("urgent") || comments.includes("Urgent") || comments.includes("URGENT"))
            ? 'magenta' : 'black'
        return `margin-top: 9px; color: ${color}; display:table-cell; width:100%`;
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
      jobProjectionShiftOptionColor(value) {
        if (value === "") {
          return "color: grey;";
        }
        return "color: black;";
      },
      jobProjectionShiftBackground(shift) {
        if (shift === "Day") {
          return "background-color:rgb(198,224,180)";
        } else if (shift === "Night") {
          return "background: linear-gradient(rgb(20,50,0),rgb(100,140,90));";
        } else if (shift === "Mixed") {
          return "background: linear-gradient(to right, rgb(198,224,180) 0%, rgb(198,224,180) 40%, rgb(20,50,0) 80%, rgb(20,50,0) 100%);";
        }
        return "background: white;"
      },
      background(priority) {
        if (!priority) {
          return "background-color:white"
        }
        return `background-color:${priorityColors[priority-1]};`
      },
      addJob(subStatus) {
        this.jobs.push(new Job( { id: 0, scope: "", value: "", status: subStatus, priority: 10} ));
        this.refreshJobProjectionTable++;
      },
      delJob() {
        for (var i=this.editedJobTable.length-1; i>=0; i--) {
          if (this.editedJobTable[i].checked) {
            const job = this.editedJobTable[i];
            if (job.id === 0) {
              let idx = this.jobs.findIndex(element => element == job);
              if (idx > -1) {
                this.jobs.splice(idx, 1);
              }
            }
            else {
              job.id = -job.id;
              job.status = JobStatus.blank;
            }
            this.refreshUpcomingJobs++;
            this.refreshConfirmedJobs++;
            this.refreshJobProjectionTable++;
          }
        }
      },
      addSchedJob() {
        if (this.jobInfo.shift === Shift.day || this.jobInfo.shift === Shift.unasigned) {
          this.jobInfo.shift = Shift.day;
          this.jobInfo.splitShift = true;
          this.jobInfo.job2 = "";
          this.$forceUpdate();
        }
      },
      editTeam(team, schedJob) {
        this.teamDialog.team = team;
        this.teamDialog.editedTeam = new Team(team);
        this.teamDialog.teamMembers = team.teamMembers.slice();
        this.teamDialog.schedJob = schedJob;
        //console.log("schedJob:", JSON.stringify(schedJob));
        for (var i=this.teamDialog.teamMembers.length; i<this.teamDialog.maxTeamMembers; i++) {
          this.teamDialog.teamMembers.push("");
        }
        this.teamDialog.checked = new Array(this.teamDialog.maxTeamMembers).fill(false);
        this.teamDialog.oper = "edit";
        this.teamDialog.title = "Edit team";
        this.teamDialog.open = true;
      },
      newTeam() {
        this.teamDialog.team = {};
        this.teamDialog.editedTeam = new Team({ team: "", vehicle: "", position: 0, teamMembers: []});
        this.teamDialog.teamMembers = [];
        for (var i=this.teamDialog.teamMembers.length; i<this.teamDialog.maxTeamMembers; i++) {
          this.teamDialog.teamMembers.push("");
        }
        this.teamDialog.checked = new Array(this.teamDialog.maxTeamMembers).fill(false);
        this.teamDialog.oper = "new";
        this.teamDialog.title = "New team";
        this.teamDialog.open = true;
      },
      updateTeamOrder() {
        this.teams.sort( (t1, t2) => t1.position - t2.position );
        for (let i=0; i<this.teams.length; i++) {
          if (this.teams[i].position < 100) {
            this.teams[i].position = i+1;
          }
        }
      },
      copyTeamMembers(source, target) {
        target.length = 0;
        for (var i=0; i<source.length; i++) {
          if (source[i].trim() !== "") {
            target.push(source[i]);
          }
        }
        if (target.length == 0) {
          target.push("-");
        }
      },
      createTeam() {
        const team = this.teamDialog.editedTeam;
        // console.log("new team:", JSON.stringify(team));
        this.teams.splice(team.position-1, 0, team);
        this.copyTeamMembers(this.teamDialog.teamMembers, team.teamMembers);
        this.updateTeamOrder();
        this.jobScheduleCount++;
        this.teamDialog.open = false;
      },
      saveTeam() {
        if (this.teamDialog.oper === "new") {
          this.createTeam();
          return;
        }

        const team = this.teamDialog.editedTeam;
        team.position -= 0.5;
        this.teamDialog.team.update(this.teamDialog.editedTeam);
        this.copyTeamMembers(this.teamDialog.teamMembers, this.teamDialog.team.teamMembers);
        this.updateTeamOrder();
        this.jobScheduleCount++;
        this.teamDialog.open = false;
      },
      deleteTeam() {
        const team = this.teamDialog.editedTeam;
        this.teams.splice(team.position, 1);
        this.jobScheduleCount++;
        this.teamDialog.open = false;
      },
      deleteTeamMember() {
        // console.log("deleteTeamMember");
        const teamMembers = this.teamDialog.editedTeam.teamMembers;
        const checked = this.teamDialog.checked;
        for (var i=teamMembers.length-1; i>=0; i--) {
          if (checked[i]) {
            checked[i] = false;
            teamMembers.splice(i, 1);
          }
        }
      },
      closeTeam() {
        this.teamDialog.open = false;
      },
    }
  }
</script>

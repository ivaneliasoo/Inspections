<template>
  <div>
    <v-container v-show="jobScheduleVisible" fluid class="my-n2 py-n2 mx-4 px-0 fill-height">
      <v-row>
        <v-col cols="10">
          <v-row>
            <v-col class="text-left mt-0 pt-0 mb-n4 pb-n4" cols="1">
              <main-menu
                @show-job-projection-overview="showJobProjectionOverview"
                @show-job-schedule="showJobSchedule"
                @show-man-power="showManPower"
                @show-man-power-forecast="showManPowerForecast"
                @open-team-dialog="openTeamDialog"
                @edit-options="editOptions"
                @save="save"
              />
            </v-col>
            <v-col class="mt-0 pt-0 mb-n4 pb-n4 " cols="4">
              <div class="text-h6" style="text-align: bottomz; display: inline">
                Job Schedule Projection
              </div>
            </v-col>
            <v-col class="text-right mt-0 pt-0 mb-n4 pb-n4" cols="2">
              <div
                id="flatpickr"
                class="rounded text-body-2"
                style="padding: 3px; box-shadow: 1px 3px lightgrey;"
              >
                <flat-pickr
                  v-model="selectedDate"
                  :config="dateConfig"
                  class="calendar"
                  placeholder="Select date"
                  name="date"
                  @on-change="gotoDate"
                />
              </div>
            </v-col>
            <v-col class="text-center mt-0 pt-0 mb-n4 pb-n4" cols="5">
              <v-btn small @click="previousWeek">
                Previous Week
              </v-btn>
              &nbsp; &nbsp;
              <v-btn small @click="nextWeek">
                Next Week
              </v-btn>
              &nbsp; &nbsp;
              <v-btn small @click="save">
                Save
              </v-btn>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <div style="height: 80vh; overflow-y: scroll">
                <table
                  id="job-sched"
                  class="my-0 py-0 job-sched"
                >
                  <colgroup>
                    <col style="width:8%;">
                    <col :span="jpTeams.length" :style="colWidth()">
                  </colgroup>
                  <thead>
                    <tr>
                      <th class="text-caption font-weight-bold">
                        Date
                      </th>
                      <th
                        v-for="(team, index) in jpTeams"
                        :key="team.id"
                        class="text-caption font-weight-bold draggable"
                        draggable="true"
                        data-source="team"
                        @drop="dropTeam($event, index, team)"
                        @dragover="dragoverHandler($event)"
                        @dragstart="teamDragStart($event, index, team)"
                      >
                        {{ team.foreman }}
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(day, index) in jobSchedule" :key="index" style="height: 0px;">
                      <td
                        class="text-caption text-left font-weight-bold date"
                        :style="dayColor(day.date)"
                        @contextmenu="openManpowerContextMenu($event, index, day)"
                      >
                        <div style="text-align: center;">
                          {{ dayOfWeek(day.date) }}<br>
                          {{ monthAndDate(day.date) }}
                        </div>
                      </td>
                      <td
                        v-for="(team, teamIndex) in jpTeams"
                        :key="team.id"
                        class="text-left draggable"
                        draggable="true"
                        data-source="scheduled-job"
                        :style="jobScheduleStyle(day.jobs[team.id])"
                        @click="onTableCellClick($event, day.jobs[team.id])"
                        @drop="dropHandler($event, index, teamIndex)"
                        @dragover="dragoverHandler($event)"
                        @dragstart="scheduledJobDragstart($event, index, teamIndex)"
                        @contextmenu="openContextMenu"
                      >
                        <div v-if="showSchedJob(day.jobs[team.id])">
                          <div v-if="splitShift(day.jobs[team.id])">
                            <label class="font-weight-bold">
                              Morning
                            </label><br>
                            {{ day.jobs[team.id].getJob1() }}<br>
                            <label class="font-weight-bold">
                              Afternoon
                            </label><br>
                            {{ day.jobs[team.id].getJob2() }}
                          </div>
                          <div v-else>
                            <label class="font-weight-bold">
                              {{ schedJobLabel(day.jobs[team.id]) }}
                            </label>
                            <br>
                            {{ day.jobs[team.id].getJob1() }}
                          </div>
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
              <div style="height:28px;" />
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-expansion-panels v-model="jobProjectionPanel" focusable>
                <v-expansion-panel
                  v-for="(jlist, jid) in jobProjectionTable"
                  :key="jid"
                >
                  <v-expansion-panel-header>{{ jlist.title }} ({{ jlist.jobs.length }})</v-expansion-panel-header>
                  <v-expansion-panel-content>
                    <job-projection-category
                      :id="jid"
                      :job-list="jlist"
                      height="490px"
                      :show-header="false"
                      @add-job="addJob"
                    />
                  </v-expansion-panel-content>
                </v-expansion-panel>
              </v-expansion-panels>
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <p justify="end" class="my-0 py-0 text-caption">
                Version {{ version }}
              </p>
            </v-col>
          </v-row>
        </v-col>
      </v-row>

      <v-dialog v-model="jobDialog.open" max-width="800px">
        <v-card>
          <v-card-title class="mb-0 pb-0">
            <span class="mx-2 headline">Assign Job</span>
          </v-card-title>
          <v-card-text>
            <v-container>
              <v-row>
                <v-col cols="12" class="my-0 py-0">
                  <v-row>
                    <v-col cols="6">
                      <div
                        id="flatpickr"
                        class="mt-2 pt-1 mb-0 pb-0 rounded text-body-2"
                        style="padding: 3px; box-shadow: 1px 3px lightgrey;"
                      >
                        <flat-pickr
                          v-model="jobDialog.startDate"
                          :config="jobDateConfig"
                          class="calendar"
                          placeholder="Select date"
                          name="startDate"
                        />
                      </div>
                    </v-col>
                    <v-col cols="6">
                      <div
                        id="flatpickr"
                        class="mt-2 pt-1 mb-0 pb-0 rounded text-body-2"
                        style="padding: 3px; box-shadow: 1px 3px lightgrey;"
                      >
                        <flat-pickr
                          v-model="jobDialog.endDate"
                          :config="jobDateConfig"
                          class="calendar"
                          placeholder="Select date"
                          name="endtDate"
                        />
                      </div>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="10">
                      <label class="font-weight-bold">
                        Team: {{ getForeman(jobInfo.team) }}
                      </label>
                    </v-col>
                    <v-col cols="2" class="mb-2 pb-2 text-right">
                      <v-btn icon small @click="flipJobs">
                        <v-icon>mdi-orbit-variant</v-icon>
                      </v-btn>
                      <v-btn icon small @click="addSchedJob">
                        <v-icon v-if="splitShift(jobInfo)">
                          mdi-border-all-variant
                        </v-icon>
                        <v-icon v-else>
                          mdi-dns-outline
                        </v-icon>
                      </v-btn>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col class="my-0 py-0">
                      <div v-if="splitShift(jobInfo)">
                        <v-row>
                          <v-col cols="10" class="my-0 py-0">
                            <label class="font-weight-bold">
                              Morning
                            </label>
                          </v-col>
                        </v-row>
                        <v-row>
                          <v-col>
                            <v-textarea
                              v-model="jobInfo.job1"
                              rows="3"
                              outlined
                            />
                          </v-col>
                        </v-row>
                        <v-row>
                          <v-col cols="10" class="my-0 py-0">
                            <label class="font-weight-bold">
                              Afternoon
                            </label>
                          </v-col>
                        </v-row>
                        <v-row>
                          <v-col>
                            <v-textarea
                              v-model="jobInfo.job2"
                              rows="3"
                              outlined
                            />
                          </v-col>
                        </v-row>
                      </div>
                      <div v-else>
                        <v-row>
                          <v-col cols="10" class="my-0 py-0">
                            <label class="font-weight-bold">
                              {{ jobInfo.shiftLabel() }}
                            </label>
                          </v-col>
                        </v-row>
                        <v-row>
                          <v-col>
                            <v-textarea
                              v-model="jobInfo.job1"
                              outlined
                            />
                          </v-col>
                        </v-row>
                      </div>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="9" class="my-0 py-0">
                      <v-radio-group
                        v-model="jobInfo.shift"
                        class="ml-8"
                        row
                      >
                        <v-radio
                          label="Day"
                          value="day"
                        />
                        <v-radio
                          label="Night"
                          value="night"
                        />
                        <v-radio
                          label="TBC"
                          value="tbc"
                        />
                      </v-radio-group>
                    </v-col>
                    <v-col cols="3" class="my-0 py-0">
                      <v-checkbox
                        v-model="jobInfo.excludeSaturday"
                        label="Exclude saturday"
                        hide-details
                      />
                      <v-checkbox
                        v-model="jobInfo.excludeSunday"
                        label="Exclude sunday"
                        hide-details
                      />
                    </v-col>
                  </v-row>
                </v-col>
              </v-row>
            </v-container>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="blue darken-1" text @click="closeSchedJob">
              Cancel
            </v-btn>
            <v-btn color="blue darken-1" text @click="saveSchedJob">
              Save &amp; Close
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <div id="content" />
    </v-container>

    <v-container v-show="jobProjectionVisible" fluid class="my-0 py-0 mx-4 px-0 fill-height">
      <v-row style="margin-right: 8px;">
        <v-col>
          <v-row>
            <v-col class="text-left mt-0 pt-0 mb-n4 pb-n4" cols="1">
              <main-menu
                @show-job-projection-overview="showJobProjectionOverview"
                @show-job-schedule="showJobSchedule"
                @show-man-power="showManPower"
                @show-man-power-forecast="showManPowerForecast"
                @edit-options="editOptions"
                @save="save"
              />
            </v-col>
            <v-col class="mt-0 pt-0 mb-n4 pb-n4" cols="10">
              <div class="text-h6" style="text-align: bottomz; display: inline">
                Job Projection Overview
              </div>
            </v-col>
            <v-col class="mt-0 pt-0 mb-n4 pb-n4" cols="1">
              <v-btn small @click="save">
                Save
              </v-btn>
            </v-col>
          </v-row>
          <v-row>
            <v-col v-for="(jlist, jid) in jobProjectionTable" :key="jid">
              <job-projection-category
                :job-status="jid"
                :job-list="jlist"
                height="680px"
                show-header
                @add-job="addJob"
                @del-job="delJob"
                @update-job-status="updateJobStatus"
              />
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-container>

    <v-container v-show="manPowerVisible" fluid class="my-0 py-0 mx-4 px-0 fill-height">
      <v-row>
        <v-col cols="12">
          <v-row>
            <v-col class="text-left mt-0 pt-0 mb-n4 pb-n4" cols="1">
              <main-menu
                @show-job-projection-overview="showJobProjectionOverview"
                @show-job-schedule="showJobSchedule"
                @show-man-power="showManPower"
                @show-man-power-forecast="showManPowerForecast"
                @open-team-dialog="openTeamDialog"
                @edit-options="editOptions"
                @save="save"
              />
            </v-col>
            <v-col class="mt-0 pt-0 mb-n4 pb-n4 " cols="6">
              <div class="text-h6" style="text-align: bottomz; display: inline">
                Man Power Planning
              </div>
            </v-col>
            <v-col class="text-right mt-0 pt-0 mb-n4 pb-n4" cols="2">
              <div
                id="flatpickr"
                class="rounded text-body-2"
                style="padding: 3px; box-shadow: 1px 3px lightgrey;"
              >
                <flat-pickr
                  v-model="selectedDate"
                  :config="dateConfig"
                  class="calendar"
                  placeholder="Select date"
                  name="date"
                  @on-change="gotoDate"
                />
              </div>
            </v-col>
            <v-col class="text-center mt-0 pt-0 mb-n4 pb-n4" cols="3">
              <v-btn small @click="previousWeek">
                Previous week
              </v-btn>
              &nbsp; &nbsp;
              <v-btn small @click="nextWeek">
                Next week
              </v-btn>
              &nbsp; &nbsp;
              <v-btn small @click="save">
                Save
              </v-btn>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <div style="height: 780px; overflow-y: scroll;">
                <table id="manpower" class="my-0 py-0 job-sched">
                  <colgroup>
                    <col style="width:4%;">
                    <col style="width:7%;">
                    <col :span="teams.length" :style="colWidthMp()">
                  </colgroup>
                  <thead>
                    <tr>
                      <th
                        colspan="3"
                        class="font-weight-bold"
                        style="font-size: 1.2em; background-color: rgb(48,84,150); color:white"
                      >
                        Manpower Planning Guide
                      </th>
                      <th
                        :colspan="teams.length-1"
                        class="font-weight-bold"
                        style="font-size: 1.2em; background-color: rgb(48,84,150); color:white"
                      />
                    </tr>
                  </thead>
                  <tbody
                    v-for="(day, index) in jobSchedule"
                    :key="index"
                    @contextmenu="openManpowerContextMenu($event, index, day)"
                  >
                    <tr>
                      <td
                        class="font-weight-bold date"
                        align="center"
                        rowspan="4"
                        :style="dayStyle(day.date)"
                        @click="selectDay($event, index, day)"
                      >
                        <div style="text-align: center; writing-mode: vertical-lr; transform: rotate(180deg);">
                          {{ dayOfWeek(day.date) }} {{ monthAndDate(day.date) }}
                        </div>
                      </td>
                      <td class="text-caption text-center font-weight-bold foreman">
                        Manpower: {{ day.manPowerTotals(teams).manPower }}
                      </td>
                      <td
                        v-for="(team, index) in teams"
                        :key="team.id"
                        class="text-caption text-center font-weight-bold foreman draggable"
                        draggable="true"
                        data-source="team"
                        style="background-color: rgb(180,198,231)"
                        @drop="dropTeam($event, index, team)"
                        @dragover="dragoverHandler($event)"
                        @dragstart="teamDragStart($event, index, team)"
                        @click="editTeam(team, day.jobs[team.id])"
                      >
                        {{ team.foreman }}
                      </td>
                    </tr>
                    <tr>
                      <td class="text-caption text-center font-weight-bold foreman">
                        On leave: {{ day.manPowerTotals(teams).onLeave }}
                      </td>
                      <td
                        v-for="team in teams"
                        :key="team.id"
                        class="text-caption text-center font-weight-bold"
                        @click="editTeam(team, day.jobs[team.id])"
                      >
                        {{ team.vehicle }}
                      </td>
                    </tr>
                    <tr>
                      <td class="text-caption text-center font-weight-bold foreman">
                        Teams
                        <v-btn x-small @click="copyPreviousDay(day, 'teams')">
                          Copy from yesterday
                        </v-btn>
                        <v-btn x-small @click="copyPreviousWeek(day, 'teams')">
                          Copy from last week
                        </v-btn>
                      </td>
                      <td
                        v-for="team in teams"
                        :key="team.id"
                        class="text-caption font-weight-bold"
                        @click="openTeamMemberDialog(team, day.jobs[team.id])"
                        @drop="dropTeamMember($event, day.jobs[team.id])"
                      >
                        <div
                          v-for="teamMember in getTeamMembers(day, team.id)"
                          :key="teamMember"
                          style="text-align: center;"
                          class="draggable"
                          draggable="true"
                          data-source="team-member"
                          :data-day-index="index"
                          :data-team="team.id"
                          :data-team-member="teamMember"
                          @dragover="dragoverHandler($event)"
                          @dragstart="teamMemberDragstart"
                        >
                          {{ teamMember }}
                        </div>
                        <div
                          v-for="idx in getBlankTeamSize(day, team.id)"
                          :key="idx"
                          @dragover="dragoverHandler($event)"
                        >
                              &nbsp;
                        </div>
                      </td>
                    </tr>
                    <tr>
                      <td class="text-caption text-center font-weight-bold foreman">
                        Day notes
                        <v-btn x-small @click="copyPreviousDay(day, 'dayNotes')">
                          Copy from yesterday
                        </v-btn>
                        <v-btn x-small @click="copyPreviousWeek(day, 'dayNotes')">
                          Copy from last week
                        </v-btn>
                      </td>
                      <td
                        v-for="team in teams"
                        :key="team.id"
                        class="text-left"
                        :style="jobScheduleStyle(day.jobs[team.id])"
                      >
                        <div v-if="showSchedJob(day.jobs[team.id])">
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
                            <label class="font-weight-bold">
                              {{ schedJobLabel(day.jobs[team.id]) }}
                            </label>
                            <br>
                            {{ day.jobs[team.id].getJob1() }}
                          </div>
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

    <v-dialog v-model="teamMemberDialog.open" max-width="680px">
      <v-card>
        <v-card-title>
          <span class="mx-2 headline">Edit team members</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row class="my-0 py-0">
              <v-col>
                Foreman: {{ teamMemberDialog.team.foreman }}
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-simple-table
                  id="team-memmbers"
                  class="simple-job-table"
                  fixed-header
                  height="440px"
                >
                  <colgroup>
                    <col style="width:8%;">
                    <col style="width:92%;">
                  </colgroup>
                  <thead>
                    <tr>
                      <th>
                        <v-btn icon @click="deleteJobTeamMember">
                          <v-icon>mdi-delete</v-icon>
                        </v-btn>
                      </th>
                      <th>Team members</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="teamMember in teamMemberDialog.teamMembers" :key="teamMember.id">
                      <td class="text-center">
                        <input v-model="teamMember.checked" type="checkbox">
                      </td>
                      <td>
                        <input
                          v-model="teamMember.teamMember"
                          class="text-caption text-left table-input"
                          type="text"
                          size="54"
                          @change="validateJobTeamMember(teamMember)"
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
          <v-spacer />
          <v-btn class="mb-2 pt-2" small @click="closeTeamMemberDialog">
            Cancel
          </v-btn>
          &nbsp; &nbsp;
          <v-btn class="mb-2 pt-2" small @click="saveTeamMembers">
            Save &amp; Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-container v-show="manPowerForecastVisible" fluid class="my-0 py-0 mx-4 px-0 fill-height">
      <v-row>
        <v-col cols="12">
          <v-row>
            <v-col class="text-left mt-0 pt-0 mb-n4 pb-n4" cols="1">
              <main-menu
                @show-job-projection-overview="showJobProjectionOverview"
                @show-job-schedule="showJobSchedule"
                @show-man-power="showManPower"
                @show-man-power-forecast="showManPowerForecast"
                @open-team-dialog="openTeamDialog"
                @edit-options="editOptions"
                @save="save"
              />
            </v-col>
            <v-col class="mt-0 pt-0 mb-n4 pb-n4" cols="6">
              <div class="text-h6" style="text-align: bottomz; display: inline">
                4 Day Schedule and Man Power Forecast
              </div>
            </v-col>
            <v-col class="text-right mt-0 pt-0 mb-n4 pb-n4 " cols="2">
              <div
                id="flatpickr"
                class="rounded text-body-2"
                style="padding: 3px; box-shadow: 1px 3px lightgrey;"
              >
                <flat-pickr
                  v-model="selectedDate"
                  :config="dateConfig"
                  class="calendar"
                  placeholder="Select date"
                  name="date"
                  @on-change="gotoDayWindow"
                />
              </div>
            </v-col>
            <v-col class="text-center mt-0 pt-0 mb-n4 pb-n4" cols="3">
              <v-btn small @click="prevDayWindow">
                Previous
              </v-btn>
              &nbsp; &nbsp;
              <v-btn small @click="nextDayWindow">
                Next
              </v-btn>
              &nbsp; &nbsp;
              <v-btn small @click="save">
                Save
              </v-btn>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <div style="height: 780px; overflow-y: scroll;">
                <table
                  id="manpower-forecast"
                  class="my-0 py-0 job-sched"
                >
                  <colgroup>
                    <col style="width:4%;">
                    <col style="width:7%;">
                    <col :span="teams.length" :style="colWidthMp()">
                  </colgroup>
                  <thead>
                    <tr>
                      <th
                        colspan="3"
                        class="font-weight-bold"
                        style="font-size: 1.2em; background-color: rgb(48,84,150); color:white"
                      >
                        Manpower Planning Guide
                      </th>
                      <th
                        :colspan="teams.length-1"
                        class="font-weight-bold"
                        style="font-size: 1.2em; background-color: rgb(48,84,150); color:white"
                      />
                    </tr>
                  </thead>
                  <tbody v-for="(day, index) in dayWindow.days" :key="index">
                    <tr>
                      <td class="font-weight-bold date" align="center" rowspan="4" :style="dayStyle(day.date)">
                        <div style="text-align: center; writing-mode: vertical-lr; transform: rotate(180deg);">
                          {{ dayOfWeek(day.date) }} {{ monthAndDate(day.date) }}
                        </div>
                      </td>
                      <td class="text-caption text-center font-weight-bold foreman">
                        Manpower: {{ day.manPowerTotals(teams).manPower }}
                      </td>
                      <td
                        v-for="team in teams"
                        :key="team.id"
                        class="text-caption text-center font-weight-bold foreman"
                        style="background-color: rgb(180,198,231)"
                      >
                        {{ team.foreman }}
                      </td>
                    </tr>
                    <tr>
                      <td class="text-caption text-center font-weight-bold foreman">
                        On leave: {{ day.manPowerTotals(teams).onLeave }}
                      </td>
                      <td
                        v-for="team in teams"
                        :key="team.id"
                        class="text-caption text-center font-weight-bold"
                      >
                        {{ team.vehicle }}
                      </td>
                    </tr>
                    <tr>
                      <td class="text-caption text-center font-weight-bold foreman">
                        Teams
                      </td>
                      <td v-for="team in teams" :key="team.id" class="text-caption font-weight-bold">
                        <div
                          v-for="teamMember in getTeamMembers(day, team.id)"
                          :key="teamMember"
                          style="text-align: center;"
                        >
                          {{ teamMember }}
                        </div>
                      </td>
                    </tr>
                    <tr>
                      <td class="text-caption text-center font-weight-bold foreman">
                        Day notes
                      </td>
                      <td
                        v-for="team in teams"
                        :key="team.id"
                        class="text-left"
                        :style="jobScheduleStyle(getSchedJob(day,team.id))"
                      >
                        <div v-if="showSchedJob(day.jobs[team.id])">
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
                            <label class="font-weight-bold">
                              {{ schedJobLabel(day.jobs[team.id]) }}
                            </label>
                            <br>
                            {{ day.jobs[team.id].getJob1() }}
                          </div>
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

    <v-dialog v-model="teamDialog.open" max-width="680px">
      <v-card v-show="teamDialog.showList">
        <v-card-title>
          <span class="mx-2 headline">Teams</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row class="my-0 py-0">
              <v-col align="right">
                <v-btn class="mt-0 pt-0 mb-n4 pb-n4" small @click="newTeam">
                  New Team
                </v-btn>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-simple-table
                  id="teams"
                  class="simple-job-table"
                  fixed-header
                  height="440px"
                >
                  <colgroup>
                    <col style="width:10%;">
                    <col style="width:40%;">
                    <col style="width:40%;">
                    <col style="width:10%;">
                  </colgroup>
                  <thead>
                    <tr>
                      <th />
                      <th class="text-center">
                        Foreman
                      </th>
                      <th class="text-center">
                        Vehicle license
                      </th>
                      <th />
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="(team, index) in tdTeams" :key="index">
                      <td class="text-center">
                        {{ index+1 }}
                      </td>
                      <td class="text-center">
                        {{ team.foreman }}
                      </td>
                      <td class="text-center">
                        {{ team.vehicle }}
                      </td>
                      <td class="text-center">
                        <v-icon dense @click="editTeam(team)">
                          mdi-pencil
                        </v-icon>
                      </td>
                    </tr>
                  </tbody>
                </v-simple-table>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn class="mb-2 pt-2" small @click="teamDialog.open=false">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
      <v-card v-show="teamDialog.showForm">
        <v-card-title>
          <span class="mx-2 headline">{{ teamDialog.title }}</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row class="my-0 py-0">
              <v-col>
                <v-text-field
                  v-model="teamDialog.editedTeam.foreman"
                  class="my-n2 py-0"
                  label="Foreman"
                  @blur="validateTeamForeman(teamDialog.editedTeam)"
                />
              </v-col>
            </v-row>
            <v-row class="my-0 py-0">
              <v-col>
                <v-text-field v-model="teamDialog.editedTeam.vehicle" class="my-n2 py-0" label="Vehicle license" />
              </v-col>
            </v-row>
            <v-row v-show="teamDialog.oper!=='new'" class="my-0 py-0">
              <v-col align="right">
                <v-btn class="mt-0 pt-0 mb-n4 pb-n4" small @click="deleteTeam">
                  Delete Team
                </v-btn>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-simple-table
                  id="team-memmbers"
                  class="simple-job-table"
                  fixed-header
                  height="440px"
                >
                  <colgroup>
                    <col style="width:8%;">
                    <col style="width:92%;">
                  </colgroup>
                  <thead>
                    <tr>
                      <th>
                        <v-btn icon @click="deleteTeamMember">
                          <v-icon>mdi-delete</v-icon>
                        </v-btn>
                      </th>
                      <th>Team members</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="teamMember in teamDialog.teamMembers" :key="teamMember.id">
                      <td class="text-center">
                        <input v-model="teamMember.checked" type="checkbox">
                      </td>
                      <td>
                        <input
                          v-model="teamMember.teamMember"
                          class="text-caption text-left table-input"
                          type="text"
                          size="54"
                          @change="validateTeamMember(teamMember)"
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
          <v-spacer />
          <v-btn class="mb-2 pt-2" small @click="closeTeam">
            Cancel
          </v-btn>
          &nbsp; &nbsp;
          <v-btn class="mb-2 pt-2" small @click="saveTeam">
            Save &amp; Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="showOptionsDialog" max-width="800px">
      <v-card>
        <v-card-title class="mb-0 pb-0">
          <span class="mx-2 headline">Options</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col class="my-0 py-0">
                <v-text-field
                  v-model="options.scheduleWeeks"
                  label="Number of weeks in schedule"
                  type="number"
                  min="1"
                  max="3"
                  :rules="numberOfWeeksRules"
                  @blur="checkScheduleWeeks"
                />
              </v-col>
            </v-row>
            <v-row>
              <v-col class="my-0 py-0">
                <v-text-field
                  v-model="options.autosaveInterval"
                  label="Autosave interval (seconds)"
                  type="number"
                  @blur="checkAutosaveInterval"
                />
              </v-col>
            </v-row>
            <v-row>
              <v-col class="my-0 py-0">
                <v-checkbox
                  v-model="options.showContextMenu"
                  label="Show context menu for copying rows"
                  hide-details
                />
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="saveOptions">
            Save as default (for all users)
          </v-btn>
          <v-spacer />
          <v-btn color="blue darken-1" text @click="showOptionsDialog=false">
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-menu
      v-model="showContextMenu"
      :position-x="xcoord"
      :position-y="ycoord"
      absolute
      offset-y
    >
      <v-list>
        <v-list-item>
          <v-list-item-title @click="copy">
            Copy
          </v-list-item-title>
        </v-list-item>
        <v-list-item>
          <v-list-item-title @click="paste">
            Paste
          </v-list-item-title>
        </v-list-item>
        <v-list-item>
          <v-list-item-title @click="deleteScheduleEntry">
            Delete entry
          </v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>

    <v-menu
      v-model="showManpowerContextMenu"
      :close-on-content-click="false"
      :nudge-width="200"
      :position-x="xcoord"
      :position-y="ycoord"
      absolute
      offset-y
    >
      <v-card>
        <v-card-title class="text-body-1">
          Copy schedule rows
        </v-card-title>

        <v-card-text>
          <v-radio-group v-model="manpowerContextMenu.mode">
            <v-radio
              label="Copy previous day"
              value="day"
            />
            <v-radio
              label="Copy previous Week"
              value="week"
            />
          </v-radio-group>
        </v-card-text>

        <v-card-actions>
          <v-spacer />
          <v-btn @click="showManpowerContextMenu=false">
            Cancel
          </v-btn>
          <v-btn @click="copyPreviousScheduleData">
            OK
          </v-btn>
          <v-spacer />
        </v-card-actions>
      </v-card>
    </v-menu>

    <v-dialog v-model="alert" max-width="600px">
      <v-card>
        <v-card-title class="text-body-1" />
        <v-card-text>
          <p class="header3" style="white-space: pre-wrap;">
            {{ userMessage }}
          </p>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn @click="closeMessage">
            OK
          </v-btn>
          <v-spacer />
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import flatPickr from 'vue-flatpickr-component'
import 'flatpickr/dist/flatpickr.css'
import 'flatpickr/dist/themes/material_blue.css'

import { Job, SchedJob, Team, Shift, Selection, Clipboard, JobStatus, Day, JobState }
  from '../../composables/jp_entity.js'
import { datediff, date2string, string2date, addDays, isSunday, isSaturday, isMonday, addDays2Date }
  from '../../composables/jp_util.js'

const priorityColors = [
  'tomato', 'tomato', 'orange', 'orange', 'yellow', 'yellow', 'lightgreen', 'lightgreen', 'limegreen', 'limegreen'
]

const dayColors = [
  'rgb(198,89,17)', 'rgb(180,198,231)', 'rgb(180,198,231)', 'rgb(180,198,231)', 'rgb(180,198,231)', 'rgb(180,198,231)', 'rgb(248,203,173)'
]

export default {
  name: 'JobPlanner',
  components: {
    flatPickr
  },
  data: () => ({
    version: 'v1.303',
    showContextMenu: false,
    showManpowerContextMenu: false,
    xcoord: 0,
    ycoord: 0,
    jobScheduleVisible: true,
    jobProjectionVisible: false,
    manPowerVisible: false,
    manPowerForecastVisible: false,
    title: 'Job Planner',
    userMessage: '',
    timeoutId: null,
    timeStamp: null,
    alert: false,
    jobs: [],
    dayWindow: { days: [], size: 4 },
    priorityColors,
    selection: null,
    copyData: null,
    jobScheduleCount: 0,
    firstUpdate: true,
    currentDate: '',
    selectedDate: '',
    dayIndex: {},
    deletedSchedJobs: [],
    schedJobGroups: {},
    teams_: [],
    jobInfo: new SchedJob(),
    editedJob: {},
    changeJobDialog: false,
    changeJobDialog2: false,
    jobDialog: { open: false, startDate: '', endDate: '' },
    jobTableDialog: { open: false, option: '', title: '', change: false },
    teamDialog: {
      open: false,
      showList: true,
      showForm: false,
      oper: '',
      title: '',
      maxTeamMembers: 8,
      teamMembers: [],
      team: {},
      editedTeam: {},
      schedJob: {}
    },
    teamMemberDialog: {
      open: false,
      maxTeamMembers: 8,
      team: {},
      teamMembers: [],
      schedJob: {}
    },
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
    manpowerContextMenu: {
      mode: 0,
      index: 0,
      day: {}
    },
    jobProjectionPanel: 0,
    refreshJobProjectionTable: 0,
    refreshConfirmedJobs: 0,
    refreshUpcomingJobs: 0,
    refreshTeamMembers: 0,
    refreshJobTeamMembers: 0,
    refreshTeams: 0,
    showOptionsDialog: false,
    options: {
      scheduleWeeks: 1,
      autosaveInterval: 120
    },
    numberOfWeeksRules: [
      v => !!v || 'Required',
      v => v >= 1 || 'Should be above 0',
      v => v <= 3 || 'Should not be above 3',
    ],
    notify: () => {}
  }),
  computed: {
    jobSchedule () {
      this.currentDate
      this.jobScheduleCount

      if (this.currentDate === '') {
        this.currentDate = date2string(new Date())
      }

      const date = string2date(this.currentDate)
      const dayOfWeek = date.getDay()
      const adjust = dayOfWeek == 0 ? -6 : 1 // add this to make monday the first day instead of sunday
      const firstDay = new Date(date.setDate(date.getDate() - dayOfWeek + adjust))
      const days = []
      if (this.options.scheduleWeeks < 1) {
        this.options.scheduleWeeks = 1
      }
      if (this.options.scheduleWeeks > 3) {
        this.options.scheduleWeeks = 3
      }
      for (let i = 0; i < this.options.scheduleWeeks * 7; i++) {
        const d = addDays(firstDay, i)
        const sday = date2string(d)
        // const dd = this.dayIndex[sday];
        const day = this.getDay(sday)
        days.push(day)
      }
      return days
    },
    confirmedJobs () {
      this.refreshConfirmedJobs
      const confirmedJobs = this.jobs.filter(job => job.id > -1 && job.state === JobState.confirmed)
      this.sort(confirmedJobs)
      for (let i = confirmedJobs.length; i < 25; i++) {
        const job = new Job({ id: 0, scope: '', value: '', status: JobStatus.blank, priority: '' })
        confirmedJobs.push(job)
      }
      return confirmedJobs
    },
    upcomingJobs () {
      this.refreshUpcomingJobs
      const upcomingJobs = this.jobs.filter(job => job.id > -1 && job.state === JobState.upcoming)
      this.sort(upcomingJobs)
      for (let i = upcomingJobs.length; i < 25; i++) {
        const job = new Job({ id: 0, scope: '', value: '', status: JobStatus.blank, priority: '' })
        upcomingJobs.push(job)
      }
      return upcomingJobs
    },
    editedJobTable () {
      return (this.jobTableDialog.option === JobState.confirmed) ? this.confirmedJobs : this.upcomingJobs
    },
    jobProjectionTable () {
      this.refreshJobProjectionTable
      const jpt = {
        inProgress: {
          title: 'Confirmed jobs/in progress',
          jobs: this.jobs
            .filter(job => job.id > -1 && job.status === JobStatus.inProgress)
          // .sort( (a, b) => a.id - b.id )
        },
        onHold: {
          title: 'Confirmed but on hold jobs',
          jobs: this.jobs
            .filter(job => job.id > -1 && job.status === JobStatus.onHold)
          // .sort( (a, b) => a.id - b.id )
        },
        highChance: {
          title: 'High chance jobs',
          jobs: this.jobs
            .filter(job => job.id > -1 && job.status === JobStatus.highChance)
          // .sort( (a, b) => a.id - b.id )
        },
        standBy: {
          title: 'Stand-by jobs',
          jobs: this.jobs
            .filter(job => job.id > -1 && job.status === JobStatus.standBy)
          // .sort( (a, b) => a.id - b.id )
        }
      }
      return jpt
    },
    jobShiftIcon () {
      return this.jobInfo.splitShift ? 'mdi-dns-outline' : 'check-box-outline-blank'
    },
    teams () {
      this.refreshTeams
      const teams = this.teams_
        .filter(team => team.position > -1)
        .sort((t1, t2) => t1.position - t2.position)
        .map((team, index) => {
          team.position = team.position < 100 ? index + 1 : team.position
          return team
        })
      return teams
    },
    jpTeams () {
      return this.teams.filter(team => team.foreman.toLowerCase() !== 'on leave')
    },
    tdTeams () {
      return this.teams.filter(team => team.position < 100)
    }
  },
  mounted () {
    const self = this
    document.body.addEventListener('keydown', function (e) {
      e = e || window.event
      // var key = e.which || e.keyCode; // keyCode detection
      const key = e.which || e.code // keyCode detection
      const ctrl = e.ctrlKey ? e.ctrlKey : ((key === 17)) // ctrl detection

      if (key == 86 && ctrl) {
        // console.log("Ctrl + V Pressed !");
        self.paste()
      } else if (key == 67 && ctrl) {
        // console.log("Ctrl + C Pressed !");
        self.copy()
      }
    }, false)
    this.initialize()
  },
  updated () {
    if (this.firstUpdate) {
      this.firstUpdate = false
      this.selectedDate = date2string(new Date())
      this.currentDate = this.selectedDate
      this.initDayWindow()
    }
    this.notify()
  },
  methods: {
    openContextMenu (e) {
      e.preventDefault()
      this.showContextMenu = false
      this.xcoord = e.clientX
      this.ycoord = e.clientY
      this.showManpowerContextMenu = false
      this.$nextTick(() => {
        this.showContextMenu = true
      })
    },
    openManpowerContextMenu (e, index, day) {
      e.preventDefault()

      if (!this.options.showContextMenu) {
        return
      }

      this.showContextMenu = false
      this.manpowerContextMenu.mode = ''

      this.showManpowerContextMenu = false
      this.xcoord = e.clientX
      this.ycoord = e.clientY

      this.manpowerContextMenu.index = index
      this.manpowerContextMenu.day = day

      this.$nextTick(() => {
        this.showManpowerContextMenu = true
      })
    },
    checkScheduleWeeks () {
      if (this.options.scheduleWeeks < 1) {
        this.options.scheduleWeeks = 1
      } else if (this.options.scheduleWeeks > 3) {
        this.options.scheduleWeeks = 3
      }
    },
    checkAutosaveInterval () {
      if (this.options.autosaveInterval < 20) {
        this.options.autosaveInterval = 20
      } else if (this.options.autosaveInterval > 600) {
        this.options.autosaveInterval = 600
      }
    },
    updateTeamCount () {
      this.$forceUpdate()
    },
    showSchedJob (schedJob) {
      if (!schedJob) {
        return false
      }
      return !((schedJob.excludeSaturday && isSaturday(schedJob.date)) ||
            (schedJob.excludeSunday && isSunday(schedJob.date)))
    },
    getDay (sday) {
      let day = this.dayIndex[sday]
      if (!day) {
        day = new Day({ date: sday, jobs: {} })
        this.dayIndex[sday] = day
      }
      this.teams.forEach((team) => {
        if (!day.jobs[team.id]) {
          day.jobs[team.id] = new SchedJob({
            id: 0,
            team: team.id,
            date: sday,
            shift: Shift.unasigned,
            job1: '',
            job2: '',
            teamMembers: []
          })
        }
      })
      return day
    },
    initialize () {
      this.getSchedData()
    },
    endpoint (op) {
      if (!op) {
        op = ''
      }
      // return `http://localhost:5000/api/jobschedule${op}`
      return `/jobschedule${op}`
    },
    getSchedData () {
      this.$axios({
        url: this.endpoint(),
        method: 'GET',
        crossDomain: true
      }).then((response) => {
        this.initJobSchedule(response.data, true)
      })
    },
    initJobs (jobs) {
      jobs.forEach((ajob) => {
        const job = new Job(ajob)
        this.jobs.push(job)
      })
    },
    updateTeams (teams) {
      this.teams_ = this.teams_.filter(team => team.position > -1)
      teams.forEach((team) => {
        this.teams_.push(new Team(team))
      })
      this.teams_.sort((t1, t2) => t1.position - t2.position)
      this.refreshTeams++
      this.$forceUpdate()
    },
    updateSchedJob (schedJob) {
      let sjGroup = this.schedJobGroups[schedJob.id]
      if (!sjGroup) {
        sjGroup = {}
        this.schedJobGroups[schedJob.id] = sjGroup
      }

      const date = schedJob.date
      let day = this.dayIndex[date]
      if (!day) {
        day = new Day({ id: 0, date, jobs: {} })
        this.dayIndex[date] = day
      }

      const sj = new SchedJob(schedJob)
      day.jobs[schedJob.team] = sj
      sjGroup[date] = sj
    },
    initJobSchedule (scheduleData, updateOtions) {
      this.dayIndex = {}
      this.deletedSchedJobs = []
      this.schedJobGroups = {}
      this.jobs = []
      this.teams_ = []

      SchedJob.groupIndex = this.schedJobGroups
      this.updateTeams(scheduleData.teams)
      this.initJobs(scheduleData.jobs)
      const schedJobs = scheduleData.schedJobs
      schedJobs.forEach(schedJob => this.updateSchedJob(schedJob))

      this.lastUpdate = scheduleData.lastUpdate

      if (updateOtions) {
        console.log('APIVersion', scheduleData.apiVersion)
        this.options = scheduleData.options
        this.currentDate = date2string(new Date())
      }

      if (this.options.autosaveInterval < 20) {
        this.options.autosaveInterval = 20
      }

      if (this.options.autosaveInterval) {
        setTimeout(this.save, this.options.autosaveInterval * 1000)
      }
    },
    getTeamMembers (day, teamId) {
      const sj = day.jobs[teamId]
      return this.showSchedJob(sj) ? sj.teamMembers : []
    },
    getBlankTeamSize (day, teamId) {
      const sj = day.jobs[teamId]
      const sjlen = sj ? sj.teamMembers.length : 0
      let max = 0
      for (const job of Object.values(day.jobs)) {
        if (job.teamMembers.length > max) {
          max = job.teamMembers.length
        }
      }
      const len = max === 0 ? 1 : max - sjlen
      return len
    },
    getSchedJob (day, teamId) {
      const sj = day.jobs[teamId]
      return sj
    },
    getDays () {
      const days = Object.values(this.dayIndex)
      return days.sort((a, b) => a.date - b.date)
    },
    initDayWindow () {
      this.gotoDayWindow([], this.currentDate)
    },
    gotoDayWindow (selectedDates, dateStr) {
      const days = this.getDays()
      if (days.length == 0) {
        return
      }
      const dayWindow = this.dayWindow
      this.currentDate = dateStr
      const startDate = string2date(dateStr)
      for (let i = 0; i < dayWindow.size; i++) {
        const date = addDays(startDate, i)
        dateStr = date2string(date)
        const day = this.getDay(dateStr)
        dayWindow.days[i] = day
      }
      this.$forceUpdate()
    },
    nextDayWindow () {
      const date = addDays(string2date(this.currentDate), this.dayWindow.size)
      this.gotoDayWindow([], date2string(date))
    },
    prevDayWindow () {
      const date = addDays(string2date(this.currentDate), -(this.dayWindow.size))
      this.gotoDayWindow([], date2string(date))
    },
    showJobSchedule () {
      this.refreshJobProjectionTable++
      this.manPowerVisible = false
      this.manPowerForecastVisible = false
      this.jobProjectionVisible = false
      this.jobScheduleVisible = true
    },
    showJobProjectionOverview () {
      this.manPowerVisible = false
      this.manPowerForecastVisible = false
      this.jobScheduleVisible = false
      this.jobProjectionVisible = true
    },
    showManPower () {
      this.jobScheduleVisible = false
      this.manPowerForecastVisible = false
      this.jobProjectionVisible = false
      this.manPowerVisible = true
    },
    showManPowerForecast () {
      this.jobScheduleVisible = false
      this.manPowerVisible = false
      this.jobProjectionVisible = false
      this.manPowerForecastVisible = true
    },
    gotoDate (selectedDates, dateStr) {
      this.unselect()
      this.currentDate = dateStr
    },
    previousWeek () {
      const date = string2date(this.currentDate)
      // const currDay = date.getDay();
      const nextMonday = addDays(date, -7)
      this.unselect()
      this.currentDate = date2string(nextMonday)
      this.selectedDate = this.currentDate
    },
    nextWeek () {
      const date = string2date(this.currentDate)
      // const currDay = date.getDay();
      const nextMonday = addDays(date, 7)
      this.unselect()
      this.currentDate = date2string(nextMonday)
      this.selectedDate = this.currentDate
    },
    dayOfWeek (date) {
      const str = new Intl.DateTimeFormat('en-US', { weekday: 'long', month: 'long', day: 'numeric' }).format(string2date(date))
      return str.substring(0, str.indexOf(',') + 1)
    },
    dayStyle (strDate) {
      const date = string2date(strDate)
      return `vertical-align: middle; background-color: ${dayColors[date.getDay()]};`
    },
    dayColor (strDate) {
      const border = strDate === date2string(new Date()) ? 'border: 3px solid blue;' : ''
      return `${border}background-color: ${dayColors[string2date(strDate).getDay()]};`
    },
    monthAndDate (strDate) {
      const date = string2date(strDate)
      const str = new Intl.DateTimeFormat('en-US', { weekday: 'long', month: 'long', day: 'numeric' }).format(date)
      return str.substring(str.indexOf(',') + 1).trim()
    },
    isBlank (job) {
      return (!job.id && !job.scope && !job.salesPerson && !job.priority && !job.value)
    },
    getSchedJobs (includeDeleted) {
      const schedJobs = {}
      for (const date of Object.keys(this.dayIndex)) {
        const day = this.dayIndex[date]
        for (const team of Object.keys(day.jobs)) {
          const sj = day.jobs[team]
          if (sj.id != 0) {
            schedJobs[sj.getPk()] = sj
          }
        }
      }
      if (includeDeleted) {
        for (const sj of this.deletedSchedJobs) {
          sj.id = -1
          schedJobs[sj.getPk()] = sj
        }
      }
      return Object.values(schedJobs)
    },
    save () {
      // console.log("Saving data");
      const schedJobs = this.getSchedJobs(true)

      const isBlank = job => (!job.id && !job.scope && !job.salesPerson && !job.priority && !job.value)

      let jobs = this.jobs
      jobs = jobs.filter(job => !isBlank(job))
      jobs.forEach(job => job.priority === '' ? 0 : job.priority)

      const data = { lastUpdate: this.lastUpdate, jobs, schedJobs, teams: this.teams_ }

      const self = this
      this.$axios({
        url: this.endpoint(),
        method: 'PUT',
        data,
        crossDomain: true
      })
        .then(function (response) {
          self.initJobSchedule(response.data, false)
          self.initDayWindow()
        })
        .catch(function (error) {
          console.log(error)
        })
    },
    saveOptions () {
      const data = this.options
      const self = this
      this.$axios({
        url: this.endpoint('/jobplanner-options'),
        method: 'PUT',
        data,
        crossDomain: true
      })
        .then(function (response) {
          self.showOptionsDialog = false
        })
        .catch(function (error) {
          console.log(error)
        })
    },
    copy () {
      if (!this.selection) {
        return
      }
      if (this.selection.col === 0) {
        // A whole row was selected by clicking on the date column
        // In this case selection.startRow is the row we want to select
        const date = this.jobSchedule[this.selection.startRow].date
        this.copyData = new Clipboard(-1, date, date)
      } else {
        // A scheduled job was selected
        // In this case we want to select selection.startRow - 1
        this.copyData = new Clipboard(this.teams[this.selection.col - 1].id,
          this.jobSchedule[this.selection.startRow - 1].date,
          this.selection.endRow - this.selection.startRow + 1)
      }
    },
    paste () {
      if (!this.copyData || !this.selection) {
        return
      }

      // console.log("copyData.team", this.copyData.team)
      if (this.copyData.team === -1) {
        this.pasteRow()
        return
      }

      const team = this.teams[this.selection.col - 1].id
      const foreman = this.teams[this.selection.col - 1].foreman

      if (foreman == 'On leave') {
        return
      }

      const source = {
        startDate: this.copyData.startDate,
        numRows: this.copyData.numRows,
        team: this.copyData.team
      }
      const target = {
        startDate: this.jobSchedule[this.selection.startRow - 1].date,
        numRows: -1,
        team
      }

      this.copySchedJobs(source, target, false, true)

      this.unselect()
      this.$forceUpdate()
    },
    copyPreviousScheduleData () {
      console.log('mode', this.manpowerContextMenu.mode)
      if (this.manpowerContextMenu.mode == 'day') {
        this.copyPreviousDay(this.manpowerContextMenu.day, 'all')
      } else if (this.manpowerContextMenu.mode == 'week') {
        this.copyPreviousWeek(this.manpowerContextMenu.day, 'all')
      }
    },
    copyPreviousWeek (day, whatToCopy) {
      // const strDate = this.manpowerContextMenu.day.date;
      const strDate = day.date
      const date = string2date(strDate)
      let dayOfWeek = date.getDay()
      let adjust = dayOfWeek == 0 ? -6 : 1 // add this to make monday the first day instead of sunday
      const firstDayThisWeek = new Date(date.setDate(date.getDate() - dayOfWeek + adjust))

      const sameDayLastWeek = addDays(date, -7)
      dayOfWeek = sameDayLastWeek.getDay()
      adjust = sameDayLastWeek == 0 ? -6 : 1 // add this to make monday the first day instead of sunday
      const firstDayLastWeek = new Date(sameDayLastWeek.setDate(sameDayLastWeek.getDate() - dayOfWeek + adjust))

      // const teams = Object.keys(this.manpowerContextMenu.day.jobs);
      const teams = Object.keys(day.jobs)
      for (let i = 0; i < 7; i++) {
        this.copyRow(date2string(addDays(firstDayLastWeek, i)),
          date2string(addDays(firstDayThisWeek, i)), teams, 1, whatToCopy)
      }

      this.unselect()
      this.jobScheduleCount++
      this.$forceUpdate()

      this.showManpowerContextMenu = false
    },
    copyPreviousDay (day, whatToCopy) {
      // const date = this.manpowerContextMenu.day.date;
      const date = day.date
      let previousDay
      if (isMonday(date)) {
        previousDay = addDays2Date(date, -3)
      } else if (isSunday(date)) {
        previousDay = addDays2Date(date, -2)
      } else {
        previousDay = addDays2Date(date, -1)
      }

      // const teams = Object.keys(this.manpowerContextMenu.day.jobs);
      const teams = Object.keys(day.jobs)
      this.copyRow(previousDay, date, teams, 1, whatToCopy)

      this.unselect()
      this.jobScheduleCount++
      this.$forceUpdate()

      this.showManpowerContextMenu = false
    },
    copyRow (fromDate, toDate, teams, numRows, whatToCopy) {
      for (let i = 0; i < teams.length; i++) {
        const source = {
          startDate: fromDate,
          numRows,
          team: teams[i]
        }
        const target = {
          startDate: toDate,
          numRows: 1,
          team: teams[i]
        }
        if (!whatToCopy || whatToCopy === 'all') {
          this.copySchedJobs(source, target, false)
        } else {
          this.copySchedJob(source, target, whatToCopy)
        }
      }
    },
    pasteRow () {
      if (this.selection.col !== 0) {
        return
      }

      const days = this.getDays()
      const lastDay = days[days.length - 1]

      const teams = Object.keys(lastDay.jobs)
      // When copying whole row selection.startRow has the index of the row to copy
      // no need to subtract 1
      const targetDate = this.jobSchedule[this.selection.startRow].date
      for (let i = 0; i < teams.length; i++) {
        const source = {
          startDate: this.copyData.startDate,
          numRows: 1,
          team: teams[i]
        }
        const target = {
          startDate: targetDate,
          numRows: 1,
          team: teams[i]
        }
        this.copySchedJobs(source, target, false)
      }

      this.unselect()
      this.jobScheduleCount++
      this.$forceUpdate()
    },
    scheduledJobDragstart (event, dayIndex, teamIndex) {
      const source = event.target.getAttribute('data-source')
      const data = { source, dayIndex, teamIndex }
      event.dataTransfer.setData('application/json', JSON.stringify(data))
    },
    dropJob (srcData, targetIndex, teamIndex) {
      const teamId = this.teams[teamIndex].id
      const foreman = this.teams[teamIndex].foreman

      if (foreman === 'On leave') {
        return
      }

      const date = this.jobSchedule[targetIndex].date

      const job = this.jobs.find(item => item.id == parseInt(srcData.jobId))
      const currSchedJob = this.jobSchedule[targetIndex].jobs[teamId]

      if (currSchedJob.shift == Shift.unasigned) {
        const shift = foreman === 'Store' ? Shift.unasigned : Shift.day
        const schedJob = new SchedJob({
          id: 0,
          team: teamId,
          date,
          shift,
          job1: job.scope,
          job2: job.scope,
          lastUpdate: null
        })
        this.copyOneToOne(schedJob, -1, date, teamId, true)
        const currentDate = date2string(new Date())
        if (datediff(currentDate, date) >= 0) {
          job.status = foreman === 'Store' ? JobStatus.onHold : JobStatus.inProgress
          this.refreshJobProjectionTable++
        }
      }

      this.$forceUpdate()
    },
    dropHandler (event, dayIndex, teamIndex) {
      event.preventDefault()
      const srcData = JSON.parse(event.dataTransfer.getData('application/json'))

      if (srcData.source === 'confirmed-job' || srcData.source === 'upcoming-job') {
        this.dropJob(srcData, dayIndex, teamIndex)
        return
      }

      this.dropScheduledJob(srcData, dayIndex, teamIndex)
    },
    dropScheduledJob (srcData, targetIndex, teamIndex) {
      const teamId = this.teams[teamIndex].id
      const foreman = this.teams[teamIndex].foreman

      if (foreman === 'On leave') {
        return
      }

      const sourceIndex = srcData.dayIndex

      const srcDate = this.jobSchedule[sourceIndex].date
      const targetDate = this.jobSchedule[targetIndex].date

      if ((srcData.teamIndex === teamIndex) && (srcDate === targetDate)) {
        // source and target are the same cell
        return
      }

      let sourceData, targetData
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
          startDate: this.jobSchedule[this.selection.startRow - 1].date,
          numRows: this.selection.endRow - this.selection.startRow + 1,
          team: this.teams[srcData.teamIndex].id
        }
        targetData = {
          startDate: targetDate,
          numRows: -1,
          team: teamId
        }
      }

      this.copySchedJobs(sourceData, targetData, true, true)

      this.jobScheduleCount++
      this.unselect()
    },
    teamDragStart (event, index, team) {
      const source = event.target.getAttribute('data-source')
      const data = { source, teamIndex: index, team }
      event.dataTransfer.setData('application/json', JSON.stringify(data))
    },
    dropTeam (event, index, team) {
      const srcData = JSON.parse(event.dataTransfer.getData('application/json'))
      if (srcData.source !== 'team') {
        return
      }
      const srcTeam = this.teams[srcData.teamIndex]
      if (srcTeam.position >= 100) {
        return
      }
      const tgtTeam = this.teams[index]
      srcTeam.position = tgtTeam.position < 100 ? tgtTeam.position + 0.5 : 99
      // tgtTeam.position += tgtTeam.position < 100 ? 0.5 : 0;
      this.refreshTeams++
      this.jobScheduleCount++
    },
    sameJob (startDate, numRows, team) {
      if (numRows <= 1) {
        return true
      }
      const d = string2date(startDate)
      for (let i = 1; i < numRows; i++) {
        const d1 = date2string(addDays(d, i))
        const d2 = date2string(addDays(d, i - 1))
        if (this.dayIndex[d1].jobs[team].id !== this.dayIndex[d2].jobs[team].id) {
          return false
        }
      }
      return true
    },
    nextGroupId () {
      const ids = Object.keys(this.schedJobGroups).sort((id1, id2) => id1 - id2)
      return ids.length == 0 ? 1 : parseInt(ids[ids.length - 1]) + 1
    },
    nextGroupElement (dates, group, from) {
      for (let i = from; i < dates.length; i++) {
        const date = dates[i]
        const sj = group[date]
        if (sj) {
          return i
        }
      }
      return -1
    },
    updateSchedJobGroup (groupId) {
      let id = groupId
      const group = this.schedJobGroups[id]
      const len = Object.entries(group).length
      const dates = Object.keys(group).sort((d1, d2) => d1 < d2 ? -1 : (d1 === d2 ? 0 : 1))
      let newGroup = {}
      let i = this.nextGroupElement(dates, group, 0)
      for (; i < len; i++) {
        const date = dates[i]
        const sj = group[date]
        if (!sj) {
          break
        }
        sj.id = id
        newGroup[date] = sj
      }
      this.schedJobGroups[id] = newGroup

      // check for a second group
      id = this.nextGroupId()
      newGroup = {}
      i = this.nextGroupElement(dates, group, i)

      if (i > -1) {
        for (; i < len; i++) {
          const date = dates[i]
          const sj = group[date]
          if (!sj) {
            break
          }
          sj.id = id
          newGroup[date] = sj
        }
        this.schedJobGroups[id] = newGroup
      }
    },
    copySchedJob (source, target, whatToCopy) {
      const sourceSchedJob = this.dayIndex[source.startDate].jobs[source.team]
      // if (sourceSchedJob.isBlank()) {
      //   return;
      // }

      const d = target.startDate
      const day = this.dayIndex[d]

      let schedJob = this.dayIndex[d].jobs[source.team]

      if (schedJob.isBlank()) {
        schedJob = this.copyOneToOne(schedJob, -1, d, source.team, false)
        schedJob.excludeSaturday = false
        schedJob.excludeSunday = false
      }

      if (whatToCopy === 'teams') {
        schedJob.teamMembers = sourceSchedJob.teamMembers.slice()
      } else {
        schedJob.job1 = sourceSchedJob.job1
        schedJob.job2 = sourceSchedJob.job2
        schedJob.shift = sourceSchedJob.shift
        schedJob.splitShift = sourceSchedJob.splitShift
      }

      schedJob.setLastUpdate()
    },
    copySchedJobs (source, target, deleteSource, clearFlags) {
      const sameJob = this.sameJob(source.startDate, source.numRows, source.team)

      if (sameJob) {
        const src = { startDate: source.startDate, numRows: source.numRows, team: source.team }
        const numRows = target.numRows == -1 ? source.numRows : target.numRows
        const trgt = { startDate: target.startDate, numRows, team: target.team }
        this.copyOneToMany(src, trgt, clearFlags)
      } else {
        for (let i = 0; i < source.numRows; i++) {
          const src = { startDate: addDays2Date(source.startDate, i), numRows: 1, team: source.team }
          const trgt = { startDate: addDays2Date(target.startDate, i), numRows: 1, team: target.team }
          this.copyOneToMany(src, trgt)
        }
      }

      if (deleteSource) {
        this.deleteSchedJobs(source)
      }
      // this.printGroups();
    },
    deleteSchedJobs (source) {
      const deleted = {}
      const startDate = string2date(source.startDate)
      for (let i = 0; i < source.numRows; i++) {
        const date = date2string(addDays(startDate, i))
        const sj = this.dayIndex[date].jobs[source.team]
        if (sj.isBlank()) {
          continue
        }
        // console.log("date", date, "sj", JSON.stringify(sj))
        const groupId = sj.id
        delete this.dayIndex[date].jobs[source.team]
        // console.log("schedJobGroups[groupId]", this.schedJobGroups[groupId]);
        this.schedJobGroups[groupId][date] = null
        sj.id = -1
        this.deletedSchedJobs.push(sj)
        deleted[groupId] = true
      }
      for (const groupId of Object.keys(deleted)) {
        this.updateSchedJobGroup(groupId)
      }
    },
    copyOneToMany (source, target, clearFlags) {
      const sourceSchedJob = this.dayIndex[source.startDate].jobs[source.team]
      if (sourceSchedJob.isBlank()) {
        const targetSchedJob = this.dayIndex[target.startDate].jobs[target.team]
        if (targetSchedJob && !targetSchedJob.isBlank()) {
          // console.log("targetSchedJob (not blank)", JSON.stringify(targetSchedJob));
          this.deleteSchedJobs(target)
        }
        return
      }

      const targetStartDate = string2date(target.startDate)

      const newGroup = {}
      const newId = this.nextGroupId()
      this.schedJobGroups[newId] = newGroup

      for (let i = 0; i < target.numRows; i++) {
        const d = date2string(addDays(targetStartDate, i))
        let day = this.dayIndex[d]
        if (!day) {
          day = new Day({ id: 0, date: d, jobs: {} })
          this.dayIndex[d] = day
        }

        const schedJob = new SchedJob(sourceSchedJob)
        schedJob.id = newId
        schedJob.date = d
        schedJob.team = target.team
        if (clearFlags) {
          schedJob.excludeSaturday = false
          schedJob.excludeSunday = false
        }
        schedJob.lastUpdate = null
        day.jobs[target.team] = schedJob

        newGroup[d] = schedJob

        const idx = this.teams.findIndex(t => t.id == target.team)
        if (source.team !== target.team) {
          schedJob.teamMembers = this.teams[idx].teamMembers.slice()
        }
      }
    },
    copyOneToOne (schedJob, groupId, date, team, assignTeamMembers) {
      let group
      if (groupId == -1) {
        group = {}
        groupId = this.nextGroupId()
        this.schedJobGroups[groupId] = group
      } else {
        group = this.schedJobGroups[groupId]
      }

      let day = this.dayIndex[date]
      if (!day) {
        day = new Day({ id: 0, date, jobs: {} })
        this.dayIndex[date] = day
      }

      day.jobs[team] = schedJob

      schedJob.id = groupId
      schedJob.date = date
      schedJob.team = team
      group[date] = schedJob

      if (assignTeamMembers) {
        const idx = this.teams.findIndex(t => t.id == team)
        schedJob.teamMembers = this.teams[idx].teamMembers.slice()
      }

      return schedJob
    },
    teamMemberDragstart (ev) {
      const data = {
        dayIndex: ev.target.getAttribute('data-day-index'),
        team: ev.target.getAttribute('data-team'),
        teamMember: ev.target.getAttribute('data-team-member')
      }
      ev.dataTransfer.setData('application/json', JSON.stringify(data))
    },
    dropTeamMember (event, targetJob) {
      event.preventDefault()
      const blankJob = targetJob.isBlank()
      const teamMembers = targetJob.teamMembers
      const srcData = event.dataTransfer.getData('application/json')
      const src = JSON.parse(srcData)
      const dayIndex = parseInt(src.dayIndex)
      const srcJob = this.jobSchedule[dayIndex].jobs[src.team]

      const srcTeamMembers = srcJob.teamMembers
      srcTeamMembers.splice(srcTeamMembers.indexOf(src.teamMember), 1)
      teamMembers.push(src.teamMember)

      if (blankJob) {
        this.copyOneToOne(new SchedJob(targetJob), -1, targetJob.date, targetJob.team, false)
      }

      srcJob.setLastUpdate()
      targetJob.setLastUpdate()
      this.$forceUpdate()
    },
    dragoverHandler (event) {
      event.preventDefault()
      event.dataTransfer.dropEffect = 'move'
    },
    colWidth () {
      const str = `width:${92 / this.teams.length}%;`
      return str
    },
    colWidthMp () {
      const str = `width:${90 / this.teams.length}%;`
      return str
    },
    findAncestor (el, tag) {
      for (; el.tagName != tag; el = el.parentElement) { ; }
      return el
    },
    unselect () {
      if (!this.selection) {
        return
      }
      const table = document.getElementById('job-sched')
      for (let i = this.selection.startRow; i <= this.selection.endRow; i++) {
        const cell = table.rows[i].cells[this.selection.col]
        while (cell.classList.contains('selected')) {
          cell.classList.remove('selected')
        }
      }
      this.selection = null
    },
    unselectDay (table) {
      for (let i = this.selection.startRow; i <= this.selection.endRow; i++) {
        const tbody = table.tBodies[i]
        while (tbody.classList.contains('selected')) {
          tbody.classList.remove('selected')
        }
      }
      this.selection = null
    },
    selectDay (event, index, day) {
      if (this.selection && this.selection.col !== 0) {
        this.unselect()
      }

      const tb = this.findAncestor(event.target, 'TBODY')
      const table = this.findAncestor(tb, 'TABLE')
      const tbodyIndex = [].slice.call(table.tBodies).indexOf(tb)

      // No previous selection
      if (!this.selection) {
        tb.classList.add('selected')
        this.selection = new Selection(0, tbodyIndex)
        return
      }

      // User clicked on the same day, unselect it
      if (this.selection.startRow === tbodyIndex) {
        this.unselectDay(table)
        return
      }

      this.unselectDay(table)
      tb.classList.add('selected')
      this.selection = new Selection(0, tbodyIndex)
    },
    onTableCellClick (event, job) {
      if (!this.timeoutId) {
        this.timeoutId = setTimeout(() => {
          this.onTableCellSingleClick(event, job)
          this.timeoutId = null
        }, 250)// tolerance in ms
      } else {
        // double click
        // open job dialog
        clearTimeout(this.timeoutId)
        this.timeoutId = null
        this.openSchedJobDialog(job)
      }
    },
    onTableCellSingleClick (event, job) {
      if (this.selection && this.selection.col === 0) {
        this.unselectDay(document.getElementById('manpower'))
      }

      if (job.shift == Shift.unasigned) {
        this.unselect()
      }

      const td = this.findAncestor(event.target, 'TD')
      const tr = this.findAncestor(td, 'TR')

      // No previous selection
      if (!this.selection) {
        td.classList.add('selected')
        this.selection = new Selection(td.cellIndex, tr.rowIndex)
        return
      }

      // Clicked on cell on another column
      if ((this.selection.col != td.cellIndex) || (tr.rowIndex < this.selection.startRow)) {
        // console.log("Clicked on cell on another column. Unselect all selections and select this cell");
        this.unselect()
        td.classList.add('selected')
        this.selection = new Selection(td.cellIndex, tr.rowIndex)
        return
      }

      // Clicked on a selected cell. Unselect it
      if ((tr.rowIndex >= this.selection.startRow && tr.rowIndex <= this.selection.endRow)) {
        // console.log("Clicked on a selected cell. Unselect all selections");
        this.unselect()
        return
      }

      // console.log("Clicked on cell on the same column, below current selection. Add this cell to selection");
      const table = document.getElementById('job-sched')
      for (let i = this.selection.endRow + 1; i <= tr.rowIndex; i++) {
        const cell = table.rows[i].cells[td.cellIndex]
        cell.classList.add('selected')
      }
      this.selection.endRow = tr.rowIndex
    },
    splitShift (jobInfo) {
      if (jobInfo) {
        return jobInfo.splitShift
      }
      return this.jobInfo.splitShift
    },
    schedJobInfo (date, team) {
      return date + ':' + team
    },
    schedJobLabel (jobInfo) {
      if (!jobInfo) {
        return ''
      }
      if (jobInfo.excludeSunday && isSunday(jobInfo.date)) {
        return ''
      }
      if (jobInfo.shift == Shift.unasigned) {
        return ''
      }
      if (jobInfo.shift === Shift.night) {
        return 'Night'
      }
      if (jobInfo.shift === Shift.day) {
        return 'Day'
      }
    },
    getForeman (teamId) {
      const idx = this.teams.findIndex(team => team.id === teamId)
      return idx > -1 ? this.teams[idx].foreman : ''
    },
    openSchedJobDialog (job) {
      this.editedJob = job
      this.jobInfo = new SchedJob(job)
      if (job.shift === Shift.unasigned) {
        this.jobInfo.shift = Shift.day
      }
      this.jobDialog.startDate = job.startDate
      this.jobDialog.endDate = job.endDate
      this.jobDialog.open = true
    },
    getDayIndex (date) {
      const jobSchedule = this.jobSchedule
      return jobSchedule.findIndex(day => day.date === date)
    },
    saveSchedJob () {
      if (!this.editedJob.isBlank() && this.editedJob.startDate === this.jobDialog.startDate &&
            this.editedJob.endDate === this.jobDialog.endDate) {
        this.editedJob.update(this.jobInfo)
        this.jobDialog.open = false
        return
      }

      const sourceData = {
        startDate: this.editedJob.date,
        numRows: 1,
        team: this.jobInfo.team
      }
      const targetData = {
        startDate: this.jobDialog.startDate,
        numRows: datediff(this.jobDialog.startDate, this.jobDialog.endDate) + 1,
        team: this.jobInfo.team
      }

      this.editedJob.update(this.jobInfo)
      if (this.editedJob.shift != Shift.unasigned && this.editedJob.job1) {
        const team = this.teams.find(team => team.id === this.jobInfo.team)
        this.editedJob.teamMembers = team ? team.teamMembers.slice() : []
      }
      const schedJob = this.editedJob

      this.copySchedJobs(sourceData, targetData, false)

      const team = this.editedJob.team
      if (this.editedJob.startDate < this.jobDialog.startDate) {
        const numRows = this.editedJob.endDate < this.jobDialog.startDate
          ? datediff(this.editedJob.startDate, this.editedJob.endDate)
          : datediff(this.editedJob.startDate, this.jobDialog.startDate)
        var source1 = {
          startDate: this.editedJob.startDate,
          numRows,
          team
        }
      }

      if (this.editedJob.endDate > this.jobDialog.endDate) {
        var source2 = {
          startDate: addDays2Date(this.jobDialog.endDate, 1),
          numRows: datediff(this.jobDialog.endDate, this.editedJob.endDate),
          team
        }
      }

      // this will be called after Vue has updated the page to delete the scheduled jobs
      // that need to be deleted
      this.notify = () => {
        this.notify = () => {}

        if (this.editedJob.startDate < this.jobDialog.startDate) {
          this.deleteSchedJobs(source1)
        }

        if (this.editedJob.endDate > this.jobDialog.endDate) {
          this.deleteSchedJobs(source2)
        }

        this.jobScheduleCount++
      }

      this.jobScheduleCount++
      this.$forceUpdate()
      this.jobDialog.open = false
    },
    deleteScheduleEntry () {
      if (!this.selection) {
        return
      }

      const team = this.teams[this.selection.col - 1].id
      const foreman = team.foreman

      if (foreman == 'On leave') {
        return
      }

      const date = this.jobSchedule[this.selection.startRow - 1].date
      // const schedJob = new SchedJob({id: 0, team: team.id,
      //         date: date, shift: Shift.unasigned,
      //         job1: "", job2: "", teamMembers : []});

      const source = {
        startDate: this.jobSchedule[this.selection.startRow - 1].date,
        numRows: this.selection.endRow - this.selection.startRow + 1,
        team
      }

      // this.copyOneToOne(schedJob, -1, date, team, false);

      this.notify = () => {
        this.notify = () => {}
        this.deleteSchedJobs(source)
        this.jobScheduleCount++
      }

      this.unselect()
      this.$forceUpdate()
    },
    flipJobs () {
      const temp = this.jobInfo.job1
      this.jobInfo.job1 = this.jobInfo.job2
      this.jobInfo.job2 = temp
      this.$forceUpdate()
    },
    closeSchedJob () {
      this.jobDialog.open = false
    },
    sort (jobTable) {
      jobTable.sort((a, b) => {
        const pa = a.priority ? a.priority : 1000000
        const pb = b.priority ? b.priority : 1000000
        return pa - pb
      })
    },
    jobScheduleStyle (jobInfo) {
      const shift = jobInfo.shift
      const fontSize = '0.7em'

      if ((jobInfo.excludeSunday && isSunday(jobInfo.date)) || (jobInfo.excludeSaturday && isSaturday(jobInfo.date))) {
        return `background-color:white;font-size:${fontSize};`
      }

      // if (jobInfo.id > 0) {
      //   const team = this.teams.find( team => team.id == jobInfo.team )
      //   if (team.foreman === "Store") {
      //     return `background-color:lightblue;font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`;
      //   }
      // }

      const job1 = jobInfo.job1.toLowerCase().trim()
      if (job1.includes('holiday')) {
        return `background-color:yellow;font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`
      } else if (job1.includes('on leave')) {
        return `background-color:rgb(68,114,196);color:white;font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`
      } else if (job1.includes('rest')) {
        return `background-color:rgb(255,230,153);font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`
      } else if (jobInfo.job1.includes('TBC')) {
        return `background-color:rgb(198,89,17);font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`
      }

      if (shift == Shift.day) {
        return `background-color:rgb(198,224,180);font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`
      } else if (shift == Shift.night) {
        return `background-image:linear-gradient(rgb(20,50,0),rgb(100,140,90));color:white;font-size:${fontSize};padding-bottom:5px;margin-bottom:0px;height:0px;`
      } else if (shift == Shift.unasigned) {
        return `background-color:white;font-size:${fontSize};`
      } else if (shift == Shift.tbc) {
        return `background-color:rgb(198,89,17);font-size:${fontSize};`
      }
    },
    jobProjectionShiftOptionColor (value) {
      if (value === '') {
        return 'color: grey;'
      }
      return 'color: black;'
    },
    background (priority) {
      if (!priority) {
        return 'background-color:white'
      }
      return `background-color:${priorityColors[priority - 1]};`
    },
    addJob (status) {
      this.jobs.push(new Job({ id: 0, scope: '', value: '', status, priority: 10 }))
      this.refreshJobProjectionTable++
    },
    updateJobStatus (jobId, status) {
      console.log('updateJobStatus', jobId, status)
      const idx = this.jobs.findIndex(job => job.id == jobId)
      if (idx > -1) {
        console.log('idx', idx)
        this.jobs[idx].status = status
        console.log('job', JSON.stringify(this.jobs[idx]))
        this.refreshJobProjectionTable++
        this.$forceUpdate()
      }
    },
    delJob (jobs) {
      for (const job of jobs) {
        if (job.id === 0) {
          const idx = this.jobs.findIndex(element => element == job)
          if (idx > -1) {
            this.jobs.splice(idx, 1)
          }
        } else {
          job.id = -job.id
          job.status = JobStatus.blank
        }
        this.refreshJobProjectionTable++
      }
    },
    addSchedJob () {
      if (this.jobInfo.shift === Shift.day || this.jobInfo.shift === Shift.unasigned) {
        this.jobInfo.shift = Shift.day
        this.jobInfo.splitShift = !this.jobInfo.splitShift
        // this.jobInfo.job2 = "";
        this.$forceUpdate()
      }
    },
    openTeamDialog () {
      this.teamDialog.open = true
    },
    teamMembers (team) {
      const members = team.teamMembers ? team.teamMembers : []
      const teamMembers = []

      for (let i = 0; i < members.length; i++) {
        teamMembers.push({ id: i + 1, checked: false, teamMember: members[i] })
      }

      for (let i = teamMembers.length; i < this.teamMemberDialog.maxTeamMembers; i++) {
        teamMembers.push({ id: i + 1, checked: false, teamMember: '' })
      }
      return teamMembers
    },
    editTeam (team, schedJob) {
      this.teamDialog.team = team
      this.teamDialog.editedTeam = new Team(team)
      this.teamDialog.schedJob = schedJob
      this.teamDialog.teamMembers = this.teamMembers(team)

      this.teamDialog.oper = 'edit'
      this.teamDialog.title = 'Edit team'
      this.teamDialog.showList = false
      this.teamDialog.showForm = true
    },
    newTeam () {
      this.teamDialog.team = new Team({ id: 0, foreman: '', vehicle: '', position: 0, teamMembers: [] })
      this.teamDialog.editedTeam = new Team({ id: 0, foreman: '', vehicle: '', position: 0, teamMembers: [] })

      this.teamDialog.oper = 'new'
      this.teamDialog.title = 'New team'

      this.teamDialog.showList = false
      this.teamDialog.showForm = true
    },
    copyTeamMembers (source, target) {
      target.length = 0
      for (let i = 0; i < source.length; i++) {
        const member = source[i].teamMember
        if (member.trim() !== '') {
          target.push(member)
        }
      }
    },
    checkTeamForeman (team) {
      if (team.foreman.trim() === '') {
        return 'empty'
      }
      for (const t of this.teams) {
        if (t.id != team.id && t.foreman === team.foreman) {
          return 'dup'
        }
      }
      return 'ok'
    },
    validateTeamForeman (team) {
      const status = this.checkTeamForeman(team)
      if (status === 'empty') {
        this.showMessage('You must enter the Foreman')
        return false
      } else if (status === 'dup') {
        this.showMessage(`${team.foreman} is in another team`)
        return false
      }
      return true
    },
    validateTeamMember (teamMember) {
      // verify that the new team member doesn't exist in any other team
      teamMember.teamMember = teamMember.teamMember.trim()
      for (const team of this.teams) {
        if (team.id != teamMember.id) {
          const members = team.teamMembers
          if (members.includes(teamMember.teamMember)) {
            this.showMessage(`${teamMember.teamMember} is in ${team.foreman}'s team`)
            teamMember.teamMember = ''
            break
          }
        }
      }
    },
    checkTeamPosition (team) {
      if (parseInt(team.position) < 0) {
        team.position = 0
      } else if (parseInt(team.position) > 99) {
        team.position = 99
      }
    },
    saveTeam () {
      const team = this.teamDialog.editedTeam
      if (!this.validateTeamForeman(team)) {
        return
      }
      this.checkTeamPosition(team)
      if (this.teamDialog.oper === 'new') {
        this.teams_.splice(team.position, 0, team)
        this.copyTeamMembers(this.teamDialog.teamMembers, team.teamMembers)
      } else {
        // team.position -= 0.5;
        this.teamDialog.team.update(team)
        this.copyTeamMembers(this.teamDialog.teamMembers, this.teamDialog.team.teamMembers)
      }
      // this.updateTeamOrder();
      this.refreshTeams++
      this.jobScheduleCount++
      this.teamDialog.showForm = false
      this.teamDialog.showList = true
    },
    deleteTeam () {
      // if (this.teamDialog.oper === "new") {
      //   return;
      // }
      if (this.teamDialog.editedTeam.position >= 100) {
        return
      }
      const teamId = this.teamDialog.editedTeam.id
      const team = this.teams_.find(t => t.id == teamId)
      if (team) {
        team.position = -1
      }
      this.refreshTeams++
      this.jobScheduleCount++
      this.teamDialog.showForm = false
      this.teamDialog.showList = true
    },
    deleteTeamMember () {
      const teamMembers = this.teamDialog.teamMembers
      for (let i = teamMembers.length - 1; i >= 0; i--) {
        if (teamMembers[i].checked) {
          teamMembers.splice(i, 1)
        }
      }
    },
    updateTeamOrder () {
      this.teams.sort((t1, t2) => t1.position - t2.position)
      for (let i = 0; i < this.teams.length; i++) {
        if (this.teams[i].position < 100) {
          this.teams[i].position = i + 1
        }
      }
    },
    closeTeam () {
      this.teamDialog.showForm = false
      this.teamDialog.showList = true
    },
    validateJobTeamMember (teamMember) {
      // validadte that the new team member does not exist in any other scheduled job on the same date
      const schedJob = this.teamMemberDialog.schedJob
      const day = this.dayIndex[schedJob.date]
      teamMember.teamMember = teamMember.teamMember.trim()
      for (const sj of Object.values(day.jobs)) {
        if (sj != this.teamMemberDialog.schedJob && sj.teamMembers.includes(teamMember.teamMember)) {
          const team = this.teams_.find(team => team.id === sj.team)
          this.showMessage(`${teamMember.teamMember} is in ${team.foreman}'s team`)
          teamMember.teamMember = ''
          break
        }
      }
    },
    jobTeamMembers (schedJob) {
      const members = schedJob.teamMembers ? schedJob.teamMembers : []
      const teamMembers = []

      for (let i = 0; i < members.length; i++) {
        teamMembers.push({ id: i + 1, checked: false, teamMember: members[i] })
      }

      for (let i = teamMembers.length; i < this.teamMemberDialog.maxTeamMembers; i++) {
        teamMembers.push({ id: i + 1, checked: false, teamMember: '' })
      }
      return teamMembers
    },
    openTeamMemberDialog (team, schedJob) {
      this.teamMemberDialog.open = true
      this.teamMemberDialog.team = team
      this.teamMemberDialog.schedJob = schedJob
      this.teamMemberDialog.teamMembers = this.jobTeamMembers(schedJob)
    },
    closeTeamMemberDialog () {
      this.teamMemberDialog.open = false
    },
    deleteJobTeamMember () {
      const teamMembers = this.teamMemberDialog.teamMembers
      for (let i = teamMembers.length - 1; i >= 0; i--) {
        if (teamMembers[i].checked) {
          teamMembers.splice(i, 1)
        }
      }
    },
    saveTeamMembers () {
      const schedJob = this.teamMemberDialog.schedJob

      const blankJob = schedJob.isBlank()
      const teamMembers = this.teamMemberDialog.teamMembers
      this.copyTeamMembers(teamMembers, schedJob.teamMembers)

      if (blankJob) {
        this.copyOneToOne(new SchedJob(schedJob), -1, schedJob.date, schedJob.team, false)
      }
      schedJob.setLastUpdate()

      this.teamMemberDialog.open = false
      this.$forceUpdate()
    },
    editOptions () {
      this.showOptionsDialog = true
    },
    showMessage (msg) {
      this.userMessage = msg
      this.alert = true
    },
    closeMessage () {
      this.userMessage = ''
      this.alert = false
    }
  }
}
</script>

<style scoped>
html {
  overflow-y: auto;
}

.job-sched {
  width: 99%;
  /* height: 99%; */
  height: 80vh;
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

.job-sched tbody .selected {
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
  border: 2px solid lightgray;
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

.v-expansion-panel-content :deep(.v-expansion-panel-content__wrap) {
  padding: 0 !important;
}
</style>

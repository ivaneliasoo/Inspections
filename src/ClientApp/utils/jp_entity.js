import { toIsoDate, isSunday, isSaturday } from './jp_util'

export const JobState = {
  upcoming: 'upcoming',
  confirmed: 'confirmed',
}

export const JobStatus = {
  inProgress: 'inProgress',
  onHold: 'onHold',
  standBy: 'standBy',
  highChance: 'highChance',
  blank: 'blank',
}

export class Job {
  id
  #scope
  #status
  #priority
  #value
  #tag
  #comments
  #teams
  #teamCount
  #duration
  #salesPerson
  #shift
  lastUpdate
  updated

  constructor(job) {
    if (job === undefined) {
      this.id = 0
      this.scope = ''
      this.status = ''
      this.priority = ''
      this.value = ''
      this.tag = ''
      this.comments = ''
      this.teams = ''
      this.teamCount = ''
      this.duration = ''
      this.salesPerson = ''
      this.shift = ''
      this.lastUpdate = toIsoDate(new Date())
      this.updated = false
    } else {
      this.id = job.id
      this.scope = job.scope
      this.status = job.status
      this.priority = job.priority
      this.value = job.value
      this.tag = job.tag
      this.comments = job.comments
      this.teams = job.teams
      this.teamCount = job.teamCount
      this.duration = job.duration
      this.salesPerson = job.salesPerson
      this.shift = job.shift == null ? '' : job.shift
      this.lastUpdate = job.lastUpdate
      this.updated = job.updated
    }
  }

  get scope() {
    return this.#scope
  }

  set scope(scope) {
    this.#scope = scope
    this.setlastUpdate()
  }

  get state() {
    if ([JobStatus.inProgress, JobStatus.onHold].includes(this.#status)) {
      return JobState.confirmed
    } else if (
      [JobStatus.standBy, JobStatus.highChance].includes(this.#status)
    ) {
      return JobState.upcoming
    }
    return null
  }

  get status() {
    return this.#status
  }

  set status(s) {
    this.#status = s
    this.setlastUpdate()
  }

  get priority() {
    return this.#priority
  }

  set priority(p) {
    this.#priority = p
    this.setlastUpdate()
  }

  get value() {
    return this.#value
  }

  set value(v) {
    this.#value = v
    this.setlastUpdate()
  }

  get tag() {
    return this.#tag
  }

  set tag(t) {
    this.#tag = t
    this.setlastUpdate()
  }

  get comments() {
    return this.#comments
  }

  set comments(c) {
    this.#comments = c
    this.setlastUpdate()
  }

  get teams() {
    return this.#teams
  }

  set teams(t) {
    this.#teams = t
    this.setlastUpdate()
  }

  get teamCount() {
    return this.#teamCount
  }

  set teamCount(tc) {
    this.#teamCount = tc
    this.setlastUpdate()
  }

  get duration() {
    return this.#duration
  }

  set duration(d) {
    this.#duration = d
    this.setlastUpdate()
  }

  get shift() {
    return this.#shift
  }

  set shift(s) {
    this.#shift = s
    this.setlastUpdate()
  }

  get salesPerson() {
    return this.#salesPerson
  }

  set salesPerson(sp) {
    this.#salesPerson = sp
    this.setlastUpdate()
  }

  setlastUpdate() {
    this.updated = true
  }

  toJSON() {
    return {
      id: this.id,
      scope: this.scope,
      status: this.status,
      priority: this.priority === '' ? 0 : Number(this.priority),
      value: this.value,
      tag: this.tag,
      teams: this.teams,
      teamCount: this.teamCount,
      comments: this.comments,
      duration: this.duration,
      shift: this.shift,
      salesPerson: this.salesPerson,
      lastUpdate: this.lastUpdate,
      updated: this.updated,
    }
  }
}

export const Shift = {
  day: 'day',
  night: 'night',
  tbc: 'tbc',
  // rest: 'rest',
  // onLeave: 'onLeave',
  unasigned: 'unasigned',
}

export class SchedJob {
  static groupIndex

  #id
  #date
  #team
  #job1
  #job2
  #shift
  #splitShift
  #teamMembers
  #excludeSaturday
  #excludeSunday
  lastUpdate
  updated

  constructor(sj) {
    if (!sj) {
      this.id = 0
      this.date = ''
      this.team
      this.job1 = ''
      this.job2 = ''
      this.shift = ''
      this.splitShift = false
      this.teamMembers = []
      this.excludeSaturday = true
      this.excludeSunday = true
      this.lastUpdate = null
      this.updated = false
    } else {
      this.id = sj.id ? sj.id : 0
      this.date = sj.date
      this.team = sj.team
      this.shift = sj.shift
      this.splitShift = !!sj.splitShift
      this.job1 = sj.job1
      this.job2 = sj.job2
      this.teamMembers = sj.teamMembers
        ? typeof sj.teamMembers === 'string'
          ? JSON.parse(sj.teamMembers)
          : sj.teamMembers.slice()
        : []
      this.excludeSaturday = sj.excludeSaturday
      this.excludeSunday = sj.excludeSunday
      this.lastUpdate = sj.lastUpdate
      this.updated = sj.updated
    }
  }

  get id() {
    return this.#id
  }

  set id(id) {
    this.#id = id
    this.setLastUpdate()
  }

  get date() {
    return this.#date
  }

  set date(d) {
    this.#date = d
    this.setLastUpdate()
  }

  getPk() {
    return `${this.team}:${this.date}`
  }

  get startDate() {
    if (this.id == 0) {
      return this.date
    }
    if (!SchedJob.groupIndex) {
      console.log('Error. groupIndex is null')
      return ''
    }
    const g = SchedJob.groupIndex[this.id]
    if (!g) {
      return ''
    }
    const group = Object.keys(SchedJob.groupIndex[this.id])
    return group.reduce((d1, d2) => (d1 < d2 ? d1 : d2), group[0])
  }

  get endDate() {
    if (this.id == 0) {
      return this.date
    }
    if (!SchedJob.groupIndex) {
      console.log('Error. groupIndex is null')
      return ''
    }
    const g = SchedJob.groupIndex[this.id]
    if (!g) {
      return ''
    }
    const group = Object.keys(SchedJob.groupIndex[this.id])
    return group.reduce((d1, d2) => (d2 < d1 ? d1 : d2), group[0])
  }

  get team() {
    return this.#team
  }

  set team(t) {
    this.#team = t
    this.setLastUpdate()
  }

  get splitShift() {
    return this.#splitShift
  }

  set splitShift(isSplitShift) {
    this.#splitShift = isSplitShift
    this.setLastUpdate()
  }

  getJob1() {
    return this.excludeSunday && isSunday(this.date) ? '' : this.#job1
  }

  get job1() {
    return this.#job1
  }

  set job1(scope) {
    this.#job1 = scope
    this.setLastUpdate()
  }

  getJob2() {
    return this.excludeSunday && isSunday(this.date) ? '' : this.#job2
  }

  get job2() {
    return this.#job2
  }

  set job2(scope) {
    this.#job2 = scope
    this.setLastUpdate()
  }

  get shift() {
    return this.#shift
  }

  set shift(s) {
    this.#shift = s
    this.setLastUpdate()
  }

  getTeamMembers() {
    return this.teamMembers
  }

  get teamMembers() {
    return this.#teamMembers
  }

  set teamMembers(tm) {
    this.#teamMembers = tm
    this.setLastUpdate()
  }

  teamSize() {
    if (this.excludeSaturday && isSaturday(this.date)) {
      return 0
    }
    if (this.excludeSunday && isSunday(this.date)) {
      return 0
    }
    return this.teamMembers ? this.teamMembers.length : 0
  }

  get excludeSaturday() {
    return this.#excludeSaturday
  }

  set excludeSaturday(es) {
    this.#excludeSaturday = es
    this.setLastUpdate()
  }

  get excludeSunday() {
    return this.#excludeSunday
  }

  set excludeSunday(es) {
    this.#excludeSunday = es
    this.setLastUpdate()
  }

  isBlank() {
    if (
      this.id === 0 &&
      this.job1.trim() === '' &&
      this.job2.trim() === '' &&
      this.teamMembers.length == 0
    ) {
      return true
    }
    return false
  }

  setLastUpdate() {
    this.updated = true
  }

  update(sj) {
    // this.id = sj.id;
    this.team = sj.team
    this.date = sj.date
    this.shift = sj.shift
    this.splitShift = sj.splitShift
    this.job1 = sj.job1
    this.job2 = sj.job2
    this.teamMembers =
      typeof sj.teamMembers === 'string'
        ? JSON.parse(sj.teamMembers)
        : sj.teamMembers
    this.excludeSaturday = sj.excludeSaturday
    this.excludeSunday = sj.excludeSunday

    if (this.#job2.trim() === '') {
      this.splitShift = false
    }
    this.lastUpdate = sj.lastUpdate
    this.updated = sj.updated
  }

  toJSON() {
    return {
      id: this.id,
      date: this.date,
      startDate: this.startDate,
      endDate: this.endDate,
      team: this.team,
      job1: this.job1,
      job2: this.job2,
      shift: this.shift,
      splitShift: this.splitShift,
      teamMembers: JSON.stringify(this.teamMembers),
      excludeSaturday: this.excludeSaturday,
      excludeSunday: this.excludeSunday,
      lastUpdate: this.lastUpdate,
      updated: this.updated,
    }
  }

  shiftLabel() {
    return this.shift === 'day' ? 'Day' : this.shift === 'night' ? 'Night' : ''
  }
}

export class Day {
  constructor(day) {
    if (day === undefined) {
      // this.id = 0;
      this.date
      this.jobs = {}
    } else {
      // this.id = day.id;
      this.date = day.date
      this.jobs = day.jobs
    }
  }

  manPowerTotals(teams) {
    if (teams.length === 0) {
      return { manPower: 0, onLeave: 0 }
    }
    const teamMap = {}
    for (const team of teams) {
      teamMap[team.id] = team
    }
    let onLeave = 0
    let manPower = 0
    for (const schedJob of Object.values(this.jobs)) {
      const foreman = teamMap[schedJob.team].foreman
      if (foreman.toLowerCase().trim() === 'on leave') {
        onLeave += schedJob.teamSize()
      } else {
        manPower += schedJob.teamSize()
      }
    }
    return { manPower, onLeave }
  }
}

export class Team {
  #id
  #foreman
  #vehicle
  #position
  #teamMembers
  updated
  lastUpdate

  constructor(t) {
    if (!t) {
      this.#id = 0
      this.#foreman = ''
      this.#vehicle = ''
      ;(this.#position = ''), (this.#teamMembers = [])
      this.lastUpdate = toIsoDate(new Date())
      this.updated = false
    } else {
      this.#id = t.id ? t.id : 0
      this.#foreman = t.foreman
      this.#vehicle = t.vehicle
      this.#position = t.position
      this.#teamMembers =
        typeof t.teamMembers === 'string'
          ? JSON.parse(t.teamMembers)
          : t.teamMembers
      this.lastUpdate = t.lastUpdate
      this.updated = t.updated
    }
  }

  get id() {
    return this.#id
  }

  set id(id) {
    this.#id = id
    this.setLastUpdate()
  }

  get foreman() {
    return this.#foreman
  }

  set foreman(f) {
    this.#foreman = f
    this.setLastUpdate()
  }

  get vehicle() {
    return this.#vehicle
  }

  set vehicle(v) {
    this.#vehicle = v
    this.setLastUpdate()
  }

  get position() {
    return this.#position
  }

  set position(p) {
    this.#position = p
    this.setLastUpdate()
  }

  get teamMembers() {
    return this.#teamMembers
  }

  set teamMembers(tm) {
    this.#teamMembers = tm
    this.setLastUpdate()
  }

  update(t) {
    this.#id = t.id ? t.id : 0
    this.#foreman = t.foreman
    this.#vehicle = t.vehicle
    this.#position = t.position
    this.#teamMembers =
      typeof t.teamMembers === 'string'
        ? JSON.parse(t.teamMembers)
        : t.teamMembers
    this.lastUpdate = t.lastUpdate
    this.updated = t.updated
  }

  setLastUpdate() {
    this.updated = true
  }

  toJSON() {
    return {
      id: this.id,
      foreman: this.foreman,
      vehicle: this.vehicle,
      position: this.position,
      teamMembers: JSON.stringify(this.teamMembers),
      lastUpdate: this.lastUpdate,
      updated: this.updated,
    }
  }
}

export class Selection {
  constructor(col, startRow) {
    this.col = col
    this.startRow = startRow
    this.endRow = startRow
  }
}

export class Clipboard {
  constructor(team, startDate, numRows) {
    this.team = team
    this.startDate = startDate
    this.numRows = numRows
  }
}

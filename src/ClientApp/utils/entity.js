import { getColName } from './util'

export class EnergyReport {
  id
  name
  location
  template
  fileName
  circuit
  dateCreated
  chartLegendOption
  cover
  rawCsvData
  lastUpdate
  updated

  constructor(er) {
    this.id = 0
    this.name = ''
    this.location = ''
    this.template = 0
    this.fileName = ''
    this.circuit = ''
    this.chartLegendOption = ''
    this.dateCreated = new Date()
    this.rawCsvData = []

    this.lastUpdate = null
    this.updated = false

    if (er) {
      const props = Object.getOwnPropertyNames(er)
      for (const prop of props) {
        this[prop] = er[prop]
      }
    }

    if (!this.cover) {
      this.cover = {
        title: '',
        client: '',
        separation: 0,
        reportAuthor: '',
        instrumentUsed: '',
        serialNumber: '',
      }
    }
  }

  toJSON() {
    return {
      id: this.id,
      name: this.name,
      location: this.location,
      template: parseInt(this.template),
      fileName: this.fileName,
      circuit: this.circuit,
      chartLegendOption: this.chartLegendOption,
      dateCreated: this.dateCreated.toISOString(),
      cover: {
        title: this.cover.title,
        client: this.cover.client,
        separation: parseInt(this.cover.separation),
        reportAuthor: this.cover.reportAuthor,
        instrumentUsed: this.cover.instrumentUsed,
        serialNumber: this.cover.serialNumber,
      },
      rawCsvData: JSON.stringify(this.rawCsvData),
      lastUpdate: this.lastUpdate,
      updated: this.updated,
    }
  }
}

export class Mapping {
  series
  #col
  color
  param

  constructor(m) {
    if (m) {
      const props = Object.getOwnPropertyNames(m)
      for (const prop of props) {
        this[prop] = m[prop]
      }
    }
  }

  get col() {
    return getColName(this.#col)
  }

  set col(c) {
    this.#col = c
  }
}

export class Param {
  name
  #col

  constructor(p) {
    if (p) {
      const props = Object.getOwnPropertyNames(p)
      for (const prop of props) {
        this[prop] = p[prop]
      }
    }
  }

  get col() {
    return getColName(this.#col)
  }

  set col(c) {
    this.#col = c
  }
}

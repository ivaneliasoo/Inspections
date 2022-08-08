export class Item {
  itemNumber
  description
  noCables
  unitCost
  units
  materialMarkup
  labourCostUnit

  constructor(item) {
    this.itemNumber = ''
    this.description = ''
    this.noCables = 0
    this.unitCost = 0.0
    this.units = 0
    this.materialMarkup = 0.0
    this.labourCostUnit = 0.0

    if (item) {
      const props = Object.getOwnPropertyNames(item)
      for (const prop of props) {
        this[prop] = item[prop]
      }
    }
  }

  toJSON() {
    return {
      itemNumber: this.itemNumber,
      description: this.description,

      noCables: Number(this.noCables),
      unitCost: Number(this.unitCost),
      units: Number(this.units),
      labourCostUnit: Number(this.labourCostUnit),
      materialMarkup: Number(this.materialMarkup),
    }
  }

  materialCost() {
    return this.noCables && this.noCables !== 0
      ? this.noCables * this.units * this.unitCost
      : this.units * this.unitCost
  }

  labourCost() {
    if (
      this.materialCost() === 0 &&
      this.units === 0 &&
      this.labourCostUnit > 0
    ) {
      this.units = 1
    }
    return this.units * this.labourCostUnit
  }

  workPrice() {
    return (
      this.materialCost() * (1 + this.materialMarkup / 100) + this.labourCost()
    )
  }
}

export class Section {
  #secNumber
  description
  materialMarkup
  finalMarkup
  items

  constructor(sec) {
    this.secNumber = ''
    this.description = ''
    this.materialMarkup = 0
    this.finalMarkup = 0

    if (sec) {
      const props = Object.getOwnPropertyNames(sec)
      for (const prop of props) {
        this[prop] = sec[prop]
      }
    }

    if (!this.items || this.items.length == 0) {
      this.items = [
        new Item({
          itemNumber: '',
          description: '',
          noCables: 0,
          units: 0,
          unitCost: 0.0,
          labourCostUnit: 0.0,
          materialMarkup: this.materialMarkup,
        }),
      ]
    }
  }

  get secNumber() {
    return this.#secNumber
  }

  set secNumber(sn) {
    this.#secNumber = sn
    this.renumberItems()
  }

  toJSON() {
    return {
      secNumber: this.secNumber,
      description: this.description,
      materialMarkup: Number(this.materialMarkup),
      finalMarkup: Number(this.finalMarkup),
      items: this.items,
    }
  }

  updateMaterialMarkup() {
    const materialMarkup = this.materialMarkup
    const items = this.items
    if (items) {
      for (const item of items) {
        item.materialMarkup = materialMarkup
      }
    }
  }

  renumberItems() {
    const items = this.items
    if (items) {
      for (let i = 0; i < items.length; i++) {
        items[i].itemNumber = this.secNumber + '.' + (i + 1)
      }
    }
  }

  materialCost() {
    return this.items
      ? this.items.reduce((sum, item, index) => sum + item.materialCost(), 0)
      : 0
  }

  labourCost() {
    return this.items
      ? this.items.reduce((sum, item, index) => sum + item.labourCost(), 0)
      : 0
  }

  summation() {
    return this.items
      ? this.items.reduce((sum, item, index) => sum + item.workPrice(), 0)
      : 0
  }

  toQuotePrice() {
    const finalMarkup = this.finalMarkup ? this.finalMarkup : 0
    const x = this.summation() * (1 + finalMarkup / 100)
    return x % 5 >= 2.5 ? parseInt(x / 5) * 5 + 5 : parseInt(x / 5) * 5
  }
}

export class CostSheet {
  id
  project
  location
  isTemplate
  dateCreated
  materialMarkup
  labourDailyRate
  labourNightMultiplier
  finalMarkup
  finalOverallMarkup
  sections
  options
  lastUpdate
  updated

  constructor(cs) {
    this.id = 0
    this.project = ''
    this.location = ''
    this.isTemplate = false
    this.dateCreated = new Date()
    this.materialMarkup = 0
    this.labourDailyRate = 0
    this.labourNightMultiplier = 0
    this.finalMarkup = 0
    this.finalOverallMarkup = 0
    this.options = {
      numberAlignment: 'center',
      textAlignment: 'left',
    }

    this.lastUpdate = null
    this.updated = false

    if (cs) {
      const props = Object.getOwnPropertyNames(cs)
      for (const prop of props) {
        this[prop] = cs[prop]
      }
    }

    if (this.options === null) {
      this.options = {
        numberAlignment: 'center',
        textAlignment: 'left',
      }
    }

    if (!this.sections || this.sections.length == 0) {
      this.sections = [
        new Section({
          secNumber: '',
          description: '',
          labourDailyRate: 0,
          labourNightMultiplier: 0,
          materialMarkup: this.materialMarkup,
          finalMarkup: this.finalMarkup,
        }),
      ]
    }
  }

  toJSON() {
    return {
      id: this.id,
      project: this.project,
      location: this.location,
      isTemplate: this.isTemplate,
      dateCreated: this.dateCreated.toISOString(),
      materialMarkup: Number(this.materialMarkup),
      labourDailyRate: Number(this.labourDailyRate),
      labourNightMultiplier: Number(this.labourNightMultiplier),
      finalMarkup: Number(this.finalMarkup),
      finalOverallMarkup: Number(this.finalOverallMarkup),
      sections: this.sections,
      options: this.options,
      lastUpdate: this.lastUpdate,
      updated: this.updated,
    }
  }

  updateMaterialMarkup() {
    const materialMarkup = this.materialMarkup
    const sections = this.sections
    if (sections) {
      for (const section of sections) {
        section.materialMarkup = materialMarkup
        section.updateMaterialMarkup()
      }
    }
  }

  updateFinalMarkup() {
    const finalMarkup = this.finalMarkup
    const sections = this.sections
    if (sections) {
      for (const section of sections) {
        section.finalMarkup = finalMarkup
      }
    }
  }

  renumberSections() {
    const sections = this.sections
    for (let i = 0; i < sections.length; i++) {
      sections[i].secNumber = (i + 1).toString()
    }
  }

  materialCost() {
    return this.sections
      ? this.sections.reduce(
          (sum, section, index) => sum + section.materialCost(),
          0
        )
      : 0
  }

  labourCost() {
    return this.sections
      ? this.sections.reduce(
          (sum, section, index) => sum + section.labourCost(),
          0
        )
      : 0
  }

  toQuotePrice() {
    // const x = this.sections ? this.sections.reduce((sum, section, index) => sum + section.toQuotePrice(), 0) : 0;
    // return (x % 5) >= 2.5 ? parseInt(x / 5) * 5 + 5 : parseInt(x / 5) * 5;
    const sum = this.sections
      ? this.sections.reduce(
          (sum, section, index) => sum + section.toQuotePrice(),
          0
        )
      : 0
    const finalMarkup = Number(this.finalMarkup)
    return sum * (1 + finalMarkup / 100)
  }

  totalSales(delta) {
    if (!delta) {
      return this.toQuotePrice()
    }
    const finalMarkup = Number(this.finalOverallMarkup)
    return (
      (this.toQuotePrice() * (1 + (finalMarkup + delta) / 100)) /
      (1 + finalMarkup / 100)
    )
  }

  grossProfit(delta) {
    return this.totalSales(delta) - this.materialCost() - this.labourCost()
  }

  totalSalesPercent() {
    return 100
  }

  materialCostPercent(delta) {
    return (this.materialCost() * 100) / this.totalSales(delta)
  }

  labourCostPercent(delta) {
    return (this.labourCost() * 100) / this.totalSales(delta)
  }

  grossProfitPercent(delta) {
    return (this.grossProfit(delta) * 100) / this.totalSales(delta)
  }

  numberOfDaysComplete() {
    return this.labourCost() / this.labourDailyRate
  }

  numberOfNightsComplete() {
    return (
      this.labourCost() / (this.labourDailyRate * this.labourNightMultiplier)
    )
  }
}

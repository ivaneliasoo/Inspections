
export class Item {
    #id;
    #itemNumber;
    #description;
    #noCables;
    #unitCost;
    #units;
    #materialMarkup;
    #laborCostUnit;
    lastUpdate;
    updated;

    constructor(item) {
        if (!item) {
            this.id = 0;
            this.itemNumber = "";
            this.description = "";

            this.noCables = 0;
            this.unitCost = 0.0;
            this.units = 0;
            this.materialMarkup = 0.0;
            this.laborCostUnit = 0.0;

            this.lastUpdate = null;
            this.updated = false;
        } else {
            this.id = (item.id) ? item.id : 0;
            this.itemNumber = item.itemNumber;
            this.description = item.description;

            this.noCables = item.noCables;
            this.unitCost = item.unitCost;
            this.units = item.units;
            this.materialMarkup = item.materialMarkup;
            this.laborCostUnit = item.laborCostUnit;

            this.lastUpdate = item.lastUpdate;
            this.updated = item.updated;
        }
    }

    get id() {
        return this.#id;
    }

    set id(id) {
        this.#id = id;
        this.updated = true;
    }

    get itemNumber() {
        return this.#itemNumber;
    }

    set itemNumber(itemNum) {
        this.#itemNumber = itemNum; 
        this.updated = true;
    }

    get description() {
        return this.#description;
    }

    set description(desc) {
        this.#description = desc;
        this.updated = true;
    }

    get noCables() {
        return this.#noCables;
    }

    set noCables(nc) {
        this.#noCables = nc;
        this.updated = true;
    }

    get unitCost() {
        return this.#unitCost;
    }

    set unitCost(uc) {
        this.#unitCost = uc;
        this.updated = true;
    }

    get units() {
        return this.#units;
    }

    set units(u) {
        this.#units = u;
        this.updated = true;
    }

    get materialMarkup() {
        return this.#materialMarkup;
    }

    set materialMarkup(mm) {
        this.#materialMarkup = mm;
        this.updated = true;
    }

    get laborCostUnit() {
        return this.#laborCostUnit;
    }

    set laborCostUnit(lcu) {
        this.#laborCostUnit = lcu;
        this.updated = true;
    }

    materialCost() {
        return this.noCables ? this.noCables*this.units*this.unitCost : this.units*this.unitCost;
    }

    labourCost() {
        return this.units * this.laborCostUnit;
    }

    workPrice() {
        return this.materialCost()*(1+this.materialMarkup/100) + this.labourCost();
    }
}

export class Section {
    #id;
    #secNumber;
    #subSection;
    #description;
    #materialMarkup;
    #finalMarkup;
    #items;
    lastUpdate;
    updated;

    constructor(sec) {
        if (!sec) {
            this.id = 0;
            this.secNumber = "";
            this.subSection = "";
            this.description = "";
            this.materialMarkup = 0;
            this.finalMarkup = 0;
            this.items = [];
            this.lastUpdate = null;
            this.updated = false;
        } else {
            this.id = (sec.id) ? sec.id : 0;
            this.secNumber = sec.secNumber;
            this.subSection = sec.subSection;
            this.description = sec.description;
            this.materialMarkup = sec.materialMarkup;
            this.finalMarkup = sec.finalMarkup;
            this.items = sec.items;
            this.lastUpdate = sec.lastUpdate;
            this.updated = sec.updated;
        }
    }

    get id() {
        return this.#id;
    }

    set id(id) {
        this.#id = id;
        this.updated = true;
    }

    get secNumber() {
        return this.#secNumber;
    }

    set secNumber(sn) {
        this.#secNumber = sn;
        this.updated = true;
    }

    get subSection() {
        return this.#subSection;
    }

    set subSection(ss) {
        this.#subSection = ss;
        this.updated = true;
    } 

    get description() {
        return this.#description;
    }

    set description(desc) {
        this.#description = desc;
        this.updated = true;
    }

    get materialMarkup() {
        return this.#materialMarkup;
    }

    set materialMarkup(mm) {
        this.#materialMarkup = mm;
        this.updated = true;
    }

    get finalMarkup() {
        return this.#finalMarkup;
    }

    set finalMarkup(fm) {
        this.#finalMarkup = fm;
        this.updated = true;
    }

    get items() {
        return this.#items;
    }

    set items(it) {
        this.#items = it;
        this.updated = true;
    }

    materialCost() {
        return this.items ? this.items.reduce((sum, item, index) => sum + item.materialCost(), 0) : 0;
    }

    labourCost() {
        return this.items ? this.items.reduce((sum, item, index) => sum + item.labourCost(), 0) : 0;
    }

    summation() {
        return this.items ? this.items.reduce((sum, item, index) => sum + item.workPrice(), 0) : 0;
    }

    toQuotePrice() {
        const finalMarkup = this.finalMarkup ? this.finalMarkup : 0;
        return this.summation() * (1 + finalMarkup/100);
    }
}

export class CostSheet {
    #id;
    #project;
    #location;
    #dateCreated;
    #materialMarkup;
    #finalMarkup;
    #sections;
    lastUpdate;
    updated;

    constructor(cs) {
        if (!cs) {
            this.id = 0;
            this.project = "";
            this.location = "";
            this.dateCreated = "";
            this.materialMarkup = 0.0;
            this.finalMarkup = 0.0;
            this.sections = [];
            this.lastUpdate = null;
            this.updated = false;
        } else {
            this.id = (cs.id) ? cs.id : 0;
            this.project = cs.project;
            this.location = cs.location;
            this.dateCreated = cs.dateCreated;
            this.materialMarkup = cs.materialMarkup;
            this.finalMarkup = cs.finalMarkup;
            this.sections = cs.sections;
            this.lastUpdate = cs.lastUpdate;
            this.updated = cs.updated;
        }
    }

    get id() {
        return this.#id;
    }

    set id(id) {
        this.#id = id;
        this.updated = true;
    }

    get project() {
      return this.#project;
    }

    set project(pr) {
      this.#project = pr;
      this.updated = true;
    }

    get location() {
        return this.#location;
    }

    set location(loc) {
        this.#location = loc;
        this.updated = true;
    }

    get materialMarkup() {
        console.log("materialMarkup", this.#materialMarkup);
        return this.#materialMarkup;
    }

    set materialMarkup(mm) {
        this.#materialMarkup = mm;
        this.updated = true;
    }

    get finalMarkup() {
        return this.#finalMarkup;
    }

    set finalMarkup(fm) {
        this.#finalMarkup = fm;
        this.updated = true;
    }

    get sections() {
        return this.#sections;
    }

    set sections(sec) {
        this.#sections = sec;
        this.updated = true;
    }

    get dateCreated() {
        return this.#dateCreated;
    }

    set dateCreated(d) {
        this.#dateCreated = d;
        this.updated = true;
    }

    materialCost() {
        return this.sections ? this.sections.reduce((sum, section, index) => sum + section.materialCost(), 0) : 0;
    }

    labourCost() {
        return this.sections ? this.sections.reduce((sum, section, index) => sum + section.labourCost(), 0) : 0;
    }

    toQuotePrice() {
        return this.sections ? this.sections.reduce((sum, section, index) => sum + section.toQuotePrice(), 0) : 0;
    }
}

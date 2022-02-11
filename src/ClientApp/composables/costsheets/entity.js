
export class Item {
    #id;
    #description;
    #noCables;
    #unitCost;
    #units;
    #materialMarkUp;
    #laborCostUnit;
    lastUpdate;
    updated;

    constructor(item) {
        if (!item) {
            this.id = 0;
            this.description = "";

            this.noCables = 0;
            this.unitCost = 0.0;
            this.units = 0;
            this.materialMarkUp = 0.0;
            this.laborCostUnit = 0.0;

            this.lastUpdate = null;
            this.updated = false;
        } else {
            this.id = (item.id) ? item.id : 0;
            this.description = item.description;

            this.noCables = item.noCables;
            this.unitCost = item.unitCost;
            this.units = item.units;
            this.materialMarkUp = item.materialMarkUp;
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

    get materialMarkUp() {
        return this.#materialMarkUp;
    }

    set materialMarkUp(mm) {
        this.#materialMarkUp = mm;
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
        return 0.0;
    }

    labourCost() {
        return 0.0;
    }

    workPrice() {
        return 0.0;
    }
}

export class Section {
    #id;
    #secNumber;
    #subSection;
    #description;
    #materialMarkup;
    #items;
    lastUpdate;
    updated;

    constructor(sec) {
        if (!sj) {
            this.id = 0;
            this.secNumber = "";
            this.subSection = "";
            this.description = "";
            this.materialMarkUp = 0;
            this.items = [];
            this.lastUpdate = null;
            this.updated = false;
        } else {
            this.id = (sec.id) ? sec.id : 0;
            this.secNumber = sec.secNumber;
            this.subSection = sec.subSection;
            this.description = sec.description;
            this.materialMarkUp = sec.materialMarkUp;
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
        this.secNumber = sn;
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

    get materialMarkUp() {
        return this.#materialMarkup;
    }

    set materialMarkUp(mm) {
        this.#materialMarkup = mm;
        this.updated = true;
    }

    get items() {
        return this.#items;
    }

    set items(it) {
        this.#items = it;
        this.updated = true;
    }

}

export class CostSheet {
    #id;
    #project;
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
            this.dateCreated = "";
            this.materialMarkup = 0.0;
            this.#finalMarkup = 0.0;
            this.sections = [];
            this.lastUpdate = null;
            this.updated = false;
        } else {
            this.id = (cs.id) ? cs.id : 0;
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

    get materialMarkUp() {
        return this.#materialMarkup;
    }

    set materialMarkUp(mm) {
        this.#materialMarkup = mm;
        this.updated = true;
    }

    get finalMarkUp() {
        return this.#finalMarkup;
    }

    set finalMarkUp(fm) {
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
}

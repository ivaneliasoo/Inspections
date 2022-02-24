
export class Item {
    itemNumber;
    description;
    noCables;
    unitCost;
    units;
    materialMarkup;
    labourCostUnit;

    constructor(item) {
        this.itemNumber = "";
        this.description = "";
        this.noCables = 0;
        this.unitCost = 0.0;
        this.units = 0;
        this.materialMarkup = 0.0;
        this.labourCostUnit = 0.0;

        if (item) {
            const props = Object.getOwnPropertyNames(item);
            for (const prop of props) {
                this[prop] = item[prop];
            }
        }
    }

    toJSON() {
        return {
            itemNumber: this.itemNumber,
            description: this.description,

            noCables: parseFloat(this.noCables),
            unitCost: parseFloat(this.unitCost),
            units: parseFloat(this.units),
            labourCostUnit: parseFloat(this.labourCostUnit),
            materialMarkup: parseFloat(this.materialMarkup),

            items: this.items
        }
    }

    materialCost() {
        return this.noCables ? this.noCables*this.units*this.unitCost : this.units*this.unitCost;
    }

    labourCost() {
        return this.units * this.labourCostUnit;
    }

    workPrice() {
        return this.materialCost()*(1+this.materialMarkup/100) + this.labourCost();
    }
}

export class Section {
    secNumber;
    description;
    materialMarkup;
    finalMarkup;
    items;

    constructor(sec) {
        this.secNumber = "";
        this.description = "";
        this.materialMarkup = 0;
        this.finalMarkup = 0;
        this.items = [ new Item() ];

        if (sec) {
            const props = Object.getOwnPropertyNames(sec);
            for (const prop of props) {
                this[prop] = sec[prop];
            }

            if (!this.items || this.items.length == 0) {
                this.items = [
                    new Item({
                        itemNumber: "",
                        description: "",
                        noCables: 0,
                        units: 0,
                        unitCost: 0.0,
                        labourCostUnit: 0.0,
                        materialMarkup: this.materialMarkup
                    })
                ]
            }
        }
    }

    toJSON() {
        return {
            secNumber: this.secNumber,
            description: this.description,
            materialMarkup: parseFloat(this.materialMarkup),
            finalMarkup: parseFloat(this.finalMarkup),
            items: this.items
        }
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
    id;
    project;
    location;
    dateCreated;
    materialMarkup;
    labourDailyRate;
    labourNightMultiplier;
    finalMarkup;
    sections;
    lastUpdate;
    updated;

    constructor(cs) {
        this.id = 0;
        this.project = "";
        this.location = "";
        this.dateCreated = new Date();
        this.materialMarkup = 0;
        this.labourDailyRate = 0;
        this.labourNightMultiplier = 0;
        this.finalMarkup = 0;
        this.lastUpdate = null;
        this.updated = false;
        if (cs) {
            const props = Object.getOwnPropertyNames(cs);
            for (const prop of props) {
                this[prop] = cs[prop];
            }
            if (!this.sections || this.sections.length == 0) {
                this.sections = [ new Section({
                    secNumber: "",
                    description: "",
                    labourDailyRate: 0,
                    labourNightMultiplier: 0,
                    materialMarkup: this.materialMarkup, 
                    finalMarkup: this.finalMarkup})
                ]
            }
        }
    }

    toJSON() {
        return {
            id: this.id,
            project: this.project,
            location: this.location,
            dateCreated: this.dateCreated.toISOString(),
            materialMarkup: parseFloat(this.materialMarkup),
            labourDailyRate: parseFloat(this.labourDailyRate),
            labourNightMultiplier: parseFloat(this.labourNightMultiplier),
            finalMarkup: parseFloat(this.finalMarkup),
            sections: this.sections,
            lastUpdate: this.lastUpdate,
            updated: this.updated
        }
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

    grossProfit() {
        return this.toQuotePrice() - this.materialCost() - this.labourCost();
    }

    materialCostPercent() {
        return (this.materialCost()*100) / this.toQuotePrice();
    }

    labourCostPercent() {
        return (this.labourCost()*100) / this.toQuotePrice();
    }

    grossProfitPercent() {
        return (this.grossProfit()*100) / this.toQuotePrice();
    }

    numberOfDaysComplete() {
        return (this.labourCost()*100) / this.labourDailyRate;
    }
}


const dateFormat = Intl.DateTimeFormat('en-SG', { month: 'numeric', day: 'numeric', year: 'numeric', hourCycle: 'h23' });
const timeFormat = Intl.DateTimeFormat('en-SG', { hour: 'numeric', minute: 'numeric', second: 'numeric', hourCycle: 'h23' });

function excluded(col, exclude) {
    for (let excol of exclude) {
        if (col === excol.name) {
            return true;
        }
    }
    return false;
}

function copyRow(row, exclude) {
    const r = {}
    for (let prop of Object.keys(row)) {
        if (prop !== "DateTime" && !excluded(prop, exclude)) {
            if (prop.charAt(prop.length-1) === " ") {
                const p = prop.trim()
                if (!row[p]) {
                    r[p] = parseFloat(row[prop])
                }
            } else {
                r[prop] = parseFloat(row[prop])
            }
        }
    }
    return r
}

export function adjustData(data, idx, exclude) {
    //console.log(JSON.stringify(data[0]))
    const r = new Array(data.length)
    let t1 = data[0].DateTime.getTime()
    let t2 = data[idx].DateTime.getTime()
    const delta = (data[2].DateTime.getTime() - data[1].DateTime.getTime())
    for (let i=0, j=idx; i<idx; i++,j++) {
        let row1 = copyRow(data[i], exclude)
        const d = new Date(t1)
        row1["Date"] = dateFormat.format(d)
        row1["Time"] = timeFormat.format(d)
        r[i] = row1
        t1 += delta;

        if (j < r.length) {
            let row2 = copyRow(data[i], exclude)
            d = new Date(t2)
            row2["Date"] = dateFormat.format(d)
            row2["Time"] = timeFormat.format(d)
            r[j] = row2
            t2 += delta
        }
    }
    return r
}

export function adjustDates(data, startDate, exclude) {
    //console.log(JSON.stringify(data[0]))
    const r = new Array(data.length)
    //let t1 = data[0].DateTime.getTime()
    const delta = (data[2].DateTime.getTime() - data[1].DateTime.getTime())
    let t1 = startDate.getTime()
    for (let i=0; i<data.length; i++) {
        let row1 = copyRow(data[i], exclude)
        const d = new Date(t1)
        row1["Date"] = dateFormat.format(d)
        row1["Time"] = timeFormat.format(d)
        r[i] = row1
        t1 += delta;
    }
    return r
}

export function expandData(data, idx) {
    //console.log(JSON.stringify(data[0]))
    const r = []
    let t = data[0].DateTime.getTime()
    const delta = (data[2].DateTime.getTime() - data[1].DateTime.getTime())
    for (let i=0; i<idx; i++) {
        let row1 = copyRow(data[i])
        //delete row1.index
        const d = new Date(t)
        //const dt = dateTimeFormat.format(d)
        //console.log(dt);
        row1["Date"] = dateFormat.format(d)
        row1["Time"] = timeFormat.format(d)
        r.push(row1)
        t += delta
        let row2 = copyRow(data[i])
        //delete row2.index
        d = new Date(t)
        row2["Date"] = dateFormat.format(d)
        row2["Time"] = timeFormat.format(d)
        r.push(row2)
        t += delta
    }
    return r
}

export function getColName(colName) {
    if (colName === "Date" || colName === "Time" || colName === "DateTime") {
        return colName;
    }
    return colName.replace(" ", "-").toUpperCase();
}
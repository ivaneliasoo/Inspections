
function param(params, level, channel) {
    const idx = params.findIndex( param => param.includes(level) && param.includes(channel));
    return idx == -1 ? "" : params[idx];
}

function paramValue(row, level, channel) {
    for (const param of Object.keys(row)) {
        if (param.includes(level) && param.includes(channel)) {
            return parseFloat(row[param]);
        }
    }
    return 0;
}

function findPeaks(data) {
    const peaks = [];
    for (let j=0; j<data.length; j++) {
        const circuit = data[j];
        const currentData = JSON.parse(circuit.currentData);
        let l1Row = 0, l1Max = paramValue(currentData[l1Row], "L1", "Max");
        let l2Row = 0, l2Max = paramValue(currentData[l2Row], "L2", "Max");
        let l3Row = 0, l3Max = paramValue(currentData[l3Row], "L3", "Max");

        for (let i=1; i<currentData.length; i++) {
            const row = currentData[i];
            const l1 = paramValue(row, "L1", "Max");
            if (l1Max < l1) {
                l1Row = i;
                l1Max = l1;
            }
            const l2 = paramValue(row, "L2", "Max");
            if (l2Max < l2) {
                l2Row = i;
                l2Max = l2;
            }
            const l3 = paramValue(row, "L3", "Max");
            if (l3Max < l3) {
                l3Row = i;
                l3Max = l3;
            }
        }
        peaks.push({L1: l1Row, L2: l2Row, L3: l3Row});
    }
    return peaks;
}

export default function currentTable(data) {
    const peaks = findPeaks(data);

    var doc = {
        pageSize: 'A4',
        pageOrientation: 'landscape',
        pageMargins: [ 20, 20, 20, 20 ],
        content: []
    };

    const groupSize = 3;
    const groups = Math.trunc(data.length / groupSize) + ((data.length % groupSize) > 0);

    const dark  = ['#B7D9A8', '#98C8E2'];
    const light = ['#CAEABB', '#C4DFF7'];

    let idx = 0;
    for (let i=0; i<groups; i++) {
        const size = ((i<groups-1) || (data.length % groupSize == 0)) ? groupSize : data.length % groupSize;
        const cols = size * 4 + 2;

        const table = {
            //style: 'tableBody',
            fontSize: 10,
            table: {
                headerRows: 2,
                widths: Array(cols).fill('8%', 0, 2),
                body: []
            }
        }

        let width = 84/(3*3);
        table.table.widths.fill(width+'%', 2);
        
        let headers1 = [{ text: "", colSpan: 2}, {}];
        let headers2 = [
            {text: "\nDate",    fillColor: 'lightgrey', alignment: 'center'}, 
            {text: "\nChannel", fillColor: 'lightgrey', alignment: 'center'}
        ];
        for (let j=0; j<size; j++) {
            const color = j % 2;
            headers1 = headers1.concat([
                {text: data[idx+j].circuit, colSpan: 3, fillColor: dark[color], alignment: 'center'}, {}, {}
            ]);
            headers2 = headers2.concat([
                {text: "L1\nCurrent\nRMS 1/2", fillColor: light[color], alignment: 'center'}, 
                {text: "L2\nCurrent\nRMS 1/2", fillColor: light[color], alignment: 'center'}, 
                {text: "L3\nCurrent\nRMS 1/2", fillColor: light[color], alignment: 'center'}
            ]);
        }
        table.table.body.push(headers1);
        table.table.body.push(headers2);

        let color  =  ['#DDE3E9', 'white'];
        const currentData = JSON.parse(data[0].currentData);
        for (let k=0; k<currentData.length; k++) {
            const ci = k % 2;
            const date = currentData[k].date
            let tableRow = [{ text: date, rowSpan: 3, marginTop: 12, fillColor: color[ci] , alignment: 'center'}, 
                    { text: "Min", fillColor: color[ci], alignment: 'center'}];
            table.table.body.push(tableRow);
            tableRow = [{}, { text: "Avg", fillColor: color[ci], alignment: 'center' }];
            table.table.body.push(tableRow);
            tableRow = [{}, { text: "Max", fillColor: color[ci], alignment: 'center' }];
            table.table.body.push(tableRow);
        }

        for (let j=0; j<size; j++) {
            const circuit = JSON.parse(data[idx].currentData);
            let rowIdx = 2;
            for (let k=0; k<circuit.length; k++) {
                const ci = k % 2;
                const row = circuit[k];
                const params = Object.keys(row);

                const l1Min = row[param(params, "L1", "Min")];
                const l2Min = row[param(params, "L2", "Min")];
                const l3Min = row[param(params, "L3", "Min")];
                let tableRow = table.table.body[rowIdx].concat([
                    { text: l1Min, fillColor: color[ci], alignment: 'center'}, 
                    { text: l2Min, fillColor: color[ci], alignment: 'center'}, 
                    { text: l3Min, fillColor: color[ci], alignment: 'center' }
                ]);
                table.table.body[rowIdx++] = tableRow;

                tableRow = table.table.body[rowIdx].concat([
                    { text: row["L1"], fillColor: color[ci], alignment: 'center' }, 
                    { text: row["L2"], fillColor: color[ci], alignment: 'center' }, 
                    { text: row["L3"], fillColor: color[ci], alignment: 'center' }
                ]);
                table.table.body[rowIdx++] = tableRow;

                const l1Max = row[param(params, "L1", "Max")];
                const l2Max = row[param(params, "L2", "Max")];
                const l3Max = row[param(params, "L3", "Max")];

                const l1Color = peaks[idx]["L1"] == k ? "red" : "black";
                const l2Color = peaks[idx]["L2"] == k ? "red" : "black";
                const l3Color = peaks[idx]["L3"] == k ? "red" : "black";

                tableRow = table.table.body[rowIdx].concat([
                    { text: l1Max, fillColor: color[ci], color: l1Color, alignment: 'center' }, 
                    { text: l2Max, fillColor: color[ci], color: l2Color, alignment: 'center' }, 
                    { text: l3Max, fillColor: color[ci], color: l3Color, alignment: 'center' }
                ]);
                table.table.body[rowIdx++] = tableRow;
            }
            idx++;
        }

        doc.content.push(table);

        if (i < groups-1) {
            doc.content.push({
                text: "",
                pageBreak: 'after'
            })
        }
    }

    return doc;
}

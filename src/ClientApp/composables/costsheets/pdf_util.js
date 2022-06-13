
import { fmt, border } from './util.js';
import { logo } from './logo.js';

export default function pdfDocument(sheet) {
    var doc = {
        pageSize: 'LEGAL',
        pageOrientation: 'landscape',
        pageMargins: [ 10, 10, 10, 10 ]
    };

    doc.content = [
        {
            margin: [20, 0, 0, 0],
            stack: [
                {   columns: [
                        { text: 'Project:', width: 60, style: 'headline', bold: true }, 
                        { text: sheet.project, width: 300, style: 'headline', bold: true },
                        { text: "", width: 20, style: 'headline', bold: true },
                        { text: "Costing Sheet", width: 90, style: 'headline', bold: true }
                    ],
                    margin: [20,0,0,6]
                },
                {   columns: [
                        { text: 'Location:', width: 60, style: 'headline2', bold: true }, 
                        { text: sheet.location, width: 300, style: 'headline2', bold: true }
                    ],
                    margin: [20,0,0,6]
                }
            ]
        }
    ];

    const bd = border;

    // Sheet header
    const table = {
        style: 'tableBody',
        table: {
            // headerRows: 1,
            widths: ['5%', '20%', '9%', '5%', '9%', '7%', '6%', '9%', '6%', '6%', '6%', '6%', '6%'],
            body: [
                [
                    { border: bd.all, style: 'header2', fillColor: '#d5c9df', colSpan: 2, text: 'Descriptions(s)' }, {},
                    { border: bd.trb, style: 'header2', fillColor: '#c0aed0', colSpan: 5, text: 'Material(s)' }, {}, {}, {}, {},
                    { border: bd.trb, style: 'header2', fillColor: '#d5c9df', colSpan: 2, text: 'Labour' }, {},
                    { border: bd.trb, style: 'header2', fillColor: '#c0aed0', colSpan: 4, text: 'Summary' }, {}, {}, {}
                ],
                [
                    { border: bd.left, style: 'header4', fillColor: '#d5c9df', text: 'S/N' },
                    { border: bd.right, style: 'header4', fillColor: '#d5c9df', text: 'Descriptions(s)' },

                    { border: bd.none, style: 'header4', fillColor: '#c0aed0', text: 'No. of Cables\n(Apply to cables)' },
                    { border: bd.none, style: 'header4', fillColor: '#c0aed0', text: 'Unit cost' },
                    { border: bd.none, style: 'header4', fillColor: '#c0aed0', text: 'Unit(s)\n(m/nos/pcs/lot)' },
                    { border: bd.none, style: 'header4', fillColor: '#c0aed0', text: 'Material Cost' },
                    { border: bd.right, style: 'header4', fillColor: '#c0aed0', text: 'Mark up on Material' },

                    { border: bd.none, style: 'header4', fillColor: '#d5c9df', text: 'Labour Cost to do\nthe work per unit' },
                    { border: bd.right, style: 'header4', fillColor: '#d5c9df', text: 'Labour Cost' },

                    { border: bd.none, style: 'header4', fillColor: '#c0aed0', text: 'Price to do\nthe work' },
                    { border: bd.none, style: 'header4', fillColor: '#c0aed0', text: 'Summation' },
                    { border: bd.none, style: 'header4', fillColor: '#c0aed0', text: 'Final Mark Up' },
                    { border: bd.right, style: 'header4', fillColor: '#c0aed0', text: 'To Quote price' }
                ],
                [
                    { border: bd.left, fillColor: '#d5c9df', style: "normal", text: "" },
                    { border: bd.right, fillColor: '#d5c9df', style: "normal", text: "" },
                    { border: bd.none, fillColor: '#c0aed0', style: "normal", colSpan: 4, text: "" }, {}, {}, {},
                    { border: bd.right, fillColor: '#c0aed0', style: "number", text: fmt(sheet.materialMarkup, 0, '%') },
                    { border: bd.none, fillColor: '#d5c9df', style: "normal", text: "" },
                    { border: bd.right, fillColor: '#d5c9df', style: "normal", text: "" },
                    { border: bd.none, fillColor: '#c0aed0', style: "normal", text: "" },
                    { border: bd.none, fillColor: '#c0aed0', style: "number", text: "" },
                    { border: bd.none, fillColor: '#c0aed0', style: "number", text: fmt(sheet.finalMarkup, 0, '%') },
                    { border: bd.right, fillColor: '#c0aed0', style: "number", text: "" }
                ]
            ]
        }
    }

    doc.content.push(table);

    for (let i=0; i<sheet.sections.length; i++) {
        const section = sheet.sections[i];
        let row = [
            { border: bd.ltr, fillColor: '#eae4ef', style: "normal", text: section.secNumber },
            { border: bd.top, fillColor: '#e1f5f7', style: "normal", colSpan: 4, text: section.description }, {}, {}, {},
            { border: bd.top, fillColor: '#eae4ef', style: "normal", text: "" },
            { border: bd.tr,  fillColor: '#eae4ef', style: "number", text: fmt(section.materialMarkup, 0, '%') },
            { border: bd.top, fillColor: '#e1f5f7', style: "normal", text: "" },
            { border: bd.tr, fillColor: '#eae4ef', style: "normal", text: "" },
            { border: bd.tr, fillColor: '#eae4ef', style: "normal", text: "" },
            { border: bd.tr, fillColor: '#eae4ef', style: "number", text: fmt(section.summation(), 2, '$') },
            { border: bd.tr, fillColor: '#eae4ef', style: "number", text: fmt(section.finalMarkup, 0, '%') },
            { border: bd.tr, fillColor: '#eae4ef', style: "number", text: fmt(section.toQuotePrice(), 2, '$') }
        ]
        // rows.push(row);
        table.table.body.push(row);

        for (let j=0; j<section.items.length; j++) {
            const item = section.items[j];
            const row = [
                { border: bd.lr,   fillColor: '#eae4ef', style: "normal", text: item.itemNumber },
                { border: bd.lr,   fillColor: '#e1f5f7', style: "normal", text: item.description },
                { border: bd.none, fillColor: '#e1f5f7', style: "number", text: item.noCables },
                { border: bd.none, fillColor: '#e1f5f7', style: "number", text: fmt(item.unitCost, 2, '$') },
                { border: bd.none, fillColor: '#e1f5f7', style: "number", text: item.units },
                { border: bd.none, fillColor: '#eae4ef', style: "number", text: fmt(item.materialCost(),2, '$') },
                { border: bd.none, fillColor: '#eae4ef', style: "number", text: fmt(item.materialMarkup, 0, '%') },
                { border: bd.left, fillColor: '#e1f5f7', style: "number", text: fmt(item.labourCostUnit, 2, '$') },
                { border: bd.none, fillColor: '#eae4ef', style: "number", text: fmt(item.labourCost(), 2, '$') },
                { border: bd.lr,   fillColor: '#eae4ef', style: "number", text: fmt(item.workPrice(), 2, '$') },
                { border: bd.right, fillColor: '#eae4ef', style: "normal", text: "" },
                { border: bd.right, fillColor: '#eae4ef', style: "normal", text: "" },
                { border: bd.right, fillColor: '#eae4ef', style: "normal", text: "" }
            ]
            table.table.body.push(row);
        }
    }

    let row = [
        { border: bd.ltb, fillColor: '#c0aed0', style: "normal", colSpan: 4, text: "" }, {}, {}, {},
        { border: bd.tb,  fillColor: '#c0aed0', style: "normal", text: "Material Cost" },
        { border: bd.tb,  fillColor: '#c0aed0', style: "number", text: fmt(sheet.materialCost(), 2, '$') },
        { border: bd.tb,  fillColor: '#c0aed0', style: "normal", text: "" },
        { border: bd.tb,  fillColor: '#c0aed0', style: "normal", text: "Labour Cost" },
        { border: bd.tb,  fillColor: '#c0aed0', style: "number", text: fmt(sheet.labourCost(), 2, '$') },
        { border: bd.tb,  fillColor: '#c0aed0', style: "normal", colSpan: 2, text: "" }, {},
        { border: bd.tb,  fillColor: '#c0aed0', style: "normal", text: "Total Quote" },
        { border: bd.trb, fillColor: '#c0aed0', style: "number", text: fmt(sheet.toQuotePrice(), 2, '$') }
    ]
    table.table.body.push(row);

    row = [
        { border: bd.lrb, style: "normal", colSpan: 13, color: 'white', text: "." }, 
            {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}
    ]
    table.table.body.push(row);

    //Summary
    table.table.body.push([
        { border: bd.left, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.none, fillColor: '#eae4ef', style: "header3", text: "Summary" },
        { border: bd.none, fillColor: '#eae4ef', style: "normal", colSpan: 8, text: "" }, 
        {}, {}, {}, {}, {}, {}, {},
        { border: bd.right, colSpan: 3, rowSpan: 5, fillColor: '#eae4ef', 
            stack: [
                {
                    image: logo(),
                    alignment: 'center',
                    width: 100,
                    height: 80
                }
            ]
        },
        {}, {}
    ]);

    table.table.body.push([
        { border: bd.left, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.none, fillColor: '#eae4ef', style: "normal", text: "Set Material Mark Up:" },
        { border: bd.ltr,  fillColor: '#d1eee1', style: "number", colSpan: 2, text: fmt(sheet.materialMarkup, 0, '%') }, {},
        { border: bd.none, fillColor: '#eae4ef', style: "normal", colSpan: 6, text: "" }, {}, {}, {}, {}, {},
        { border: bd.right, fillColor: '#eae4ef', style: "normal", colSpan: 3, text: "" }, {}, {}
    ]);

    table.table.body.push([
        { border: bd.left, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.none, fillColor: '#eae4ef', style: "normal", text: "Set Labour Daily Rate (Day):" },
        { border: bd.ltr,  fillColor: '#d1eee1', style: "number", colSpan: 2, text: fmt(sheet.labourDailyRate, 2, '$') }, {},
        { border: bd.none, fillColor: '#eae4ef', style: "normal", colSpan: 6, text: "" }, {}, {}, {}, {}, {},
        { border: bd.right, fillColor: '#eae4ef', style: "normal", colSpan: 3, text: "" }, {}, {}
    ]);

    table.table.body.push([
        { border: bd.left, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.none, fillColor: '#eae4ef', style: "normal", text: "Set Labour multiplier (Night):" },
        { border: bd.ltr,  fillColor: '#d1eee1', style: "number", colSpan: 2, text: sheet.labourNightMultiplier }, {},
        { border: bd.none, fillColor: '#eae4ef', style: "normal", colSpan: 6, text: "" }, {}, {}, {}, {}, {},
        { border: bd.right, fillColor: '#eae4ef', style: "normal", colSpan: 3, text: "" }, {}, {}
    ]);

    table.table.body.push([
        { border: bd.left, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.none, fillColor: '#eae4ef', style: "normal", text: "Final Overall Markup:" },
        { border: bd.all,  fillColor: '#d1eee1', style: "number", colSpan: 2, text: fmt(sheet.finalMarkup, 0, '%') }, {},
        { border: bd.right, fillColor: '#eae4ef', style: "normal", colSpan: 9, text: "" },
        {}, {}, {}, {}, {}, {}, {}, {}
    ]);

    table.table.body.push([
        { border: bd.lr, fillColor: '#eae4ef', style: "normal", colSpan: 13, color: 'white', text: "." }, 
            {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}
    ]);

    table.table.body.push([
        { border: bd.left, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.ltb,  fillColor: '#7f7f7f', style: "normal", text: "Final Markup" },
        { border: bd.tb,   fillColor: '#7f7f7f', style: "normal", alignment: "center", colSpan: 2, text: "5%" }, {},
        { border: bd.tb,   fillColor: '#7f7f7f', style: "normal", alignment: "center", colSpan: 2, text: "10%" }, {},
        { border: bd.trb,  fillColor: '#7f7f7f', style: "normal", alignment: "center", colSpan: 2, text: "15%" }, {},
        { border: bd.none, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.none, fillColor: '#eae4ef', style: "normal", colSpan: 3, text: "Translate to number of days to complete:"}, 
        {}, {},
        { border: [false, false,  true,  false], fillColor: '#eae4ef', style: "normal", alignment: "center", text: fmt(sheet.numberOfDaysComplete(), 2) }
    ]);

    table.table.body.push([
        { border: bd.left, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.lrb,  fillColor: '#eae4ef', style: "normal", text: "Total Sales:" },
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.totalSales(5), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.totalSalesPercent(5), 0, '%') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.totalSales(), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.totalSalesPercent(), 0, '%') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.totalSales(15), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.totalSalesPercent(15), 0, '%') }, 
        { border: bd.none, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.none, fillColor: '#eae4ef', style: "normal", colSpan: 3, text: "Translate to number of nights to complete:"}, 
        {}, {},
        { border: [false, false,  true,  false], fillColor: '#eae4ef', style: "normal", alignment: "center", text: fmt(sheet.numberOfNightsComplete(), 2) }

    ]);

    table.table.body.push([
        { border: bd.left, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.lrb,  fillColor: '#eae4ef', style: "normal", text: "Material Cost:" },
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.materialCost(5), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.materialCostPercent(5), 0, '%') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.materialCost(), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.materialCostPercent(), 0, '%') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.materialCost(15), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.materialCostPercent(15), 0, '%') }, 
        { border: bd.none, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.ltr,  fillColor: '#31b3c5', style: "normal", text: "Super" },
        { border: bd.ltr,  fillColor: '#eae4ef', style: "normal", colSpan: 2, text: "When GP >20%" }, {},
        { border: bd.right, fillColor: '#eae4ef', style: "normal", text: "" }
    ]);

    table.table.body.push([
        { border: bd.left, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.lrb,  fillColor: '#eae4ef', style: "normal", text: "Material Cost:" },
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.labourCost(5), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.labourCostPercent(5), 0, '%') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.labourCost(), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.labourCostPercent(), 0, '%') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.labourCost(15), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.labourCostPercent(15), 0, '%') }, 
        { border: bd.none, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.ltr,  fillColor: '#9eedca', style: "normal", text: "Healthy" },
        { border: bd.ltr,  fillColor: '#eae4ef', style: "normal", colSpan: 2, text: "When 10% > GP > 19.9%" }, {},
        { border: bd.right, fillColor: '#eae4ef', style: "normal", text: "" }
    ]);

    table.table.body.push([
        { border: bd.left, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.lrb,  fillColor: '#eae4ef', style: "normal", text: "Gross Profit:" },
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.grossProfit(5), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.grossProfitPercent(5), 0, '%') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.grossProfit(), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.grossProfitPercent(), 0, '%') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.grossProfit(15), 2, '$') }, 
        { border: bd.rb,   fillColor: '#eae4ef', style: "number", text: fmt(sheet.grossProfitPercent(15), 0, '%') }, 
        { border: bd.none, fillColor: '#eae4ef', style: "normal", text: "" },
        { border: bd.all,  fillColor: '#c04225', style: "normal", text: "Low" },
        { border: bd.all,  fillColor: '#eae4ef', style: "normal", colSpan: 2, text: "When GP <10%" }, {},
        { border: bd.right, fillColor: '#eae4ef', style: "normal", text: "" }
    ]);

    table.table.body.push([
        { border: bd.lrb, fillColor: '#eae4ef', style: "normal", colSpan: 13, color: 'white', text: "." }, 
            {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}
    ]);

    doc.styles = {
        headline: {
            fontSize: 14,
            bold: true
        },
        headline2: {
            fontSize: 12,
            bold: true
        },
        header1: {
            fontSize: 14,
            bold: true
        },
        header2: {
            fontSize: 14,
            bold: true,
            alignment: 'center'
        },
        header3: {
            fontSize: 12,
            bold: true
        },
        header4: {
            fontSize: 9,
            alignment: 'center'
        },
        body: {
            fontSize: 10
        },
        body2: {
            fontSize: 9
        },
        normal: {
            fontSize: 9
        },
        number: {
            fontSize: 9,
            alignment: 'right'
        },
        tableHeader: {
            bold: true,
            fontSize: 10,
            color: 'black'
        },
        tableBody: {
            bold: true,
            fontSize: 9,
            color: 'black'
        }
    }

    doc.defaultStyle = {
        fontSize: 9
    }

    // doc.images = {
    //     logo: 'https://~/static/Logo.jpeg',
    //     width: 200,
    //     height: 200
    // }

    return doc;
}

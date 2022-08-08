import { minMax, timePercent } from './charts.js'

function showPeaks (cat) {
  for (const column of cat.mappings) {
    if (column.showPeaks) {
      return true
    }
  }
  return false
}

export default function document (report, page2, background, period, categories, charts, time, data, minMaxData, suffix) {
  const doc = {
    pageSize: 'LETTER',
    footer: function (currentPage) {
      if (currentPage > 1) {
        return {
          columns: [
            { text: '', width: 55 },
            { text: currentPage.toString() + ' | Page', width: 400, style: 'normal', color: 'grey' },
            { text: 'www.chengseng.com.sg', style: 'normal', color: 'grey' }
          ]
        }
      }
      return ''
    },
    background: function (page) {
      if (page == 1) {
        return [
          {
            svg:
                        `<svg>
                            <rect x="0" y="0" width="680" height="800" style="fill: #FFFF00;"/>
                        </svg>`
          },
          {
            svg: background, width: 620, height: 800
          }
        ]
      } else if (page2.include && page == 2) {
        return [
          {
            svg:
                        `<svg>
                            <line x1="50" y1="35" x2="560" y2="35" style="stroke:lightsteelblue;stroke-width:4px;"/>
                        </svg>`
          }
        ]
      }
    }
  }

  const cover = report.cover
  const separation = !cover.separation ? 200 : Number(cover.separation)
  const dateFormat = Intl.DateTimeFormat('en-SG', { month: 'short', day: 'numeric', year: 'numeric' })
  const timeFormat = Intl.DateTimeFormat('en-SG', { hour: 'numeric', minute: 'numeric', second: 'numeric' })

  doc.content = [
    // Cover page
    {
      margin: [25, 40, 0, 0],
      stack: [
        {
          columns: [
            { width: 300, text: cover.title, style: 'headline' },
          ]
        },
        { text: '', margin: [0, 5, 0, 0] },
        {
          canvas: [
            {
              type: 'line',
              x1: 0,
              y1: 5,
              x2: 120,
              y2: 5,
              lineWidth: 3
            }
          ]
        },
        { text: '', margin: [0, 40, 0, 40] },
        {
          columns: [
            { text: 'Client:', width: 50, style: 'body', bold: true },
            { text: cover.client, width: 300, style: 'body' }
          ],
          margin: [0, 0, 0, 6]
        },
        { text: 'Date of Measurement:', width: 100, style: 'body', bold: true },
        {
          text: period,
          style: 'body',
          margin: [0, 0, 0, 6]
        },
        { text: 'Site Location:', width: 100, style: 'body', bold: true },
        {
          columns: [
            { text: cover.siteLocation, width: 300, style: 'body' },
          ]
        },
        { text: '', margin: [0, 0, 0, separation] },
        {
          columns: [
            { text: 'Instrument Used:', width: 100, style: 'body2' },
            { text: cover.instrumentUsed, style: 'body2' }
          ],
          margin: [0, 0, 0, 6]
        },
        {
          columns: [
            { text: 'Serial Number:', width: 100, style: 'body2' },
            { text: cover.serialNumber, style: 'body2' }
          ],
          margin: [0, 0, 0, 6]
        },
        {
          columns: [
            { text: 'Report Date:', width: 100, style: 'body2' },
            { text: dateFormat.format(new Date()), style: 'body2' }
          ],
          margin: [0, 0, 0, 6]
        },
        {
          columns: [
            { text: 'Report Author:', width: 100, style: 'body2', bold: true },
            { text: cover.reportAuthor, style: 'body2', bold: true },
          ],
          pageBreak: 'after'
        },
      ]
    },
    // end cover page
  ]

  // Page 2
  const pg2 = {
    margin: [10, 20, 0, 0],
    stack: [
      { text: page2.title, style: 'header1', margin: [0, 0, 0, 5] },
      { text: page2.subtitle, style: 'header2', margin: [0, 0, 0, 20] },
      { text: period, style: 'header3', margin: [0, 0, 0, 20] },
      { text: 'Energy Pass/Fail Requirements Table', style: 'header3', margin: [0, 0, 0, 5] },
    ]
  }

  doc.content.push(pg2)

  // Create empty summary tables. They will be filled in the sections (categories) loop
  const summaryReqTable = {
    margin: [10, 5],
    style: 'tableBody',
    table: {
      headerRows: 1,
      widths: ['10%', '32%', '16%', '42%'],
      body: [
        [
          { text: 'Section', style: 'tableHeader', bold: true },
          { text: 'Power Quality Parameter', style: 'tableHeader', bold: true },
          { text: 'Energy Compliance', style: 'tableHeader', bold: true },
          { text: 'Remarks', style: 'tableHeader', bold: true }
        ]
      ]
    }
  }

  doc.content.push(summaryReqTable)
  doc.content.push({ text: 'Energy Additional Information Table', style: 'header3', margin: [10, 10, 0, 5] })
  const additionInfoTable = {
    margin: [10, 5],
    style: 'tableBody',
    table: {
      headerRows: 1,
      widths: ['10%', '40%', '50%'],
      body: [
        [
          { text: 'Section', style: 'tableHeader', bold: true },
          { text: 'Power Quality Parameter', style: 'tableHeader', bold: true },
          { text: 'Remarks', style: 'tableHeader', bold: true }
        ]
      ]
    },
    pageBreak: 'after'
  }

  doc.content.push(additionInfoTable)

  // end page 2

  let section = 0
  let subsection = 0
  const base = 97
  for (let i = 0; i < categories.length; i++) {
    const cat = categories[i]

    if (cat.disabled) {
      continue
    }

    let title = cat.title
    var secNumber // for summary table
    if (title.includes('${s}')) {
      section++
      subsection = 0
      secNumber = section
      title = title.replace(/\${s}/i, section)
    } else if (title.includes('${ss}')) {
      if (subsection == 0) {
        section++
      }
      secNumber = section + String.fromCharCode(base + subsection)
      title = title.replace(/\${ss}/i, section + String.fromCharCode(base + subsection))
      subsection++
    }
    doc.content.push({
      text: title, style: 'header3', margin: [0, 0, 0, 10]
    })

    const genInfo = {
      stack: []
    }

    if (cat.text.incParam) {
      genInfo.stack.push({
        text: [{ text: cat.text.paramName + ':  ', bold: true }, cat.text.paramValue],
        margin: [0, 0, 0, 6]
      })
    }
    if (cat.text.incParamDef) {
      let paramDef = cat.text.paramDef
      if (paramDef.includes('${td}')) {
        paramDef = paramDef.replace(/\${td}/i, time)
      }
      genInfo.stack.push({
        text: [{ text: 'Parameter definition:  ', bold: true }, paramDef],
        margin: [0, 0, 0, 6]
      })
    }
    if (cat.text.incLim) {
      genInfo.stack.push({
        text: [{ text: 'Limitations:  ', bold: true }, cat.text.limitations],
        margin: [0, 0, 0, 6]
      })
    }

    doc.content.push(genInfo)

    var sectionPass = true
    if (cat.text.includeRequirements) {
      const reqTable = {
        margin: [0, 10],
        style: 'tableBody',
        table: {
          headerRows: 1,
          body: []
        }
      }

      const headers = []
      headers.push({ text: 'Requirement', style: 'tableHeader', bold: true });
      [cat.text.reqHeader1, cat.text.reqHeader2, cat.text.reqHeader3].forEach(function (header) {
        if (header && header.trim() !== '') {
          headers.push({ text: header, style: 'tableHeader', bold: true })
        }
      })
      headers.push({ text: 'Result', style: 'tableHeader', bold: true })

      reqTable.table.body.push(headers)
      const percentLabel = ' % of the time: '
      cat.text.requirements.forEach((req) => {
        if (req.include) {
          const units = cat.yAxisName
          const reqLimits = { min: parseFloat(req.ymin), max: parseFloat(req.ymax) }
          const reqText = req.timePercent + percentLabel + reqLimits.min + units + ' ~ ' + reqLimits.max + units
          const reqRow = [{ text: reqText, bold: false }]
          let pass = true;
          [req.value1, req.value2, req.value3].forEach((val) => {
            const value = val ? val.toString() : null
            if (value && value.trim() !== '') {
              const limits = { min: Number.MAX_VALUE, max: Number.MIN_VALUE }
              const col = cat.factor ? value + suffix : value
              // minMax(data, col, limits);
              minMax(minMaxData[col], limits)
              const timePct = timePercent(data, col, reqLimits)
              if (timePct < parseFloat(req.timePercent)) {
                pass = false
                sectionPass = false
              }
              const str = limits.min.toFixed(2) + units + ' ~ ' + limits.max.toFixed(2) + units
              reqRow.push({ text: str, bold: false })
            }
          })
          const result = pass ? 'PASS' : 'FAIL'
          const rcolor = pass ? 'green' : 'red'
          reqRow.push({ text: result, color: rcolor })
          reqTable.table.body.push(reqRow)
        }
      })

      doc.content.push(reqTable)
    }

    const re = /[.:]/g
    const regex = re.exec(title)
    const pos = regex ? regex.index + 1 : -1
    const str = pos < 0 ? title : title.substring(pos).trim()
    if (i < 2) {
      summaryReqTable.table.body.push([
        { text: secNumber },
        { text: str },
        { text: { text: sectionPass ? 'PASS' : 'FAIL', color: sectionPass ? 'green' : 'red' } },
        { text: page2.requirements[i].remarks, bold: false }
      ])
    } else {
      additionInfoTable.table.body.push([
        { text: secNumber, bold: false },
        { text: str, bold: false },
        { text: page2.additionalInfo[i - 2].remarks }
      ])
    }

    // Show table with peak values
    if (showPeaks(cat)) {
      const peakTable = {
        margin: [0, 10],
        style: 'tableBody',
        table: {
          headerRows: 2,
          body: []
        }
      }

      const headers = ['Peak Values']
      peakTable.table.body.push(headers)
      const peakDates = ['Date']
      const peakTime = ['Time']
      const peaks = ['Value [' + cat.yAxisName + ']']
      peakTable.table.body.push(peakDates)
      peakTable.table.body.push(peakTime)
      peakTable.table.body.push(peaks)

      // console.log("minMaxData", JSON.stringify(minMaxData))
      for (const column of cat.mappings) {
        if (column.showPeaks) {
          const columnLabel = report.chartLegendOption === 'use-param-name' ? column.param : column.col
          headers.push({ text: columnLabel, style: 'tableHeader', bold: true })
          const minMax = minMaxData[column.col]
          const dt = data[minMax.maxIndex].DateTime
          const date = dateFormat.format(dt)
          const time = timeFormat.format(dt)
          const value = parseFloat(minMax.max).toFixed(2)
          // console.log(column.col, value);
          peakDates.push({ text: date, color: 'black', bold: false })
          peakTime.push({ text: time, color: 'black', bold: false })
          if (column.col.toLowerCase().includes('max')) {
            peaks.push({ text: value, color: 'red', bold: true })
          } else {
            peaks.push({ text: value, color: 'black', bold: false })
          }
        }
      }

      doc.content.push(peakTable)
    }

    doc.content.push({
      margin: [0, 10],
      image: charts[i * 2],
      width: 540
    })

    const lastElement = {
      margin: [0, 10],
      image: charts[i * 2 + 1],
      width: 540
    }
    if (i < categories.length - 1) {
      lastElement.pageBreak = 'after'
    }

    doc.content.push(lastElement)
  }

  doc.styles = {
    headline: {
      fontSize: 40,
      bold: true
    },
    header1: {
      fontSize: 26,
      bold: true
    },
    header2: {
      fontSize: 18,
      bold: true
    },
    header3: {
      fontSize: 12,
      bold: true
    },
    body: {
      fontSize: 14
    },
    body2: {
      fontSize: 12
    },
    normal: {
      fontSize: 9
    },
    tableHeader: {
      bold: true,
      fontSize: 9,
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

  return doc
}

// PDF file contents

export default function document (cover, page2, background, period, categories, charts, time) {
  const doc = {
    pageSize: 'LETTER',
    footer (currentPage) {
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
    background (page) {
      // eslint-disable-next-line eqeqeq
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
      // eslint-disable-next-line eqeqeq
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

  doc.content = [
    // Cover page
    {
      margin: [25, 40, 0, 0],
      stack: [
        {
          columns: [
            { width: 300, text: cover.title, style: 'headline' }
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
            { text: cover.siteLocation, width: 300, style: 'body' }
          ]
        },
        { text: '', margin: [0, 130, 0, 130] },
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
            { text: cover.reportDate, style: 'body2' }
          ],
          margin: [0, 0, 0, 6]
        },
        {
          columns: [
            { text: 'Report Author:', width: 100, style: 'body2', bold: true },
            { text: cover.reportAuthor, style: 'body2', bold: true }
          ],
          pageBreak: 'after'
        }
      ]
    },
    // end cover page
    // Page 2
    page2.include
      ? {
          margin: [10, 20, 0, 0],
          stack: [
            { text: page2.title, style: 'header1', margin: [0, 0, 0, 20] },
            { text: page2.subtitle, style: 'header2', margin: [0, 0, 0, 40] },
            { text: page2.p1, style: 'body', margin: [0, 0, 0, 20] },
            { text: page2.p2, style: 'body', margin: [0, 0, 0, 20] },
            { text: page2.p3, style: 'header2', bold: true, margin: [20, 10, 40, 20] },
            { text: page2.p4, style: 'body', margin: [0, 10, 0, 20] },
            { text: page2.p5, style: 'body' }
          ],
          pageBreak: 'after'
        }
      : {}
    // end page 2
  ]

  let section = 0
  let subsection = 0
  const base = 97
  for (let i = 0; i < categories.length; i++) {
    const cat = categories[i]

    if (cat.disabled) {
      continue
    }

    let title = cat.title
    // eslint-disable-next-line no-template-curly-in-string
    if (title.includes('${s}')) {
      section++
      subsection = 0
      title = title.replace(/\${s}/i, section)
    // eslint-disable-next-line no-template-curly-in-string
    } else if (title.includes('${ss}')) {
      if (subsection === 0) {
        section++
      }
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
      // eslint-disable-next-line no-template-curly-in-string
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

      cat.text.requirements.forEach(function (req) {
        if (req.include) {
          const reqRow = [{ text: req.requirement, bold: false }];
          [req.value1, req.value2, req.value3].forEach(function (value) {
            if (value && value.trim() !== '') {
              reqRow.push({ text: value, bold: false })
            }
          })
          reqRow.push({ text: req.result, color: 'green' })
          reqTable.table.body.push(reqRow)
        }
      })

      doc.content.push(reqTable)
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

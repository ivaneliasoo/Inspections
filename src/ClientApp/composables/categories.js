import md5 from 'blueimp-md5'

export function getColumns (catTemplate) {
  const cats = catTemplate.categories
  const cols = []
  for (let i = 0; i < cats.length; i++) {
    const mappings = cats[i].mappings
    const factor = cats[i].factor
    for (let j = 0; j < mappings.length; j++) {
      const col = { name: mappings[j].col }
      if (factor) {
        col.factor = factor
        col.suffix = ' '
        // }
      }
      cols.push(col)
    }
  }
  const calcColumns = catTemplate.calcColumns
  for (let i = 0; i < calcColumns.length; i++) {
    const params = calcColumns[i].params
    for (let j = 0; j < params.length; j++) {
      const col = params[j].col
      cols.push({ name: col })
    }
  }
  return cols
}

export function checkTemplate (template, csvCols, templateCols) {
  const columns = {}
  for (let i = 0; i < csvCols.length; i++) {
    columns[csvCols[i]] = true
  }
  const calcColumns = {}
  const calcCols = template.calcColumns
  for (let i = 0; i < calcCols.length; i++) {
    calcColumns[calcCols[i].name] = true
  }
  for (let j = 0; j < templateCols.length; j++) {
    const col = templateCols[j].name
    if (!columns[col] && !calcColumns[col]) {
      // TODO: do not log in production mode
      console.log('Column not found: ', templateCols[j].name)
      return false
    }
  }
  return true
}

export function findCategoryTemplate (templates, cols) {
  if (!cols || cols.length < 6) {
    return null
  }
  const columnMap = {}
  for (let i = 0; i < cols.length; i++) {
    columnMap[cols[i]] = true
  }
  for (let i = 0; i < templates.length; i++) {
    const templateCols = getColumns(templates[i])
    let gotit = true
    for (let j = 0; j < templateCols.length; j++) {
      if (!columnMap[templateCols[i].name]) {
        gotit = false
        break
      }
    }
    if (gotit) {
      return templates[i]
    }
  }
  return null
}

export function categoryTemplateFrom (aCatTemplate) {
  const catTemplate = Object.assign({}, aCatTemplate)
  const cats = catTemplate.categories
  for (let i = 0; i < cats.length; i++) {
    cats[i].histogram = ''
    const mappings = cats[i].mappings
    for (let j = 0; j < mappings.length; j++) {
      mappings[j].col = ''
    }
  }
  const calcColumns = catTemplate.calcColumns
  for (let i = 0; i < calcColumns.length; i++) {
    const params = calcColumns[i].params
    for (let j = 0; j < params.length; j++) {
      params[j].col = ''
    }
  }
  return catTemplate
}

export function getCalcColumns (template) {
  const cols = []
  const calcColumns = template.calcColumns
  for (let i = 0; i < calcColumns.length; i++) {
    cols.push(calcColumns[i].name)
  }
  return cols
}

export function getID (columns) {
  const str = columns.reduce((a, b) => a + b)
  const hash = md5(str)
  // TODO: Do not log in production mode
  console.log('hash:', hash)
  return hash
}

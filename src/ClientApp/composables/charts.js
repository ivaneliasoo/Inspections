
export function minMax (data, col, limits) {
  for (let i = 0; i < data.length; i++) {
    const value = data[i][col]
    if (value < limits.min) {
      limits.min = value
    }
    if (value > limits.max) {
      limits.max = value
    }
  }
}

export function sepLineChartOptions (csvData, category, suffix) {
  const seriesInfo = category.mappings
  const chartLimits = category.chartLimits
  const series = []
  const legend = []
  const xaxis = []
  const yaxis = []
  const grid = []
  const colors = []

  const slen = seriesInfo.length
  for (let i = 0; i < slen; i++) {
    const col = category.factor
      ? seriesInfo[i].col + suffix
      : seriesInfo[i].col
    legend.push(col)

    colors.push(seriesInfo[i].color)

    grid.push({
      // TODO: Use ===  insted of ==
      top: (i == 0 ? 10 : 100 * (i / slen) + (i == slen - 1 ? -2 : 4)) + '%',
      // height: 86
      height: 260 / slen
    })

    xaxis.push({
      type: 'value',
      min: csvData[0].index,
      max: csvData[csvData.length - 1].index,
      axisLabel: {
        // TODO: Use ===  insted of ==
        show: i == slen - 1,
        formatter (value) {
          const date = csvData[value].DateTime
          if (!date) {
            return ''
          }
          const texts = [(date.getMonth() + 1), date.getDay(), date.getFullYear()]
          return texts.join('/')
        }
      },
      show: true,
      gridIndex: i
    })

    let limits = { min: Number.MAX_VALUE, max: Number.MIN_VALUE }
    if (chartLimits.enable) {
      limits = { min: parseFloat(chartLimits.ymin), max: parseFloat(chartLimits.ymax) }
    }

    minMax(csvData, col, limits)
    const percentMargin = 0.1
    const margin = (limits.max - limits.min) * percentMargin

    yaxis.push({
      name: category.yAxisName,
      nameLocation: 'center',
      nameGap: 40,
      type: 'value',
      min: (limits.min - margin).toFixed(2),
      max: (limits.max + margin).toFixed(2),
      show: true,
      gridIndex: i
    })
    series.push({
      xAxisIndex: i,
      yAxisIndex: i,
      name: col,
      type: 'line',
      symbol: 'none',
      z: slen - i,
      encode: { x: 'index', y: col },
      markLine: {
        symbol: ['none', 'none'],
        lineStyle: { color: 'red', type: 'solid' },
        data: [{ yAxis: limits.min }, { yAxis: limits.max }]
      }
    })
  }

  return {
    title: {
      text: category.chartTitle,
      top: 0,
      left: 'center',
      textStyle: {
        fontSize: 14
      }
    },
    color: colors,
    legend: {
      icon: 'rect',
      top: 'bottom',
      itemWidth: 20,
      padding: [10, 0, 0, 0],
      textStyle: { fontSize: '10' },
      data: legend
    },
    dataset: {
      source: csvData
    },
    grid,
    xAxis: xaxis,
    yAxis: yaxis,
    series
  }
}

export function lineChartOptions (csvData, category, suffix) {
  const seriesInfo = category.mappings
  const chartLimits = category.chartLimits
  const series = []
  const legend = []
  const colors = []

  let limits = { min: Number.MAX_VALUE, max: Number.MIN_VALUE }
  if (chartLimits.enable) {
    limits = { min: parseFloat(chartLimits.ymin), max: parseFloat(chartLimits.ymax) }
  }

  const slen = seriesInfo.length
  for (let i = 0; i < slen; i++) {
    const col = category.factor
      ? seriesInfo[i].col + suffix
      : seriesInfo[i].col
    legend.push(col)
    colors.push(seriesInfo[i].color)

    minMax(csvData, col, limits)
    series.push({
      name: col,
      type: 'line',
      symbol: 'none',
      z: slen - i,
      // lineStyle: { color: seriesInfo[i].color },
      encode: { x: 'index', y: col }
    })
  }

  series[0].markLine = {
    symbol: ['none', 'none'],
    lineStyle: { color: 'red', type: 'solid' },
    data: [{ yAxis: limits.min }, { yAxis: limits.max }]
  }

  const percentMargin = 0.1
  const margin = (limits.max - limits.min) * percentMargin

  // const colors = category.chartColor ? category.chartColor : [
  //     'red', 'yellow', 'blue', '#d2c7e1', '#e1c7d2', '#a5c8fa', '#a5fac7', '#cdf098', '#f0c098'];

  return {
    title: {
      text: category.chartTitle,
      left: 'center',
      textStyle: {
        fontSize: 14
      }
    },
    color: colors,
    legend: {
      icon: 'rect',
      top: 'bottom',
      itemWidth: 20,
      textStyle: { fontSize: '10' },
      data: legend
    },
    dataset: {
      source: csvData
    },
    xAxis: {
      type: 'value',
      min: csvData[0].index,
      max: csvData[csvData.length - 1].index,
      axisLabel: {
        show: true,
        formatter (value) {
          const date = csvData[value].DateTime
          if (!date) {
            return ''
          }
          const texts = [(date.getMonth() + 1), date.getDay(), date.getFullYear()]
          return texts.join('/')
        }
      }
    },
    yAxis: {
      name: category.yAxisName,
      nameLocation: 'center',
      nameGap: 40,
      type: 'value',
      min: (limits.min - margin).toFixed(2),
      max: (limits.max + margin).toFixed(2)
    },
    series
  }
}

export function histogramOptions (category, binValues, binCount) {
  return {
    title: {
      text: category.histogramTitle,
      left: 'center',
      textStyle: {
        fontSize: 14
      }
    },
    color: [category.histoColor],
    xAxis: {
      name: category.yAxisName,
      nameLocation: 'center',
      nameGap: 40,
      type: 'category',
      data: binValues
    },
    yAxis: {
      name: 'Count',
      nameLocation: 'center',
      nameGap: 40,
      type: 'value'
    },
    series: [{
      data: binCount,
      type: 'bar'
    }]
  }
}

export function barChartOptions (category, data) {
  return {
    title: {
      text: category.histogramTitle,
      left: 'center',
      textStyle: {
        fontSize: 14
      }
    },
    dataset: {
      source: data
    },
    color: [category.histoColor],
    xAxis: {
      name: '',
      nameLocation: 'center',
      nameGap: 40,
      type: 'category'
    },
    yAxis: {
      name: category.yAxisName,
      nameLocation: 'center',
      nameGap: 40,
      type: 'value'
    },
    series: [{
      name: '', type: 'bar', encode: { x: 'day', y: 'value' }
    }]
  }
}

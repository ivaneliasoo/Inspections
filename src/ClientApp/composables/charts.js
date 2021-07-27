
export function minMax(data, col, limits) {
  for (var i=0; i<data.length; i++) {
    const value = data[i][col];
    if (value < limits.min) {
      limits.min = value;
    }
    if (value > limits.max) {
      limits.max = value;
    }
  }
  return limits;
}

export function timePercent(data, col, limits) {
  var inCount = 0;
  for (var i=0; i<data.length; i++) {
    const value = data[i][col];
    if (value >= limits.min && value <= limits.max) {
      inCount++;
    }
  }
  return (inCount/data.length)*100;
}

export function sepLineChartOptions(csvData, category, suffix) {

  const seriesInfo = category.mappings;
  var series = [];
  var legend = [];
  var xaxis = [];
  var yaxis = [];
  var grid = [];
  var colors = [];

  const slen = seriesInfo.length;
  for (var i=0; i<slen; i++) {
    const col = category.factor ?
       seriesInfo[i].col + suffix :
       seriesInfo[i].col;
    legend.push(col);

    colors.push(seriesInfo[i].color);

    grid.push ({
      top: (i==0 ? 10 : 100*(i/slen)+(i == slen-1 ? -2 : 4)) + '%',
      height: 260/slen
    });

    xaxis.push({
      type: 'value',
      min: csvData[0].index,
      max: csvData[csvData.length - 1].index,
      axisLabel: {
        show: i == slen-1 ? true : false,
        formatter: function (value) {
          var date = csvData[value].DateTime;
          if (!date) {
            return "";
          }
          var str = ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '/' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())) + '/' + date.getFullYear()
          return str;
        }
      },
      show: true,
      gridIndex: i
    })

    const markLine = {
      symbol: ['none', 'none'],
      data: []
    }

    if (category.text.includeRequirements) {
      const req = category.text.requirements;
      if (req[0].include) {
        markLine.data.push({ yAxis: parseFloat(req[0].ymin), lineStyle: { color: "red", type: 'solid' } }),
        markLine.data.push({ yAxis: parseFloat(req[0].ymax), lineStyle: { color: "red", type: 'solid' } })
      }
      if (req[1].include) {
        markLine.data.push({ yAxis: parseFloat(req[1].ymin), lineStyle: { color: "blue", type: 'solid' } }),
        markLine.data.push({ yAxis: parseFloat(req[1].ymax), lineStyle: { color: "blue", type: 'solid' } })
      }
    }    

    var limits = { min: Number.MAX_VALUE, max: Number.MIN_VALUE };
    const chartLimits = category.chartLimits;
    if (chartLimits.enable) {
      limits = { min: parseFloat(chartLimits.ymin), max: parseFloat(chartLimits.ymax) };
    }    
    minMax(csvData, col, limits);

    if (markLine.data.length == 0) {
      markLine.data.push({ yAxis: limits.min, lineStyle: { color: "red", type: 'solid' } }),
      markLine.data.push({ yAxis: limits.max, lineStyle: { color: "red", type: 'solid' } })
    } else if (markLine.data.length == 2) {
      limits.min = Math.min(limits.min, markLine.data[0].yAxis)
      limits.max = Math.max(limits.max, markLine.data[1].yAxis)
    } else if (markLine.data.length == 4) {
      limits.min = Math.min(limits.min, markLine.data[0].yAxis, markLine.data[2].yAxis)
      limits.min = Math.max(limits.max, markLine.data[1].yAxis, markLine.data[3].yAxis)
    }  

    const percentMargin = 0.1;
    const margin = (limits.max - limits.min) * percentMargin;
  
    yaxis.push({
      name: category.yAxisName ? `[${category.yAxisName}]` : "",
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
        name: col, type: 'line', symbol: 'none', z: slen-i, 
        encode: { x: 'index', y: col },
        markLine: markLine
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
    grid: grid,
    xAxis: xaxis,
    yAxis: yaxis,
    series: series
  }
}

export function lineChartOptions(csvData, category, suffix) {

  const seriesInfo = category.mappings;
  var series = [];
  var legend = [];
  var colors = [];

  const markLine = {
    symbol: ['none', 'none'],
    data: []
  }  
  if (category.text.includeRequirements) {
    const req = category.text.requirements;
    if (req[0].include) {
      markLine.data.push({ yAxis: parseFloat(req[0].ymin), lineStyle: { color: "red", type: 'solid' } }),
      markLine.data.push({ yAxis: parseFloat(req[0].ymax), lineStyle: { color: "red", type: 'solid' } })
    }
    if (req[1].include) {
      markLine.data.push({ yAxis: parseFloat(req[1].ymin), lineStyle: { color: "blue", type: 'solid' } }),
      markLine.data.push({ yAxis: parseFloat(req[1].ymax), lineStyle: { color: "blue", type: 'solid' } })
    }
  }    

  var limits = { min: Number.MAX_VALUE, max: Number.MIN_VALUE };
  const chartLimits = category.chartLimits;
  if (chartLimits.enable) {
    limits = { min: parseFloat(chartLimits.ymin), max: parseFloat(chartLimits.ymax) };
  }

  const slen = seriesInfo.length;
  for (var i=0; i<slen; i++) {
    const col = category.factor ?
       seriesInfo[i].col + suffix :
       seriesInfo[i].col;
    legend.push(col);
    colors.push(seriesInfo[i].color);

    minMax(csvData, col, limits);
    series.push({
        name: col, type: 'line', symbol: 'none', z: slen-i, 
        encode: { x: 'index', y: col }
    })
  }

  if (markLine.data.length == 0) {
    markLine.data.push({ yAxis: limits.min, lineStyle: { color: "red", type: 'solid' } }),
    markLine.data.push({ yAxis: limits.max, lineStyle: { color: "red", type: 'solid' } })
  } else if (markLine.data.length == 2) {
    limits.min = Math.min(limits.min, markLine.data[0].yAxis)
    limits.max = Math.max(limits.max, markLine.data[1].yAxis)
  } else if (markLine.data.length == 4) {
    limits.min = Math.min(limits.min, markLine.data[0].yAxis, markLine.data[2].yAxis)
    limits.max = Math.max(limits.max, markLine.data[1].yAxis, markLine.data[3].yAxis)
  } 

  series[0].markLine = markLine;

  const percentMargin = 0.1;
  const margin = (limits.max - limits.min) * percentMargin;
  
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
        formatter: function (value) {
          var date = csvData[value].DateTime;
          if (!date) {
            return "";
          }
          var str = ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '/' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())) + '/' + date.getFullYear()
          return str;
        }
      }
    },
    yAxis: {
      name: category.yAxisName ? `[${category.yAxisName}]` : "",
      nameLocation: 'center',
      nameGap: 40,
      type: 'value',
      min: (limits.min - margin).toFixed(2),
      max: (limits.max + margin).toFixed(2)
    },
    series: series,
  }
}

export function histogramOptions(category, binValues, binCount) {
  return {
    title: {
      text: category.histogramTitle,
      left: 'center',
      textStyle: {
        fontSize: 14
      }
    },
    color: [ category.histoColor ],
    xAxis: {
      name: category.yAxisName ? `[${category.yAxisName}]` : "",
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

export function barChartOptions(category, data) {
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
    color: [ category.histoColor ],
    xAxis: {
      name: '',
      nameLocation: 'center',
      nameGap: 40,
      type: 'category',
    },
    yAxis: {
      name: category.yAxisName ? `[${category.yAxisName}]` : "",
      nameLocation: 'center',
      nameGap: 40,
      type: 'value'
    },
    series: [{
      name: "", type: 'bar', encode: { x: 'day', y: 'value' }
    }]
  }
}

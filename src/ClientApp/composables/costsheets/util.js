import { HTML5_FMT } from 'moment'

export function endpoint (p) {
  const param = p ? '/' + p : ''
  // return `http://localhost:5000/api/costsheet${param}`
  return `/costsheet${param}`
}

export function fmt (value, decimals, fmt) {
  // value = Number(value).toFixed(decimals);
  value = Number(value).toLocaleString('en', { minimumFractionDigits: decimals, maximumFractionDigits: decimals })
  if (!fmt) {
    return value
  }
  if (fmt === '%') {
    return value + '%'
  } else if (fmt === '$') {
    return '$' + ' ' + value
  } else {
    return value
  }
}

export const border = {
  none: [false, false, false, false],
  all: [true, true, true, true],
  left: [true, false, false, false],
  lt: [true, true, false, false],
  ltr: [true, true, true, false],
  lr: [true, false, true, false],
  ltb: [true, true, false, true],
  lrb: [true, false, true, true],
  lb: [true, false, false, true],
  top: [false, true, false, false],
  tr: [false, true, true, false],
  trb: [false, true, true, true],
  tb: [false, true, false, true],
  tr: [false, true, true, false],
  right: [false, false, true, false],
  rb: [false, false, true, true],
  bottom: [false, false, false, true]
}


export const millisInDay = 60 * 60 * 24 * 1000;

export function datediff(date1, date2) {
  // Take the difference between the dates and divide by milliseconds per day.
  // Round to nearest whole number to deal with DST.
  const d1 = string2date(date1);
  const d2 = string2date(date2);
  return Math.round((d2.getTime()-d1.getTime())/millisInDay);
}

export function dateWithoutTime(dateTime) {
  var date = new Date(dateTime);
  date.setHours(0, 0, 0, 0);
  return date.getTime();
}

export function dateString(etime) {
  const d = new Date(etime)
  return date2string(d)
}

export function date2string(d) {
  return Intl.DateTimeFormat('en-CA', { month: 'numeric', day: 'numeric', year: 'numeric' }).format(d)
}

export function toIsoDate(date) {
  let str = new Date(date.getTime() - (date.getTimezoneOffset() * 60000)).toJSON();
  return (str.substr(str.length - 1) === "Z") ? str.substring(0, str.length - 1) : str;
}

export function string2date(s) {
  var b = s.split(/\D+/);
  return new Date(b[0], b[1]-1, b[2])
}

export function addDays(date, days) {
  var result = new Date(date);
  result.setDate(result.getDate() + days);
  return result;
}

export function* dateIterator(startDate, endDate) {
  for (let i = string2date(startDate); i <= string2date(endDate); i=addDays(i, 1)) {
      yield date2string(i);
  }
  return null;
}

export function isSunday(strDate) {
  return string2date(strDate).getDay() == 0;
}

export function isSaturday(strDate) {
  return string2date(strDate).getDay() == 6;
}

export function printGroups(schedJobGroups) {
  console.log("printGroups")
  for (const groupId of Object.keys(schedJobGroups)) {
    const group = schedJobGroups[groupId];
    let psj;
    for (const date of Object.keys(group)) {
      const sj = group[date];
      console.log(`${sj.id} ${sj.team} ${sj.date}`)
      if (psj && psj == sj) {
        console.log(`${psj.date} and ${sj.date} are the same `)
      }
      if (psj && psj.getJob1() == sj.getJob1()) {
        console.log("jobs are the same object")
      }
    }
  }
}

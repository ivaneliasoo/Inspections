export * from './PrintHelper'

export function debounce<F extends Function> (func:F, wait:number):F {
  let timeoutID:number

  if (!Number.isInteger(wait)) {
    wait = 300
  }

  // conversion through any necessary as it wont satisfy criteria otherwise
  return <any> function (this:any, ...args: any[]) {
    clearTimeout(timeoutID)
    const context = this

    timeoutID = window.setTimeout(function () {
      func.apply(context, args)
    }, wait)
  }
};

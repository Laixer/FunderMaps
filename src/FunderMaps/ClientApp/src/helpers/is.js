
/**
 * 
 * https://github.com/jonschlinkert/isobject
 */
export const isObject = (val) => {
  return val != null && typeof val === 'object' && Array.isArray(val) === false;
}

/**
 * https://stackoverflow.com/a/643827
 */
export const isDate = (val) => {
  return Object.prototype.toString.call(val) === '[object Date]'
}
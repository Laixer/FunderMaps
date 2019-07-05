/**
 * Helper functions for Vue render function
 */

/**
 * Merge static classes, if any, into a classList object
 * 
 * @param {} param0 
 */
export const combineClassLists = ({ classList, context }) => {
  if (context.data.staticClass) {
    classList[context.data.staticClass] = true
  }
  return classList;
}

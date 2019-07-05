
/**
 * The menu item model
 */


/**
 * Provide meaningful defaults for MenuItems
 *  Used in SideBar and Profile Menu
 */
let ProgressStepModel = function ({
  status, icon, step, to, clickable
}) {
  this.status = status || 'disabled';
  this.iconName = icon || false;
  this.step = step || 1;
  this.to = to || { name: 'not-found' }
  this.clickable = clickable || false;
}

export default ProgressStepModel;

/**
 * The menu item model
 */

import { icon } from 'helper/assets';

/**
 * Provide meaningful defaults for MenuItems
 *  Used in SideBar and Profile Menu
 */
let MenuItemModel = function ({
  label, active, icon, notifications, to
}) {
  this.label = label || 'Label'
  this.active = active || false;
  this.iconName = icon || false;
  this.notifications = notifications || false;
  this.to = to || { name: 'not-found' }
}

MenuItemModel.prototype.icon = function() {
  return icon({ name: this.iconName })
}
MenuItemModel.prototype.hasIcon = function() {
  return !! this.iconName
}
MenuItemModel.prototype.isActive = function() {
  return !! this.active
}

export default MenuItemModel;
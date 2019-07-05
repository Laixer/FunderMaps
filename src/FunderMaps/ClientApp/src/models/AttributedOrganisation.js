
import { generateAvatar } from 'utils/namedavatar';

/**
 * Mostly just a pretty wrapper for now
 */
// TODO: Can we remove 'role'?
let AttributedOrganisation = function ({ org, role }) {
  Object.assign(this, org);
  this.role = role;
}

AttributedOrganisation.prototype.getId = function() {
  return this.id;
}

AttributedOrganisation.prototype.getName = function() {
  return this.name;
}

AttributedOrganisation.prototype.getRole = function() {
  return this.role;
}

// ****************************************************************************
//  Avatar
// ****************************************************************************

/**
 * Attributed users don't have an avatar prop, so straight to generated avatar
 */
AttributedOrganisation.prototype.getAvatar = function() {
  return generateAvatar({ name: this.getName() })
}


export default AttributedOrganisation;
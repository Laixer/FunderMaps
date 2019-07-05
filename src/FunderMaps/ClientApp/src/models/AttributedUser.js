
// email: "batman@gotham.gov"
// first_name: "Bruce"
// id: 30141
// last_name: "Wayne"
// middle_name: null
// nick_name: "bruce"
// organization: {id: 85615, name: "Gotham City"}
// phone: "+19001117774"

import { generateAvatar } from 'utils/namedavatar';

/**
 * Just a pretty wrapper for now
 */
let AttributedUser = function ({user, role}) {
  Object.assign(this, user);
  this.role = role;
}

AttributedUser.prototype.getName = function() {
  return this.first_name + ' ' + this.last_name;
}
// ****************************************************************************
//  User Name
// ****************************************************************************

/**
 * Aim to get the most natural name by which to identify the user
 */
AttributedUser.prototype.getUserName = function() {

  // First try given and/or last name
  let name = '';
  if (this.first_name) {
    name += this.first_name;
  }
  if (this.middle_name) {
    name += ' ' + this.middle_name
  }  
  if (this.last_name) {
    name += ' ' + this.last_name
  }
  if (name) {
    return name.trim();
  }
  // Check for a specific user_name as alternative
  if (this.nick_name) {
    return this.nick_name
  }
  // Resort to email
  return this.email;
}

AttributedUser.prototype.getRole = function() {
  return this.role;
}

// ****************************************************************************
//  Avatar
// ****************************************************************************

/**
 * Attributed users don't have an avatar prop, so straight to generated avatar
 */
AttributedUser.prototype.getAvatar = function() {
  return generateAvatar({ name: this.getUserName() })
}


export default AttributedUser;
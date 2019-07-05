
// {
//   "given_name": "string",
//   "last_name": "string",
//   "avatar": "string",
//   "job_title": "string",
//   "two_factor_enabled": true,
//   "phone_number_confirmed": true,
//   "email_confirmed": true,
//   "lockout_enabled": true,
//   "id": "string",
//   "user_name": "string",
//   "email": "string",
//   "phone_number": "string",
//   "lockout_end": "2019-05-27T21:09:25.212Z"
// }

import { generateAvatar } from 'utils/namedavatar'

/**
 * Just a pretty wrapper for now
 */
let OrgUserModel = function (user) {
  Object.assign(this, user);
}

// ****************************************************************************
//  User Name
// ****************************************************************************

/**
 * Aim to get the most natural name by which to identify the user
 */
OrgUserModel.prototype.getUserName = function() {
  let user = this.user;

  // First try given and/or last name
  let name = '';
  if (user.given_name) {
    name += user.given_name;
  }
  if (user.last_name) {
    name += ' ' + user.last_name
  }
  if (name) {
    return name.trim();
  }
  // Check for a specific user_name as alternative
  if (user.user_name) {
    return user.user_name
  }
  // Resort to email
  return user.email;
}

// ****************************************************************************
//  Role
// ****************************************************************************

/**
 * The translated role name
 */
OrgUserModel.prototype.getRoleName = function() {
  switch(this.role.name) {
    case 'Admin':
      return 'Admin';
    case 'Superuser':
      return 'Beheerder';
    case 'Verifier':
      return 'Reviewer'
    case 'Writer':
      return 'Verwerker'
    case 'Reader':
      return 'Alleen lezen'
  }
  return this.role.name
}

/**
 * Whether or not the user is able to approve / disapprove reports
 */
OrgUserModel.prototype.canReview = function() {
  return ['Admin', 'Superuser', 'Reviewer'].includes(this.role.name);
}

/**
 * Whether or not the user is able to create and edit reports
 */
OrgUserModel.prototype.canCreate = function() {
  return ['Admin', 'Superuser', 'Creator'].includes(this.role.name);
}

// ****************************************************************************
//  Avatar
// ****************************************************************************

/**
 * Get the avatar image
 */
OrgUserModel.prototype.getAvatar = function() {
  if (this.hasAvatar()) {
    return this.user.avatar
  }
  return this.generateAvatar();
}

/**
 * Whether the user has uploaded an Avatar
 */
OrgUserModel.prototype.hasAvatar = function() {
  return this.user.avatar !== '' && this.user.avatar !== null
}

/**
 * Generate a default avatar
 */
OrgUserModel.prototype.generateAvatar = function() {
  return generateAvatar({ name: this.getUserName() })
}

export default OrgUserModel;
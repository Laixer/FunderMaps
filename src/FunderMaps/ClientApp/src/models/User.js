
/**
 * The User model - contains the profile information of the active user
 * 
 * Note: look at the OrgUser model for the (profile) information superusers 
 *       and admins can access regarding other users.
 */

import { generateAvatar } from 'utils/namedavatar'

let UserModel = function ({
  given_name, last_name, avatar, job_title, phone_number
}) {
  this.given_name = given_name || ''
  this.last_name = last_name || ''
  this.avatar = avatar || ''
  this.job_title = job_title || ''
  this.phone_number = phone_number || ''
}

// ****************************************************************************
//  Rights
// ****************************************************************************

// TODO: implement
UserModel.prototype.canReview = function() {
  return false
}

UserModel.prototype.canCreate = function() {
  return false
}

UserModel.prototype.canReview = function() {
  return false
}

// ****************************************************************************
//  Username
// ****************************************************************************

/**
 * Aim to get the most natural name by which to identify the user
 */
UserModel.prototype.getUserName = function() {
  
  // First try given and/or last name
  let name = '';
  if (this.given_name) {
    name += this.given_name;
  }
  if (this.last_name) {
    name += ' ' + this.last_name
  }
  if (name) {
    return name.trim();
  }
  
  // ...
  return this.job_title || 'Me';
}

// ****************************************************************************
//  Avatar
// ****************************************************************************

/**
 * Get the avatar image
 */
UserModel.prototype.getAvatar = function() {
  if (this.hasAvatar()) {
    return this.avatar
  }
  return this.generateAvatar();
}

/**
 * Whether the user has uploaded an Avatar
 */
UserModel.prototype.hasAvatar = function() {
  return this.avatar !== '' && this.avatar !== null
}

/**
 * Generate a default avatar
 */
UserModel.prototype.generateAvatar = function() {
  return generateAvatar({ name: this.getUserName() })
}


export default UserModel;
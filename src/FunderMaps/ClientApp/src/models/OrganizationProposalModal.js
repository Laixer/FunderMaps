
// {
//   id, name, email, token
// }

/**
 * Just a pretty wrapper for now
 */
let OrganizationProposalModel = function (org) {
  Object.assign(this, org);
}

OrganizationProposalModel.prototype.getId = function() {
  return this.id;
}

export default OrganizationProposalModel;
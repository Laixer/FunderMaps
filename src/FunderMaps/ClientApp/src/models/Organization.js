// Object as returned by API 
// 
// {  
//   "id":"50a33362-774e-481c-9101-987b831630a4",
//   "name":"Gotham City",
//   "email":"city@gotham.gov",
//   "phone_number":"+19008886591",
//   "registration_number":null,
//   "branding_logo":null,
//   "invoice_name":"Wayne Enterprises",
//   "invoice_po_number":"WA-9901",
//   "invoice_email":"invoice@gotham.gov",
//   "home_street":"City Hall Park",
//   "home_address_number":1,
//   "home_address_number_postfix":null,
//   "home_city":"Gotham",
//   "home_postbox":null,
//   "home_zipcode":"10007",
//   "home_state":"New Jersey",
//   "home_country":null,
//   "postal_street":"City Hall Park",
//   "postal_address_number":1,
//   "postal_address_number_postfix":null,
//   "postal_city":"Gotham",
//   "postal_postbox":null,
//   "postal_zipcode":"10007",
//   "postal_state":"New Jersey",
//   "postal_country":null
// };

/**
 * Just a pretty wrapper for now
 */
let OrganizationModel = function (org) {
  Object.assign(this, org);
}

OrganizationModel.prototype.getId = function() {
  return this.id;
}

export default OrganizationModel;
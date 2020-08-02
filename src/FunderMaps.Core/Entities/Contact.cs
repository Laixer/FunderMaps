using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Contact information.
    /// </summary>
    public sealed class Contact : BaseEntity
    {
        /// <summary>
        ///     Contact email.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Contact name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Contact phone number.
        /// </summary>
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     Print object as name.
        /// </summary>
        /// <returns>String representing contact.</returns>
        public override string ToString() => Email;

        public override void Validate()
        {
            base.Validate();

            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}

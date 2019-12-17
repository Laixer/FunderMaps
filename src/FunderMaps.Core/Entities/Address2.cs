namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Access entity.
    /// </summary>
    public class Address2 : BaseEntity
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Postcode.
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// Street.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Building number.
        /// </summary>
        public short Number { get; set; }

        /// <summary>
        /// Building number addition.
        /// </summary>
        public string NumberAddition { get; set; }

        /// <summary>
        /// Building letter.
        /// </summary>
        public string NumberLetter { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        public string City { get; set; }
    }
}

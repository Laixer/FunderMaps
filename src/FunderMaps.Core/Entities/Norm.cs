using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    // FUTURE: can be removed.
    /// <summary>
    /// Report norm.
    /// </summary>
    public class Norm
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        [IgnoreDataMember]
        public int Id { get; set; }

        /// <summary>
        /// Conform F3O standard.
        /// </summary>
        public bool? ConformF3o { get; set; }

        /// <summary>
        /// Report object.
        /// </summary>
        [IgnoreDataMember]
        public Report IdNavigation { get; set; }
    }
}

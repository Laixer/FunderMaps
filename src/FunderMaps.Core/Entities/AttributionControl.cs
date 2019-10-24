using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Attribution control.
    /// </summary>
    public class AttributionControl : AccessControl
    {
        /// <summary>
        /// Attribution key.
        /// </summary>
        [IgnoreDataMember]
        public int _Attribution { get; set; }

        /// <summary>
        /// Attribution object.
        /// </summary>
        [Required]
        public Attribution Attribution { get; set; }
    }
}

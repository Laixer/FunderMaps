using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.ViewModels
{
    /// <summary>
    ///     Inquiry status input model.
    /// </summary>
    public sealed class InquiryStatusInputModel
    {
        /// <summary>
        ///     Verification result.
        /// </summary>
        [Required]
        public AuditStatus Status { get; set; }

        /// <summary>
        ///     New status reason.
        /// </summary>
        public string Message { get; set; }
    }
}

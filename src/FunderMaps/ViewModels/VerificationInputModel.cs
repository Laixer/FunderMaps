using System.ComponentModel.DataAnnotations;

namespace FunderMaps.ViewModels
{
    /// <summary>
    /// Report verification input model.
    /// </summary>
    public sealed class VerificationInputModel
    {
        /// <summary>
        /// Posible verification results.
        /// </summary>
        public enum VerificationResult
        {
            /// <summary>
            /// Report is in order and can be marked as verified.
            /// </summary>
            Verified,

            /// <summary>
            /// Report is rejected.
            /// </summary>
            Rejected,
        }

        /// <summary>
        /// Verification result.
        /// </summary>
        [Required]
        public VerificationResult? Result { get; set; }

        /// <summary>
        /// Rejection or approval reason.
        /// </summary>
        public string Message { get; set; }
    }
}

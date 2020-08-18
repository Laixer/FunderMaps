using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Entity status change DTO.
    /// </summary>
    public class StatusChangeDto
    {
        /// <summary>
        ///     Status change message.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Message { get; set; }
    }
}

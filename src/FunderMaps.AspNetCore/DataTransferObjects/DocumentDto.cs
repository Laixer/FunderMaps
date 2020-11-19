using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Document DTO.
    /// </summary>
    public record DocumentDto
    {
        /// <summary>
        ///     Document output name.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; init; }
    }
}

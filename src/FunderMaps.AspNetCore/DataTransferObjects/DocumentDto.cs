using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects;

// TODO: This is not a DTO, but a view model. Move to core? 
/// <summary>
///     Document DTO.
/// </summary>
public record DocumentDto
{
    /// <summary>
    ///     Document output name.
    /// </summary>
    [Required]
    public string? Name { get; init; }
}

using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects;

/// <summary>
///     Reviewer DTO.
/// </summary>
public class ReviewerDto
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Reviewer firstname.
    /// </summary>
    public string GivenName { get; set; }

    /// <summary>
    ///     Reviewer lastname.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    ///     Unique email address.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; }
}

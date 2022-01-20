namespace FunderMaps.AspNetCore.Authorization;

/// <summary>
///     Authorization policy names.
/// </summary>
public static class AuthorizationPolicy
{
    /// <summary>
    ///     Administrator policy.
    /// </summary>
    public const string AdministratorPolicy = nameof(AdministratorPolicy);

    /// <summary>
    ///     Superuser and administrator policy.
    /// </summary>
    public const string SuperuserAdministratorPolicy = nameof(SuperuserAdministratorPolicy);

    /// <summary>
    ///     Superuser policy.
    /// </summary>
    public const string SuperuserPolicy = nameof(SuperuserPolicy);

    /// <summary>
    ///     Verifier and administrator policy.
    /// </summary>
    public const string VerifierAdministratorPolicy = nameof(VerifierAdministratorPolicy);

    /// <summary>
    ///     Verifier policy.
    /// </summary>
    public const string VerifierPolicy = nameof(VerifierPolicy);

    /// <summary>
    ///     Writer and administrator policy.
    /// </summary>
    public const string WriterAdministratorPolicy = nameof(WriterAdministratorPolicy);

    /// <summary>
    ///     Writer policy.
    /// </summary>
    public const string WriterPolicy = nameof(WriterPolicy);

    /// <summary>
    ///     Reader and administrator policy.
    /// </summary>
    public const string ReaderAdministratorPolicy = nameof(ReaderAdministratorPolicy);

    /// <summary>
    ///     Reader policy.
    /// </summary>
    public const string ReaderPolicy = nameof(ReaderPolicy);
}

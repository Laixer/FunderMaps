namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Record control.
    /// </summary>
    public abstract class AccessControl : RecordControl
    {
        /// <summary>
        /// Record access policy.
        /// </summary>
        public AccessPolicy AccessPolicy { get; set; } = AccessPolicy.Private;

        /// <summary>
        /// Is record public.
        /// </summary>
        /// <returns>True if public.</returns>
        public bool IsPublic() => AccessPolicy == AccessPolicy.Public;

        /// <summary>
        /// Is record private.
        /// </summary>
        /// <returns>True if private.</returns>
        public bool IsPrivate() => AccessPolicy == AccessPolicy.Private;
    }
}

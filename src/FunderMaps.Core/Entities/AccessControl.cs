﻿using FunderMaps.Core.Types;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Record control.
    /// </summary>
    public abstract class AccessControl : RecordControl
    {
        /// <summary>
        ///     Record access policy.
        /// </summary>
        /// <remarks>Default to <see cref="AccessPolicy.Private"/>.</remarks>
        public AccessPolicy AccessPolicy { get; set; } = AccessPolicy.Private;

        /// <summary>
        ///     Is record public.
        /// </summary>
        /// <returns><c>True</c> if public.</returns>
        public bool IsPublic => AccessPolicy == AccessPolicy.Public;

        /// <summary>
        ///     Is record private.
        /// </summary>
        /// <returns><c>True</c> if private.</returns>
        public bool IsPrivate => AccessPolicy == AccessPolicy.Private;
    }
}

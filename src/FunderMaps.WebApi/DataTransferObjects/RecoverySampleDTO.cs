using FunderMaps.Core.Types;
using System;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Recovery sample DTO.
    /// </summary>
    public sealed class RecoverySampleDto
    {
        /// <summary>
        ///     Recovery sample identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Project sample identifier.
        /// </summary>
        public int Recovery { get; set; }

        /// <summary>
        ///     Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     Status.
        /// </summary>
        public RecoveryStatus Status { get; set; }

        /// <summary>
        ///     Type.
        /// </summary>
        public RecoveryType Type { get; set; }

        /// <summary>
        ///     Pile type.
        /// </summary>
        public PileType PileType { get; set; }

        /// <summary>
        ///     Contractor organization identifier.
        /// </summary>
        public Guid? Contractor { get; set; }

        /// <summary>
        ///     Facade.
        /// </summary>
        public Facade[] Facade { get; set; }

        /// <summary>
        ///     Permit.
        /// </summary>
        public string Permit { get; set; }

        /// <summary>
        ///     Permit date.
        /// </summary>
        public DateTime PermitDate { get; set; }

        /// <summary>
        ///     Recovery date.
        /// </summary>
        public DateTime RecoveryDate { get; set; }

        /// <summary>
        ///     Record create date.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     Record last update.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        ///     Record delete date.
        /// </summary>
        public DateTime? DeleteDate { get; set; }
    }
}

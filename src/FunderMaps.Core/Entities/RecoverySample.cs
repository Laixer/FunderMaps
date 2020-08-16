using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities.Report;
using FunderMaps.Core.Types;
using System;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Recovery sample entity.
    /// </summary>
    public sealed class RecoverySample : RecordControl<RecoverySample, int>, IReportEntity<RecoverySample>
    {
        public RecoverySample()
            : base(e => e.Id)
        {
        }

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
        [Address]
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
        ///     Recovery object.
        /// </summary>
        public Recovery RecoveryNavigation { get; set; }

        /// <summary>
        ///     Address object.
        /// </summary>
        public Address AddressNavigation { get; set; }

        public override void InitializeDefaults()
        {
            Id = 0;
            CreateDate = DateTime.MinValue;
            UpdateDate = null;
            DeleteDate = null;
            RecoveryNavigation = null;
            AddressNavigation = null;
        }
    }
}

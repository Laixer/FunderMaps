using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Models.Fis
{
    public class FoundationRecovery
    {
        public FoundationRecovery()
        {
            FoundationRecoveryEvidence = new HashSet<FoundationRecoveryEvidence>();
            FoundationRecoveryRepair = new HashSet<FoundationRecoveryRepair>();
        }

        [BindProperty(Name = "id")]
        public int Id { get; set; }

        [BindProperty(Name = "note")]
        public string Note { get; set; }

        [BindProperty(Name = "create_date")]
        public DateTime CreateDate { get; set; }

        [BindProperty(Name = "update_date")]
        public DateTime? UpdateDate { get; set; }

        [BindProperty(Name = "delete_date")]
        public DateTime? DeleteDate { get; set; }

        [Required]
        [BindProperty(Name = "type")]
        public string Type { get; set; }

        [Required]
        [BindProperty(Name = "year")]
        public short? Year { get; set; }

        [Required]
        [BindProperty(Name = "address")]
        public Guid Address { get; set; }

        [Required]
        [BindProperty(Name = "access_policy")]
        public string AccessPolicy { get; set; }

        [Required]
        [BindProperty(Name = "attribution")]
        public int Attribution { get; set; }

        [IgnoreDataMember]
        public virtual AccessPolicy AccessPolicyNavigation { get; set; }

        [IgnoreDataMember]
        public virtual Address AddressNavigation { get; set; }

        [IgnoreDataMember]
        public virtual Attribution AttributionNavigation { get; set; }

        [IgnoreDataMember]
        public virtual FoundationRecoveryType TypeNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FoundationRecoveryEvidence> FoundationRecoveryEvidence { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FoundationRecoveryRepair> FoundationRecoveryRepair { get; set; }
    }
}

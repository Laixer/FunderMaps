﻿using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Extensions;
using System;
using System.Linq;

namespace FunderMaps.IntegrationTests.Faker
{
    public class IncidentFaker : Faker<Incident>
    {
        public Address Address { get; } = new AddressFaker().Generate();
        public Contact Contact { get; } = new ContactFaker().Generate();

        public IncidentFaker()
        {
            RuleFor(f => f.Id, f => f.Random.Replace("FIR######-#####"));
            RuleFor(f => f.ClientId, f => f.Random.Number(1, 99));
            RuleFor(f => f.FoundationType, f => f.PickRandom<FoundationType>());
            RuleFor(f => f.ChainedBuilding, f => f.Random.Bool());
            RuleFor(f => f.Owner, f => f.Random.Bool());
            RuleFor(f => f.FoundationRecovery, f => f.Random.Bool());
            RuleFor(f => f.NeightborRecovery, f => f.Random.Bool());
            RuleFor(f => f.FoundationDamageCause, f => f.PickRandom<FoundationDamageCause>());
            RuleFor(f => f.DocumentFile, f => Enumerable.Range(0, f.Random.Int(0, 3)).Select(x => f.Internet.RemoteFileWithSecureUrl()).ToArray());
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.InternalNote, f => f.Lorem.Text());
            RuleFor(f => f.FoundationDamageCharacteristics, f => f.Random.ArrayElements((FoundationDamageCharacteristics[])Enum.GetValues(typeof(FoundationDamageCharacteristics))));
            RuleFor(f => f.EnvironmentDamageCharacteristics, f => f.Random.ArrayElements((EnvironmentDamageCharacteristics[])Enum.GetValues(typeof(EnvironmentDamageCharacteristics))));
            RuleFor(f => f.Email, f => Contact.Email);
            RuleFor(f => f.Address, f => Address.Id);
            RuleFor(f => f.AuditStatus, f => f.PickRandom<AuditStatus>());
            RuleFor(f => f.QuestionType, f => f.PickRandom<IncidentQuestionType>());
            RuleFor(f => f.Meta, f => new { Gateway = f.Commerce.Product() });
        }
    }
}

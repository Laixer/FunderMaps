using Bogus;
using Bogus.Extensions;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Extensions;
using FunderMaps.WebApi.DataTransferObjects;
using System;
using System.Linq;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="IncidentDto"/>.
    /// </summary>
    public class IncidentDtoFaker : Faker<IncidentDto>
    {
        public IncidentDtoFaker()
        {
            RuleFor(f => f.Id, f => f.Random.Replace("FIR######-#####"));
            RuleFor(f => f.ClientId, f => f.Random.Number(1, 99));
            RuleFor(f => f.FoundationType, f => f.PickRandom<FoundationType>());
            RuleFor(f => f.ChainedBuilding, f => f.Random.Bool());
            RuleFor(f => f.Owner, f => f.Random.Bool());
            RuleFor(f => f.FoundationRecovery, f => f.Random.Bool());
            RuleFor(f => f.NeighborRecovery, f => f.Random.Bool());
            RuleFor(f => f.FoundationDamageCause, f => f.PickRandom<FoundationDamageCause>());
            RuleFor(f => f.DocumentFile, f => Enumerable.Range(0, f.Random.Int(0, 3)).Select(x => f.System.FileName()).ToArray());
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.InternalNote, f => f.Lorem.Text());
            RuleFor(f => f.AuditStatus, f => f.PickRandom<AuditStatus>());
            RuleFor(f => f.QuestionType, f => f.PickRandom<IncidentQuestionType>());
            RuleFor(f => f.FoundationDamageCharacteristics, f => f.Random.ArrayElements((FoundationDamageCharacteristics[])Enum.GetValues(typeof(FoundationDamageCharacteristics))));
            RuleFor(f => f.EnvironmentDamageCharacteristics, f => f.Random.ArrayElements((EnvironmentDamageCharacteristics[])Enum.GetValues(typeof(EnvironmentDamageCharacteristics))));
            RuleFor(f => f.Email, f => f.Internet.Email());
            RuleFor(f => f.Name, f => f.Person.FullName.OrNull(f, .3f));
            RuleFor(f => f.PhoneNumber, f => f.Phone.PhoneNumber("###########").OrNull(f, .3f));
            RuleFor(f => f.Address, f => $"gfm-{f.Random.Hash(32)}");
        }
    }
}

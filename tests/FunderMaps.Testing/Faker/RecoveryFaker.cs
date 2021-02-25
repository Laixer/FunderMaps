using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Extensions;
using System;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="Recovery"/>.
    /// </summary>
    public class RecoveryFaker : Faker<Recovery>
    {
        public RecoveryFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.Type, f => f.PickRandom<RecoveryDocumentType>());
            RuleFor(f => f.DocumentFile, f => f.Internet.RemoteFileWithSecureUrl());
            RuleFor(f => f.DocumentDate, f => f.Date.Between(DateTime.Parse("1000-01-01"), DateTime.Now));
            RuleFor(f => f.DocumentName, f => f.Commerce.Product());
        }
    }
}

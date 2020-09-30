using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Contact repository.
    /// </summary>
    internal class ContactRepository : RepositoryBase<Contact, string>, IContactRepository
    {
        /// <summary>
        ///     Create new <see cref="Contact"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Contact"/>.</returns>
        public override async ValueTask<string> AddAsync(Contact entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO application.contact(
                    email,
                    name,
                    phone_number)
                VALUES (
                    @email,
                    @name,
                    @phone_number)
                ON CONFLICT DO NOTHING";

            await using var context = await DbContextFactory(sql);

            MapToWriter(context.Command, entity);

            await context.NonQueryAsync(affectedGuard: false);

            return entity.Email;
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.contact";

            return ExecuteScalarUnsignedLongCommandAsync(sql);
        }

        /// <summary>
        ///     Delete <see cref="Contact"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(string email)
        {
            var sql = @"
                DELETE
                FROM    application.contact
                WHERE   email = @email";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("email", email);

            await context.NonQueryAsync();
        }

        private static void MapToWriter(DbCommand cmd, Contact entity)
        {
            cmd.AddParameterWithValue("email", entity.Email);
            cmd.AddParameterWithValue("name", entity.Name);
            cmd.AddParameterWithValue("phone_number", entity.PhoneNumber);
        }

        private static Contact MapFromReader(DbDataReader reader)
            => new Contact
            {
                Email = reader.GetSafeString(0),
                Name = reader.GetSafeString(1),
                PhoneNumber = reader.GetSafeString(2),
            };

        /// <summary>
        ///     Retrieve <see cref="Contact"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Contact"/>.</returns>
        public override async ValueTask<Contact> GetByIdAsync(string email)
        {
            var sql = @"
                SELECT  email,
                        name,
                        phone_number
                FROM    application.contact
                WHERE   email = @email
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("email", email);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve all <see cref="Contact"/>.
        /// </summary>
        /// <returns>List of <see cref="Contact"/>.</returns>
        public override async IAsyncEnumerable<Contact> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  email,
                        name,
                        phone_number
                FROM    application.contact";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Update <see cref="Contact"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask UpdateAsync(Contact entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    UPDATE  application.contact
                    SET     name = @name,
                            phone_number = @phone_number
                    WHERE   email = @email";

            await using var context = await DbContextFactory(sql);

            MapToWriter(context.Command, entity);

            await context.NonQueryAsync();
        }
    }
}

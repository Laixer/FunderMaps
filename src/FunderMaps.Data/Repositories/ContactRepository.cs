using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
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
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public ContactRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

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

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            MapToWriter(cmd, entity);

            // Cannot ensure affect, row can already exist.
            await cmd.ExecuteNonQueryAsync();

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

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("email", email);
            await cmd.ExecuteNonQueryEnsureAffectedAsync();
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

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("email", email);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

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

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderCanHaveZeroRowsAsync();
            while (await reader.ReadAsync())
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

            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);

            MapToWriter(cmd, entity);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }
    }
}

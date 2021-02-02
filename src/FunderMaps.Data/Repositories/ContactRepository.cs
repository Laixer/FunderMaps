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
        // FUTURE: Email should also be normalized.
        /// <summary>
        ///     Create new <see cref="Contact"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Contact"/>.</returns>
        public override async Task<string> AddAsync(Contact entity)
        {
            var sql = @"
                INSERT INTO application.contact(
                    email,
                    name,
                    phone_number)
                VALUES (
                    @email,
                    NULLIF(trim(@name), ''),
                    NULLIF(trim(@phone_number), ''))
                ON CONFLICT DO NOTHING";

            await using var context = await DbContextFactory.CreateAsync(sql);

            MapToWriter(context, entity);

            await context.NonQueryAsync(affectedGuard: false);

            return entity.Email;
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async Task<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.contact";

            await using var context = await DbContextFactory.CreateAsync(sql);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="Contact"/>.
        /// </summary>
        /// <param name="email">Entity email.</param>
        public override async Task DeleteAsync(string email)
        {
            ResetCacheEntity(email);

            var sql = @"
                DELETE
                FROM    application.contact
                WHERE   email = @email";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("email", email);

            await context.NonQueryAsync();
        }

        public static void MapToWriter(DbContext context, Contact entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            context.AddParameterWithValue("email", entity.Email);
            context.AddParameterWithValue("name", entity.Name);
            context.AddParameterWithValue("phone_number", entity.PhoneNumber);
        }

        public static Contact MapFromReader(DbDataReader reader, int offset = 0)
            => new()
            {
                Email = reader.GetSafeString(offset + 0),
                Name = reader.GetSafeString(offset + 1),
                PhoneNumber = reader.GetSafeString(offset + 2),
            };

        /// <summary>
        ///     Retrieve <see cref="Contact"/> by id.
        /// </summary>
        /// <param name="email">Unique identifier.</param>
        /// <returns><see cref="Contact"/>.</returns>
        public override async Task<Contact> GetByIdAsync(string email)
        {
            if (TryGetEntity(email, out Contact entity))
            {
                return entity;
            }

            var sql = @"
                SELECT  -- Contact
                        c.email,
                        c.name,
                        c.phone_number
                FROM    application.contact AS c
                WHERE   c.email = @email
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("email", email);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader));
        }

        /// <summary>
        ///     Retrieve all <see cref="Contact"/>.
        /// </summary>
        /// <returns>List of <see cref="Contact"/>.</returns>
        public override async IAsyncEnumerable<Contact> ListAllAsync(INavigation navigation)
        {
            var sql = @"
                SELECT  -- Contact
                        c.email,
                        c.name,
                        c.phone_number
                FROM    application.contact AS c";

            ConstructNavigation(ref sql, navigation, "c");

            await using var context = await DbContextFactory.CreateAsync(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }

        /// <summary>
        ///     Update <see cref="Contact"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async Task UpdateAsync(Contact entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            ResetCacheEntity(entity);

            var sql = @"
                    UPDATE  application.contact
                    SET     name = NULLIF(trim(@name), '')),
                            phone_number = NULLIF(trim(@phone_number), ''))
                    WHERE   email = @email";

            await using var context = await DbContextFactory.CreateAsync(sql);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }
    }
}

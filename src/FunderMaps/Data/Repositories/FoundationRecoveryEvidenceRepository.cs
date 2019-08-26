using Dapper;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    // TODO: Check the document url if it is an string or an integer.
    // For now the document string is converted to an integer so that it is possible to use the command for the repository.
    // There needs to be a better way to check for the document url.

    /// <summary>
    /// The foundation recovery evidence repository to perform CRUD operations as wel as other operations
    /// </summary>
    public class FoundationRecoveryEvidenceRepository : RepositoryBase<FoundationRecoveryEvidence, string>, IFoundationRecoveryEvidenceRepository
    {
        /// <summary>
        /// Create a new instance of the foundation recovery evidence repository
        /// </summary>
        /// <param name="dbProvider">The database provider</param>
        public FoundationRecoveryEvidenceRepository(DbProvider dbProvider) : base(dbProvider)
        { }

        /// <summary>
        /// Get a report based on the Id
        /// </summary>
        /// <param document="id">The document id</param>
        /// <returns></returns>
        public override async Task<FoundationRecoveryEvidence> GetByIdAsync(string document)
        {
            var sql = @"SELECT * 
                FROM report.foundation_recovery_evidence  
                WHERE document = @Document 
                AND delete_date IS NULL";

            var parameters = new { Document = document };
            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<FoundationRecoveryEvidence>(sql, parameters));
            return result.First();
        }

        /// <summary>
        /// Return a list of foundation recovery evidence items
        /// </summary>
        /// <param name="navigation">The navigation values</param>
        /// <returns>An list of foundation recovery evidences</returns>
        public override async Task<IReadOnlyList<FoundationRecoveryEvidence>> ListAllAsync(Navigation navigation)
        {
            var sql = @"
                SELECT * 
                FROM report.foundation_recovery_evidence 
                WHERE delete_date IS NULL
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<FoundationRecoveryEvidence>(sql, navigation));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Return a list of items based on the organization id
        /// </summary>
        /// <param name="org_id">The organization Id</param>
        /// <param name="navigation">The navigation values</param>
        /// <returns></returns>
        public async Task<IReadOnlyList<FoundationRecoveryEvidence>> ListAllAsync(int org_id, Navigation navigation)
        {
            var sql = @"
                SELECT * 
                FROM report.foundation_recovery_evidence 
                WHERE document = @Owner 
                AND delete_date IS NULL
                ORDER BY create_date DESC
                OFFSET @Offset
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<FoundationRecoveryEvidence>(sql, new { Owner = org_id.ToString(), navigation.Offset, navigation.Limit }));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Add new foundation recovery evidence to the database
        /// </summary>
        /// <param name="entity">The foundation recovery evidence entity</param>
        /// <returns>The just created evidence</returns>
        public override async Task<FoundationRecoveryEvidence> AddAsync(FoundationRecoveryEvidence entity)
        {
            var type = string.Empty;
            switch (entity.Type)
            {
                case FoundationRecoveryEvidenceType.Permit:
                    type = "permit";
                    break;
                case FoundationRecoveryEvidenceType.FoundationReport:
                    type = "foundation_report";
                    break;
                case FoundationRecoveryEvidenceType.ArchiveReport:
                    type = "archive_report";
                    break;
                case FoundationRecoveryEvidenceType.OwnerEvidence:
                    type = "owner_evidence";
                    break;
            }

            var sql = @"
                INSERT INTO report.foundation_recovery_evidence(  
                        name,
                        document,
                        note,
                        type,
                        recovery)
                VALUES(
                        @Name,
                        @Document,
                        @Note,
                        @Type,
                        @Recovery)
                RETURNING document";

            //TODO: Check that recovery belongs to a user organization.

            // The complete entity is passed as a parameter for the SQL query.
            var id = await RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<string>(sql, new
            {
                entity.Name,
                entity.Document,
                entity.Note,
                type,
                entity.Recovery,
            }));

            // Return the just added object
            return await GetByIdAsync(id);
        }

        /// <summary>
        /// Mark the foundation recovery evidence as deleted
        /// </summary>
        /// <param name="entity">The foundation recovery evidence that needs to be deleted</param>
        /// <returns></returns>
        public override Task DeleteAsync(FoundationRecoveryEvidence entity)
        {
            string sql = @"
                UPDATE report.foundation_recovery_evidence
                SET delete_date = CURRENT_TIMESTAMP
                WHERE document = @Document
                AND delete_date IS NULL";

            // Return the SQL command for deleting the entity
            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Update the foundation recovery evidence
        /// </summary>
        /// <param name="entity">The foundation recovery evidence </param>
        /// <returns>The command for updating the entity</returns>
        public override Task UpdateAsync(FoundationRecoveryEvidence entity)
        {
            var type = string.Empty;
            switch (entity.Type)
            {
                case FoundationRecoveryEvidenceType.Permit:
                    type = "permit";
                    break;
                case FoundationRecoveryEvidenceType.FoundationReport:
                    type = "foundation_report";
                    break;
                case FoundationRecoveryEvidenceType.ArchiveReport:
                    type = "archive_report";
                    break;
                case FoundationRecoveryEvidenceType.OwnerEvidence:
                    type = "owner_evidence";
                    break;
            }

            var sql = @"
                UPDATE report.foundation_recovery_evidence 
                SET                         
                    name = @Name,
                    note = @Note,
                    type = @Type,
                    recovery = @Recovery 
                WHERE document = @Document 
                AND delete_date IS NULL";


            // Return the SQL command for updating the entity
            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, new
            {
                entity.Name,
                entity.Document,
                entity.Note,
                type,
                entity.Recovery,
            }));
        }

        /// <summary>
        /// Get the amount of foundation recovery evidence reports
        /// </summary>
        /// <returns>The command for counting the amount of reports</returns>
        public override Task<uint> CountAsync()
        {
            var sql = @"
                SELECT COUNT(*)
                FROM report.foundation_recovery_evidence";

            return RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<uint>(sql));
        }

        /// <summary>
        /// Get the amount of foundation recovery evidence reports based on the organization id
        /// </summary>
        /// <param name="org_id">The id of the organization</param>
        /// <returns>The command for counting the amount of reports</returns>
        public Task<uint> CountAsync(int org_id)
        {
            var sql = @"
                SELECT COUNT(*)
                FROM report.foundation_recovery_evidence
                WHERE document = @Document";

            return RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<uint>(sql, new { Document = org_id.ToString() }));
        }


    }
}

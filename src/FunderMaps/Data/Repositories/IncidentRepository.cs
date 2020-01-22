using Dapper;
using FunderMaps.Core.Extensions;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using FunderMaps.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    public class IncidentRepository : IIncidentRepository
    {
        private readonly DbProvider _dbProvider;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public IncidentRepository(DbProvider dbProvider) => _dbProvider = dbProvider;

        public async Task SaveIncidentAsync(IncidentInputViewModel entity)
        {
            using var connection = _dbProvider.ConnectionScope();

            {
                var sql1 = @"
                    INSERT INTO application.contact(email, name, phone_number)
	                    VALUES (@Email, @Name, @Phonenumber)
	                ON CONFLICT DO NOTHING";

                if (!string.IsNullOrEmpty(entity.Email))
                {
                    await connection.ExecuteAsync(sql1, new
                    {
                        entity.Name,
                        entity.Email,
                        entity.Phonenumber
                    });
                }
            }

            var sql = @"
                INSERT INTO application.incident(
                    address,
                    foundation_type,
                    chained_building,
                    owner,
                    foundation_recovery,
                    foundation_damage_cause,
                    document_name,
                    contact,
                    foundation_damage_characteristics,
                    environment)
	            VALUES (
                    @ConvAddress,
                    @ConvFoundationType::application.foundation_type,
                    @ChainedBuilding,
                    @Owner,
                    @FoundationRecovery,
                    @ConvFoundationDamageCause::application.foundation_damage_cause,
                    @DocumentName,
                    @Email,
                    @ConvFoundationDamageCharacteristics::application.foundation_damage_characteristics[],
                    @ConvEnvironmentDamageCharacteristics::application.environment_damage_characteristics[])";

            var dynamicParameters = new DynamicParameters(entity);
            dynamicParameters.Add("ConvFoundationType", entity.FoundationType.ToMemberName());
            dynamicParameters.Add("ConvFoundationDamageCause", entity.FoundationDamageCause.ToMemberName());
            dynamicParameters.Add("ConvAddress", entity.Address.Id);
            dynamicParameters.Add("ConvFoundationDamageCharacteristics", "{" + string.Join(",", entity.FoundationDamageCharacteristics.Select(s => s.ToMemberName())) + "}");
            dynamicParameters.Add("ConvEnvironmentDamageCharacteristics", "{" + string.Join(",", entity.EnvironmentDamageCharacteristics.Select(s => s.ToMemberName())) + "}");

            await connection.ExecuteScalarAsync<int>(sql, dynamicParameters);
        }
    }
}

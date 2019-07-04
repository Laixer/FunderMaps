using System.Threading.Tasks;
using FunderMaps.Core.Event;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;

namespace FunderMaps.Event.Handlers
{
    internal class UpdateUserProfileHandler : IEventTriggerHandler<IUpdateUserProfileEvent>
    {
        private readonly IConfiguration _configuration;

        public UpdateUserProfileHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task HandleEventAsync(IUpdateUserProfileEvent triggerEvent)
        {
            var user = triggerEvent.User;

            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("FISConnection")))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"SELECT attestation.update_principal(@GivenName, NULL, @LastName, @PhoneNumber, @AttestationPrincipalId);", user);
            }
        }
    }
}

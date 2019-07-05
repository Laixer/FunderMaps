using System.Threading.Tasks;
using FunderMaps.Core.Event;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;

namespace FunderMaps.Event.Handlers
{
    /// <summary>
    /// Handle user profile updates.
    /// </summary>
    internal class UpdateUserProfileHandler : IEventTriggerHandler<IUpdateUserProfileEvent>
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public UpdateUserProfileHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Handle the event.
        /// </summary>
        /// <param name="triggerEvent">Trigger event paramenters.</param>
        /// <returns></returns>
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

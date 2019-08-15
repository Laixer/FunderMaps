using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;
using Laixer.EventBus;
using Laixer.EventBus.Handler;
using System.Threading;

namespace FunderMaps.Event.Handlers
{
    /// <summary>
    /// Handle user profile updates.
    /// </summary>
    internal class UpdateUserProfileHandler : IEventHandler<IUpdateUserProfileEvent>
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
        /// <param name="context">Trigger event context.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task HandleEventAsync(EventHandlerContext<IUpdateUserProfileEvent> context, CancellationToken cancellationToken = default)
        {
            var user = context.Event.User;

            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("FISConnection")))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"SELECT attestation.update_principal(@GivenName, NULL, @LastName, @PhoneNumber, @AttestationPrincipalId);", user);
            }
        }
    }
}

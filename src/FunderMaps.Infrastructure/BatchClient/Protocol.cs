using FunderMaps.BatchNode;

namespace FunderMaps.Infrastructure.BatchClient
{
    /// <summary>
    ///     Client connector to the batch node.
    /// </summary>
    public static class Protocol
    {
        /// <summary>
        ///     Protocol version.
        /// </summary>
        public const int protocolVersion = 0xa1;

        /// <summary>
        ///     Construct protocol header.
        /// </summary>
        /// <param name="userAgent">User agent string.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Created protocol header.</returns>
        public static FunderMapsProtocol BuildProtocol(string userAgent, long errorCode = 0)
            => new FunderMapsProtocol
            {
                Version = protocolVersion,
                UserAgent = userAgent,
                ErrorCode = errorCode,
            };

        /// <summary>
        ///     Test if the protocol is compatible.
        /// </summary>
        public static bool IsCompatible(FunderMapsProtocol protocol)
            => protocol.Version == Protocol.protocolVersion;
    }
}

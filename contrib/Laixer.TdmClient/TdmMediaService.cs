using RestSharp;
using System;

namespace TdmClient
{
    /// <summary>
    /// The TDM media service can download files from media identifiers.
    /// </summary>
    public class TdmMediaService
    {
        /// <summary>
        /// These constants are defined in the following application
        /// programming interface documentation documents:
        /// 
        /// 1.) 'Media - geschaald opvragen brainbay v1.2.pdf' section 2.2
        /// 
        /// NOTE: Any future amends in the application programming
        ///       interface *should* only change these constants.
        /// </summary>
        private class TdmMediaConstants
        {
            public const string MediaBaseUri = "https://media.nvm.nl/";
        }

        private readonly Lazy<IRestClient> restClient;

        protected IRestClient RemoteClient { get => restClient.Value; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="baseUri">Remote service base uri.</param>
        /// <param name="config"><see cref="TdmConfig"/>.</param>
        public TdmMediaService(string baseUri, TdmConfig config)
        {
            restClient = new Lazy<IRestClient>(() => new RestClient(baseUri));
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="config"><see cref="TdmConfig"/>.</param>
        public TdmMediaService(TdmConfig config)
            : this(TdmMediaConstants.MediaBaseUri, config)
        {
        }

        // TODO: Handle error 404
        // TODO: make async
        public byte[] FetchResource(string resource)
        {
            var request = new RestRequest(resource);
            return RemoteClient.DownloadData(request);
        }

        // TODO: Handle error 400
        // TODO: Handle error 404
        // TODO: make async
        public byte[] FetchResourceCustomSize(string resource, int width, int height)
        {
            var request = new RestRequest($"/{width}x{height}/{resource}");
            return RemoteClient.DownloadData(request);
        }

        // TODO: Handle error 400
        // TODO: Handle error 404
        // TODO: make async
        public byte[] FetchResource256(string resource)
        {
            var request = new RestRequest($"/256x/{resource}");
            return RemoteClient.DownloadData(request);
        }

        // TODO: Handle error 400
        // TODO: Handle error 404
        // TODO: make async
        public byte[] FetchResource1024(string resource)
        {
            var request = new RestRequest($"/1024x/{resource}");
            return RemoteClient.DownloadData(request);
        }
    }
}

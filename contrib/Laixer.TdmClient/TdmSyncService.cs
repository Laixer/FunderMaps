using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using TdmClient.Auth;
using TdmClient.Exceptions;

namespace TdmClient
{
    // FUTURE: Suggestions API
    // FUTURE: Split base & impl.
    public class TdmSyncService : ITdmSyncService
    {
        /// <summary>
        /// These constants are defined in the following application
        /// programming interface documentation documents:
        /// 
        /// 1.) 'Distributie Sync API_v1.2 brainbay.pdf' section 2.1.1
        /// 2.) 'Distributie Wonen API V15-13 brainbay.pdf' section 2.1, 3.1.1
        /// 
        /// NOTE: Any future amends in the application programming
        ///       interface *should* only change these constants.
        /// </summary>
        private class TdmSyncConstants
        {
            public const string ProdBaseUri = "https://distributie.nvm.nl/";
            public const string CertBaseUri = "https://distributie.cert.nvm.nl/";
            public const string AuthHandlerUri = "https://gaws.nvm.nl/webservices/oauthwebsite/ndsoauthhandler.ashx";
            public const string AuthHandlerAccUri = "http://gaws.acc.nvm.nl/webservices/oauthwebsite/ndsoauthhandler.ashx";

            public const int WonenApiVersion = 15;
            public const int BusinessApiVersion = 13;
            public const int AlvApiVersion = 21;

            public const string WonenApiOg = "wonen";
            public const string BusinessApiOg = "business";
            public const string AlvApiOg = "alv";

            public const string ObjectApiDetails = "objectdetails";
            public const string ProjectApiDetails = "projectdetails";
            public const string ObjectTypeApiDetails = "objecttypedetails";
        }

        private readonly Lazy<IRestClient> restClient;
        private readonly TdmConfig config;

        protected IRestClient RemoteClient
        {
            get
            {
                if (!restClient.IsValueCreated)
                {
                    restClient.Value.Authenticator = new OAuth1Authenticator(new OAuth1Context
                    {
                        ConsumerKey = config.ConsumerKey,
                        ConsumerSecret = config.ConsumerSecret,
                        RequestTokenEndpoint = config.UseAcceptanceMode ? TdmSyncConstants.AuthHandlerAccUri : TdmSyncConstants.AuthHandlerUri,
                        AccessTokenEndpoint = config.UseAcceptanceMode ? TdmSyncConstants.AuthHandlerAccUri : TdmSyncConstants.AuthHandlerUri,
                    });
                }
                return restClient.Value;
            }
        }

        /// <summary>
        /// Data response type.
        /// </summary>
        public enum ContextType
        {
            Xml,
            Json,
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <remarks>
        /// Postpone performance hit of <see cref="IRestClient"/> on first call.
        /// </remarks>
        /// <param name="baseUri">Remote service base uri.</param>
        /// <param name="config"><see cref="TdmConfig"/>.</param>
        public TdmSyncService(string baseUri, TdmConfig config)
        {
            this.config = config;
            restClient = new Lazy<IRestClient>(() => new RestClient(baseUri));
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="config"><see cref="TdmConfig"/>.</param>
        public TdmSyncService(TdmConfig config)
            : this(config.UseAcceptanceMode ? TdmSyncConstants.CertBaseUri : TdmSyncConstants.ProdBaseUri, config)
        {
        }

        protected async Task<T> SynchronizeBaseAsync<T>(string ogApi, int apiVersion, int offset, uint take = 350, ContextType contextType = ContextType.Xml)
        {
            var request = new RestRequest("/{og}/sync/{version}/v1/synchronize/", Method.GET);
            request.AddUrlSegment("og", ogApi);
            request.AddUrlSegment("version", apiVersion);
            request.AddParameter("take", take > 350 ? 350 : take);
            request.AddParameter("contenttype", contextType == ContextType.Json ? "json" : "xml");
            request.AddParameter("synchronizationidVan", offset);

            var response = await RemoteClient.ExecuteGetTaskAsync<T>(request).ConfigureAwait(false);

            // Possible status codes according to documentation.
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;
                case HttpStatusCode.BadRequest:
                    throw new InvalidRequestParameterException();
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException();
                case HttpStatusCode.Forbidden:
                    throw new ResourceForbiddenException();
                case HttpStatusCode.ServiceUnavailable:
                    throw new RemoteServiceUnavailableException();
                default:
                    break;
            }

            throw new InvalidOperationException($"Unknown HTTP response: {response.StatusCode}");
        }

        public Task<DeserializeObjectsList> WonenSynchronizeAsync(int offset, uint take = 350)
            => SynchronizeBaseAsync<DeserializeObjectsList>(TdmSyncConstants.WonenApiOg, TdmSyncConstants.WonenApiVersion, offset, take, ContextType.Json);

        public Task<DeserializeObjectsList> BusinessSynchronizeAsync(int offset, uint take = 350)
            => SynchronizeBaseAsync<DeserializeObjectsList>(TdmSyncConstants.BusinessApiOg, TdmSyncConstants.BusinessApiVersion, offset, take, ContextType.Json);

        public Task<DeserializeObjectsList> AlvSynchronizeAsync(int offset, uint take = 350)
            => SynchronizeBaseAsync<DeserializeObjectsList>(TdmSyncConstants.AlvApiOg, TdmSyncConstants.AlvApiVersion, offset, take, ContextType.Json);


        protected async Task<string> DetailsBaseRawAsync(string ogApi, int apiVersion, string detailsApi, string id, ContextType contextType = ContextType.Xml)
        {
            var request = new RestRequest("/{og}/{version}/v1/{details}/{guid}", Method.GET);
            request.AddUrlSegment("og", ogApi);
            request.AddUrlSegment("version", apiVersion);
            request.AddUrlSegment("details", detailsApi);
            request.AddUrlSegment("guid", id);
            request.AddParameter("contenttype", contextType == ContextType.Json ? "json" : "xml");

            var response = await RemoteClient.ExecuteGetTaskAsync(request).ConfigureAwait(false);

            // Possible status codes according to documentation.
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content;
                case HttpStatusCode.BadRequest:
                    throw new InvalidRequestParameterException();
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException();
                case HttpStatusCode.Forbidden:
                    throw new ResourceForbiddenException();
                case HttpStatusCode.NotFound:
                    throw new ResourceNotFoundException();
                default:
                    break;
            }

            throw new InvalidOperationException($"Unknown HTTP response: {response.StatusCode}");
        }

        public Task<string> WonenObjectDetailsRawAsync(string id, ContextType contextType)
            => DetailsBaseRawAsync(TdmSyncConstants.WonenApiOg, TdmSyncConstants.WonenApiVersion, TdmSyncConstants.ObjectApiDetails, id, contextType);

        public Task<string> BusinessObjectDetailsRawAsync(string id, ContextType contextType)
            => DetailsBaseRawAsync(TdmSyncConstants.BusinessApiOg, TdmSyncConstants.BusinessApiVersion, TdmSyncConstants.ObjectApiDetails, id, contextType);

        // NOTE: The ALV objects are retrieved via another webservice.
        public async Task<string> AlvObjectDetailsRawAsync(string id, ContextType contextType)
        {
            var request = new RestRequest("/webservices/Distributie/{og}/v1/{details}/{guid}", Method.GET);
            request.AddUrlSegment("og", TdmSyncConstants.AlvApiOg);
            request.AddUrlSegment("details", TdmSyncConstants.ObjectApiDetails);
            request.AddUrlSegment("guid", id);
            request.AddParameter("contenttype", contextType == ContextType.Json ? "json" : "xml");

            var response = await RemoteClient.ExecuteGetTaskAsync(request).ConfigureAwait(false);

            // Possible status codes according to documentation.
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content;
                case HttpStatusCode.BadRequest:
                    throw new InvalidRequestParameterException();
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException();
                case HttpStatusCode.Forbidden:
                    throw new ResourceForbiddenException();
                case HttpStatusCode.NotFound:
                    throw new ResourceNotFoundException();
                default:
                    break;
            }

            throw new InvalidOperationException($"Unknown HTTP response: {response.StatusCode}");
        }

        public Task<string> ProjectDetailsRawAsync(string id, ContextType contextType)
            => DetailsBaseRawAsync(TdmSyncConstants.WonenApiOg, TdmSyncConstants.WonenApiVersion, TdmSyncConstants.ProjectApiDetails, id, contextType);

        public Task<string> ObjectTypeDetailsRawAsync(string id, ContextType contextType)
            => DetailsBaseRawAsync(TdmSyncConstants.WonenApiOg, TdmSyncConstants.WonenApiVersion, TdmSyncConstants.ObjectTypeApiDetails, id, contextType);


        protected async Task<T> DetailsBaseAsync<T>(string ogApi, int apiVersion, string detailsApi, string id, ContextType contextType = ContextType.Xml)
        {
            var request = new RestRequest("/{og}/{version}/v1/{details}/{guid}", Method.GET);
            request.AddUrlSegment("og", ogApi);
            request.AddUrlSegment("version", apiVersion);
            request.AddUrlSegment("details", detailsApi);
            request.AddUrlSegment("guid", id);
            request.AddParameter("contenttype", contextType == ContextType.Json ? "json" : "xml");

            var response = await RemoteClient.ExecuteGetTaskAsync<T>(request).ConfigureAwait(false);

            // Possible status codes according to documentation.
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;
                case HttpStatusCode.BadRequest:
                    throw new InvalidRequestParameterException();
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException();
                case HttpStatusCode.Forbidden:
                    throw new ResourceForbiddenException();
                case HttpStatusCode.NotFound:
                    throw new ResourceNotFoundException();
                default:
                    break;
            }

            throw new InvalidOperationException($"Unknown HTTP response: {response.StatusCode}");
        }

        public Task<T> WonenObjectDetailsAsync<T>(string id)
            => DetailsBaseAsync<T>(TdmSyncConstants.WonenApiOg, TdmSyncConstants.WonenApiVersion, TdmSyncConstants.ObjectApiDetails, id, ContextType.Json);

        public Task<T> BusinessObjectDetailsAsync<T>(string id)
            => DetailsBaseAsync<T>(TdmSyncConstants.BusinessApiOg, TdmSyncConstants.BusinessApiVersion, TdmSyncConstants.ObjectApiDetails, id, ContextType.Json);

        // NOTE: The ALV objects are retrieved via another webservice.
        public async Task<T> AlvObjectDetailsAsync<T>(string id, ContextType contextType = ContextType.Xml)
        {
            var request = new RestRequest("/webservices/Distributie/{og}/v1/{details}/{guid}", Method.GET);
            request.AddUrlSegment("og", TdmSyncConstants.AlvApiOg);
            request.AddUrlSegment("details", TdmSyncConstants.ObjectApiDetails);
            request.AddUrlSegment("guid", id);
            request.AddParameter("contenttype", contextType == ContextType.Json ? "json" : "xml");

            var response = await RemoteClient.ExecuteGetTaskAsync<T>(request).ConfigureAwait(false);

            // Possible status codes according to documentation.
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;
                case HttpStatusCode.BadRequest:
                    throw new InvalidRequestParameterException();
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException();
                case HttpStatusCode.Forbidden:
                    throw new ResourceForbiddenException();
                case HttpStatusCode.NotFound:
                    throw new ResourceNotFoundException();
                default:
                    break;
            }

            throw new InvalidOperationException($"Unknown HTTP response: {response.StatusCode}");
        }

        public Task<T> ProjectDetailsAsync<T>(string id)
            => DetailsBaseAsync<T>(TdmSyncConstants.WonenApiOg, TdmSyncConstants.WonenApiVersion, TdmSyncConstants.ProjectApiDetails, id, ContextType.Json);

        public Task<T> ObjectTypeDetailsAsync<T>(string id)
            => DetailsBaseAsync<T>(TdmSyncConstants.WonenApiOg, TdmSyncConstants.WonenApiVersion, TdmSyncConstants.ObjectTypeApiDetails, id, ContextType.Json);

        /// <summary>
        /// Media identifier type.
        /// </summary>
        protected enum MediaIdType
        {
            MediaGuid,
            ObjectGuid,
            ProjectGuid,
            ObjectTypeGuid,
        }

        protected async Task<T> MediaSearchBaseAsync<T>(string id, MediaIdType mediaType, ContextType contextType = ContextType.Xml)
        {
            var request = new RestRequest("/media/v1/zoeken", Method.GET);

            switch (mediaType)
            {
                case MediaIdType.MediaGuid:
                    request.AddParameter("MediaGuid", id);
                    break;
                case MediaIdType.ObjectGuid:
                    request.AddParameter("ObjectGuid", id);
                    break;
                case MediaIdType.ProjectGuid:
                    request.AddParameter("ProjectGuid", id);
                    break;
                case MediaIdType.ObjectTypeGuid:
                    request.AddParameter("ObjectTypeGuid", id);
                    break;
            }

            request.AddParameter("ContentType", contextType == ContextType.Json ? "json" : "xml");

            var response = await RemoteClient.ExecuteGetTaskAsync<T>(request).ConfigureAwait(false);

            // Possible status codes according to documentation.
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;
                case HttpStatusCode.BadRequest:
                    throw new InvalidRequestParameterException();
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException();
                case HttpStatusCode.Forbidden:
                    throw new ResourceForbiddenException();
                case HttpStatusCode.NotFound:
                    throw new ResourceNotFoundException();
                default:
                    break;
            }

            throw new InvalidOperationException($"Unknown HTTP response: {response.StatusCode}");
        }

        public Task<T> MediaSearchByMediaIdAsync<T>(string id)
            => MediaSearchBaseAsync<T>(id, MediaIdType.MediaGuid, ContextType.Json);

        public Task<T> MediaSearchByMediaIdXmlAsync<T>(string id)
            => MediaSearchBaseAsync<T>(id, MediaIdType.MediaGuid, ContextType.Xml);

        public Task<T> MediaSearchByObjectIdAsync<T>(string id)
            => MediaSearchBaseAsync<T>(id, MediaIdType.ObjectGuid, ContextType.Json);

        public Task<T> MediaObjectTypeProjectAsync<T>(string id)
            => MediaSearchBaseAsync<T>(id, MediaIdType.ProjectGuid, ContextType.Json);

        public Task<T> MediaObjectTypeObjectTypeAsync<T>(string id)
            => MediaSearchBaseAsync<T>(id, MediaIdType.ObjectTypeGuid, ContextType.Json);

        public async Task<DeserializeMediaList> MediaSynchronizeAsync(int offset, uint take = 350)
        {
            async Task<IRestResponse> TryRequest()
            {
                IRestResponse _response = null;
                for (int i = 0; i < 5; ++i)
                {
                    var request = new RestRequest("/media/v1/zoeken", Method.GET);
                    request.AddParameter("take", take > 350 ? 350 : take);
                    request.AddParameter("MutatieIdVan", offset);
                    request.AddParameter("ContentType", "json");

                    _response = await RemoteClient.ExecuteTaskAsync(request).ConfigureAwait(false);
                    if (_response.StatusCode == HttpStatusCode.OK)
                    {
                        return _response;
                    }
                }

                return _response;
            }

            var response = await TryRequest().ConfigureAwait(false);

            // Possible status codes according to documentation.
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return JsonConvert.DeserializeObject<DeserializeMediaList>(response.Content);
                case HttpStatusCode.BadRequest:
                    throw new InvalidRequestParameterException();
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException();
                case HttpStatusCode.Forbidden:
                    throw new ResourceForbiddenException();
                case HttpStatusCode.NotFound:
                    throw new ResourceNotFoundException();
                default:
                    break;
            }

            throw new InvalidOperationException($"Unknown HTTP response: {response.StatusCode}");
        }

        /// <summary>
        /// Object identifier type.
        /// </summary>
        protected enum ObjectIdType
        {
            ProjectGuid,
            ObjectTypeGuid,
        }

        protected async Task<T> ObjectSearchBaseAsync<T>(string id, ObjectIdType? objectType, ContextType contextType = ContextType.Xml)
        {
            var request = new RestRequest("/wonen/15/v1/zoeken", Method.GET);
            request.AddParameter("contenttype", contextType == ContextType.Json ? "json" : "xml");

            if (objectType != null)
            {
                switch (objectType)
                {
                    case ObjectIdType.ProjectGuid:
                        request.AddParameter("ProjectGuid", id);
                        break;
                    case ObjectIdType.ObjectTypeGuid:
                        request.AddParameter("ObjectTypeGuid", id);
                        break;
                }
            }

            var response = await RemoteClient.ExecuteGetTaskAsync<T>(request).ConfigureAwait(false);

            // Possible status codes according to documentation.
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;
                case HttpStatusCode.BadRequest:
                    throw new InvalidRequestParameterException();
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedRequestException();
                case HttpStatusCode.Forbidden:
                    throw new ResourceForbiddenException();
                case HttpStatusCode.NotFound:
                    throw new ResourceNotFoundException();
                default:
                    break;
            }

            throw new InvalidOperationException($"Unknown HTTP response: {response.StatusCode}");
        }

        public Task<T> ObjectSearchAllAsync<T>()
            => ObjectSearchBaseAsync<T>(null, null, ContextType.Json);

        public Task<T> ObjectObjectTypeProjectAsync<T>(string id)
            => ObjectSearchBaseAsync<T>(id, ObjectIdType.ProjectGuid, ContextType.Json);

        public Task<T> ObjectObjectTypeObjectTypeAsync<T>(string id)
            => ObjectSearchBaseAsync<T>(id, ObjectIdType.ObjectTypeGuid, ContextType.Json);
    }
}

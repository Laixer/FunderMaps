using RestSharp;
using System;
using System.Collections.Specialized;

namespace TdmClient.Extensions;

/// <summary>
/// Extensions on <see cref="IRestRequest"/>.
/// </summary>
internal static class RestRequestExtensions
{
    /// <summary>
    /// Add the parameters to the request from a <see cref="NameValueCollection"/>.
    /// </summary>
    /// <param name="restRequest">Request to extend.</param>
    /// <param name="collection">Collection to convert.</param>
    /// <returns></returns>
    public static IRestRequest AddQueryParameterRange(this IRestRequest restRequest, NameValueCollection collection)
    {
        if (restRequest == null)
        {
            throw new ArgumentNullException(nameof(restRequest));
        }

        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        foreach (var item in collection.AllKeys)
        {
            if (item == null) { continue; }
            if (collection[item] == null) { continue; }

            // TODO: Remove existing header

            restRequest.AddQueryParameter(item, collection[item]);
        }

        return restRequest;
    }
}

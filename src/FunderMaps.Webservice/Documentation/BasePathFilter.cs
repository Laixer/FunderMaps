using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FunderMaps.Webservice.Documentation;

/// <summary>
///     Document filter for controller basepath.
/// </summary>
public class BasePathFilter : IDocumentFilter
{
    /// <summary>
    ///     Add basepath prefix to the documentation.
    /// </summary>
    /// <param name="swaggerDoc"><see cref="OpenApiDocument"/></param>
    /// <param name="context"><see cref="DocumentFilterContext"/></param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc is null)
        {
            throw new ArgumentNullException(nameof(swaggerDoc));
        }
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        OpenApiPaths paths = new OpenApiPaths();
        foreach (var path in swaggerDoc.Paths)
        {
            paths.Add($"/api{path.Key}", path.Value);
        }
        swaggerDoc.Paths = paths;
    }
}

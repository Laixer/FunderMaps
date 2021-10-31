using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace FunderMaps.Webservice.Documentation;

/// <summary>
///     Display filter for <see cref="Enum"/> types.
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    /// <summary>
    ///     This creates a human readable description for each enum.
    /// </summary>
    /// <remarks>
    ///     The description is "{integer} {name}".
    /// </remarks>
    /// <param name="schema"><see cref="OpenApiSchema"/></param>
    /// <param name="context"><see cref="SchemaFilterContext"/></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is null)
        {
            throw new ArgumentNullException(nameof(schema));
        }
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();
            foreach (var e in Enum.GetValues(context.Type))
            {
                var index = (int)e;
                var name = e.ToString();
                schema.Enum.Add(new OpenApiString($"{index} {name}"));
            }
        }
    }
}

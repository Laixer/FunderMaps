using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FunderMaps.Extensions
{
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Inject human-friendly descriptions for Operations, Parameters and Schemas based
        /// on XML Comment files if they can be found.
        /// </summary>
        /// <param name="path">Direcoty to find the file.</param>
        /// <param name="file">Documentation file.</param>
        public static void IncludeXmlCommentsIfDocumentation(this SwaggerGenOptions swaggerGenOptions, string path, string file)
        {
            var filePath = Path.Combine(path, file);
            if (File.Exists(filePath))
            {
                swaggerGenOptions.IncludeXmlComments(filePath);
            }
        }
    }
}

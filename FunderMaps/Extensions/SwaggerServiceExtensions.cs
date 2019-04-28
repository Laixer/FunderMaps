using System;
using System.Collections.Generic;
using FunderMaps.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FunderMaps.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Constants.ApplicationName.ToLower(), new Info
                {
                    Version = Constants.ApplicationVersion.ToString(),
                    Title = $"{Constants.ApplicationName} Backend",
                    Description = "Internal API between frontend and backend",
                });

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                });

                options.CustomSchemaIds((type) => type.FullName);
                options.IncludeXmlCommentsIfDocumentation(AppContext.BaseDirectory, $"Documentation{Constants.ApplicationName}.xml");
                options.DescribeAllEnumsAsStrings();
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{Constants.ApplicationName.ToLower()}/swagger.json", $"{Constants.ApplicationName} Backend API");
                options.DocExpansion(DocExpansion.None);
            });

            return app;
        }
    }
}

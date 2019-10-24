using Laixer.Identity.Dapper;
using Laixer.Identity.Dapper.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IdentityBuilder"/> for adding Dapper stores.
    /// </summary>
    public static class IdentityBuilderExtensions
    {
        /// <summary>
        /// Adds an Dapper implementation of identity information stores.
        /// </summary>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddDapperStores(this IdentityBuilder builder, Action<IdentityDapperOptions> options)
        {
            builder.Services.Configure(options);

            AddStores(builder.Services, builder.UserType, builder.RoleType);
            return builder;
        }

        /// <summary>
        /// Add custom user and role store to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="userType"></param>
        /// <param name="roleType"></param>
        private static void AddStores(IServiceCollection services, Type userType, Type roleType)
        {
            var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException($"{nameof(userType)} is not a IdentityUser.");
            }

            var identityRoleType = FindGenericBaseType(roleType, typeof(IdentityRole<>));
            if (identityRoleType == null)
            {
                throw new InvalidOperationException($"{nameof(userType)} is not a NotIdentityRole.");
            }

            var keyType = identityUserType.GenericTypeArguments[0];

            var userStoreType = typeof(UserStore<,>).MakeGenericType(userType, keyType);
            var roleStoreType = typeof(RoleStore<,>).MakeGenericType(roleType, keyType);

            services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
            services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}

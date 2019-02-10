using Microsoft.EntityFrameworkCore.Migrations;

namespace FunderMaps.Extensions
{
    public static class MigrationBuilderExtensions
    {
        /// <summary>
        /// Create database extension if not exist.
        /// </summary>
        /// <param name="name">Extension name.</param>
        public static void EnsureExtension(this MigrationBuilder builder, string name)
        {
            builder.Sql($"CREATE EXTENSION IF NOT EXISTS \"{name}\"");
        }

        /// <summary>
        /// Drop database extension if exists.
        /// </summary>
        /// <param name="name">Extension name.</param>
        public static void DropExtension(this MigrationBuilder builder, string name)
        {
            builder.Sql($"DROP EXTENSION IF EXISTS \"{name}\"");
        }
    }
}

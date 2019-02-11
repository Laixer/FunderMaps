using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FunderMaps.Data.Migrations
{
    public partial class CreateIdentitySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema("identity");

            migrationBuilder.CreateTable(
                name: "role",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, name: "id"),
                    Name = table.Column<string>(maxLength: 256, nullable: true, name: "name"),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true, name: "normalized_name"),
                    ConcurrencyStamp = table.Column<string>(nullable: true, name: "concurrency_stamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, name: "id"),
                    UserName = table.Column<string>(maxLength: 256, nullable: false, name: "username"),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: false, name: "normalized_username"),
                    GivenName = table.Column<string>(maxLength: 256, nullable: true, name: "given_name"),
                    LastName = table.Column<string>(maxLength: 256, nullable: true, name: "last_name"),
                    Email = table.Column<string>(maxLength: 256, name: "email"),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true, name: "normalized_email"),
                    Avatar = table.Column<string>(maxLength: 256, nullable: true, name: "avatar"),
                    EmailConfirmed = table.Column<bool>(nullable: false, defaultValue: false, name: "email_confirmed"),
                    JobTitle = table.Column<string>(nullable: true, name: "job_title"),
                    PasswordHash = table.Column<string>(nullable: true, name: "password_hash"),
                    SecurityStamp = table.Column<string>(nullable: true, name: "security_stamp"),
                    ConcurrencyStamp = table.Column<string>(nullable: true, name: "concurrency_stamp"),
                    PhoneNumber = table.Column<string>(nullable: true, name: "phone_number"),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false, defaultValue: false, name: "phone_number_confirmed"),
                    TwoFactorEnabled = table.Column<bool>(nullable: false, defaultValue: false, name: "two_factor_enabled"),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true, name: "lockout_end"),
                    LockoutEnabled = table.Column<bool>(nullable: false, defaultValue: false, name: "lockout_enabled"),
                    AccessFailedCount = table.Column<int>(nullable: false, name: "access_failed_count"),
                    AttestationPrincipalId = table.Column<int>(nullable: true, name: "attestation_principal_id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role_claim",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, name: "id"),
                    RoleId = table.Column<Guid>(nullable: false, name: "role_id"),
                    ClaimType = table.Column<string>(nullable: true, name: "claim_type"),
                    ClaimValue = table.Column<string>(nullable: true, name: "claim_value")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claim", x => x.Id);
                    table.ForeignKey(
                        name: "fk_role_claim_role_id",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_claim",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, name: "id"),
                    UserId = table.Column<Guid>(nullable: false, name: "user_id"),
                    ClaimType = table.Column<string>(nullable: true, name: "claim_type"),
                    ClaimValue = table.Column<string>(nullable: true, name: "claim_value")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claim", x => x.Id);
                    table.ForeignKey(
                        name: "fk_user_claim_user_id",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_login",
                schema: "identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false, name: "login_provider"),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false, name: "provider_key"),
                    ProviderDisplayName = table.Column<string>(nullable: true, name: "provider_display_name"),
                    UserId = table.Column<Guid>(nullable: false, name: "user_id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_login", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "fk_user_login_user_id",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false, name: "user_id"),
                    RoleId = table.Column<Guid>(nullable: false, name: "role_id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "fk_user_role_role_id",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_role_user_id",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_token",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false, name: "user_id"),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false, name: "name"),
                    Value = table.Column<string>(nullable: true, name: "value")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_token", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "fk_user_token_user_id",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_role_claim_role_id",
                schema: "identity",
                table: "role_claim",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "idx_role_normalized_name",
                schema: "identity",
                table: "role",
                column: "normalized_name",
                unique: true,
                filter: "\"normalized_name\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "idx_user_claim_user_id",
                schema: "identity",
                table: "user_claim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_login_user_id",
                schema: "identity",
                table: "user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_role_role_id",
                schema: "identity",
                table: "user_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_normalized_email",
                schema: "identity",
                table: "user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "idx_user_normalized_username",
                schema: "identity",
                table: "user",
                column: "normalized_username",
                unique: true,
                filter: "\"normalized_username\" IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("role_claim", "identity");

            migrationBuilder.DropTable("user_claim", "identity");

            migrationBuilder.DropTable("user_login", "identity");

            migrationBuilder.DropTable("user_role", "identity");

            migrationBuilder.DropTable("user_token", "identity");

            migrationBuilder.DropTable("role", "identity");

            migrationBuilder.DropTable("user", "identity");

            migrationBuilder.DropSchema("identity");
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace FunderMaps.Helpers
{
    public class Constants
    {
        public static readonly PasswordOptions PasswordPolicy = new PasswordOptions
        {
            RequireDigit = false,
            RequireLowercase = false,
            RequireNonAlphanumeric = false,
            RequireUppercase = false,
            RequiredLength = 6,
            RequiredUniqueChars = 1,
        };

        public const string AdministratorRole = "Administrator";
        public const string SuperuserRole = "Superuser";
        public const string VerifierRole = "Verifier";
        public const string WriterRole = "Writer";
        public const string ReaderRole = "Reader";
    }
}

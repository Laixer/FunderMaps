namespace FunderMaps.AspNetCore.Authorization
{
    public static class AuthorizationPolicy
    {
        public const string AdministratorPolicy = nameof(AdministratorPolicy);
        public const string SuperuserAdministratorPolicy = nameof(SuperuserAdministratorPolicy);
        public const string SuperuserPolicy = nameof(SuperuserPolicy);
        public const string VerifierAdministratorPolicy = nameof(VerifierAdministratorPolicy);
        public const string VerifierPolicy = nameof(VerifierPolicy);
        public const string WriterAdministratorPolicy = nameof(WriterAdministratorPolicy);
        public const string WriterPolicy = nameof(WriterPolicy);
        public const string ReaderAdministratorPolicy = nameof(ReaderAdministratorPolicy);
        public const string ReaderPolicy = nameof(ReaderPolicy);
    }
}

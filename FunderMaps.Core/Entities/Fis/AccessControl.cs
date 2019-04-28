using System;

namespace FunderMaps.Core.Entities.Fis
{
    public enum AccessPolicy
    {
        Public,
        Private,
    }

    public abstract class AccessControl : RecordControl
    {
        public AccessPolicy AccessPolicy { get; set; } = AccessPolicy.Private;

        public bool IsPublic() => AccessPolicy == AccessPolicy.Public;
        public bool IsPrivate() => AccessPolicy == AccessPolicy.Private;
    }
}

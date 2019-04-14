using System;

namespace FunderMaps.Models.Fis
{
    public class AccessControl
    {
        public AccessPolicy AccessPolicy { get; set; }

        public static readonly string Public = "public";
        public static readonly string Private = "private";

        public bool IsPublic() => AccessPolicy.Id == Public;
        public bool IsPrivate() => AccessPolicy.Id == Private;
    }
}

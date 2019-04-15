using System;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Fis
{
    public class AccessControl
    {
        [IgnoreDataMember]
        public string _AccessPolicy { get; set; }

        public AccessPolicy AccessPolicy { get; set; }

        public static readonly string Public = "public";
        public static readonly string Private = "private";

        public bool IsPublic() => AccessPolicy.Id == Public;
        public bool IsPrivate() => AccessPolicy.Id == Private;
    }
}

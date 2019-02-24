using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Models.Fis
{
    public class AccessControl
    {
        public AccessPolicy AccessPolicy { get; set; }

        public bool IsPublic() => AccessPolicy == AccessPolicy.Public;
        public bool IsPrivate() => AccessPolicy == AccessPolicy.Private;
    }
}

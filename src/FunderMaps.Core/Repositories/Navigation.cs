using System;

namespace FunderMaps.Core.Repositories
{
    public class Navigation
    {
        public int Offset { get; set; }
        public int Limit { get; set; }

        public Navigation(int offset, int limit)
        {
            Offset = offset;
            Limit = limit;
        }
    }
}

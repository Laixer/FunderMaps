namespace FunderMaps.Core.Repositories
{
    public class Navigation
    {
        public uint Offset { get; set; }
        public uint Limit { get; set; }

        public Navigation(uint offset, uint limit)
        {
            Offset = offset;
            Limit = limit;
        }
    }
}

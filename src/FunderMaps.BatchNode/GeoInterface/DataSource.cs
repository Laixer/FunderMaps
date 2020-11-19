using FunderMaps.BatchNode.Command;
using FunderMaps.Core.Types;

namespace FunderMaps.BatchNode.GeoInterface
{
    internal abstract class DataSource
    {
        public GeometryFormat Format { get; set; }

        public virtual string Read(CommandInfo commandInfo)
        {
            return ToString();
        }

        public virtual string Write(CommandInfo commandInfo)
        {
            return ToString();
        }
    }
}

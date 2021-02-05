using System.Collections.Generic;
using FunderMaps.Core.Threading.Command;

namespace FunderMaps.Core.MapBundle
{
    internal class LayerSource
    {
        public IEnumerable<string> Layers { get; set; }

        public virtual void Imbue(CommandInfo commandInfo)
        {
            if (Layers != null)
            {
                foreach (var item in Layers)
                {
                    commandInfo.ArgumentList.Add(item.Trim());
                }
            }
        }

        public static LayerSource Enumerable(IEnumerable<string> layers)
            => new LayerSource
            {
                Layers = layers,
            };
    }
}

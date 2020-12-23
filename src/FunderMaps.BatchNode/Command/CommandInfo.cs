using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FunderMaps.BatchNode.Command
{
    /// <summary>
    ///     Command descriptor.
    /// </summary>
    public class CommandInfo
    {
        /// <summary>
        ///     Command filename.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     Command arguments.
        /// </summary>
        public ICollection<string> ArgumentList { get; } = new Collection<string>();

        /// <summary>
        ///     Command environment values.
        /// </summary>
        public IDictionary<string, string> Environment { get; } = new Dictionary<string, string>();

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public CommandInfo()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public CommandInfo(string fileName) => FileName = fileName;
    }
}

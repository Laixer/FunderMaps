using System.IO;

namespace FunderMaps.BatchNode.GeoInterface
{
    internal class FileDataSource : DataSource
    {
        public string PathPrefix { get; set; }
        public string Name { get; set; }
        public string Extension => VectorDatasetBuilder.ExportFormatTuple(Format).Item2;
        public string FileName => $"{Name}{Extension}";

        public override string ToString()
        {
            return !string.IsNullOrEmpty(PathPrefix) ? Path.Combine(PathPrefix, FileName) : FileName;
        }
    }
}

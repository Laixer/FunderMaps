namespace FunderMaps.Core.MapBundle
{
    internal class FileDataSource : DataSource
    {
        public string PathPrefix { get; init; }
        public string Name { get; init; }
        public string Extension { get; init; }
        public string FileName => $"{Name}{Extension}";

        public override string ToString()
        {
            return !string.IsNullOrEmpty(PathPrefix) ? Path.Combine(PathPrefix, FileName) : FileName;
        }
    }
}

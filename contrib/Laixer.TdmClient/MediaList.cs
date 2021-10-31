using Newtonsoft.Json;
using System.Collections.Generic;
using TdmClient.Entities.Media;

namespace TdmClient;

public class SearchResults
{
    [JsonProperty(PropertyName = "@versie")]
    public string Versie { get; set; }

    [JsonProperty(PropertyName = "@resultcount")]
    public int Resultcount { get; set; }

    [JsonProperty(PropertyName = "@skip")]
    public int Skip { get; set; }

    [JsonProperty(PropertyName = "@take")]
    public int Take { get; set; }

    [JsonProperty(PropertyName = "@requesturl")]
    public string Requesturl { get; set; }

    [JsonProperty(PropertyName = "media")]
    public List<Media> Media { get; set; }
}

public class DeserializeMediaList
{
    [JsonProperty(PropertyName = "zoekresultaten")]
    public SearchResults SearchResults { get; set; }
}

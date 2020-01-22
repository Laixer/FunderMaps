using Newtonsoft.Json;
using System;

namespace TdmClient.Entities.Media
{
    public class MediaMeta
    {
        [JsonProperty(PropertyName = "mediaguid")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "isactief")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "mutatieid")]
        public int MutationId { get; set; }

        [JsonProperty(PropertyName = "mutatiedatumtijd")]
        public DateTime MutationDate { get; set; }
    }

    public class MediaReference
    {
        [JsonProperty(PropertyName = "ogsoort")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "objectguid")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "objectid")]
        public int ObjectId { get; set; }
    }

    public class MediaFunctional
    {
        [JsonProperty(PropertyName = "ishoofdmedium")]
        public bool IsMainMedia { get; set; }

        [JsonProperty(PropertyName = "volgnummer")]
        public int Order { get; set; }

        [JsonProperty(PropertyName = "omschrijving")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "mediumgroep")]
        public string Group { get; set; }
    }

    public class MediaFile
    {
        [JsonProperty(PropertyName = "mediummimetype")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "bestandsgrootte")]
        public int Size { get; set; }

        [JsonProperty(PropertyName = "hoogte")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "breedte")]
        public int Width { get; set; }
    }

    public class Media
    {
        [JsonProperty(PropertyName = "meta")]
        public MediaMeta Meta { get; set; }

        [JsonProperty(PropertyName = "referentie")]
        public MediaReference Reference { get; set; }

        [JsonProperty(PropertyName = "functioneel")]
        public MediaFunctional Functional { get; set; }

        [JsonProperty(PropertyName = "bestandsinformatie")]
        public MediaFile File { get; set; }
    }
}

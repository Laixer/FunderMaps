using System;
using System.Collections.Generic;

namespace TdmClient
{
    public class Referentie
    {
        public string ReferentieGUID { get; set; }
        public string ReferentieEntiteit { get; set; }
        public string OGSoort { get; set; }
    }

    public class SynchronizationItem
    {
        public string SynchronizationID { get; set; }
        public DateTime SynchronizationDateTime { get; set; }
        public Referentie Referentie { get; set; }
    }

    public class SynchronizationCollection
    {
        public List<SynchronizationItem> SynchronizationItem { get; set; }
    }

    public class SynchronizationResponse
    {
        //[JsonProperty(PropertyName = "@versie")]
        public string Versie { get; set; }

        //[JsonProperty(PropertyName = "@resultcount")]
        public string Resultcount { get; set; }

        //[JsonProperty(PropertyName = "@requestsynchronizationidvan")]
        public string Requestsynchronizationidvan { get; set; }

        //[JsonProperty(PropertyName = "@requesttake")]
        public string Requesttake { get; set; }

        //[JsonProperty(PropertyName = "@resultsynchronizationidvan")]
        public string Resultsynchronizationidvan { get; set; }

        //[JsonProperty(PropertyName = "@resultsynchronizationidtot")]
        public string Resultsynchronizationidtot { get; set; }

        //[JsonProperty(PropertyName = "@requesturl")]
        public string Requesturl { get; set; }

        public SynchronizationCollection SynchronizationCollection { get; set; }
    }

    public class DeserializeObjectsList
    {
        public SynchronizationResponse SynchronizationResponse { get; set; }
    }
}

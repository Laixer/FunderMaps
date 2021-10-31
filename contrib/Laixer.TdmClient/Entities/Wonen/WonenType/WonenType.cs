using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TdmClient.Entities.Wonen.WonenType;

class WonenType
{
    public WonenObjectTypeBeeld Objecttypebeeld { get; set; }
}

public class WonenObjectTypeBeeld
{
    public WonenObjectType Objecttype { get; set; }
}

public class WonenObjectType
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ObjectTypeGuid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ProjectGuid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? MIDASObjecttypeID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? TIARAObjecttypeID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? NVMVestigingNR { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OfficeInfo MyProperty { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? MIDASProjectID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? TIARAProjectID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime Aanmelddatum { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public UnsubscribeData Afmeldgegevens { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ObjectTypeDetails ObjecttypeDetails { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public WonenTypeWonen Wonen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Media Media { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CoApplicants Medeaanmelders { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public History Historie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ASPNaam { get; set; }
}

public class OfficeInfo
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam1 { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Plaats { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Straat { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Huisnummer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Huisnummertoevoeging { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Postcode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Telefoon { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Internetadres { get; set; }
}

public class UnsubscribeData
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime Afmeldatum { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Reden { get; set; }
}

public class ObjectTypeDetails
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Valuta { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime DatumInvoer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AvailabilityStatus StatusBeschikbaarheid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KoopHuur { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalEenheden { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalVrijEenheden { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime DatumStartBouw { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime DatumOpleveringVanaf { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Inschrijfvoorwaarden { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Wachttijd { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Measurements Maten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Presentation Presentatie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public FinancialInfo FinancieleGegevens { get; set; }
}


public class AvailabilityStatus
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string HuidigeStatus { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string IsActief { get; set; }
}


public class Measurements
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public GrossContent BrutoInhoud { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public LivingArea Woonoppervlakte { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PlotArea Perceeloppervlakte { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public LivingroomArea Woonkameroppervlakte { get; set; }
}

public class GrossContent
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Van { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? TotEnMet { get; set; }
}

public class LivingArea
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Van { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? TotEnMet { get; set; }
}

public class PlotArea
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Van { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? TotEnMet { get; set; }
}

public class LivingroomArea
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Van { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? TotEnMet { get; set; }
}

public class Presentation
{

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Aanbiedingstekst { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string AanbiedingstekstEngels { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public InternetPlacements Internetplaatsingen { get; set; }
}

public class InternetPlacements
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<InternetPlacement> Interplaatsing { get; set; } = new List<InternetPlacement>();
}

public class InternetPlacement
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Plaatsing { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Prijsvermelding { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime DatumVrijgave { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string DatumEind { get; set; }
}

public class FinancialInfo
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RentalPrice Huurprijs { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PurchaseContractPrice KoopAanneemsom { get; set; }
}

public class RentalPrice
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<uint?> Van { get; set; } = new List<uint?>();

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<uint?> TotEnMet { get; set; } = new List<uint?>();
}

public class PurchaseContractPrice
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<uint?> Van { get; set; } = new List<uint?>();

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<uint?> TotEnMet { get; set; } = new List<uint?>();
}

public class WonenTypeWonen
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public WonenTypeWonenDetails WonenDetails { get; set; }
}

public class WonenTypeWonenDetails
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Destination Bestemming { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Praktijkruimte { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Maintenance Onderhoud { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ShedStorage SchuurBerging { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Garden Tuin { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public MainGarden HoofdTuin { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Miscellaneous Diversen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Installation Installatie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ServicesWonen VoorzieningenWonen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Garage Garage { get; set; }

    public Parking Parkeren { get; set; }
}

public class Destination
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string PermanenteBewoning { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Recreation Recreatie { get; set; }
}

public class Recreation
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Recreatiewoning { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string InPark { get; set; }
}

public class Maintenance
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Inside Binnen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Outside Buiten { get; set; }
}

public class Inside
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Waardering { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Memo { get; set; }
}

public class Outside
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Waardering { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Memo { get; set; }
}

public class ShedStorage
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Soort { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Provisions Voorzieningen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public InsulationForms Isolatievormen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? TotaalAantal { get; set; }
}

public class Provisions
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Voorzieningen { get; set; } = new List<string>();

}

public class Garden
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Gardentypes Tuintypen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Positie { get; set; }
}

public class Gardentypes
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Tuintype { get; set; } = new List<string>();
}

public class MainGarden
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Positie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Achterom { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }
}

public class Dimensions
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Lengte { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Breedte { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Oppervlakte { get; set; }
}

public class Miscellaneous
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public InsulationForms Isolatievormen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Roof Dak { get; set; }
}

public class InsulationForms
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Isolatie { get; set; } = new List<string>();
}

public class Roof
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Materials Materialen { get; set; }
}

public class Materials
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Materiaal { get; set; } = new List<string>();
}

public class Installation
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public TypesOfHeating SoortenVerwarming { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Boiler CVKetel { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public TypesOFHotWater SoortenWarmWater { get; set; }
}

public class TypesOfHeating
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Verwarming { get; set; } = new List<string>();
}

public class Boiler
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string CVKetelType { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Bouwjaar { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Brandstof { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Eigendom { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Combiketel { get; set; }
}

public class TypesOFHotWater
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> WarmWater { get; set; } = new List<string>(11);
}

public class ServicesWonen
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Voorziening { get; set; } = new List<string>(); //TOOO: Make Array?? with fixed size of 11?
}

public class Parking
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Facilities Faciliteiten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Toelichting { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Capaciteit { get; set; }
}

public class Facilities
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Faciliteit { get; set; } = new List<string>();
}

public class Garage
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Soorten Soorten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Provisions Voorzieningen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Capaciteit { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Isolatievormen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? TotaalAantalGarages { get; set; }
}

public class Media
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<MediumMutationItem> Medium { get; set; } = new List<MediumMutationItem>();
}

public class MediumMutationItem
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string MediumGroep { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public long? MediumID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public bool IsHoofdMedium { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string FileNaam { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string MediumMimeType { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Omschrijving { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string MediumKenmerk { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string URL { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Guid MediaGuid { get; set; }
}

public class CoApplicants
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<CoApplicant> MedeAanmelder { get; set; } = new List<CoApplicant>();
}

public class CoApplicant
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string NVMVestigingNR { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam1 { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Plaats { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Straat { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Huisnummer { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Huisnummertoevoeging { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Postcode { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Telefoon { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Internetadres { get; set; }
}

public class History
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public HistoryBuy Koop { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public HistoryRent Huur { get; set; }
}

public class HistoryBuy
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<HistoryBuyPrice> Koopprijs { get; set; } = new List<HistoryBuyPrice>();
}

public class HistoryBuyPrice
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<uint?> Van { get; set; } = new List<uint?>();

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<uint?> TotEnMet { get; set; } = new List<uint?>();
}

public class HistoryRent
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<HistoryRentPrice> Huurprijs { get; set; } = new List<HistoryRentPrice>();
}

public class HistoryRentPrice
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<uint?> Van { get; set; } = new List<uint?>();

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<uint?> TotEnMet { get; set; } = new List<uint?>();
}

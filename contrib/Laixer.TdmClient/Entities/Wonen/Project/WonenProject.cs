using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TdmClient.Entities.Wonen.Project;

class WonenProject
{
    public WonenProjectBeeld Projectbeeld { get; set; }
}

public class WonenProjectBeeld
{
    public WonenProjectProject Project { get; set; }
}

public class WonenProjectProject
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ProjectGuid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? MIDASProjectID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? NVMVestigingNR { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OfficeInfo Kantoorgegevens { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime Aanmeldatum { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public UnsubscribeData Afmeldgegevens { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ProjectDetails ProjectDetails { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CoApplicants MedeAanmelders { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Media Media { get; set; }

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

public class ProjectDetails
{
    public string Valuta { get; set; }
    public string Projectnaam { get; set; }
    public Address Adres { get; set; }
    public DateTime DatumInvoer { get; set; }
    public AvailabilityStatus StatusBeschikbaarheid { get; set; }
    public string Koophuur { get; set; }
    public string Bouwvorm { get; set; }
    public uint? AantalEenheden { get; set; }
    public DateTime DatumStartBouw { get; set; }
    public DateTime DatumOpleveringVanaf { get; set; }
    public DateTime DatumStartVerkoop { get; set; }
    public Contacts Contactpersonen { get; set; }
    public Measurements Maten { get; set; }
    public Presentation Presentatie { get; set; }
    public FinancialInfo FinancieleGegevens { get; set; }
}

public class Address
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Postcode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Woonplaats { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Land { get; set; }
}

public class AvailabilityStatus
{

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string HuidigeStatus { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string IsActief { get; set; }
}


public class Contacts
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<Contact> Contactpersoon { get; set; } = new List<Contact>();
}

public class Contact
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Telefoon { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Email { get; set; }
}

public class Measurements
{

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public GrossContent BrutoInhoud { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public LivingArea Woonoppervlakte { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PlotArea Perceeloppervlakte { get; set; }
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

public class Presentation
{

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Aanbiedingstekst { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string AanbiedingstekstEngels { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public InternetPlacements Internetplaatsingen { get; set; }

    public string Website { get; set; }
    public string Omgeving { get; set; }
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

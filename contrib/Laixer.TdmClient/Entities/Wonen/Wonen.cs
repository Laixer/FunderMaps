using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TdmClient.Entities.Wonen;

public class Wonen
{
    public WonenObjectBeeld Objectbeeld { get; set; }
}

public class WonenObjectBeeld
{
    public Guid id { get; set; }
    public WonenObject Object { get; set; }
}

public class WonenObject
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Guid ObjectGuid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Guid? ObjectTypeGuid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Guid? ProjectGuid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? MIDASObjectID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? MIDASProjectID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? MIDASObjecttypeID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public long? ObjectID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? ProjectID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? ObjectTypeID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? WoningvoorraadID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? NVMVestigingNR { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OfficeInfo Kantoorgegevens { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime Aanmelddatum { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime Afmelddatum { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ObjectDetails ObjectDetails { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ProjectInformation ProjectInformatie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ObjectWonen Wonen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public BuildingLand Bouwgrond { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OverigOG OverigOG { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Cadastre Kadaster { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CoApplicants MedeAanmelders { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Media Media { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ObjectTransaction Transactiegegevens { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public History Historie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ASPNaam { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic Openhuizendagen { get; set; }
}

public class ObjectWonen
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public WonenDetails WonenDetails { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ResidentialBuilding Woonhuis { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Apartment Appartement { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Floors Woonlagen { get; set; }
}

public class WonenDetails
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Destination Bestemming { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public SizesAndPositions MatenEnLigging { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Constructionyear Bouwjaar { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Maintenance Onderhoud { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ShedStorage SchuurBerging { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Miscellaneous Diversen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Approvals Keurmerken { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Energylabel Energielabel { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public decimal EPC { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Installation Installatie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ServicesWonen VoorzieningenWonen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Toelichting { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Garden Tuin { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public MainGarden Hoofdtuin { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Garage Garage { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Parking Parkeren { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Durability Duurzaamheid { get; set; }

}

public class Constructionyear
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<YearDescription> JaarOmschrijving { get; set; } = new List<YearDescription>(5);

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Periode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string InAanbouw { get; set; }
}

public class YearDescription
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Jaar { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Omschrijving { get; set; }
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

public class Miscellaneous
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Particularities Bijzonderheden { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public InsulationForms Isolatievormen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Roof Dak { get; set; }
}

public class Particularities
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Bijzonderheid { get; set; } = new List<string>();
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

public class Approvals
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Keurmerk { get; set; } = new List<string>();
}

public class Energylabel
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Energieklasse { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public decimal EnergieIndex { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime Einddatum { get; set; }
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

public class Garden
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Gardentypes Tuintypen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Kwaliteit { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? TotaleOppervlakte { get; set; }
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
    public InsulationForms Isolatievormen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? TotaalAantalGarages { get; set; }
}

public class Soorten
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Soort { get; set; } = new List<string>();
}

public class Provisions
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Voorzieningen { get; set; } = new List<string>();

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

public class InsulationForms
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Isolatie { get; set; } = new List<string>();
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

public class Durability
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic EnergieKenmerken { get; set; }
}

public class EnergyFeatures
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Energie { get; set; } = new List<string>();
}

public class Destination
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string HuidigGebruik { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string HuidigeBestemming { get; set; }

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

public class SizesAndPositions
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Positions Liggingen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? BrutoInhoud { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? GebruiksoppervlakteWoonfunctie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? GebruiksoppervlakteOverigeFuncties { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? BuitenruimtesGebouwgebonden { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? ExterneBergruimte { get; set; }
}

public class Positions
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Ligging { get; set; } = new List<string>();
}

public class ResidentialBuilding
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string SoortWoning { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string TypeWoning { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KenmerkWoning { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KwaliteitWoning { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Woningtypecode { get; set; }
}

public class Apartment
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string SoortAppartement { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KenmerkAppartement { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KwaliteitAppartement { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string OpenPortiek { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Woonlaag { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalWoonlagen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string VVEChecklistAanwezig { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public VVEFeatures VVEKenmerken { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Woningtypecode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public decimal BijdrageVVE { get; set; }
}

public class VVEFeatures
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string InschrijvingKvK { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string VergaderingVVE { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string PeriodiekeBijdrage { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Reservefonds { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Onderhoudsverwachting { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Opstalverzekering { get; set; }
}

public class Floors
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Basement Kelder { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public GroundfloorOrFlat BeganeGrondOfFlat { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<Floor> Verdieping { get; set; } = new List<Floor>();

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<Zolder> Zolder { get; set; } = new List<Zolder>(2);

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<Vliering> Vliering { get; set; } = new List<Vliering>(2);
}

public class Basement
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? VerdiepingNr { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Omschrijving { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalKamers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalSlaapkamers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Livingroom Woonkamer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Kitchen Keuken { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic Badkamer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OtherRooms OverigeRuimten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Ligging { get; set; }
}

public class GroundfloorOrFlat
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? VerdiepingNr { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Omschrijving { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalKamers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalSlaapkamers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Livingroom Woonkamer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Kitchen Keuken { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic Badkamer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OtherRooms OverigeRuimten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Ligging { get; set; }
}

public class Floor
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? VerdiepingNr { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Omschrijving { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalKamers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalSlaapkamers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Livingroom Woonkamer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Kitchen Keuken { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic Badkamer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OtherRooms OverigeRuimten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Ligging { get; set; }
}

public class Zolder //TODO: Translate? --> attic????? will cause overlap with "Zolder" if translated
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? VerdiepingNr { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Omschrijving { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalKamers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalSlaapkamers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Livingroom Woonkamer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Kitchen Keuken { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic Badkamer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OtherRooms OverigeRuimten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Ligging { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AtticOptions ZolderVlieringOpties { get; set; }
}

public class Vliering //TODO: Translate? --> attic????? will cause overlap with "Zolder" if translated
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? VerdiepingNr { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Naam { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Omschrijving { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalKamers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalSlaapkamers { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Livingroom Woonkamer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Kitchen Keuken { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic Badkamer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OtherRooms OverigeRuimten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Ligging { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AtticOptions ZolderVlieringOpties { get; set; }
}

public class Livingroom
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Types Types { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Trap { get; set; }
}

public class Kitchen
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic Types { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Vernieuwd { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Provisions Voorzieningen { get; set; }
}

public class Types
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic Type { get; set; }
}

public class Bathroom
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Provisions Voorzieningen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }
}

public class OtherRooms
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic OverigeRuimte { get; set; }
}

public class AtticOptions
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<AtticOptionItem> ZolderVlieringOptie { get; set; } = new List<AtticOptionItem>();
}

public class AtticOptionItem
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ZolderVlieringOptie { get; set; }
}

public class BuildingLand
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Destination Bestemming { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Oppervlakte { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Bouwrijp { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Positions Liggingen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Toelichting { get; set; }
}

public class OverigOG
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IndoorGarage InpandigeGarage { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Garage Garage { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public UndergroundParkingspace Parkeerkelder { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Parkinglot Parkeerplaats { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Storage Berging { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CaravanSite Woonwagenstandplaats { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public MobileHomePitch Stacaravanstandplaats { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Berth Ligplaats { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public LowerPart Onderstuk { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Storage Opslagruimte { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Woningtypecode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Toelichting { get; set; }

}

public class IndoorGarage
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Facilities Voorzieningen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Capaciteit { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public InsulationForms Isolatievormen { get; set; }
}

public class UndergroundParkingspace
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Facilities Voorzieningen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Capaciteit { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }
}

public class Parkinglot
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Capaciteit { get; set; }
}

public class CaravanSite
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }
}

public class MobileHomePitch
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }
}

public class Berth
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }
}

public class LowerPart
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Provisions Voorzieningen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public InsulationForms Isolatievormen { get; set; }
}

public class Storage
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Provisions Voorzieningen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Dimensions Afmetingen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public InsulationForms Isolatievormen { get; set; }
}

public class ProjectInformation
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? ProjectID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? ObjectTypeID { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Bouwnummer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public FinancialInfo FinancieleGegevens { get; set; }
}

public class FinancialInfo
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Huurprijs { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Koopaanneemsom { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Aanneemsom { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Grondprijs { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public decimal ServicekostenPerMaand { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string GemeentelijkeBepalingen { get; set; }
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
    public short Postcode4 { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Telefoon { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Internetadres { get; set; }
}

public class AddressCoordinates
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public double X { get; set; } // TODO: was int

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public double Y { get; set; } // TODO: was int
}

public class AddressNetherlands
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Straatnaam { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Huisnummer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string HuisnummerToevoeging { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Postcode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public short Postcode4 { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Buurt { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Wijk { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Woonplaats { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Gemeente { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Land { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AddressCoordinates Coordinaten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public BAGidentificationcode BAGidentificatiecode { get; set; }
}

public class BAGidentificationcode
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Gemeentecode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Objecttypecode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Volgnummer { get; set; }
}

public class AddressInternational
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Adresregel1 { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Adresregel2 { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Woonplaats { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Land { get; set; }
}

public class Address
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AddressNetherlands Nederlands { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AddressInternational Internationaal { get; set; }
}

public class ObjectDetails
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Valuta { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Address Adres { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Buy Koop { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Rent Huur { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Auction Veiling { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public decimal? ServicekostenPerMaand { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Aanvaarding { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime DatumAanvaarding { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ObjectAanmelding { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string RedenAanmelding { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Koopmengvorm Koopmengvorm { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime DatumInvoer { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AvailabilityStatus StatusBeschikbaarheid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Bouwvorm { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Vertrouwelijk { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string EigenBelang { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public InternetPlacements Internetplaatsingen { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RentStatus StatusVerhuurd { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string CollegialeInformatie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public InformationObligation Informatieplicht { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Aanbiedingstekst { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string AanbiedingstekstEngels { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CombinationObject CombinatieObject { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Contacts Contactpersonen { get; set; }
}

public class CombinationObject
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public BOGObject BOGObject { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AgriculturalCompany AgrarischBedrijf { get; set; }
}

public class BOGObject
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Situatie { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? ObjectID { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Oppervlakte { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Status { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Guid ObjectGuid { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ObjectDetailsUrl { get; set; }
}

public class AgriculturalCompany
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? ObjectID { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Guid ObjectGuid { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ObjectDetailUrl { get; set; }
}

public class InformationObligation
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Informatie { get; set; } = new List<string>();
}

public class RentStatus
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string NietVerhuurd { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public PartialRented GedeeltelijkVerhuurd { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string VolledigVerhuurd { get; set; }
}

public class PartialRented
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? PercentageVerhuurd { get; set; }
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

public class AvailabilityStatus
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string HuidigeStatus { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string IsActief { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime DatumVoorbehoudTot { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public SoldUnderReservation VerkochtOnderVoorbehoud { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RentedWithRestriction VerhuurdOnderVoorbehoud { get; set; }
}

public class SoldUnderReservation
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime Datum { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Koopprijs { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KoopConditie { get; set; }
}

public class RentedWithRestriction
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime Datum { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Huurprijs { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string HuurConditie { get; set; }
}

public class Koopmengvorm
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Mengvorm { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string AfwijkingVanKoopprijs { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Toelichting { get; set; }
}

public class Auction
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Vanafprijs { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Zoekprijs { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string SoortVeiling { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string VeilingType { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime StartDatumTijd { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime EindDatumTijd { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AuctionSites VeilingSites { get; set; }
}

public class AuctionSites
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> VeilingSite { get; set; } = new List<string>();
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

public class Buy
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Prijsvoorvoegsel { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? Koopprijs { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KoopConditie { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KoopSpecificatie { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public WOZ WOZ { get; set; }
}

public class WOZ
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string WOZWaarde { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string WOZWaardePeildatum { get; set; }
}

public class Rent
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Prijsvoorvoegsel { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? Huurprijs { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string HuurConditie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RentSpecifications HuurSpecificaties { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? KostenHuurder { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? BerekendePunten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? Waarborgsom { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Beschikbaarheid { get; set; }
}

public class ObjectTransaction
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string TransactieConditie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string TransactieDetail { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string RedenIntrekking { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string EconomischeOverdracht { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Toelichting { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Transactieprijs { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? WaarvanPrijsParkeerplaats { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime DatumOndertekeningAkte { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime DatumTransport { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public EstateAgent Makelaar { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Collegiaal { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int AfmeldendKantoor { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public BuyerInformation Koperinformatie { get; set; }
}

public class BuyerInformation
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public HouseholdSituation Huishoudensituatie { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AbandonedObject AchtergelatenObject { get; set; }
}

public class AbandonedObject
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AbandonedObjectObjectDetails ObjectDetails { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AbandonedObjectWonen Wonen { get; set; }
}

public class AbandonedObjectObjectDetails
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Postcode { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Huisnummer { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string HuisnumerToevoeging { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string TypeBewoning { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Transactieprijs { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Huurprijs { get; set; }
}

public class AbandonedObjectWonen
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AbandonedObjectResidentialBuilding Woonhuis { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AbandonedObjectApartment Appartement { get; set; }

}
public class AbandonedObjectResidentialBuilding
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string SoortWoning { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string TypeWoning { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KenmerkWoning { get; set; }
}

public class AbandonedObjectApartment
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string SoortAppartement { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KenmerkAppartement { get; set; }
}

public class HouseholdSituation
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Kopertype { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Inwonend { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? AantalLedenHuishouden { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string SamenstellingHuishouden { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string LeeftijdHoofdkostwinner { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string LeeftijdOudsteKind { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Gezinsinkomen { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RelocationReasons Verhuisredenen { get; set; }
}

public class RelocationReasons
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<string> Verhuisreden { get; set; } = new List<string>();
}

public class EstateAgent
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string NVMVestigingNR { get; set; }
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
    public BuyHistory Koop { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RentHistory Huur { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AuctionHistory Veiling { get; set; }
}

public class AuctionHistory
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<AuctionHistoryItem> Veilinghistorie { get; set; } = new List<AuctionHistoryItem>();
}

public class AuctionHistoryItem
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? VanafPrijs { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Zoekprijs { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string SoortVeiling { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string VeilingType { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime StartDatumTijd { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime EindDatumTijd { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AuctionSites VeilingSites { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime BeginGeldigheid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime EindeGeldigheid { get; set; }
}

public class BuyHistory
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<BuyHistoryItem> Koophistorie { get; set; } = new List<BuyHistoryItem>();
}

public class BuyHistoryItem
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Prijsvoorvoegsel { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Koopprijs { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KoopConditie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic KoopSpecificatie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime BeginGeldigheid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime EindeGeldigheid { get; set; }
}

public class RentHistory
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<RentHistoryItem> Huurhistorie { get; set; } = new List<RentHistoryItem>();
}

public class RentHistoryItem
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Prijsvoorvoegsel { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Huurprijs { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string HuurConditie { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public RentSpecifications HuurSpecificaties { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime BeginGeldigheid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime EindeGeldigheid { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? KostenHuurder { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? BerekendePunten { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? Waarborgsom { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Beschikbaarheid { get; set; }
}

public class RentSpecifications
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public dynamic HuurSpecificatie { get; set; }
}

public class Cadastre
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<CadastralInformation> KadastraleInformatie { get; set; } = new List<CadastralInformation>();
}

public class CadastralInformation
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? KadastraalID { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public CadastralData KadastraleGegevens { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public OwnershipInformation EigendomsGegevens { get; set; }
}

public class CadastralData
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KadastraleGemeente { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string KadastraleGemeenteCode { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Sectie { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Perceel { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Indexnummer { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public uint? Oppervlakte { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Omvang { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Aandeel { get; set; }
}

public class OwnershipInformation
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Soort { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public decimal ErfpachtPerJaar { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string VastVariabel { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Erfpachtduur { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime Einddatum { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Afkoopoptie { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime AfgekochtTot { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string ErfpachtEeuwigdurendAfgekocht { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Erfpachtgever { get; set; }
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

public class OpenHouseDays
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IList<OpenHouseDay> Openhuizendag { get; set; } = new List<OpenHouseDay>();
}

public class OpenHouseDay
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime StartDatumTijd { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DateTime EindDatumTijd { get; set; }
}

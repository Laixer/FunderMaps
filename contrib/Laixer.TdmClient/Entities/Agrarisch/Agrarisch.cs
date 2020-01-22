using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TdmClient.Entities.Agrarisch
{
    public class Agrarisch
    {
        public AgrarischObjectBeeld Objectbeeld { get; set; }
    }

    public class AgrarischObjectBeeld
    {
        public Guid id { get; set; }
        public AgrarischObject Object { get; set; }

    }

    public class AgrarischObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? MIDASObjectID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TIARAObjectID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid ObjectGuid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Aanmelddatum { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Afmelddatum { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NVMVestigingNR { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OfficeInfo KantoorGegevens { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ObjectDetails ObjectDetails { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AenLV AenLV { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingPlot Bouwkavel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Cadastre Kadaster { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CoApplicants MedeAanmelders { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Transactions Transacties { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Media Media { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ASPNaam { get; set; }
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
        public string Aanvaarding { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumAanvaarding { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectAanmelding { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RedenAanmelding { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AardAanmelding { get; set; }

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
        public Contacts Contactpersonen { get; set; }
    }

    public class Address
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AddressNetherlands Nederlands { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AddressInternational Internationaal { get; set; }
    }
    public class AddressNetherlands
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Straatnaam { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HouseNumber Huisnummer { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HuisnummerToevoeging { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Postcode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Woonplaats { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Gemeente { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Provincie { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Land { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AddressCoordinates Coordinaten { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BAGidentificationcode BAGidentificatiecode { get; set; }
    }

    public class HouseNumber
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Hoofdnummer { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Series Reeks { get; set; }
    }

    public class Series
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Begin { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Eind { get; set; }
    }

    public class AddressCoordinates
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double X { get; set; } // TODO: was int

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double Y { get; set; } // TODO: was int
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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AddressCoordinates Coordinaten { get; set; }
    }

    public class Buy
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Prijsvoorvoegsel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Koopprijs { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? KoopprijsPerHa { get; set; }

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
        public string HuurSpecificatie { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bemiddelingskosten { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? BerekendePunten { get; set; }
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
        public string SiteNaam { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SiteAdres { get; set; }
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

    public class InternetPlacements
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<InternetPlacement> Interplaatsing { get; set; } = new List<InternetPlacement>();

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime OpenHuis { get; set; }
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

    public class InformationObligation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Informatie { get; set; } = new List<string>();
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

    public class AenLV
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MainFunction Hoofdfunctie { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SecondaryFunction Nevenfunctie { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingBlocks Bouwblokken { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FieldPlotSurface VeldkavelsOppervlakte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HousePlotSurface HuiskavelOppervlakteMyProperty { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Milieuvergunning { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CommercialProperties Bedrijfswoningen { get; set; }
    }

    public class MainFunction
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ArableFarm Akkerbouwbedrijf { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LooseGround LosseGrond { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RidingStables ManegePensionstalling { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DairyFarm Melkveehouderij { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MainFunctionOthers Overig { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HorseFarm Paardenhouderij { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PoultryBusiness Pluimveebedrijf { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HorticulturalCompany Tuinbouwbedrijf { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PigFarm Varkenshouderij { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public VealCalfFarm Vleeskalverenbedrijf { get; set; }

    }

    public class ArableFarm
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GroundTypes Grondsoorten { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public StorageSheds OpslagSchuren { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MachineWarehouses Machineloodsen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Productierechten { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OtherProvisions OverigeVoorzieningen { get; set; }
    }

    public class StorageSheds
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<Storageshed> Opslagschuur { get; set; } = new List<Storageshed>();
    }

    public class Storageshed
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Omschrijving { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwjaar { get; set; }
    }

    public class MachineWarehouses
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<MachineWarehouse> Machineloods { get; set; } = new List<MachineWarehouse>();
    }

    public class MachineWarehouse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Omschrijving { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwjaar { get; set; }
    }

    public class LooseGround
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Lengte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Breedte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GroundTypes Grondsoorten { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LandTypes TypenLand { get; set; }
    }

    public class LandTypes
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> TypeLand { get; set; } = new List<string>();
    }

    public class RidingStables
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Paardenboxen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RidingHalls Rijhallen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Warehouses Loodsen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OtherProvisions OverigeVoorzieningen { get; set; }
    }

    public class DairyFarm
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalLigboxen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalJongveePlaatsen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OtherRebounds Opstanden { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Melksysteem { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OtherProvisions OverigeVoorzieningen { get; set; }

    }

    public class MainFunctionOthers
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TypeOnderneming { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Superstructures Opstallen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Installations Installaties { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OtherProvisions OverigeVoorzieningen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GroundTypes Grondsoorten { get; set; }
    }

    public class Superstructures
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<Superstructure> Opstal { get; set; } = new List<Superstructure>();
    }

    public class Superstructure
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Omschrijving { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwjaar { get; set; }
    }

    public class HorseFarm
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Paardenboxen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RidingHalls Rijhallen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public WalkingStable Loopstal { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Warehouses Loodsen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Fences Afrasteringen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OtherProvisions OverigeVoorzieningen { get; set; }
    }

    public class RidingHalls
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<RidingHall> Rijhal { get; set; } = new List<RidingHall>();
    }

    public class RidingHall
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwjaar { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Kantine { get; set; }
    }

    public class WalkingStable
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
    }

    public class Warehouses
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<Warehouse> Loods { get; set; } = new List<Warehouse>();
    }

    public class Warehouse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Omschrijving { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwjaar { get; set; }
    }

    public class Fences
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Afrastering { get; set; } = new List<string>();
    }

    public class PoultryBusiness
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TypeOnderneming { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TypeOfHousing TypeHuisvesting { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwjaar { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ManureSilos Mestsilos { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Pluimveerechten { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Groenlabel { get; set; }
    }

    public class TypeOfHousing
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> TypeHuisvesting { get; set; } = new List<string>();
    }

    public class HorticulturalCompany
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? TypeOnderneming { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CultivationTypes TeelTypen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Ground Grond { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Greenhouses GlasOpstanden { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OtherRebounds OverigeOpstanden { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Installations Installaties { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Erfverharding { get; set; }

    }

    public class CultivationTypes
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> TeeltType { get; set; } = new List<string>();
    }

    public class Ground
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Kavellengte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? KavelBreedte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? BebouwdeBreedte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GroundTypes Grondsoorten { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Drainage { get; set; }
    }

    public class GroundTypes
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Grondsoort { get; set; } = new List<string>();
    }

    public class Greenhouses
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<GreenHouse> GlasOpstand { get; set; } = new List<GreenHouse>();
    }

    public class GreenHouse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Omschrijving { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwjaar { get; set; }
    }

    public class OtherRebounds
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<Rebound> Opstand { get; set; } = new List<Rebound>();
    }

    public class Rebound
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Omschrijving { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwjaar { get; set; }
    }

    public class Installations
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Installatie { get; set; } = new List<string>();
    }

    public class PigFarm
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? TypeOnderneming { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalVleesvarkerns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalFokvarkerns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalBiggen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalVleesvarkensstallen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalFokvarkensstallen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalBiggenstallen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwjaar { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ManureSilos Mestsilos { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Varkensrechten { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OtherProvisions OverigeVoorzieningen { get; set; }
    }

    public class VealCalfFarm
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalKalveren { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwjaar { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ManureSilos MestSilos { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OtherProvisions OverigeVoorzieningen { get; set; }
    }

    public class ManureSilos
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ManureSilo> MestSilo { get; set; } = new List<ManureSilo>();
    }

    public class ManureSilo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Inhoud { get; set; }
    }

    public class OtherProvisions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Voorzieningen { get; set; } = new List<string>();
    }

    public class SecondaryFunction
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FunctieAgrarisch { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FunctionNotAgricultural FunctieNietAgrarisch { get; set; }
    }

    public class BuildingBlocks
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<BuildingBlock> Bouwblok { get; set; } = new List<BuildingBlock>();
    }

    public class BuildingBlock
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Surface Oppervlakte { get; set; }
    }

    public class FieldPlotSurface
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Hectare { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Are { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Centiare { get; set; }
    }

    public class HousePlotSurface
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Hectare { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Are { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Centiare { get; set; }
    }

    public class CommercialProperties
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalBedrijfswoningen { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MainHouse Hoofdwoning { get; set; }
    }

    public class MainHouse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? BrutoInhoud { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Constructionyear Bouwjaar { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Maintenance Onderhoud { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Energylabel Energielabel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal EPC { get; set; }
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

    public class Energylabel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Energieklasse { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal EnergieIndex { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Einddatum { get; set; }
    }

    public class BuildingPlot
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Hoofdbestemming { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Nevenbestemming { get; set; }
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

    public class Transactions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ObjectTransaction> Transactiegegevens { get; set; } = new List<ObjectTransaction>();
    }

    public class ObjectTransaction
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TransactieConditie { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TransactieDetail { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InVerhuurdeVerpachteStaat { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RedenIntrekking { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EconomischeOverdracht { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Toelichting { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionObject ObjectTransactie { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumOndertekeningAkte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumTransport { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Surface Oppervlakte { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Collegiaal { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public EstateAgent Makelaar { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int AfmeldendKantoor { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuyerInformation Koperinformatie { get; set; }
    }

    public class TransactionObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Transactieprijs { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceComponents PrijsOnderdelen { get; set; }
    }

    public class PriceComponents
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<PriceComponent> MyProperty { get; set; } = new List<PriceComponent>();
    }

    public class PriceComponent
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Onderdeel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Prijs { get; set; }
    }

    public class Surface
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Hectare { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Are { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Centiare { get; set; }
    }

    public class EstateAgent
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

    public class BuyerInformation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HouseholdSituation Huishoudensituatie { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ReUse Herbestemming { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Expropriation Onteigening { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AbandonedObject AchtergelatenObject { get; set; }
    }

    public class HouseholdSituation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Kopertype { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SamenstellingHuishouden { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LeeftijdHoofdkostwinner { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalLedenHuishouden { get; set; }
    }

    public class ReUse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Voortzetting { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FunctieAgrarisch { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FunctionNotAgricultural MyProperty { get; set; }
    }

    public class FunctionNotAgricultural
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Functie { get; set; } = new List<string>();

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Overig { get; set; }
    }

    public class Expropriation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? WaardeOnroerendeZaak { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Herinvesteringschade { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Financieringschade { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? BijkomendeSchadeloosstelling { get; set; }
    }

    public class AbandonedObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AbandonedObjectObjectDetails ObjectDetails { get; set; }
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
        public bool IsHoofdMedium { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? MediumID { get; set; }

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
}

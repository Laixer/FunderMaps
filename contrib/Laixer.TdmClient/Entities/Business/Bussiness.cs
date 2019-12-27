using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TdmClient.Entities.Business
{
    public class Business
    {
        public BusinessObjectBeeld Objectbeeld { get; set; }
    }

    public class BusinessObjectBeeld
    {
        public Guid id { get; set; }
        public BusinessObject Object { get; set; }
    }

    public class BusinessObject
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
        public Functions Functies { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Cadastre Kadaster { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CoApplicants MedeAanmelders { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ObjectTransaction Transactiegegevens { get; set; }
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

    public class ObjectDetails
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Valuta { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Address Adres{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Buy Koop { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Rent Huur { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Auction Veiling { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Aanvaarding { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DatumAanvaarding { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectAanmelding { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumInvoer { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public StatusAvailability MyProperty { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Bouwvorm { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Vertrouwelijk { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EigenBelang { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public InternetPlacements Internetplaatsingen{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CollegialeInformatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public InformationObligation Informatieplicht { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Aanbiedingstekst { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AanbiedingstekstEngels { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? OppervlaktePerceel { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Hoofdfunctie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ResidenceObject Woonobject { get; set; }
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

    public class AddressCoordinates
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double X { get; set; } // TODO: was int
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double Y { get; set; } // TODO: was int
    }

    public class Buy
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string KoopConditie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public WOZ WOZ { get; set; }
    }

    public class Rent
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HuurConditie { get; set; }
    }

    public class Auction
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Vanafprijs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Zoekprijs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SoortVeiling { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Veilingtype { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StartDatumTijd { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EindDatumTijd { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SiteNaam { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SiteAdres { get; set; }
    }

    public class StatusAvailability
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HuidigeStatus { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IsActief { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumVoorbehoudtot { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SoldUnderReservation VerkochtOnderVoorbehoud { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RentedWithRestriction VerhuurdOnderVoorbehoud { get; set; }
    }

    public class SoldUnderReservation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Datum { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Koopprijs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string KoopConditie { get; set; }

    }

    public class RentedWithRestriction
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Datum { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Huurprijs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HuurConditie { get; set; }

    }

    public class InternetPlacements
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<InternetPlacement> Internetplaatsing { get; set; } = new List<InternetPlacement>();
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime OpenHuis { get; set; }
    }

    public class InternetPlacement
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Plaatsing { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumVrijgave { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumEind { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Prijsvermelding { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Adresvermelding { get; set; }
    }

    public class InformationObligation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Informatie { get; set; } = new List<string>();
    }

    public class ResidenceObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Situatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? ObjectID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectDetailsUrl { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectGuid { get; set; }
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

    public class Functions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OfficeSpace Kantoorruimte{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RetailSpace Winkelruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BusinessSpace Bedrijfsruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingLand Bouwgrond { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PavedOutdoorArea VerhardeBuitenterrein { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CateringIndustry Horeca { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Leisure Leisure { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Investment Belegging { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SocialProperty MaatschappelijkVastgoed { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Others Overige { get; set; }
    }

    public class OfficeSpace
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? InUnitsVanaf { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IndicatieTurnkey { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AirTreatments Luchtbehandelingen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Verdiepingen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DeliveryLevel Opleveringsniveau { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FlexPlek { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MeetcertificaatAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MultiTentantBuilding Verzamelgebouw { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingDetails GebouwDetails { get; set; }
    }

    public class RetailSpace
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PublieksgerichteDienstverlening { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Detailhandel { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Showroom { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? InUnitsVanaf { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? VerkoopVloeroppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BijdrageWinkeliersvereniging { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForAcquisition TerOvername { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Verdiepingen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Frontbreedte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Branchebeperking { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HorecaToegestaan { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Welstandsklasse { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MultiTentantBuilding Verzamelgebouw { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingDetails GebouwDetails { get; set; }
    }

    public class MultiTentantBuilding
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CommonRooms GemeenschappelijkeRuimtes { get; set; }
    }

    public class CommonRooms
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> GemeenschappelijkeRuimte { get; set; } = new List<string>();
    }

    public class BusinessSpace
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BusinessHall Bedrijfshal { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LogistiekeFunctie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BusinessOfficeSpace Kantoorruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Terrain Terrein{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingDetails GebouwDetails { get; set; }
    }

    public class BusinessHall
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? InUnitsVanaf { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? VrijeHoogte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? VrijeOverspanning { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Vloerbelasting { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int MyProperty { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AirTreatments Luchtbehandelingen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BusinessHallProvisions Voorzieningen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
    }

    public class BusinessHallProvisions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Voorziening { get; set; } = new List<string>(3);
    }

    public class BusinessOfficeSpace
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Verdiepingen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AirTreatments Luchtbehandelingen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DeliveryLevel Opleveringsniveau { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
    }

    public class AirTreatments
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Luchtbehandeling { get; set; } = new List<string>(3);
    }

    public class DeliveryLevel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Voorzining { get; set; } = new List<string>();
    }

    public class Terrain
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ExpansionPosibilities Uitbredingsmogelijkheden { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
    }

    public class ExpansionPosibilities
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Procentueel { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? InVierkanteMeters { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwhoogte { get; set; }
    }

    public class BuildingLand
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Bebouwingsmogelijkheid { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingVolume Bouwvolume { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? InUnitsVanaf { get; set; }
    }

    public class BuildingVolume
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Procentueel { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? InVierkanteMeters { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bouwhoogte { get; set; }
    }

    public class PavedOutdoorArea
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? PercentageVerhard { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Omheind { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Afsluitbaar { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TypeVerharding { get; set; }
    }

    public class CateringIndustry
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BeverageSector Drankensector { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FastfoodSector Fastfoodsector { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RestaurantSector Restaurantsector { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HotelSector Hotelsector { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? VerkoopVloeroppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LocatedOnFloor GelegenOpVerdieping { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Regions Regios { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IndicatieHorecaConcentratiegebied { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForAcquisition TerOvername { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Welstandsklasse { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingDetails GebouwDetails { get; set; }
    }

    public class BeverageSector
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HorecaTypes HorecaTypen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Horecaruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? NietHorecaruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? HuidigeAantalZitplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonruimteAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? WoningOppervlakteInVierkantemeters { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? HuurinkomsterWoonruimtePerJaar { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Terras { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? OmzetLaatsteBoekjaar { get; set; }
    }

    public class FastfoodSector
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HorecaTypes HorecaTypen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Horecaruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? HuidigeAantalZitplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonruimteAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? WoningOppervlakteInVierkantemeters { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? HuurinkomsterWoonruimtePerJaar { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Terras { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? OmzetLaatsteBoekjaar { get; set; }
    }

    public class RestaurantSector
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HorecaTypes HorecaTypen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Horecaruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? NietHorecaruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? HuidigeAantalZitplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonruimteAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? WoningOppervlakteInVierkantemeters { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? HuurinkomsterWoonruimtePerJaar { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Terras { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? OmzetLaatsteBoekjaar { get; set; }
    }

    public class HotelSector
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HorecaTypes HorecaTypen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Horecaruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? NietHorecaruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalHotelkamers { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonruimteAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? WoningOppervlakteInVierkantemeters { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? HuurinkomsterWoonruimtePerJaar { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Terras { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? OmzetLaatsteBoekjaar { get; set; }
    }

    public class HorecaTypes
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> HorecaType { get; set; } = new List<string>();
    }

    public class LocatedOnFloor
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<uint> Verdieping { get; set; } = new List<uint>();
    }

    public class Leisure
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Camping Camping { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BungalowPark Bungalowpark{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DayRecreation Dagrecreatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Wellness Wellness { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LeisureOthers Overig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingDetails GebouwDetails { get; set; }
    }

    public class Camping
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForAcquisition TerOvername { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonobjectAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalStaanplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Jaarplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LeisureProvisions Voorzieningen { get; set; }
    }

    public class BungalowPark
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForAcquisition TerOvername { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonobjectAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LeisureProvisions Voorzieningen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalVerblijven { get; set; }
    }

    public class DayRecreation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForAcquisition TerOvername { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonobjectAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LeisureProvisions Voorzieningen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? CapaciteitInPersonen { get; set; }
    }

    public class Wellness
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForAcquisition TerOvername { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonobjectAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LeisureProvisions Voorzieningen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? CapaciteitInPersonen { get; set; }
    }

    public class LeisureOthers
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForAcquisition TerOvername { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonobjectAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LeisureProvisions Voorzieningen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalVerblijven { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? CapaciteitInPersonen { get; set; }
    }

    public class LeisureProvisions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Voorziening { get; set; } = new List<string>();
    }

    public class Investment
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BeleggingType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public InvestmentHoreca Horeca{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public InvestmentLivingspace Woonruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public InvestmentGround Grond { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Portefeulle { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AdresTonen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Regions Regios{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalZelfstandigVerhuurbareeenheden { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalHuurders { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? BrutoHuuropbrengstKaleHuur { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? BrutoAanvangRendement { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Empty Leegstaand { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? ResterendeLooptijdContractenInMaanden { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BTWBelast { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingDetails GebouwDetails { get; set; }
    }

    public class InvestmentHoreca
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> HorecaType { get; set; } = new List<string>(4);
    }

    public class InvestmentLivingspace
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonruimteType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? LeegstandInAantalWooneenheden { get; set; }
    }

    public class InvestmentGround
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Categorie { get; set; }
    }

    public class Regions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Regio { get; set; } = new List<string>();
    }

    public class Empty
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Huurwaarde { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? VierkanteMeters { get; set; }
    }

    public class SocialProperty
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HealthcareInstitution ZorgInstelling { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SportInstitution SportInstelling { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CulturalInstitution CultureleInstelling { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ReligiousInstitution ReligieuzeInstelling { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public EducationalInstitution OnderwijsInstelling { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingDetails GebouwDetails { get; set; }
    }

    public class HealthcareInstitution
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Praktijkruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalPlaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Destination Bestemmingen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BTWBelast { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SocialPropertyProvision Voorzieningen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HerbestemmingMogelijk { get; set; }
    }

    public class Destination
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalZorgWooneenheden { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Huisvestingsvorm { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Sanitair { get; set; }
    }

    public class SportInstitution
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonruimteAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BTWBelast { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SocialPropertyProvision Voorzieningen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HerbestemmingMogelijk { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TerugleveringVanDiensten { get; set; }
    }

    public class CulturalInstitution
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonruimteAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BTWBelast { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SocialPropertyProvision Voorzieningen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HerbestemmingMogelijk { get; set; }
    }

    public class ReligiousInstitution
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonruimteAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BTWBelast { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SocialPropertyProvision Voorzieningen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HerbestemmingMogelijk { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TerugleveringVanDiensten { get; set; }
    }

    public class EducationalInstitution
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WoonruimteAanwezig { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BTWBelast { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SocialPropertyProvision Voorzieningen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HerbestemmingMogelijk { get; set; }
    }

    public class SocialPropertyProvision
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Voorziening { get; set; } = new List<string>();
    }

    public class Others
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Categorie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? InUnitsVanaf { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Verdiepingen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuildingDetails GebouwDetails { get; set; }
    }

    public class BuildingDetails
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Constructionyear Bouwjaar{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Gebouwnaam { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Maintenance Onderhoud { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Location Lokatie { get; set; }
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

    public class Location
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Positions Liggingen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PositionFeatures Liggingskenmerken { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Accessibility Bereikbaarheid { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BusinessProvisions Voorzieningen{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BusinessParking Parkeren { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Toelichting { get; set; }
    }

    public class Positions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Ligging { get; set; } = new List<string>();
    }

    public class PositionFeatures
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Liggingskenmerk { get; set; } = new List<string>();
    }

    public class Accessibility
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SnelwegAfrit { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NSStation { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NSVoorhalte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Busknooppunt { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Tramknooppunt { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Metroknooppunt { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Bushalte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Tramhalte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Metrohalte { get; set; }

    }

    public class BusinessProvisions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Bank Bank { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Relaxation Ontspanning { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Restaurant Restaurant { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Store Winkel { get; set; }
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

    public class Bank
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Afstand { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Aantal { get; set; }
    }

    public class Relaxation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Afstand { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Aantal { get; set; }
    }

    public class Restaurant
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Afstand { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Aantal { get; set; }
    }

    public class Store
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Afstand { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Aantal { get; set; }
    }

    public class BusinessParking
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Parkeerfaciliteiten { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NonCoveredParkingPlaces NietOverdekteParkeerplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CoveredParkingPlaces OverdekteParkeerplaatsen { get; set; }
    }

    public class NonCoveredParkingPlaces
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Aantal { get; set; }
    }

    public class CoveredParkingPlaces
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Aantal { get; set; }
    }

    public class PriceSpecification
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Prijs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BTWBelast { get; set; }
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

    public class ObjectTransaction
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TransactieConditie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TransactieDetail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IndicatieOpenbaar { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuyTenant KopenHuurder { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public EstateAgent Makelaar { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AfmeldendKantoor { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SBICodes SBIcodes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Rented Verhuurd { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Sold Verkocht { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Auctioned Geveild { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SoldWithRegistration VerkochtBijInschrijving { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Withdrawn Ingetrokken { get; set; }
    }

    public class BuyTenant
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Naam { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RedenVestiging { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PostcodeHerkomst { get; set; }
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

    public class SBICodes
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> SBIcode { get; set; } = new List<string>();
    }

    public class Rented
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HuurConditie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumIngangHuurovereenkomst { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DuurHuurovereenkomst { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumOndertekeningAkte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? OppervlaktePerceel { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Incentives Incentives { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NonCoveredParkingPlaces NietOverdekteParkeerplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CoveredParkingPlaces OverdekteParkeerplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ServiceCosts  ServiceKosten{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionBusinessSpace Bedrijfsruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionBuildingLand Bouwgrond { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionCateringIndustry Horeca { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionOfficeSpace Kantoorruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionOthers Overige { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionRetailSpace Winkelruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionSocialProperty MaatschappelijkVastgoed { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionLeisure Leisure { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionPavedOutdoorArea VerhardBuitenrerrein { get; set; }
    }

    public class Incentives
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IndicatieIncentives { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? ConstanteWaardeIncentives { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? AantalMaandenIncentives { get; set; }
    }

    public class Sold
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string KoopConditie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IndicatieSaleAndLeaseBack { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumOndertekeningAkte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumTransport { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? OppervlaktePerceel { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NationaliteitBelegger { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TypeBelegger { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NonCoveredParkingPlaces NietOverdekteParkeerplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CoveredParkingPlaces OverdekteParkeerplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionBusinessSpace Bedrijfsruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionBuildingLand Bouwgrond { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionCateringIndustry Horeca { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionOfficeSpace Kantoorruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionOthers Overige { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionRetailSpace Winkelruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionSocialProperty MaatschappelijkVastgoed { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionLeisure Leisure { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionPavedOutdoorArea VerhardBuitenrerrein { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionInvestment Belegging { get; set; }
    }

    public class TransactionInvestment
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
    }

    public class Auctioned
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string KoopConditie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumOndertekeningAkte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumTransport { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? OppervlaktePerceel { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NonCoveredParkingPlaces NietOverdekteParkeerplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CoveredParkingPlaces OverdekteParkeerplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionBusinessSpace Bedrijfsruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionBuildingLand Bouwgrond { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionCateringIndustry Horeca { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionOfficeSpace Kantoorruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionOthers Overige { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionRetailSpace Winkelruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionSocialProperty MaatschappelijkVastgoed { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionLeisure Leisure { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionPavedOutdoorArea VerhardBuitenrerrein { get; set; }
    }

    public class SoldWithRegistration
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string KoopConditie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumOndertekeningAkte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumTransport { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? OppervlaktePerceel { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NonCoveredParkingPlaces NietOverdekteParkeerplaatsen{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CoveredParkingPlaces OverdekteParkeerplaatsen { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionBusinessSpace Bedrijfsruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionBuildingLand Bouwgrond { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionCateringIndustry Horeca { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionOfficeSpace Kantoorruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionOthers Overige { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionRetailSpace Winkelruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionSocialProperty MaatschappelijkVastgoed { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionLeisure Leisure { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionPavedOutdoorArea VerhardBuitenrerrein { get; set; }
    }

    public class TransactionBusinessSpace
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionBusinessHall Bedrijfshal { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BusinessSpaceOfficeSpace Kantoorruimte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TransactionTerrain Terrein { get; set; }
    }

    public class TransactionBusinessHall
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
    }

    public class BusinessSpaceOfficeSpace
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
    }

    public class TransactionTerrain
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
    }

    public class TransactionBuildingLand
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
    }

    public class TransactionCateringIndustry
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForAcquisition TerOVername { get; set; }
    }

    public class TransactionOfficeSpace
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
    }

    public class TransactionOthers
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
    }

    public class TransactionRetailSpace
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForAcquisition TerOVername { get; set; }
    }

    public class TransactionSocialProperty
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
    }

    public class TransactionLeisure
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ForAcquisition TerOVername { get; set; }
    }

    public class ForAcquisition
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceInventoryGoodwill PrijsInventarisGoodwill { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Personeel { get; set; }
    }

    public class PriceInventoryGoodwill
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Prijs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BTWBelast { get; set; }
    }

    public class TransactionPavedOutdoorArea
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Oppervlakte { get; set; }
    }

    public class Withdrawn
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RedenIntrekking { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DatumOndertekeningAkte { get; set; }
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
        public Guid MediaGuid { get; set; }
    }

    public class History
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HistoryBuy Koop { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HistoryRent Huur{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HistoryAuction Veiling { get; set; }
    }

    public class HistoryBuy
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BuyHistory Koophistorie { get; set; }
    }

    public class BuyHistory
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Begin { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Eind { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RentPriceSpecification PrijsSpecificatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string KoopConditie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public WOZ WOZ{ get; set; }
    }

    public class WOZ
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WOZWaarde { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WOZWaardePeildatum { get; set; }
    }

    public class HistoryRent
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<RentHistory> Huurhistorie { get; set; } = new List<RentHistory>();
    }

    public class RentHistory
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Begin { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Eind { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RentPriceSpecification PrijsSpecificatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HuurConditie { get; set; }
    }

    public class RentPriceSpecification
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Prijsvoorvoegsel { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BTWWBelast { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ServiceCosts Servicekosten { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Prijs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrijsVan { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrijsTot { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrijsCollegiaaltonen { get; set; }
    }

    public class ServiceCosts
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PriceSpecification PrijsSpecificatie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ServiceKostenConditie { get; set; }
    }

    public class HistoryAuction
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<AuctionHistory> Veilinghistorie { get; set; } = new List<AuctionHistory>();
    }

    public class AuctionHistory
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Begin { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Eind { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? Vanafprijs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Zoekprijs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SoortVeiling { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Veilingtype { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartDatum { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EindDatum { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SiteNaam { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SiteAdres { get; set; }
    }
}

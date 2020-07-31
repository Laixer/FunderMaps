using FunderMaps.Webservice.Enums;
using FunderMaps.Webservice.Exceptions;
using FunderMaps.Webservice.Utility;

namespace FunderMaps.Webservice.Mapping
{

    /// <summary>
    /// Contains mapping functionality between <see cref="ProductType"/>s 
    /// and their respective names.
    /// </summary>
    internal static class ProductTypeMapper
    {
        /// <summary>
        /// Maps a given <paramref name="name"/> to the corresponding <see cref="ProductType"/>.
        /// TODO Niet hardcoden.
        /// TODO Hoe doen we dit elegant?
        /// </summary>
        /// <remarks>
        /// This changes the <paramref name="name"/> to lower case before comparing.
        /// </remarks>
        /// <param name="name"><see cref="Product"/> name</param>
        /// <returns><see cref="ProductType"/></returns>
        public static ProductType Map(string name)
        {
            name.ThrowIfNullOrEmpty();
            name = name.ToUpperInvariant();
            return name switch
            {
                "HERSTEL" => ProductType.StatisticsBuildingsRestored,
                "MELDINGEN" => ProductType.StatisticsIncidents,
                "ONDERZOEKEN" => ProductType.StatisticsReports,
                "BOUWJAREN" => ProductType.StatisticsConstructionYears,
                "VASTGESTELDE_GEGEVENS" => ProductType.StatisticsDataCollected,
                "FUNDERING_VERHOUDING" => ProductType.StatisticsFoundationRatio,
                "FUNDERING_RISICO" => ProductType.StatisticsFoundationRisk,
                "PAND" => ProductType.Building,
                "KOSTEN" => ProductType.Costs,
                "BESCHRIJVING" => ProductType.Description,
                "FUNDERING" => ProductType.Foundation,
                "FUNDERING_PLUS" => ProductType.FoundationPlus,
                "VOLLEDIG" => ProductType.Complete,
                "RISICO" => ProductType.Risk,
                _ => throw new ProductNotFoundException(nameof(name)),
            };
        }
    }
}

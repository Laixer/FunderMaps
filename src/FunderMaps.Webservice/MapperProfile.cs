using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.Webservice.DataTransferObjects;

namespace FunderMaps.Webservice
{
    /// <summary>
    ///     Mapper profile for proper DTO mapping.
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        ///     Setup mapping profiles.
        /// </summary>
        public MapperProfile()
        {
            CreateMap<ProductTelemetry, ProductTelemetryDto>();
        }
    }
}

using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi.Profiles
{
    /// <summary>
    /// Mapping profile.
    /// </summary>
    public class RecoverySampleProfile : Profile
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public RecoverySampleProfile()
            => CreateMap<RecoverySample, RecoverySampleDTO>().ReverseMap();
    }
}

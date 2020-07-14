using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi.Profiles
{
    /// <summary>
    /// Mapping profile.
    /// </summary>
    public class InquirySampleProfile : Profile
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public InquirySampleProfile()
            => CreateMap<InquirySample, InquirySampleDTO>().ReverseMap();
    }
}

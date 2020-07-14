using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi.Profiles
{
    /// <summary>
    /// Mapping profile.
    /// </summary>
    public class InquiryProfile : Profile
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public InquiryProfile()
            => CreateMap<Inquiry, InquiryDTO>().ReverseMap();
    }
}

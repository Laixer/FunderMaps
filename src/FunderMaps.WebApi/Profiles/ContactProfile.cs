using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi.Profiles
{
    /// <summary>
    /// Mapping profile.
    /// </summary>
    public class ContactProfile : Profile
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public ContactProfile()
            => CreateMap<Contact, IncidentDTO>().ReverseMap();
    }
}

using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.WebApi.Profiles
{
    /// <summary>
    ///     Mapping profile.
    /// </summary>
    public class IncidentProfile : Profile
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentProfile()
            => CreateMap<Incident, IncidentDto>()
                .IncludeMembers(s => s.ContactNavigation)
                .ReverseMap();
    }
}
